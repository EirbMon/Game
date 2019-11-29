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
    GameManager gameManager;
    public float speed = 3.0f;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    
    public int health {  get { return currentHealth; }}
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;
    string name;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    Rigidbody2D rigidbody2d;

    public GameObject currentInterObj = null;
    public PokemonObject currentInterObjScript = null;
    public Inventory inventory;

    private bool testBool = true; 

    
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) {
        this.transform.Find("CM vcam1").gameObject.SetActive(false);
        this.transform.Find("InventoryCanvas").gameObject.SetActive(false);
        }
    }

     public override void OnStartLocalPlayer()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        NetworkInstanceId playerId = this.netId;
        Debug.Log("Start Dresser with ID: " + playerId);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SendMessageToReact("user_pokemon");
        base.OnStartLocalPlayer();
        gameObject.name = "Dresser(Local)";
    }


    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer){
            return;
        }

        if (SceneManager.GetSceneByName("CombatScene").isLoaded)
            return;
        

        if (Input.GetButtonDown("Interact") && currentInterObj){
            TakeItem();
            CmdRemoveItem();
            //if (isServer)
            //    RpcTakeItem();
            //else
            //    CmdTakeItem();
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            testBool = !testBool;
            this.transform.Find("InventoryCanvas").gameObject.SetActive(testBool);  
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
        Vector2 new_position = new Vector2(x,y); 
        rigidbody2d.position = new_position;
        //rigidbody2d.MovePosition(new_position);
    }

    public void EnterCombat(){

        if (isLocalPlayer){
        Debug.Log("Enter Combat");
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive); 
        this.transform.Find("InventoryCanvas").gameObject.SetActive(false);  
        }
    }

    public void LeaveCombat(){
        Debug.Log("Leave Combat");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
        this.transform.Find("InventoryCanvas").gameObject.SetActive(true);  
    }

    public void RetrievePokemonList(string JSONString){

        // Create Pokemon in Game

        var PokemonsJSON = JSON.Parse(JSONString);
        int N = PokemonsJSON.Count;
        Debug.Log("Retrieving " + N + " pokemons");
                
        for (int i=0; i<N; i++){    
            float pos_x = -100f;
            float pos_y = -100f;
            var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
            Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);
            // Create Pokemon item in Inventory
            inventory.AddItem(Pokemon);

            // Deactivate Pokemon
            Pokemon.GetComponent<PokemonObject>().visible = false;
            Pokemon.GetComponent<PokemonObject>().Deactivate(false);

        }
    }

    [ClientRpc]
    void RpcRemoveItem(){
        if (currentInterObjScript.inventory){
                currentInterObjScript.visible = false;
                currentInterObjScript.Deactivate(false);
        }
    }

    [Command]
    void CmdRemoveItem(){
        RpcRemoveItem();
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

                    if (inventory.AddItem(currentInterObj)){
                        currentInterObjScript.visible = false;
                        currentInterObjScript.Deactivate(false);
                        gameManager.SendMessageToReact(currentInterObjScript.ConvertToString());
                }
        }
    }

    public void CatchPokemon(string JSONString){

        var PokemonsJSON = JSON.Parse(JSONString);
        int N = PokemonsJSON.Count;
        Debug.Log("CatchPokemon");
                
        for (int i=0; i<N; i++){    
            var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject)), new Vector2(0, 0), Quaternion.identity) as GameObject;
            Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);
            inventory.AddItem(Pokemon);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
            if (isLocalPlayer) {
                
                if (other.CompareTag("interactionobject")){
                    currentInterObj = other.gameObject;
                    currentInterObjScript = currentInterObj.GetComponent <PokemonObject> ();
                } 

                if (other.CompareTag("HerbeHaute")){
                    currentInterObj = other.gameObject;
                    Debug.Log("Haute herbe contact");   
                    if (Random.Range(0,15) == 5){
                        EnterCombat();
                    }
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
    }

    void SavePlayer(){
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        string saveText = "{ \"Player\" : [{ \"name\":" + "\"" + this.name + "\"," + "\"position_x\":" + "\"" + horizontal + "\"," + "\"position_y\":" + "\"" + vertical + "\"}]}";
        gameManager.SendMessageToReact(saveText);
    }

    void LoadPlayer(string JSONString){

        var DresserJSON = JSON.Parse(JSONString)["Dresser"];
        this.name = DresserJSON["name"];
        float pos_x = float.Parse(DresserJSON[0]["position_x"],CultureInfo.InvariantCulture.NumberFormat);
        float pos_y = float.Parse(DresserJSON[0]["position_y"],CultureInfo.InvariantCulture.NumberFormat);
        transform.position = new Vector2(pos_x, pos_y);
        //rigidbody2d.MovePosition(transform.position);

    }
}