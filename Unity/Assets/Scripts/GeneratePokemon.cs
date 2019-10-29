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
        // GET JSON from REACT //
        // JEAN, c'est ici qu'il faut changer le JSONString //
        // Le JSONString correspond aux JSON que tu envoies à partir de react. //  
        string JSONString = "{\"Pokemons\":[{\"type\":\"Pikachu\",\"name\":\"PikaPika\",\"color\":\"yellow\",\"position_x\":\"-56.5\",\"position_y\":\"4.1\"},{\"type\":\"Carapuce\",\"name\":\"CaraCara\",\"color\":\"blue\",\"position_x\":\"-57.44\",\"position_y\":\"4.2\"},{\"type\":\"Salameche\",\"name\":\"SalaSala\",\"color\":\"red\",\"position_x\":\"-55.5\",\"position_y\":\"4.1\"}]}";
        // LA LIGNE string JSONString = "..." DOIT ETRE REMPLACER PAR LA FONCTION DE RECEPTION DE MESSAGE REACT // 
        //

        
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

    // Update is called once per frame
    void Update()
    {
        
       
    }
}
