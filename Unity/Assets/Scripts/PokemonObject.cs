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
    public float exp = 0;

    [SyncVar(hook="Deactivate")]
    public bool visible = true;

    void Start(){
        health = max_health;
    }

    public void Deactivate(bool visible){
        this.gameObject.SetActive(visible); 
    }

    public void TakeDamage(float damage){
        this.health = this.health - damage;
        Debug.Log("Health = " + this.health);
    }

    public void increaseExp(float gain){
        Debug.Log("TYPE POKEMON FIGHT: " + this.type);
        this.exp = this.exp + gain;
        if (this.exp >= 100){
            levelUp();
            this.exp = this.exp - 100;
        }
        Debug.Log("Exp = " + this.exp + " & Level = " + this.level);
    }

    public void levelUp(){
        this.level = this.level + 1;
    }

    public void Initiate (JSONNode PokemonJSON){
        this.type = PokemonJSON["type"];
        this.pokemon_name = PokemonJSON["name"];
        this.color = PokemonJSON["color"];
        this.max_health = PokemonJSON["max_health"];
        this.level = PokemonJSON["level"];
        this.exp = PokemonJSON["exp"];
    }

    public string ConvertToString(){
        string message = "{\"type\":" + "\"" + this.type + "\","  + "\"name\":" + "\"" + this.name + "\"," + "\"level\":" + "\"" + this.level + "\"}";
        return message;
    }



}
