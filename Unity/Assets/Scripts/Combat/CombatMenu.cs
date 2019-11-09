using System.Collections;
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

    public GameObject Pokemon = null;

     private float timer = 0;
 
    private float timerMax = 0;

    private bool enter = false;

    void Start(){
        SendMessageToReact();
        string JSONString = "{\"Pokemons\":[{\"type\":\"Roucoul\",\"name\":\"PikaPika\",\"color\":\"yellow\",\"position_x\":\"-56.5\",\"position_y\":\"3.6\"}]}";
        GenerateWildPokemon(JSONString);
    }

    void Update(){


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

        if (Pokemon.GetComponent<PokemonObject>().health > 0)
            Pokemon.GetComponent<PokemonObject>().TakeDamage(25);        
              
        if (Pokemon.GetComponent<PokemonObject>().health <= 0){
            StartCoroutine(EndFight());
        }
    }

    public void CatchPokemon(){
        Debug.Log("Catch");
        SceneManager.UnloadSceneAsync("CombatScene");
        GameObject.Find("Dresser(Clone)").GetComponent<DresserController>().catch_pokemon = true;
    }

    public void GenerateWildPokemon (string JSONString) {

        GameObject.Find("Dresser(Clone)").GetComponent<DresserController>().CatchedPokemon = JSONString;
        var PokemonsJSON = JSON.Parse(JSONString)["Pokemons"];
        Debug.Log("Get Random Orphelin Pokemon ");

        float pos_x = 300f;
        float pos_y = 100f;
        Debug.Log(PokemonsJSON[0]);
        Pokemon = (GameObject)Instantiate(Resources.Load(PokemonsJSON[0]["type"], typeof(GameObject)), new Vector2(pos_x, pos_y), Quaternion.identity) as GameObject;
        Pokemon.GetComponent<PokemonObject>().type = PokemonsJSON[0]["type"];
        Pokemon.GetComponent<PokemonObject>().pokemon_name = PokemonsJSON[0]["name"];
        Pokemon.GetComponent<PokemonObject>().color = PokemonsJSON[0]["color"];
        Pokemon.GetComponent<PokemonObject>().position_x = pos_x;
        Pokemon.GetComponent<PokemonObject>().position_y = pos_y;
        Pokemon.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        Pokemon.transform.localScale += new Vector3(300f, 400f, 0f);
    }

    IEnumerator EndFight()
    {
        Pokemon.transform.Rotate (Vector3.forward * -90);
        yield return new WaitForSeconds(2.5f);
        RunAwayCombat();

    }
}
