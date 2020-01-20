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
    public int N = 0;
    public Button[] InventoryButtons = new Button[6];    
    GameManager gameManager;

    [DllImport("__Internal")]
    private static extern void DoInteraction(string message);

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
                N++;
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
                InventoryButtons[i].image.rectTransform.localScale -= new Vector3(1f, 0f, 0f);
                break;
            }
        }
    }

    public void RemoveAllItem(){
        for (int i = 0; i<inventory.Length; i++){
            if (inventory[i] != null){
                Destroy(inventory[i]);
                InventoryButtons[i].image.rectTransform.localScale -= new Vector3(1f, 0f, 0f);
                inventory[i] = null;
                InventoryButtons[i].image.overrideSprite = null;
            }
        }
    }
}