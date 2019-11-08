﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Text;
using SimpleJSON;
using System.Globalization;

public class CombatMenu : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DoInteraction(string message);

    void Start(){
        SendMessageToReact();
    }

    // La fonction SendPokemonToReact est appellé dans "Inventory.cs". 
    public void SendMessageToReact(){

        string message = "{\"type\":\"1\"}";
        Debug.Log("Message: " + message);
        
        try{
            DoInteraction(message);
        }
        catch{
            Debug.Log("Do interaction fail");
        }
    }

    public void RunAwayCombat(){
        Debug.Log("Run Away");
        SceneManager.UnloadSceneAsync("CombatScene");
    }

    public void FightCombat(){
        Debug.Log("Fight");
    }

    public void CatchPokemon(){
        Debug.Log("Catch");
    }

    public void GenerateWildPokemon (string JSONString) {

        var PokemonsJSON = JSON.Parse(JSONString)["Pokemons"];
        Debug.Log("Get Random Orphelin Pokemon ");

        float pos_x = 0f;
        float pos_y = 0f;

        var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[0]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
        Pokemon.GetComponent<PokemonObject>().type = PokemonsJSON[0]["type"];
        Pokemon.GetComponent<PokemonObject>().pokemon_name = PokemonsJSON[0]["name"];
        Pokemon.GetComponent<PokemonObject>().color = PokemonsJSON[0]["color"];
        Pokemon.GetComponent<PokemonObject>().position_x = pos_x;
        Pokemon.GetComponent<PokemonObject>().position_y = pos_y;
    }
}
