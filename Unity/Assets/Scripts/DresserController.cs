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
    public string MyEirbmons = null;
    
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

        base.OnStartLocalPlayer();
        gameObject.name = "Dresser(Local)";

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SendMessageToReact("user_pokemon");
        string MyEirbmons2 = "[{\"skills_id\": [1,2,3],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 4,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [4,5,6],\"_id\": \"5dd0571370fc0849c41dde87\",\"type\": \"Carapuce\",\"name\": \"Gerard\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 9,\"created_date\": \"2019-11-16T20:07:47.401Z\",\"updated_date\": \"2019-11-16T20:07:47.401Z\",\"__v\": 0},{\"skills_id\": [7,8,0],\"_id\": \"5dd0571370fc0849c41dde87\",\"type\": \"Salameche\",\"name\": \"Valentin\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T20:07:47.401Z\",\"updated_date\": \"2019-11-16T20:07:47.401Z\",\"__v\": 0}]";
        RetrievePokemonList(MyEirbmons2);
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
            CmdRemoveItem(currentInterObj.name);
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
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive); 
        this.transform.Find("InventoryCanvas").gameObject.SetActive(false);  
        }
    }

    public void LeaveCombat(){
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
        this.transform.Find("InventoryCanvas").gameObject.SetActive(true);  
    }

    public void RetrievePokemonList(string JSONString){

        // Create Pokemon in Game

        MyEirbmons = JSONString;

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
    void RpcRemoveItem(string name){
        Debug.Log(name);
        PokemonObject starterPokemon = GameObject.Find(name).GetComponent <PokemonObject> ();
        
        if (starterPokemon.inventory){
                starterPokemon.visible = false;
                starterPokemon.Deactivate(false);
        }
    }

    [Command]
    void CmdRemoveItem(string name){
        RpcRemoveItem(name);
    }

    void TakeItem(){

        if (!isLocalPlayer){
            return;
        }

        if (currentInterObjScript.inventory){

                    if (inventory.AddItem(currentInterObj)){
                        gameManager.SendMessageToReact(currentInterObjScript.ConvertToString());
                }
        }
    }

    public void CatchPokemon(string JSONString){

        var PokemonsJSON = JSON.Parse(JSONString);
        int N = PokemonsJSON.Count;
        var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[0]["type"], typeof(GameObject)), new Vector2(-100, -100), Quaternion.identity) as GameObject;
        Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[0]);
        inventory.AddItem(Pokemon);
        

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
                    if (Random.Range(0,12) == 5){
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