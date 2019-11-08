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

    // La fonction AddItem est appellé dans "PlayerInteract.cs" lorsque le player appuit sur E devant un Pokémon typé inventaire. 

     public void AddItem(GameObject item){

        bool itemAdded = false;

        // Add item at the last available slot.
        for (int i = 0; i<inventory.Length; i++){
            if (inventory[i] == null){
                inventory[i] = item;
                itemAdded = true;
                InventoryButtons[i].image.overrideSprite = item.GetComponent<SpriteRenderer>().sprite;
                InventoryButtons[i].image.rectTransform.localScale += new Vector3(1f, 0f, 0f);
                break;
            }
        }

        //Inventory Full
        if (!itemAdded){
            Debug.Log("Inventory Full - Item Not Added");
        }

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

        var PokemonsJSON = JSON.Parse(JSONString)["Pokemons"];
        int N = PokemonsJSON.Count;
        Debug.Log("Genetaring Pokemon from JSON, N = " + N);
                
        for (int i=0; i<N; i++){    
            Debug.Log("Spawn Pokemon ");
            float pos_x = -100f;
            float pos_y = -100f;
            var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
            Pokemon.GetComponent<PokemonObject>().type = PokemonsJSON[i]["type"];
            Pokemon.GetComponent<PokemonObject>().pokemon_name = PokemonsJSON[i]["name"];
            Pokemon.GetComponent<PokemonObject>().color = PokemonsJSON[i]["color"];
            Pokemon.GetComponent<PokemonObject>().position_x = pos_x;
            Pokemon.GetComponent<PokemonObject>().position_y = pos_y;

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