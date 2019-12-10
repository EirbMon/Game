using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Globalization;
using UnityEngine.Networking;


public class ServerManager : NetworkBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    public override void OnStartServer()
    {     
        Application.targetFrameRate = 60;
        //string JSONString = "[{\"type\":\"Pikachu\",\"name\":\"PikaPika\",\"color\":\"yellow\",\"position_x\":\"-56.5\",\"position_y\":\"3.6\"},{\"type\":\"Carapuce\",\"name\":\"CaraCara\",\"color\":\"blue\",\"position_x\":\"-57.5\",\"position_y\":\"3.6\"},{\"type\":\"Salameche\",\"name\":\"SalaSala\",\"color\":\"red\",\"position_x\":\"-55.5\",\"position_y\":\"3.6\"}]";
        //gameManager.SendMessageToReact("starter_pokemon");
        //GenerateFirstPokemon(JSONString);
        
    }

    public void GenerateFirstPokemon (string JSONString) {

        var PokemonsJSON = JSON.Parse(JSONString);
        int N = PokemonsJSON.Count;
        Debug.Log("Genetaring " + N + " Server Pokemon");
                
        for (int i=0; i<N; i++){    
            float pos_x = float.Parse(PokemonsJSON[i]["position_x"],CultureInfo.InvariantCulture.NumberFormat);
            float pos_y = float.Parse(PokemonsJSON[i]["position_y"],CultureInfo.InvariantCulture.NumberFormat);
            var Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
            Pokemon.name = "StarterPokemon"+i;
            Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);
            NetworkServer.Spawn(Pokemon);
        }
    }

}
