﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Globalization;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;


public class GameManager : NetworkBehaviour
{


    [DllImport("__Internal")]
    private static extern void DoInteraction(string message);


    void Start(){
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
        Debug.Log("version 1.1");
    }

    public void SendMessageToReact(string message){
        try{
            DoInteraction(message);
        }
        catch{
            Debug.Log("DoInteraction fail");
        }
    }

    public void GenerateFirstPokemon (string JSONString) {

        var PokemonsJSON = JSON.Parse(JSONString);
        int N = PokemonsJSON.Count;
        Debug.Log("Genetaring " + N + " Server Pokemon");
                
        for (int i=0; i<N; i++){    
            float pos_x = float.Parse(PokemonsJSON[i]["position_x"],CultureInfo.InvariantCulture.NumberFormat);
            float pos_y = float.Parse(PokemonsJSON[i]["position_y"],CultureInfo.InvariantCulture.NumberFormat);
            var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
            Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);
            NetworkServer.Spawn(Pokemon);

        }

    }

    public void SelectLocalPlayer (NetworkInstanceId playerId) {
        ClientScene.FindLocalObject(playerId);
    }

}
