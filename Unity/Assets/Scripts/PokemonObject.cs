using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
public class PokemonObject : MonoBehaviour
{
    public bool inventory; //If true: object can be stored in inventory
    public string itemType = "pokemon";
    public string pokemon_name = "Valerian";
    public string type = "Pikachu";
    public string color = "red";
    public float position_x = 0.0f;
    public float position_y = 0.0f;

    [DllImport("__Internal")]
    private static extern void DoInteraction(string message);

    // La fonction SendPokemonToReact est appellé dans "Inventory.cs". 
    public void SendPokemonToReact(string message){

        Debug.Log("Message: "+message);
        gameObject.SetActive(false);
        
        try{
            DoInteraction(message);
        }
        catch{
            Debug.Log("Do interaction fail");
        }

    }
}
