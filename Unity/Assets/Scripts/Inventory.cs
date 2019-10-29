using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inventory : MonoBehaviour
{

    public GameObject[] inventory = new GameObject[10];
    

    // La fonction AddItem est appellé dans "PlayerInteract.cs" lorsque le player appuit sur E devant un Pokémon typé inventaire. 
     public void AddItem(GameObject item){

        bool itemAdded = false;

        // Add item at the last available slot.
        for (int i = 0; i<inventory.Length; i++){
            if (inventory[i] == null){
                inventory[i] = item;
                itemAdded = true;
                
                string type = item.GetComponent<PokemonObject>().type;
                string name = item.GetComponent<PokemonObject>().pokemon_name;
                string color = item.GetComponent<PokemonObject>().color;
                string message = "{\"type\":" + "\"" + type + "\","  + "\"name\":" + "\"" + name + "\"," + "\"color\":" + "\"" + color + "\"}";
                item.SendMessage("SendPokemonToReact",message);
                break;
            }
        }

        //Inventory Full
        if (!itemAdded){
            Debug.Log("Inventory Full - Item Not Added");
        }

    }
}
