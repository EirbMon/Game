using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Globalization;
using UnityEngine.Networking;

public class GeneratePokemon : NetworkBehaviour
{
    
    // Start is called before the first frame update
    public override void OnStartServer()
    {     
        string JSONString = "{\"Pokemons\":[{\"type\":\"Pikachu\",\"name\":\"PikaPika\",\"color\":\"yellow\",\"position_x\":\"-56.5\",\"position_y\":\"3.6\"},{\"type\":\"Carapuce\",\"name\":\"CaraCara\",\"color\":\"blue\",\"position_x\":\"-57.5\",\"position_y\":\"3.6\"},{\"type\":\"Salameche\",\"name\":\"SalaSala\",\"color\":\"red\",\"position_x\":\"-55.5\",\"position_y\":\"3.6\"}]}";
        Debug.Log("Server Start");
        GenerateFirstPokemon(JSONString) ;
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
            Pokemon.GetComponent<PokemonObject>().type = PokemonsJSON[i]["type"];
            Pokemon.GetComponent<PokemonObject>().pokemon_name = PokemonsJSON[i]["name"];
            Pokemon.GetComponent<PokemonObject>().color = PokemonsJSON[i]["color"];
            Pokemon.GetComponent<PokemonObject>().position_x = pos_x;
            Pokemon.GetComponent<PokemonObject>().position_y = pos_y;
            // cf PokemonObject.cs 

            NetworkServer.Spawn(Pokemon);

        }
    }
}
