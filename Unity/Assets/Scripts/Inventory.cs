using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Text;
using SimpleJSON;
using System.Globalization;
using UnityEngine.Networking;

    public class Inventory : NetworkBehaviour
    {
    public GameObject[] inventory = new GameObject[6];
    public Button[] InventoryButtons = new Button[6];    

    [DllImport("__Internal")]
    private static extern void DoInteraction(string message);

    void Start(){
        //string JSONString = "{\"Pokemons\":[{\"type\":\"Roucoul\",\"name\":\"PikaPika\",\"color\":\"yellow\",\"position_x\":\"-56.5\",\"position_y\":\"3.6\"},{\"type\":\"Florizard\",\"name\":\"CaraCara\",\"color\":\"blue\",\"position_x\":\"-57.5\",\"position_y\":\"3.6\"},{\"type\":\"Dracofeu\",\"name\":\"SalaSala\",\"color\":\"red\",\"position_x\":\"-55.5\",\"position_y\":\"3.6\"}]}";
        string JSONString = "[{\"skills_id\": [1,7,32],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert le pikachu\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,7,32],\"_id\": \"5dd0571370fc0849c41dde87\",\"type\": \"Pikachu\",\"name\": \"Gerard le pikachu\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T20:07:47.401Z\",\"updated_date\": \"2019-11-16T20:07:47.401Z\",\"__v\": 0}]";
        //RetrievePokemonList(JSONString);
    }

    // La fonction AddItem est appellé dans "PlayerInteract.cs" lorsque le player appuit sur E devant un Pokémon typé inventaire. 

     public bool AddItem(GameObject item){

        bool itemAdded = false;

        // Add item at the last available slot.
        for (int i = 0; i<inventory.Length; i++){
            if (inventory[i] == null){
                inventory[i] = item;
                itemAdded = true;
                InventoryButtons[i].image.overrideSprite = item.GetComponent<SpriteRenderer>().sprite;
                InventoryButtons[i].image.rectTransform.localScale += new Vector3(1f, 0f, 0f);
                return true;
            }
        }

        //Inventory Full
        if (!itemAdded){
            Debug.Log("Inventory Full - Item Not Added");
            return false;
        }

        return false;

    }

    // La fonction SendPokemonToReact est appellé dans "Inventory.cs". 
    public void SendMessageToReact_GetPokemonList(){

        string message = "{\"type\":" + "\"" + 2 + "\"}";
        Debug.Log("Message: " + message);
        
        try{
            DoInteraction(message);
        }
        catch{
            Debug.Log("Do interaction fail");
        }
    }

    public void RetrievePokemonList(string JSONString){

        // Create Pokemon in Game

        var PokemonsJSON = JSON.Parse(JSONString);
        Debug.Log(PokemonsJSON[0]);
        int N = PokemonsJSON.Count;
        Debug.Log("Retrieving " + N + " pokemons");
                
        for (int i=0; i<N; i++){    
            float pos_x = -100f;
            float pos_y = -100f;
            var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
            Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);
            // Create Pokemon item in Inventory
            AddItem(Pokemon);

            // Deactivate Pokemon
            Pokemon.GetComponent<PokemonObject>().visible = false;
            Pokemon.GetComponent<PokemonObject>().Deactivate(false);

        }


    }



    public bool FindItem(GameObject item){
        for (int i = 0; i<inventory.Length; i++){
            if (inventory[i] == item)
                return true;
        }
        return false;
    }

    public GameObject FindItem(string itemType){
        for (int i = 0; i<inventory.Length; i++){
            if (inventory[i] == null){
                if (inventory[i].GetComponent <PokemonObject> ().itemType == itemType)
                    return inventory[i];
            }
        }
        return null;
    }

    public void RemoveItem(GameObject item){
        for (int i = 0; i<inventory.Length; i++){
            if (inventory[i] == item){
                inventory[i] = null;
                Debug.Log(item.name + " was removed from inventory");
                InventoryButtons[i].image.overrideSprite = null;

                break;
            }
        }
    }

}