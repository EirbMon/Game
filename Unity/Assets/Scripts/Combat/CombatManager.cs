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

    public GameObject[] MyEirbmonsList = new GameObject[3];
    public int currentPokemon = 0;
    public GameObject EnnemyPokemon = null;

    public string JSONString = null;
    public string PokemonString = null;

    public string EirbmonSkills = null;

    public int[] currentSkillDamage = new int[3];
    
    public bool waiting_react_response = false;
    public bool lockRound = false;

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

        waiting_react_response = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("combat_pokemon");

        PokemonString = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().MyEirbmons;
        EirbmonSkills = GameObject.Find("GameManager").GetComponent<GameManager>().EirbmonSkills;
        InitiateEirbmon();
        IChooseYou(0);

        // DEV - Text Brut sans REACT
        //string JSONString2 = "[{\"skills_id\": [1,2,3],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,4,5],\"_id\": \"5dd0571370fc0849c41dde87\",\"type\": \"Carapuce\",\"name\": \"Gerard\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T20:07:47.401Z\",\"updated_date\": \"2019-11-16T20:07:47.401Z\",\"__v\": 0}]";
        //GenerateOrphelin(JSONString2);
        // A CONSERVE UNIQUEMENT EN DEV

        
        
    }

    void Update(){

        if (lockRound)
            return;

        if (waiting_react_response)
            return;

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
                        Fight(MyEirbmonsList[currentPokemon], EnnemyPokemon, 1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemon, MyEirbmonsList[currentPokemon], -1));
                        break;
                    case 2:
                        Fight(MyEirbmonsList[currentPokemon], EnnemyPokemon, 1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemon, MyEirbmonsList[currentPokemon], -1));
                        break;
                    case 3:
                        Fight(MyEirbmonsList[currentPokemon], EnnemyPokemon, 1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemon, MyEirbmonsList[currentPokemon], -1));
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
                        if (Random.Range(0,Mathf.Round(EnnemyPokemon.GetComponent<PokemonObject>().health/30)) == 0)
                            CatchPokemon();
                        else{
                            FailCatchPokemon();
                            ChangeMenu(CombatMenu.Main);
                            StartCoroutine(EnnemyAttack(EnnemyPokemon, MyEirbmonsList[currentPokemon], -1));
                        }
                        break;
                    case 4:
                        RunAwayCombat();
                        break;           
                }
                break;

            case CombatMenu.Bag:
                switch(currentSelection){
                    case 1:
                        IChooseYou(currentSelection-1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemon, MyEirbmonsList[currentPokemon], -1));
                        break;
                    case 2:
                        IChooseYou(currentSelection-1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemon, MyEirbmonsList[currentPokemon], -1));
                        break;
                    case 3:
                        IChooseYou(currentSelection-1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemon, MyEirbmonsList[currentPokemon], -1));
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
                        skill4.SetText("Retour");
                        break;
                    case 2:
                        skill1.SetText(skill1T);
                        skill2.SetText("> " + skill2T);
                        skill3.SetText(skill3T);
                        skill4.SetText("Retour");
                        break;
                    case 3:
                        skill1.SetText(skill1T);
                        skill2.SetText(skill2T);
                        skill3.SetText("> " + skill3T);
                        skill4.SetText("Retour");
                        break;
                    case 4:
                        skill1.SetText(skill1T);
                        skill2.SetText(skill2T);
                        skill3.SetText(skill3T);
                        skill4.SetText("> Retour");
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
        side.SetText(" Run away has worked ! You have escaped the fight.");
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
            damage = 12;
            heathbar_mode = "Dresser";
        }

        if (DefenderPokemon.GetComponent<PokemonObject>().health > 0){
            DefenderPokemon.GetComponent<PokemonObject>().TakeDamage(damage);    
            StartCoroutine(AnimationAttack(0.2f, AttackerPokemon, 1.5f*mode));
            StartCoroutine(AnimationDefense(0.05f, DefenderPokemon));
            SetHealthBar(DefenderPokemon,heathbar_mode);
        }    
              
        if (DefenderPokemon.GetComponent<PokemonObject>().health <= 0){
            DefenderPokemon.transform.Rotate(Vector3.forward * -90);
            //Pokemon.GetComponent<PokemonObject>().increaseExp(25);
            StartCoroutine(EndFight());
        };        

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
    public void FailCatchPokemon(){
        side.SetText(" Too bad ! You failed to catch the " + EnnemyPokemon.GetComponent<PokemonObject>().type + " !");
    }

    public void CatchPokemon(){

        side.SetText(" Congratulations ! You catch sucessfuly the " + EnnemyPokemon.GetComponent<PokemonObject>().type + " !");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));

        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().waiting_react_response = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("catch_pokemon");

        // DEV
        //GameObject.Find("Dresser(Local)").GetComponent<DresserController>().CatchPokemon(JSONString);
        // FIN DU DEV

        EnnemyPokemon.transform.Rotate (Vector3.forward * -90);
        StartCoroutine(EndFight());
    }

    public void GenerateOrphelin (string JSONString2) {

        waiting_react_response = false;

        try{

        JSONString = JSONString2;
        var PokemonsJSON = JSON.Parse(JSONString);
        var pokemon_position = PokemonPodium.transform.position;
        var pokemon_prefab = Resources.Load(PokemonsJSON[0]["type"], typeof(GameObject));

        EnnemyPokemon = (GameObject)Instantiate(pokemon_prefab, pokemon_position, Quaternion.identity) as GameObject;
        EnnemyPokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[0]);
        EnnemyPokemon.transform.localScale += new Vector3(4f, 4f, 0f);

        ennemy_maxhp.SetText("{0}",EnnemyPokemon.GetComponent<PokemonObject>().max_health );
        ennemy_name.SetText( EnnemyPokemon.GetComponent<PokemonObject>().type );
        ennemy_level.SetText("{0}", EnnemyPokemon.GetComponent<PokemonObject>().level );
        ennemy_hp.SetText("{0}", EnnemyPokemon.GetComponent<PokemonObject>().health );
        side.SetText(" A wild " + EnnemyPokemon.GetComponent<PokemonObject>().type + " has appeared ! ");

        }

        catch{
            Debug.LogError("Erreur GenerateOrphelin: l'Eirbmon orphelin envoyé possède un mauvais type ou format incorrecte. Voici ce que Unity recoit: " + JSONString2);
            RunAwayCombat();
        }

    }

    public void InitiateEirbmon(){



        var PokemonsJSON = JSON.Parse(PokemonString);
        int N = 0;
        if (PokemonsJSON != null)
            N = PokemonsJSON.Count;
        if (N>3)
            N = 3;

        for (int i = 0; i<N; i++){
            var pokemon_position = DresserPodium.transform.position;
            var pokemon_prefab = Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject));
            GameObject Pokemon = (GameObject)Instantiate(pokemon_prefab, pokemon_position, Quaternion.identity) as GameObject;

            Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);
            Pokemon.transform.localScale += new Vector3(4f, 4f, 0f);
            Pokemon.SetActive(false);    

            MyEirbmonsList[i] = Pokemon;       
     
            if (i==0)         
                Pokemon1T = PokemonsJSON[0]["name"];
            if (i==1)
                Pokemon2T = PokemonsJSON[1]["name"];
            if (i==2)
                Pokemon3T = PokemonsJSON[2]["name"];
        }

        MyEirbmonsList[currentPokemon] = MyEirbmonsList[0];
        
        
    }

    

    public void IChooseYou (int i) {
        try{
            MyEirbmonsList[currentPokemon].SetActive(false);

            currentPokemon = i;
            MyEirbmonsList[currentPokemon].name = "MyEirbmonsList[currentPokemon]";
            MyEirbmonsList[currentPokemon].SetActive(true);

            dresser_maxhp.SetText("{0}",MyEirbmonsList[currentPokemon].GetComponent<PokemonObject>().max_health );
            dresser_name.SetText( MyEirbmonsList[currentPokemon].GetComponent<PokemonObject>().type );
            dresser_level.SetText("{0}", MyEirbmonsList[currentPokemon].GetComponent<PokemonObject>().level );
            dresser_hp.SetText("{0}", MyEirbmonsList[currentPokemon].GetComponent<PokemonObject>().health );

            var PokemonsJSON = JSON.Parse(PokemonString);
            int skill1_id = PokemonsJSON[i]["skills_id"][0];
            int skill2_id = PokemonsJSON[i]["skills_id"][1];
            int skill3_id = PokemonsJSON[i]["skills_id"][2];
            
            var SkillsJSON = JSON.Parse(EirbmonSkills);
            skill1T = SkillsJSON[skill1_id]["name"];
            skill2T = SkillsJSON[skill2_id]["name"];
            skill3T = SkillsJSON[skill3_id]["name"];

            currentSkillDamage[0] = SkillsJSON[skill1_id]["damage"];
            currentSkillDamage[1] = SkillsJSON[skill2_id]["damage"];
            currentSkillDamage[2] = SkillsJSON[skill3_id]["damage"];

            SetHealthBar(MyEirbmonsList[currentPokemon], "Dresser");
        }
        catch{
            Debug.LogError("User cannot choose an Eirbmon for the fight (Did you have at least 1 Eirbmon ?)");
            RunAwayCombat();
        }

    }

    IEnumerator EndFight()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.UnloadSceneAsync("CombatScene");
        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().LeaveCombat();
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("end_combat");
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
        lockRound = false;

    }
}
