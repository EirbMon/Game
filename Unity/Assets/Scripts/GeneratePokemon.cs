using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Globalization;

public class GeneratePokemon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {     

        // Sur React, ajouter un truc du style :

        // onClickButton() {
        //    string JSONString = "{\"Pokemons\":[{\"type\":\"Pikachu\",\"name\":\"PikaPika\",\"color\":\"yellow\",\"position_x\":\"-56.5\",\"position_y\":\"4.1\"},{\"type\":\"Carapuce\",\"name\":\"CaraCara\",\"color\":\"blue\",\"position_x\":\"-57.44\",\"position_y\":\"4.2\"},{\"type\":\"Salameche\",\"name\":\"SalaSala\",\"color\":\"red\",\"position_x\":\"-55.5\",\"position_y\":\"4.1\"}]}";
        //    this.unityContent.send("GeneratePokemon", "GenerateFirstPokemon", JSONString);
        //}

        // Puis supprimer les 2 lignes suivantes (celle ci sont uniquement pour test) : 

        string JSONString = "{\"Pokemons\":[{\"type\":\"Pikachu\",\"name\":\"PikaPika\",\"color\":\"yellow\",\"position_x\":\"-56.5\",\"position_y\":\"4.1\"},{\"type\":\"Carapuce\",\"name\":\"CaraCara\",\"color\":\"blue\",\"position_x\":\"-57.44\",\"position_y\":\"4.2\"},{\"type\":\"Salameche\",\"name\":\"SalaSala\",\"color\":\"red\",\"position_x\":\"-55.5\",\"position_y\":\"4.1\"}]}";
        GenerateFirstPokemon(JSONString) ;

        // Puis Tester.     

        ////// 
    }

    public void GenerateFirstPokemon (string JSONString) {

        var PokemonsJSON = JSON.Parse(JSONString)["Pokemons"];
        int N = PokemonsJSON.Count;
        Debug.Log("Number of Pokemon Generated: " + N);
                
        for (int i=0; i<N; i++){    
            float pos_x = float.Parse(PokemonsJSON[i]["position_x"],CultureInfo.InvariantCulture.NumberFormat);
            float pos_y = float.Parse(PokemonsJSON[i]["position_y"],CultureInfo.InvariantCulture.NumberFormat);
            GameObject Pokemon = Instantiate(Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
            Pokemon.GetComponent<PokemonObject>().type = PokemonsJSON[i]["type"];
            Pokemon.GetComponent<PokemonObject>().pokemon_name = PokemonsJSON[i]["name"];
            Pokemon.GetComponent<PokemonObject>().color = PokemonsJSON[i]["color"];
            Pokemon.GetComponent<PokemonObject>().position_x = pos_x;
            Pokemon.GetComponent<PokemonObject>().position_y = pos_y;
            // cf PokemonObject.cs 
        }
    }
}
