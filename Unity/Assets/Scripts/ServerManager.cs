using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Globalization;
using UnityEngine.Networking;


public class ServerManager : NetworkBehaviour
{
    // Start is called before the first frame update
    public override void OnStartServer()
    {     
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        string JSONString = "{\"Pokemons\":[{\"type\":\"Pikachu\",\"name\":\"PikaPika\",\"color\":\"yellow\",\"position_x\":\"-56.5\",\"position_y\":\"3.6\"},{\"type\":\"Carapuce\",\"name\":\"CaraCara\",\"color\":\"blue\",\"position_x\":\"-57.5\",\"position_y\":\"3.6\"},{\"type\":\"Salameche\",\"name\":\"SalaSala\",\"color\":\"red\",\"position_x\":\"-55.5\",\"position_y\":\"3.6\"}]}";
        Debug.Log("Server Start");
        gameManager.GenerateFirstPokemon(JSONString);

    }

}
