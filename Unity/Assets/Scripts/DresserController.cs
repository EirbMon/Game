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
    public string MyEirbmons = null;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    Rigidbody2D rigidbody2d;

    public GameObject currentInterObj = null;
    public Inventory inventory;

    private bool showInventory = true; 
    public bool waiting_react_response = true;
    public bool isInCombat = false;
    public string ennemyPNJ;
    bool dev = false;


    
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
        base.OnStartLocalPlayer();
        gameObject.name = "Dresser(Local)";
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SendMessageToReact("user_pokemon");
        ennemyPNJ = "null";
        dev = gameManager.dev;
        
        // EN DEV UNIQUEMENT 
        if (dev){
         string MyEirbmons2 = "[{\"skills_id\": [4,5,6],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Salameche\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 4,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [7,8,0],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Carapuce\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 4,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,2,3],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 4,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,2,3],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 4,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0}]";
         RetrievePokemonList(MyEirbmons2);
        }
    }


    // Update is called once per frame
    void Update()
    {

        // Stop player action if it's not the local player, if it's waiting react response or if it's in combat.
        if (!isLocalPlayer || waiting_react_response || isInCombat)
            return;

        // Interaction (touche E)
        if (Input.GetButtonDown("Interact") && currentInterObj){
        }

        // Echap
        if (Input.GetKeyDown(KeyCode.Escape)){
            showInventory = !showInventory;
            this.transform.Find("InventoryCanvas").gameObject.SetActive(showInventory);  
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

    }

    public void Teleport(float x, float y)
    {
        Vector2 new_position = new Vector2(x,y); 
        rigidbody2d.position = new_position;
    }

    public void EnterCombat(){
        this.isInCombat = true;
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive); 
        this.transform.Find("InventoryCanvas").gameObject.SetActive(false);  
    }

    public void LeaveCombat(){
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
        this.transform.Find("InventoryCanvas").gameObject.SetActive(true);  
        this.isInCombat = false;
    }

    public void RetrievePokemonList(string JSONString){

        try{
            waiting_react_response = false;
            MyEirbmons = JSONString;


            // On supprime tous les items de l'inventaire
            inventory.RemoveAllItem();

            var PokemonsJSON = JSON.Parse(JSONString);
            int N = PokemonsJSON.Count;
            Debug.Log("Retrieving " + N + " pokemons for the user.");
      
            for (int i=0; i<N; i++){    
                float pos_x = -100f;
                float pos_y = -100f;

                // Create Pokemon
                var pokemon_prefab = Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject));
                var Pokemon = (GameObject)Instantiate(pokemon_prefab, new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
                Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);

                // On rajoute le Pokemon dans l'inventaire
                inventory.AddItem(Pokemon);

                // Hide Pokemon
                Pokemon.GetComponent<PokemonObject>().visible = false;
                Pokemon.GetComponent<PokemonObject>().Deactivate(false);
            }

        }
        catch{
            Debug.LogError("Erreur GetOwnerEirbmonList dans Unity: Verifier que le JSON envoyé soit correcte (le type doit exister). Voici ce que Unity recoit: " + JSONString);
        }
    }
    public void CatchPokemon(string JSONString){

        waiting_react_response = false;

        try{

        var PokemonsJSON = JSON.Parse(JSONString)[0];
        var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON["type"], typeof(GameObject)), new Vector2(-100, -100), Quaternion.identity) as GameObject;
        Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON);
        inventory.AddItem(Pokemon);
        gameManager.SendMessageToReact("user_pokemon");
        if (dev)
            RetrievePokemonList("[{\"skills_id\": [4,5,6],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Salameche\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 4,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [7,8,0],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Carapuce\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 4,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,2,3],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 4,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,2,3],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 4,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\":[],\"force\":0,\"xp\":0,\"available\":false,\"_id\":\"5df577c5b9998c72a6838783\",\"idInBlockchain\":3,\"__v\":0,\"field\":\"Elec\",\"hp\":66,\"lvl\":0,\"name\":\"Carapuce\",\"owner_id\":\"0x48bbceca684cde0646b787769d30d9fa38927e28\",\"type\":\"Carapuce\"}]");
        
        }
        catch{
            Debug.LogError("Erreur CatchPokemon: l'Eirbmon capturé envoyé possède un format incorrecte (mauvais type ou format de la requete. Voici ce que Unity recoit: " + JSONString);
        }
        

    }

    void OnTriggerEnter2D(Collider2D other)
    {
            if (isLocalPlayer) {

                if (isInCombat)
                    return;
                
                if (other.CompareTag("HerbeHaute")){
                    currentInterObj = other.gameObject;  
                    if (Random.Range(0,10) == 0){
                        EnterCombat();
                    }
                }

                if (other.CompareTag("EnnemyPNJ")){
                    currentInterObj = other.gameObject;
                    if (currentInterObj.GetComponent<EnnemyPNJ>().busy == false){
                        ennemyPNJ = currentInterObj.name;  
                        EnterCombat();
                    }
                }

                else
                    currentInterObj = other.gameObject;
            }
    }
    void SavePlayer(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        string saveText = "{ \"Player\" : [{ \"name\":" + "\"" + this.name + "\"," + "\"position_x\":" + "\"" + horizontal + "\"," + "\"position_y\":" + "\"" + vertical + "\"}]}";
        //gameManager.SendMessageToReact(saveText);
    }

    void LoadPlayer(string JSONString){
        var DresserJSON = JSON.Parse(JSONString)["Dresser"];
        string dresserName = DresserJSON["name"];
        float pos_x = float.Parse(DresserJSON[0]["position_x"],CultureInfo.InvariantCulture.NumberFormat);
        float pos_y = float.Parse(DresserJSON[0]["position_y"],CultureInfo.InvariantCulture.NumberFormat);
        Teleport(pos_x,pos_y);
    }
}