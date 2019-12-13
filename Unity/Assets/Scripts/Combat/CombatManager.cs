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
    public GameObject[] EnnemyPokemonList = new GameObject[3];
    public int currentEnnemyPokemon = 0;
    public int nbEnnemyPokemon = 0;
    public int nbInLifePokemon = 0;

    public string JSONString = null;
    public string PokemonString = null;
    public string EnnemyPokemonString = null;

    public string EirbmonSkills = null;

    public int[] currentSkillDamage = new int[3];
    public string[] currentSkillName = new string[3];
    public int[] currentEnnemySkillDamage = new int[3];
    public string[] currentEnnemySkillName = new string[3];

    public bool waiting_react_response = false;
    public bool isPNJ = false;
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

        string ennemyPNJ = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().ennemyPNJ;
        isPNJ = (ennemyPNJ != "null");

        if (!isPNJ){
            GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("combat_pokemon");
            // DEV - Text Brut sans REACT
            string JSONString2 = "[{\"skills_id\": [1,2,3],\"_id\": \"5dd01a65da355e20acb195b1\",\"type\": \"Pikachu\",\"name\": \"Robert\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T15:48:53.021Z\",\"updated_date\": \"2019-11-16T15:48:53.021Z\",\"__v\": 0},{\"skills_id\": [1,4,5],\"_id\": \"5dd0571370fc0849c41dde87\",\"type\": \"Carapuce\",\"name\": \"Gerard\",\"owner_id\": \"xxx_userOwnerId_xxx\",\"hp\": 110,\"field\": \"telecom\",\"force\": 5,\"xp\": 25,\"lvl\": 7,\"created_date\": \"2019-11-16T20:07:47.401Z\",\"updated_date\": \"2019-11-16T20:07:47.401Z\",\"__v\": 0}]";
            GenerateOrphelin(JSONString2);
        }
        if (isPNJ){
            string EirbmonPNJ = GameObject.Find(ennemyPNJ).GetComponent<EnnemyPNJ>().EirbmonPNJ;
            GameObject.Find("Dresser(Local)").GetComponent<DresserController>().waiting_react_response = false;
            GenerateOrphelin(EirbmonPNJ);
        }   

        PokemonString = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().MyEirbmons;
        EirbmonSkills = GameObject.Find("GameManager").GetComponent<GameManager>().EirbmonSkills;

        // DEV - Text Brut sans REACT
        EirbmonSkills = "[{\"_id\": \"5df10f5c1c9d440000336b57\",\"id\": 0,\"name\": \"Vive Attack\",\"damage\": 22,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df10fa41c9d440000336b58\",\"id\": 1,\"name\": \"Eclair\",\"damage\": 15,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df10fe31c9d440000336b59\",\"id\": 2,\"name\": \"Hate\",\"damage\": 10,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df10ff91c9d440000336b5a\",\"id\": 3,\"name\": \"UltraLaser\",\"damage\": 51,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5dd00d12f1521b52940476fc\",\"id\": 4,\"name\": \"Surf\",\"pp\": 25,\"damage\": 30,\"field\": \"telecom\",\"__v\": 0},{\"_id\": \"5df1106f1c9d440000336b5d\",\"id\": 5,\"name\": \"Trempette\",\"damage\": 10,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df110581c9d440000336b5c\",\"id\": 6,\"name\": \"Bismillah\",\"damage\": 35,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df110871c9d440000336b5e\",\"id\": 7,\"name\": \"Gros Yeux\",\"damage\": 0,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df110a61c9d440000336b5f\",\"id\": 8,\"name\": \"Lance-Flamme\",\"damage\": 15,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df110c11c9d440000336b61\",\"id\": 9,\"name\": \"Brasse\",\"damage\": 30,\"field\": \"telecom\",\"pp\": 20}]";
        
        
        InitiateEirbmon();
        IChooseYou(0);
        EnnemyIChooseYou(0);


        
        
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
                        Fight(MyEirbmonsList[currentPokemon], EnnemyPokemonList[currentEnnemyPokemon], 1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
                        break;
                    case 2:
                        Fight(MyEirbmonsList[currentPokemon], EnnemyPokemonList[currentEnnemyPokemon], 1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
                        break;
                    case 3:
                        Fight(MyEirbmonsList[currentPokemon], EnnemyPokemonList[currentEnnemyPokemon], 1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
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
                        if (isPNJ){
                            side.SetText("You can't catch an Eirbmon from a PNJ. Stealing is forbidden !");
                        }
                        else{
                            if (Random.Range(0,Mathf.Round(5*EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().health/EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().max_health)) == 0)
                                CatchPokemon();
                            else{
                                FailCatchPokemon();
                                ChangeMenu(CombatMenu.Main);
                                StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
                            }
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
                        StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
                        break;
                    case 2:
                        IChooseYou(currentSelection-1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
                        break;
                    case 3:
                        IChooseYou(currentSelection-1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
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
        if (Random.Range(0,2) == 0){
            side.SetText(" Run away has worked ! You have escaped the fight.");
            StartCoroutine(EndFight());
        }
        else{
            side.SetText(" Missed ! You didn't escape !");
            StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
        }
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

        float damage = currentSkillDamage[Random.Range(0,3)];
        string heathbar_mode = "Ennemy";

        if (AttackerPokemon.GetComponent<PokemonObject>().health <= 0 && mode > 0){
            side.SetText("Your Eirbmon is dead, so he can't attack. It's logic, no ? Why do you try..");
            return; 
        }

        if (DefenderPokemon.GetComponent<PokemonObject>().health <= 0 || AttackerPokemon.GetComponent<PokemonObject>().health <= 0){
            return;
        }


        if (mode < 0){
            int rand = Random.Range(0,3);
            damage = currentEnnemySkillDamage[rand];
            heathbar_mode = "Dresser";
            side.SetText(AttackerPokemon.GetComponent<PokemonObject>().type + " use " + currentEnnemySkillName[rand] + " !");
        }

        if (mode > 0){
            damage = currentSkillDamage[currentSelection-1];
            heathbar_mode = "Ennemy";
            side.SetText(AttackerPokemon.GetComponent<PokemonObject>().type + " use " + currentSkillName[currentSelection-1] + " !");
        }

        Debug.Log(AttackerPokemon.GetComponent<PokemonObject>().type + " attaque " + DefenderPokemon.GetComponent<PokemonObject>().type);


        if (DefenderPokemon.GetComponent<PokemonObject>().health > 0){
            DefenderPokemon.GetComponent<PokemonObject>().TakeDamage(damage);    
            SetHealthBar(DefenderPokemon,heathbar_mode);
            StartCoroutine(AnimationAttack(0.2f, AttackerPokemon, 1.5f*mode));
            StartCoroutine(AnimationClignote(0.05f, DefenderPokemon));
        }    
              
        if (DefenderPokemon.GetComponent<PokemonObject>().health <= 0){
            DefenderPokemon.transform.Rotate(Vector3.forward * -90);

            Debug.Log("current Ennemy n " + currentEnnemyPokemon);
            Debug.Log("nombtre max de Ennemy " + nbEnnemyPokemon);
            Debug.Log("nombre de Pokemon en vie " + nbInLifePokemon);
            if (mode > 0 && currentEnnemyPokemon < nbEnnemyPokemon -1)
                StartCoroutine(AnimationChangeEnnemyPokemon());
            else if (mode < 0 && nbInLifePokemon > 1)
                nbInLifePokemon = nbInLifePokemon -1;
            else if (mode < 0)
                StartCoroutine(LooseFight());
            else
                StartCoroutine(WinFight());

            return ;
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
        side.SetText(" Too bad ! You failed to catch the " + EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().type + " !");
    }

    public void CatchPokemon(){

        side.SetText(" Congratulations ! You catch sucessfuly the " + EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().type + " !");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));

        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().waiting_react_response = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("catch_pokemon");

        // DEV
        //GameObject.Find("Dresser(Local)").GetComponent<DresserController>().CatchPokemon(JSONString);
        // FIN DU DEV

        EnnemyPokemonList[currentEnnemyPokemon].transform.Rotate (Vector3.forward * -90);
        StartCoroutine(EndFight());
    }

    public void GenerateOrphelin (string JSONString2) {

        waiting_react_response = false;

        try{

        EnnemyPokemonString = JSONString2;
        InitiateEnnemyEirbmon();

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

        nbInLifePokemon = N;

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

    public void InitiateEnnemyEirbmon(){

        var PokemonsJSON = JSON.Parse(EnnemyPokemonString);
        int N = 0;
        if (PokemonsJSON != null){
            N = PokemonsJSON.Count;
            nbEnnemyPokemon = PokemonsJSON.Count;
        }
        if (N>3)
            N = 3;
        if (!isPNJ){
            N = 1;
            nbEnnemyPokemon = 1;
        }

        for (int i = 0; i<N; i++){
            var pokemon_position = PokemonPodium.transform.position;
            var pokemon_prefab = Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject));
            GameObject Pokemon = (GameObject)Instantiate(pokemon_prefab, pokemon_position, Quaternion.identity) as GameObject;

            Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);
            Pokemon.transform.localScale += new Vector3(4f, 4f, 0f);
            Pokemon.SetActive(false);    

            EnnemyPokemonList[i] = Pokemon;       
        }

        currentEnnemyPokemon = 0;
        
        
    }

    

    public void IChooseYou (int i) {
        try{
            MyEirbmonsList[currentPokemon].SetActive(false);

            currentPokemon = i;
            MyEirbmonsList[currentPokemon].name = "currentEirbmon";
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

            currentSkillName[0] = SkillsJSON[skill1_id]["name"];
            currentSkillName[1] = SkillsJSON[skill2_id]["name"];
            currentSkillName[2] = SkillsJSON[skill3_id]["name"];

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

    public void EnnemyIChooseYou (int i) {
        try{
            currentEnnemyPokemon = i;
            EnnemyPokemonList[currentEnnemyPokemon].name = "currentEnnemy";
            EnnemyPokemonList[currentEnnemyPokemon].SetActive(true);

            ennemy_maxhp.SetText("{0}",EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().max_health );
            ennemy_name.SetText( EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().type );
            ennemy_level.SetText("{0}", EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().level );
            ennemy_hp.SetText("{0}", EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().health );
            side.SetText(" A wild " + EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().type + " has appeared ! ");

            var PokemonsJSON = JSON.Parse(EnnemyPokemonString);
            int skill1_id = PokemonsJSON[i]["skills_id"][0];
            int skill2_id = PokemonsJSON[i]["skills_id"][1];
            int skill3_id = PokemonsJSON[i]["skills_id"][2];
        
            var SkillsJSON = JSON.Parse(EirbmonSkills);

            currentEnnemySkillName[0] = SkillsJSON[skill1_id]["name"];
            currentEnnemySkillName[1] = SkillsJSON[skill2_id]["name"];
            currentEnnemySkillName[2] = SkillsJSON[skill3_id]["name"];

            currentEnnemySkillDamage[0] = SkillsJSON[skill1_id]["damage"];
            currentEnnemySkillDamage[1] = SkillsJSON[skill2_id]["damage"];
            currentEnnemySkillDamage[2] = SkillsJSON[skill3_id]["damage"];

            SetHealthBar(EnnemyPokemonList[currentEnnemyPokemon], "Ennemy");
        }
        catch{
            Debug.LogError("Ennemy cannot choose an Eirbmon for the fight (Did you have at least 1 Eirbmon ?)");
            RunAwayCombat();
        }

    }     

    IEnumerator EndFight()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.UnloadSceneAsync("CombatScene");
        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().LeaveCombat();
        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().ennemyPNJ = "null";
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("end_combat");
    }

    IEnumerator AnimationAttack(float second, GameObject Pokemon, float position)
    {
        Pokemon.transform.position += new Vector3(position, position, 0f);
        yield return new WaitForSeconds(second);
        Pokemon.transform.position -= new Vector3(position, position, 0f);
    }

    IEnumerator AnimationChangeEnnemyPokemon()
    {
        int newEnnemy = currentEnnemyPokemon + 1;

        yield return new WaitForSeconds(1.0f);
        EnnemyPokemonList[currentEnnemyPokemon].SetActive(false);
        yield return new WaitForSeconds(0.6f);

        EnnemyIChooseYou(newEnnemy);
    }



    IEnumerator AnimationClignote(float second, GameObject Pokemon)
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
        if (nbEnnemyPokemon > 0){
        yield return new WaitForSeconds(1.0f);
        Fight(AttackerPokemon, DefenderPokemon, mode);
        lockRound = false;
        }
    }

        IEnumerator LooseFight()
    {
        side.SetText("SORRY, but you lost. Come back with better Eirbmons! You can check the store to get more powerful Eirbmon!");
        yield return new WaitForSeconds(1.1f);
        StartCoroutine(EndFight());
    }

        IEnumerator WinFight()
    {
        side.SetText("WOAW !!! Congratulations ! You win the fight, you're a real dresser ! Incredible !");
        yield return new WaitForSeconds(1.1f);
        StartCoroutine(EndFight());
    }
    
}
