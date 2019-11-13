using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Networking;
using SimpleJSON;
public class PokemonObject : NetworkBehaviour
{
    public bool inventory; //If true: object can be stored in inventory
    public string itemType = "pokemon";
    public string pokemon_name = "Valerian";
    public string type = "Pikachu";
    public string color = "red";
    public float max_health = 100;

    public int level = 0;

    public float health = 100;
    public float position_x = 0.0f;
    public float position_y = 0.0f;

    [SyncVar(hook="Deactivate")]
    public bool visible = true;

    [DllImport("__Internal")]
    private static extern void DoInteraction(string message);

    void Start(){
        health = max_health;
    }

    // La fonction SendPokemonToReact est appellé dans "Inventory.cs". 
    public void SendPokemonToReact(){

        string message = "{\"type\":" + "\"" + this.type + "\","  + "\"name\":" + "\"" + this.name + "\"," + "\"color\":" + "\"" + this.color + "\"}";
        Debug.Log("SendReactMessage: " + message);
        
        try{
            DoInteraction(message);
        }
        catch{
            Debug.Log("Do interaction fail");
        }
    }

    public void Deactivate(bool visible){
        this.gameObject.SetActive(visible); 
    }

    public void TakeDamage(float damage){
        this.health = this.health - damage;
        Debug.Log("Health = " + this.health);
    }

    public void Initiate (JSONNode PokemonJSON){
        this.type = PokemonJSON["type"];
        this.pokemon_name = PokemonJSON["name"];
        this.color = PokemonJSON["color"];
        this.health = max_health;
    }

}
