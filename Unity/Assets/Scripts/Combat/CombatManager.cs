using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Text;
using SimpleJSON;
using System.Globalization;
using TMPro;

public class CombatManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DoInteraction(string message);

    public GameObject Pokemon = null;
    public GameObject PokemonPodium = null;

    public GameObject DresserPodium = null;

    public string JSONString = null;

    public bool pokemonRecieved = false;
    bool first_time = true;

    public CombatMenu currentMenu;

    public int currentSelection;

    [Space(10)]
    [Header("Main")]
    public GameObject MainMenu;
    public GameObject SideInfo;
    public TextMeshProUGUI fight;
    private string fightT;
    public TextMeshProUGUI bag;
    private string bagT;
    public TextMeshProUGUI catchp;
    private string catchpT;
    public TextMeshProUGUI run;
    public TextMeshProUGUI side;
    private string runT;

    [Space(10)]
    [Header("Skills")]
    public GameObject SkillsMenu;
    public GameObject SkillsInfo;
    public TextMeshProUGUI skill1;
    public TextMeshProUGUI skill2;
    public TextMeshProUGUI skill3;
    public TextMeshProUGUI skill4;
    private string skill1T;
    private string skill2T;
    private string skill3T;
    private string skill4T;
    public TextMeshProUGUI PP;
    public TextMeshProUGUI ptype;

    [Space(10)]
    [Header("EnnemyInfo")]
    public GameObject EnnemyInfo;

    public TextMeshProUGUI ennemy_name;
    public TextMeshProUGUI ennemy_maxhp;    
    public TextMeshProUGUI ennemy_hp;
    public TextMeshProUGUI ennemy_level;

    [Space(10)]
    [Header("DresserInfo")]
    public GameObject DresserInfo;
    public TextMeshProUGUI dresser_name;
    public TextMeshProUGUI dresser_maxhp;
    public TextMeshProUGUI dresser_hp;
    public TextMeshProUGUI dresser_level;

    [Space(10)]
    [Header("Info")]
    public GameObject InfoMenu;
    public TextMeshProUGUI infotext;


    void Start(){
 
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("CombatScene"));
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("combat_pokemon");

        //JSONString = "[{\"skills_id\": [1,7,32],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert le pikachu\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,7,32],\"_id\": \"5dd0571370fc0849c41dde87\",\"type\": \"Pikachu\",\"name\": \"Gerard le pikachu\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T20:07:47.401Z\",\"updated_date\": \"2019-11-16T20:07:47.401Z\",\"__v\": 0}]";
        //GenerateWildPokemon(JSONString);

        
    }

    void Update(){

        if (!pokemonRecieved)
            return ;

        if (pokemonRecieved && first_time){

            first_time = false;
            skill1T = skill1.text;
            skill2T = skill2.text;
            skill3T = skill3.text;
            skill4T = skill4.text;
            ennemy_maxhp.SetText("{0}",Pokemon.GetComponent<PokemonObject>().max_health );
            ennemy_name.SetText( Pokemon.GetComponent<PokemonObject>().type );
            ennemy_level.SetText("{0}", Pokemon.GetComponent<PokemonObject>().level );
            ennemy_hp.SetText("{0}", Pokemon.GetComponent<PokemonObject>().health );
            side.SetText(" A wild " + Pokemon.GetComponent<PokemonObject>().type + " has appeared ! ");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)){
            if(currentSelection < 4)
            currentSelection++;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)){
            if(currentSelection > 0)
            currentSelection--;
        }

        if (Input.GetKeyDown(KeyCode.Return)){

            switch(currentMenu){
            case CombatMenu.Fight:
                switch(currentSelection){
                    case 1:
                        Fight();
                        break;
                    case 2:
                        Fight();
                        break;
                    case 3:
                        Fight();
                        break;
                    case 4:
                        ChangeMenu(CombatMenu.Main);
                        break;                
                }
                break;

            case CombatMenu.Main:
                switch(currentSelection){
                    case 1:
                        ChangeMenu(CombatMenu.Fight);
                        break;
                    case 2:
                        break;
                    case 3:
                        CatchPokemon();
                        break;
                    case 4:
                        RunAwayCombat();
                        break;           
                }
                break;

            }
        }

        if (currentSelection == 0)
            currentSelection = 1;


        switch(currentMenu){
            case CombatMenu.Fight:
                switch(currentSelection){
                    case 1:
                        skill1.SetText("> " + skill1T);
                        skill2.SetText(skill2T);
                        skill3.SetText(skill3T);
                        skill4.SetText(skill4T);
                        break;
                    case 2:
                        skill1.SetText(skill1T);
                        skill2.SetText("> " + skill2T);
                        skill3.SetText(skill3T);
                        skill4.SetText(skill4T);
                        break;
                    case 3:
                        skill1.SetText(skill1T);
                        skill2.SetText(skill2T);
                        skill3.SetText("> " + skill3T);
                        skill4.SetText(skill4T);
                        break;
                    case 4:
                        skill1.SetText(skill1T);
                        skill2.SetText(skill2T);
                        skill3.SetText(skill3T);
                        skill4.SetText("> " + skill4T);
                        break;                
                }
                break;

            case CombatMenu.Main:
                switch(currentSelection){
                    case 1:
                        fight.SetText("> FIGHT");
                        bag.SetText("BAG");
                        catchp.SetText("CATCH");
                        run.SetText("RUN");
                        break;
                    case 2:
                        fight.SetText("FIGHT");
                        bag.SetText("> BAG");
                        catchp.SetText("CATCH");
                        run.SetText("RUN");
                        break;
                    case 3:
                        fight.SetText("FIGHT");
                        bag.SetText("BAG");
                        catchp.SetText("> CATCH");
                        run.SetText("RUN");
                        break;
                    case 4:
                        fight.SetText("FIGHT");
                        bag.SetText("BAG");
                        catchp.SetText("CATCH");
                        run.SetText("> RUN");
                        break;           
                }
                break;

            }

    }

    public enum CombatMenu{
        Main,
        Side,
        Bag,
        Fight,
        Info
    }

    public void ChangeMenu(CombatMenu m){
        currentMenu = m;
        currentSelection = 1;
        switch(m){
            case CombatMenu.Main:
                MainMenu.gameObject.SetActive(true);
                SideInfo.gameObject.SetActive(true);
                SkillsMenu.gameObject.SetActive(false);
                SkillsInfo.gameObject.SetActive(false);
                InfoMenu.gameObject.SetActive(false);
                break;

            case CombatMenu.Fight:
                MainMenu.gameObject.SetActive(false);
                SideInfo.gameObject.SetActive(false);
                SkillsMenu.gameObject.SetActive(true);
                SkillsInfo.gameObject.SetActive(true);
                InfoMenu.gameObject.SetActive(false);
                break;

            case CombatMenu.Info:
                MainMenu.gameObject.SetActive(false);
                SideInfo.gameObject.SetActive(false);
                SkillsMenu.gameObject.SetActive(false);
                SkillsInfo.gameObject.SetActive(false);
                InfoMenu.gameObject.SetActive(true);
                break;
        }
    }


    public void RunAwayCombat(){
        side.SetText(" Run away has worked ! You're escaping the fight.");

        StartCoroutine(EndFight());
    }

    public void GoFightMenu(){
        MainMenu.gameObject.SetActive(false);
        SideInfo.gameObject.SetActive(false);
        SkillsMenu.gameObject.SetActive(true);
        SkillsInfo.gameObject.SetActive(true);
        InfoMenu.gameObject.SetActive(false);

    }

    public void GoMainMenu(){
        MainMenu.gameObject.SetActive(true);
        SideInfo.gameObject.SetActive(true);
        SkillsMenu.gameObject.SetActive(false);
        SkillsInfo.gameObject.SetActive(false);
        InfoMenu.gameObject.SetActive(false);

    }

    public void Fight(){

        float damage = 25;

        if (Pokemon.GetComponent<PokemonObject>().health > 0)
            Pokemon.GetComponent<PokemonObject>().TakeDamage(damage);        
              
        if (Pokemon.GetComponent<PokemonObject>().health <= 0){
            Pokemon.transform.Rotate (Vector3.forward * -90);
            //Pokemon.GetComponent<PokemonObject>().increaseExp(25);
            StartCoroutine(EndFight());
        }

        float lost_health = Pokemon.GetComponent<PokemonObject>().health;
        float pourcentage = damage / Pokemon.GetComponent<PokemonObject>().max_health;
        float lost_healthbar = pourcentage * GameObject.Find("EnnemyHealthBar").GetComponent<RectTransform>().rect.width;

        ennemy_hp.SetText("{0}", lost_health );
        GameObject.Find("EnnemyHealthBackground").GetComponent<RectTransform>().offsetMax += new Vector2(-lost_healthbar, -0);
    }

    public void CatchPokemon(){

        side.SetText(" Congratulations ! You catch sucessfuly the " + Pokemon.GetComponent<PokemonObject>().type + " !");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));

        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("catch_pokemon");
        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().CatchPokemon(JSONString);

        Pokemon.transform.Rotate (Vector3.forward * -90);
        StartCoroutine(EndFight());
    }

    public void GenerateWildPokemon (string JSONString2) {

        JSONString = JSONString2;

        var PokemonsJSON = JSON.Parse(JSONString);

        var pokemon_position = PokemonPodium.transform.position;
        var pokemon_prefab = Resources.Load(PokemonsJSON[0]["type"], typeof(GameObject));

        Pokemon = (GameObject)Instantiate(pokemon_prefab, pokemon_position, Quaternion.identity) as GameObject;
        Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[0]);

        //Pokemon.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        Pokemon.transform.localScale += new Vector3(4f, 4f, 0f);
        pokemonRecieved = true;

    }

    IEnumerator EndFight()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.UnloadSceneAsync("CombatScene");
        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().LeaveCombat();


    }
}
