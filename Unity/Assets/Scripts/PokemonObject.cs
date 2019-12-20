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
    public int idInBlockchain = 9999;
    public string itemType = "pokemon";
    public string name = "Valerian";
    public string type = "Pikachu";
    public string color = "red";
    public float max_health = 100;
    public int level = 0;
    public float health = 100;
    public float exp = 0;
    public bool visible = true;

    public int[] skills_id = new int[3];

    void Start(){
        health = max_health;
    }

    public void Deactivate(bool visible){
        this.gameObject.SetActive(visible); 
    }

    public void TakeDamage(float damage){
        this.health = this.health - damage;
    }

    public void increaseExp(float gain){
        this.exp = this.exp + gain;
        if (this.exp >= 100){
            levelUp();
            this.exp = this.exp - 100;
        }
        //Debug.Log("Exp = " + this.exp + " & Level = " + this.level);
    }

    public void levelUp(){
        this.level = this.level + 1;
    }

    public void Initiate (JSONNode PokemonJSON){
        this.idInBlockchain = PokemonJSON["idInBlockchain"];
        this.type = PokemonJSON["type"];
        this.name = PokemonJSON["name"];
        this.color = PokemonJSON["color"];
        this.max_health = PokemonJSON["hp"];
        this.health = PokemonJSON["current_hp"];
        if (PokemonJSON["current_hp"] == null)
            this.health = this.max_health;
        this.level = PokemonJSON["lvl"];
        this.exp = PokemonJSON["xp"];

        int N = PokemonJSON["skills_id"].Count;
        for (int i=0; i<N; i++){
            this.skills_id[i] = PokemonJSON["skills_id"][i];
        }
        
    }

    public string ConvertToString(){
        string message = "{\"idInBlockchain\": " + this.idInBlockchain + ", "  + "\"type\": " + "\"" + this.type + "\", "  + "\"name\": " + "\"" + this.name + "\", " + "\"lvl\": " + this.level + ", " + "\"exp\": " + this.exp + ", " + "\"hp\": " + this.health + "}";
        return message;
    }



}
