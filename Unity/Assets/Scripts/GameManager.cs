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
    }

    // Start is called before the first frame update
    public override void OnStartServer()
    {     
        string JSONString = "{\"Pokemons\":[{\"type\":\"Pikachu\",\"name\":\"PikaPika\",\"color\":\"yellow\",\"position_x\":\"-56.5\",\"position_y\":\"3.6\"},{\"type\":\"Carapuce\",\"name\":\"CaraCara\",\"color\":\"blue\",\"position_x\":\"-57.5\",\"position_y\":\"3.6\"},{\"type\":\"Salameche\",\"name\":\"SalaSala\",\"color\":\"red\",\"position_x\":\"-55.5\",\"position_y\":\"3.6\"}]}";
        Debug.Log("Server Start");
        GenerateFirstPokemon(JSONString);
    }

    public void GenerateFirstPokemon (string JSONString) {

        var PokemonsJSON = JSON.Parse(JSONString)["Pokemons"];
        int N = PokemonsJSON.Count;
        Debug.Log("Genetaring Pokemon from JSON, N = " + N);
                
        for (int i=0; i<N; i++){    
            Debug.Log("Spawn Pokemon ");
            float pos_x = float.Parse(PokemonsJSON[i]["position_x"],CultureInfo.InvariantCulture.NumberFormat);
            float pos_y = float.Parse(PokemonsJSON[i]["position_y"],CultureInfo.InvariantCulture.NumberFormat);
            var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
            Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);
            NetworkServer.Spawn(Pokemon);

        }
    }

    public void SendMessageToReact(string message){
        Debug.Log("SendReactMessage: " + message);
        try{
            DoInteraction(message);
        }
        catch{
            Debug.Log("Do interaction fail");
        }
    }

}
