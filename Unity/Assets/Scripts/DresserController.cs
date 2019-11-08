﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text;
using SimpleJSON;
using System.Globalization;

public class DresserController : NetworkBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    
    public int health {  get { return currentHealth; }}
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;
    public bool isInCombat = false;
    
    public bool catch_pokemon = false;

    public string CatchedPokemon = "null";
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    Rigidbody2D rigidbody2d;

    public GameObject currentInterObj = null;
    public PokemonObject currentInterObjScript = null;
    public Inventory inventory;

    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        Debug.Log("Start Dresser");

        if (!isLocalPlayer) {
        this.transform.Find("CM vcam1").gameObject.SetActive(false);
        this.transform.Find("InventoryCanvas").gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer){
            return;
        }

        if (isInCombat){
            Debug.Log("Start Combat Scene");
            SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive);   
            isInCombat = false;
        }

        if (SceneManager.GetSceneByName("CombatScene").isLoaded)
            return;
        

        if (Input.GetButtonDown ("Interact") && currentInterObj){
            TakeItem();
            if (isServer)
                RpcTakeItem();
            else
                CmdTakeItem();
        }

        if (catch_pokemon){
            catch_pokemon = false;
            Debug.Log("Inside catch pokemeon");
            SetPokemon(CatchedPokemon);
            
        }

        // Player localization
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Direction                
        Vector2 move = new Vector2(horizontal, vertical);          
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)){
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
                
        // Animation
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
                
        // Movement
        Vector2 position = rigidbody2d.position;        
        position = position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        if (isInvincible){
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0){
            if (isInvincible)
                return;          
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    public void Teleport(float x, float y)
    {
        transform.position = new Vector2(x, y);
        rigidbody2d.MovePosition(transform.position);
    }

    public void CombatMode(bool mode){
        isInCombat = mode;
    }

    [Command]
     void CmdTakeItem()
     {
         //Apply it to all other clients
         Debug.Log("CMD - Apply it to all other clients");
         TakeItem();
         RpcTakeItem();
     }
 
     [ClientRpc]
     void RpcTakeItem()
     {
         Debug.Log("RPC - Apply only to local client");
         if (isLocalPlayer)
             return;
         TakeItem();
     }

    void TakeItem(){

        if (!isLocalPlayer){
            return;
        }

        if (currentInterObjScript.inventory){
                    inventory.AddItem(currentInterObj);
                    currentInterObjScript.visible = false;
                    currentInterObjScript.Deactivate(false);
                    currentInterObjScript.SendPokemonToReact();
        }
    }

    public void SetPokemon(string JSONString){
        var PokemonsJSON = JSON.Parse(JSONString)["Pokemons"];
        int N = PokemonsJSON.Count;
        Debug.Log("Genetaring Pokemon from JSON, N = " + N);
                
        for (int i=0; i<N; i++){    
            Debug.Log("Spawn Pokemon ");
            float pos_x = float.Parse(PokemonsJSON[i]["position_x"],CultureInfo.InvariantCulture.NumberFormat);
            float pos_y = float.Parse(PokemonsJSON[i]["position_y"],CultureInfo.InvariantCulture.NumberFormat);
            var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
            Pokemon.GetComponent<PokemonObject>().type = PokemonsJSON[i]["type"];
            Pokemon.GetComponent<PokemonObject>().pokemon_name = PokemonsJSON[i]["name"];
            Pokemon.GetComponent<PokemonObject>().color = PokemonsJSON[i]["color"];
            Pokemon.GetComponent<PokemonObject>().position_x = pos_x;
            Pokemon.GetComponent<PokemonObject>().position_y = pos_y;
            // cf PokemonObject.cs 

            NetworkServer.Spawn(Pokemon);

            if (Pokemon.GetComponent<PokemonObject>().inventory){
                inventory.AddItem(Pokemon);
                Pokemon.GetComponent<PokemonObject>().visible = false;
                Pokemon.GetComponent<PokemonObject>().Deactivate(false);
                Pokemon.GetComponent<PokemonObject>().SendPokemonToReact();
            }

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
            if (other.CompareTag("interactionobject")){
                currentInterObj = other.gameObject;
                currentInterObjScript = currentInterObj.GetComponent <PokemonObject> ();
            } 

            if (other.CompareTag("HerbeHaute")){
                currentInterObj = other.gameObject;
                Debug.Log("Haute herbe contact");   
                if (Random.Range(0,15) == 5){
                    Debug.Log("Find Fight");   
                    isInCombat = true;
                }
            }   
    }

    void OnTriggerExit2D(Collider2D other)
    {
            if (other.CompareTag("interactionobject")){
                if (other.gameObject == currentInterObj){
                    currentInterObj = null;
                }
            }
            if (other.CompareTag("HerbeHaute")){
                if (other.gameObject == currentInterObj){
                    currentInterObj = null;
                }
            }
    }


}