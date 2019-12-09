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

    public GameObject PokemonPodium = null;
    public GameObject DresserPodium = null;

    public GameObject ChoosenPokemon = null;
    public GameObject EnnemyPokemon = null;

    public string JSONString = null;
    public string MyEirbmons = null;

    const string TabSkills = "[{\"name\":\"VIVE ATTACK\",\"damage\":22},{\"name\":\"ECLAIR\",\"damage\":15},{\"name\":\"HATE\",\"damage\":10},{\"name\":\"ULTRALASER\",\"damage\":51},{\"name\":\"SURF\",\"damage\":30},{\"name\":\"TREMPETTE\",\"damage\":20},{\"name\":\"BISMILLAH\",\"damage\":35},{\"name\":\"GROS YEUX\",\"damage\":0},{\"name\":\"LANCE-FLAMME\",\"damage\":5}]";
    public int[] currentSkillDamage = new int[3];
    
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

    [Header("Bag")]
    public GameObject Bag;
    public TextMeshProUGUI Pokemon1;
    private string Pokemon1T;
    public TextMeshProUGUI Pokemon2;
    private string Pokemon2T;
    public TextMeshProUGUI Pokemon3;
    private string Pokemon3T;
    public TextMeshProUGUI Pokemon4;
    private string Pokemon4T;
    public TextMeshProUGUI Pokemon5;
    private string Pokemon5T;
    public TextMeshProUGUI Pokemon6;
    private string Pokemon6T;
    public TextMeshProUGUI Retour;


    void Start(){
 
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("CombatScene"));
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("combat_pokemon");

        //string MyEirbmons = "[{\"skills_id\": [1,7,32],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert le pikachu\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,7,32],\"_id\": \"5dd0571370fc0849c41dde87\",\"type\": \"Pikachu\",\"name\": \"Gerard le pikachu\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T20:07:47.401Z\",\"updated_date\": \"2019-11-16T20:07:47.401Z\",\"__v\": 0}]";
        string JSONString2 = "[{\"skills_id\": [1,7,32],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,7,32],\"_id\": \"5dd0571370fc0849c41dde87\",\"type\": \"Carapuce\",\"name\": \"Gerard\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T20:07:47.401Z\",\"updated_date\": \"2019-11-16T20:07:47.401Z\",\"__v\": 0}]";

        MyEirbmons = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().MyEirbmons;
        IChooseYou(MyEirbmons,0);
        GenerateWildPokemon(JSONString2);

        var PokemonsJSON = JSON.Parse(MyEirbmons);
        int N = PokemonsJSON.Count;
        skill4T = skill4.text;
        if (N>0)         
            Pokemon1T = PokemonsJSON[0]["name"];
        if (N>1)
            Pokemon2T = PokemonsJSON[1]["name"];
        if (N>2)
            Pokemon3T = PokemonsJSON[2]["name"];

        
    }

    void Update(){

        if (!pokemonRecieved)
            return ;

        if (pokemonRecieved && first_time){

            first_time = false;
            ennemy_maxhp.SetText("{0}",EnnemyPokemon.GetComponent<PokemonObject>().max_health );
            ennemy_name.SetText( EnnemyPokemon.GetComponent<PokemonObject>().type );
            ennemy_level.SetText("{0}", EnnemyPokemon.GetComponent<PokemonObject>().level );
            ennemy_hp.SetText("{0}", EnnemyPokemon.GetComponent<PokemonObject>().health );
            side.SetText(" A wild " + EnnemyPokemon.GetComponent<PokemonObject>().type + " has appeared ! ");
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
                        Fight(ChoosenPokemon, EnnemyPokemon, 1);
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemon, ChoosenPokemon, -1));
                        break;
                    case 2:
                        Fight(ChoosenPokemon, EnnemyPokemon, 1);
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemon, ChoosenPokemon, -1));
                        break;
                    case 3:
                        Fight(ChoosenPokemon, EnnemyPokemon, 1);
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemon, ChoosenPokemon, -1));
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
                        ChangeMenu(CombatMenu.Bag);
                        break;
                    case 3:
                        CatchPokemon();
                        break;
                    case 4:
                        RunAwayCombat();
                        break;           
                }
                break;

            case CombatMenu.Bag:
                switch(currentSelection){
                    case 1:
                        IChooseYou(MyEirbmons,0);
                        ChangeMenu(CombatMenu.Main);
                        break;
                    case 2:
                        IChooseYou(MyEirbmons,1);
                        ChangeMenu(CombatMenu.Main);
                        break;
                    case 3:
                        IChooseYou(MyEirbmons,2);
                        ChangeMenu(CombatMenu.Main);
                        break;
                     case 4:
                        ChangeMenu(CombatMenu.Main);
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

            case CombatMenu.Bag:
                switch(currentSelection){
                    case 1:
                        Pokemon1.SetText("> " + Pokemon1T);
                        Pokemon2.SetText(Pokemon2T);
                        Pokemon3.SetText(Pokemon3T);
                        Retour.SetText("Retour");
                        break;
                    case 2:
                        Pokemon1.SetText(Pokemon1T);
                        Pokemon2.SetText("> " + Pokemon2T);
                        Pokemon3.SetText(Pokemon3T);
                        Retour.SetText("Retour");
                        break;
                    case 3:
                        Pokemon1.SetText(Pokemon1T);
                        Pokemon2.SetText(Pokemon2T);
                        Pokemon3.SetText("> " + Pokemon3T);
                        Retour.SetText("Retour");
                        break;
                    case 4:
                        Pokemon1.SetText(Pokemon1T);
                        Pokemon2.SetText(Pokemon2T);
                        Pokemon3.SetText(Pokemon3T);
                        Retour.SetText(" > Retour");
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
                Bag.gameObject.SetActive(false);
                break;

            case CombatMenu.Fight:
                MainMenu.gameObject.SetActive(false);
                SideInfo.gameObject.SetActive(false);
                SkillsMenu.gameObject.SetActive(true);
                SkillsInfo.gameObject.SetActive(true);
                InfoMenu.gameObject.SetActive(false);
                Bag.gameObject.SetActive(false);
                break;

            case CombatMenu.Info:
                MainMenu.gameObject.SetActive(false);
                SideInfo.gameObject.SetActive(false);
                SkillsMenu.gameObject.SetActive(false);
                SkillsInfo.gameObject.SetActive(false);
                InfoMenu.gameObject.SetActive(true);
                Bag.gameObject.SetActive(false);
                break;

            case CombatMenu.Bag:
                MainMenu.gameObject.SetActive(true);
                SideInfo.gameObject.SetActive(true);
                SkillsMenu.gameObject.SetActive(false);
                SkillsInfo.gameObject.SetActive(false);
                InfoMenu.gameObject.SetActive(false);
                Bag.gameObject.SetActive(true);
                break;
        }
    }


    public void RunAwayCombat(){
        side.SetText(" Run away has worked ! You\"re escaping the fight.");

        StartCoroutine(EndFight());
    }

    public void GoFightMenu(){
        MainMenu.gameObject.SetActive(false);
        SideInfo.gameObject.SetActive(false);
        SkillsMenu.gameObject.SetActive(true);
        SkillsInfo.gameObject.SetActive(true);
        InfoMenu.gameObject.SetActive(false);
        Bag.gameObject.SetActive(false);
    }

    public void GoMainMenu(){
        MainMenu.gameObject.SetActive(true);
        SideInfo.gameObject.SetActive(true);
        SkillsMenu.gameObject.SetActive(false);
        SkillsInfo.gameObject.SetActive(false);
        InfoMenu.gameObject.SetActive(false);
        Bag.gameObject.SetActive(false);
    }

    public void GoBagMenu(){
        MainMenu.gameObject.SetActive(true);
        SideInfo.gameObject.SetActive(true);
        SkillsMenu.gameObject.SetActive(false);
        SkillsInfo.gameObject.SetActive(false);
        InfoMenu.gameObject.SetActive(false);
        Bag.gameObject.SetActive(true);
    }

    public void Fight(GameObject AttackerPokemon, GameObject DefenderPokemon, float mode){

        float damage = currentSkillDamage[currentSelection-1];
        string heathbar_mode = "Ennemy";

        if (mode < 0){
            damage = 25;
            heathbar_mode = "Dresser";
        }

        if (DefenderPokemon.GetComponent<PokemonObject>().health > 0)
            DefenderPokemon.GetComponent<PokemonObject>().TakeDamage(damage);        
              
        if (DefenderPokemon.GetComponent<PokemonObject>().health <= 0){
            DefenderPokemon.transform.Rotate(Vector3.forward * -90);
            //Pokemon.GetComponent<PokemonObject>().increaseExp(25);
            StartCoroutine(EndFight());
        };

        StartCoroutine(AnimationAttack(0.2f, AttackerPokemon, 1.5f*mode));
        StartCoroutine(AnimationDefense(0.05f, DefenderPokemon));

        SetHealthBar(DefenderPokemon,heathbar_mode);

       }

    public void SetHealthBar(GameObject Pokemon, string heathbar_mode){

        if (heathbar_mode == "Dresser")
            dresser_hp.SetText("{0}", Pokemon.GetComponent<PokemonObject>().health );
        else
            ennemy_hp.SetText("{0}", Pokemon.GetComponent<PokemonObject>().health );

        float pourcentage = 1-Pokemon.GetComponent<PokemonObject>().health / Pokemon.GetComponent<PokemonObject>().max_health;
        float lost_healthbar = pourcentage * GameObject.Find(heathbar_mode + "HealthBar").GetComponent<RectTransform>().rect.width;
        GameObject.Find(heathbar_mode+"HealthBackground").GetComponent<RectTransform>().offsetMax = new Vector2(-lost_healthbar, GameObject.Find(heathbar_mode+"HealthBackground").GetComponent<RectTransform>().offsetMax.y);

    }

    public void CatchPokemon(){

        side.SetText(" Congratulations ! You catch sucessfuly the " + EnnemyPokemon.GetComponent<PokemonObject>().type + " !");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));

        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("catch_pokemon");
        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().CatchPokemon(JSONString);

        EnnemyPokemon.transform.Rotate (Vector3.forward * -90);
        StartCoroutine(EndFight());
    }

    public void GenerateWildPokemon (string JSONString2) {

        JSONString = JSONString2;

        var PokemonsJSON = JSON.Parse(JSONString);

        var pokemon_position = PokemonPodium.transform.position;
        var pokemon_prefab = Resources.Load(PokemonsJSON[0]["type"], typeof(GameObject));

        EnnemyPokemon = (GameObject)Instantiate(pokemon_prefab, pokemon_position, Quaternion.identity) as GameObject;
        EnnemyPokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[0]);

        //Pokemon.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        EnnemyPokemon.transform.localScale += new Vector3(4f, 4f, 0f);
        pokemonRecieved = true;

    }

    

    public void IChooseYou (string MyEirbmons, int i) {

        Destroy(GameObject.Find("ChoosenPokemon"));

        var PokemonsJSON = JSON.Parse(MyEirbmons);
        var pokemon_position = DresserPodium.transform.position;
        var pokemon_prefab = Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject));

        ChoosenPokemon = (GameObject)Instantiate(pokemon_prefab, pokemon_position, Quaternion.identity) as GameObject;
        ChoosenPokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);

        ChoosenPokemon.name = "ChoosenPokemon";

        //Pokemon.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        ChoosenPokemon.transform.localScale += new Vector3(4f, 4f, 0f);
        
        dresser_maxhp.SetText("{0}",ChoosenPokemon.GetComponent<PokemonObject>().max_health );
        dresser_name.SetText( ChoosenPokemon.GetComponent<PokemonObject>().type );
        dresser_level.SetText("{0}", ChoosenPokemon.GetComponent<PokemonObject>().level );
        dresser_hp.SetText("{0}", ChoosenPokemon.GetComponent<PokemonObject>().health );

        var SkillsJSON = JSON.Parse(TabSkills);
        int N = SkillsJSON.Count;

        int skill1_id = PokemonsJSON[i]["skills_id"][0];
        int skill2_id = PokemonsJSON[i]["skills_id"][1];
        int skill3_id = PokemonsJSON[i]["skills_id"][2];
        skill1T = SkillsJSON[skill1_id]["name"];
        skill2T = SkillsJSON[skill2_id]["name"];
        skill3T = SkillsJSON[skill3_id]["name"];

        currentSkillDamage[0] = SkillsJSON[skill1_id]["damage"];
        currentSkillDamage[1] = SkillsJSON[skill2_id]["damage"];
        currentSkillDamage[2] = SkillsJSON[skill3_id]["damage"];

        SetHealthBar(ChoosenPokemon, "Dresser");

    }

    IEnumerator EndFight()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.UnloadSceneAsync("CombatScene");
        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().LeaveCombat();
    }

    IEnumerator AnimationAttack(float second, GameObject Pokemon, float position)
    {
        Pokemon.transform.position += new Vector3(position, position, 0f);
        yield return new WaitForSeconds(second);
        Pokemon.transform.position -= new Vector3(position, position, 0f);
    }

    IEnumerator AnimationDefense(float second, GameObject Pokemon)
    {
        Pokemon.SetActive(false);
        yield return new WaitForSeconds(second);
        Pokemon.SetActive(true);
        yield return new WaitForSeconds(second);
        Pokemon.SetActive(false);
        yield return new WaitForSeconds(second);
        Pokemon.SetActive(true);
        yield return new WaitForSeconds(second);
        Pokemon.SetActive(false);
        yield return new WaitForSeconds(second);
        Pokemon.SetActive(true);
        yield return new WaitForSeconds(second);
        Pokemon.SetActive(false);
        yield return new WaitForSeconds(second);
        Pokemon.SetActive(true);
    }

        IEnumerator EnnemyAttack(GameObject AttackerPokemon, GameObject DefenderPokemon, float mode)
    {
        yield return new WaitForSeconds(1.0f);
        Fight(AttackerPokemon, DefenderPokemon, mode);
    }
}
