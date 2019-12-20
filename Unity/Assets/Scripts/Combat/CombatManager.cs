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

    public GameObject[] MyEirbmonsList = new GameObject[6];
    public int currentPokemon = 0;
    public GameObject[] EnnemyPokemonList = new GameObject[6];
    public int currentEnnemyPokemon = 0;
    public int nbEnnemyPokemon = 0;
    public int nbInLifePokemon = 0;
    public int nbMyPokemon = 0;

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
    bool dev;

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

        PokemonString = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().MyEirbmons;
        EirbmonSkills = GameObject.Find("GameManager").GetComponent<GameManager>().EirbmonSkills;
        dev = GameObject.Find("GameManager").GetComponent<GameManager>().dev;
        
        // DEV - Text Brut sans REACT
        if (dev)
            EirbmonSkills = "[{\"_id\": \"5df10f5c1c9d440000336b57\",\"id\": 0,\"name\": \"Vive Attack\",\"damage\": 22,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df10fa41c9d440000336b58\",\"id\": 1,\"name\": \"Eclair\",\"damage\": 15,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df10fe31c9d440000336b59\",\"id\": 2,\"name\": \"Hate\",\"damage\": 10,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df10ff91c9d440000336b5a\",\"id\": 3,\"name\": \"UltraLaser\",\"damage\": 51,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5dd00d12f1521b52940476fc\",\"id\": 4,\"name\": \"Surf\",\"pp\": 25,\"damage\": 30,\"field\": \"telecom\",\"__v\": 0},{\"_id\": \"5df1106f1c9d440000336b5d\",\"id\": 5,\"name\": \"Trempette\",\"damage\": 10,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df110581c9d440000336b5c\",\"id\": 6,\"name\": \"Bismillah\",\"damage\": 35,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df110871c9d440000336b5e\",\"id\": 7,\"name\": \"Gros Yeux\",\"damage\": 0,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df110a61c9d440000336b5f\",\"id\": 8,\"name\": \"Lance-Flamme\",\"damage\": 15,\"field\": \"telecom\",\"pp\": 20},{\"_id\": \"5df110c11c9d440000336b61\",\"id\": 9,\"name\": \"Brasse\",\"damage\": 30,\"field\": \"telecom\",\"pp\": 20}]";

        string ennemyPNJ = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().ennemyPNJ;
        isPNJ = (ennemyPNJ != "null");

        if (!isPNJ){

            // DEV - Text Brut sans REACT
            if (dev){
              string JSONString2 = "[{\"skills_id\":[],\"force\":0,\"xp\":0,\"available\":false,\"_id\":\"5df577c5b9998c72a6838783\",\"idInBlockchain\":3,\"__v\":0,\"field\":\"Elec\",\"hp\":66,\"lvl\":0,\"name\":\"Carapuce\",\"owner_id\":\"0x48bbceca684cde0646b787769d30d9fa38927e28\",\"type\":\"Carapuce\"}]";
              GenerateOrphelin(JSONString2);
            }
            else 
                GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("combat_pokemon");
        }
        if (isPNJ){
            string EirbmonPNJ = GameObject.Find(ennemyPNJ).GetComponent<EnnemyPNJ>().EirbmonPNJ;
            GameObject.Find("Dresser(Local)").GetComponent<DresserController>().waiting_react_response = false;
            GenerateOrphelin(EirbmonPNJ);
        }   
        
        InitiateEirbmon();
        IChooseYou(0);


        
        
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
                            float rrand = Random.Range(0,Mathf.RoundToInt(100*EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().health/EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().max_health));
                            if (rrand <= 20)
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
                        if (nbMyPokemon > 0 && currentSelection != currentPokemon+1){
                            IChooseYou(currentSelection-1);
                            lockRound = true;
                            ChangeMenu(CombatMenu.Main);
                            StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
                        }
                        break;
                    case 2:
                        if (nbMyPokemon > 1 && currentSelection != currentPokemon+1){
                            IChooseYou(currentSelection-1);
                            lockRound = true;
                            ChangeMenu(CombatMenu.Main);
                            StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
                        }
                        break;
                    case 3:
                        if (nbMyPokemon > 2 && currentSelection != currentPokemon+1){
                        IChooseYou(currentSelection-1);
                        lockRound = true;
                        ChangeMenu(CombatMenu.Main);
                        StartCoroutine(EnnemyAttack(EnnemyPokemonList[currentEnnemyPokemon], MyEirbmonsList[currentPokemon], -1));
                        }
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

        if (MyEirbmonsList[currentPokemon].GetComponent<PokemonObject>().health <= 0){
            side.SetText("Your Eirbmon is dead, you can't escape with a Zombie. You need to choose another Eirbmon in your bag first.");
            return; 
        }
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

        //Debug.Log(AttackerPokemon.GetComponent<PokemonObject>().type + " attaque " + DefenderPokemon.GetComponent<PokemonObject>().type);


        if (DefenderPokemon.GetComponent<PokemonObject>().health > 0){
            DefenderPokemon.GetComponent<PokemonObject>().TakeDamage(damage);    
            SetHealthBar(DefenderPokemon,heathbar_mode);
            StartCoroutine(AnimationAttack(0.2f, AttackerPokemon, 1.5f*mode));
            StartCoroutine(AnimationClignote(0.05f, DefenderPokemon));
        }    
              
        if (DefenderPokemon.GetComponent<PokemonObject>().health <= 0){
            DefenderPokemon.transform.Rotate(Vector3.forward * -90);

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

        if (MyEirbmonsList[currentPokemon].GetComponent<PokemonObject>().health <= 0){
            side.SetText("Did you really think you can catch an Eirbmon while yours is dead ? Nup Nup Nup !");
            return; 
        }

        side.SetText(" Too bad ! You failed to catch the " + EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().type + " ! Attack it so it can be easier for you to catch it !");
    }

    public void CatchPokemon(){

        if (MyEirbmonsList[currentPokemon].GetComponent<PokemonObject>().health <= 0){
            side.SetText("Did you really think you can catch an Eirbmon while yours is dead ? Nup Nup Nup !");
            return; 
        }

        side.SetText(" Congratulations ! You catch sucessfuly the " + EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().type + " !");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));

        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().waiting_react_response = true;
        if (!dev)
            GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("catch_pokemon");

        // DEV
        if (dev)
            GameObject.Find("Dresser(Local)").GetComponent<DresserController>().CatchPokemon(EnnemyPokemonString);
        // FIN DU DEV

        EnnemyPokemonList[currentEnnemyPokemon].transform.Rotate (Vector3.forward * -90);
        StartCoroutine(EndFight());
    }

    public void GenerateOrphelin (string JSONString2) {

        waiting_react_response = false;
        EnnemyPokemonString = JSONString2;
        InitiateEnnemyEirbmon(EnnemyPokemonString);

    }

    public void InitiateEirbmon(){

        var PokemonsJSON = JSON.Parse(PokemonString);
        int N = 0;
        if (PokemonsJSON != null)
            N = PokemonsJSON.Count;

        nbMyPokemon = N;
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

    public void InitiateEnnemyEirbmon(string EnnemyPokemonString){

        try{  
            var PokemonsJSON = JSON.Parse(EnnemyPokemonString);
            int N = 0;
            if (PokemonsJSON != null){
                N = PokemonsJSON.Count;
                nbEnnemyPokemon = PokemonsJSON.Count;
            }

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
        }
        catch{
            Debug.LogError("Erreur GenerateOrphelin: l'Eirbmon orphelin envoyé possède un mauvais type ou format incorrecte. Voici ce que Unity recoit: " + EnnemyPokemonString);
            StartCoroutine(EndFight());
        }

        currentEnnemyPokemon = 0;
        EnnemyIChooseYou(0);
        
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

        }catch{
            Debug.LogError("User cannot choose an Eirbmon for the fight (Did you have at least 1 Eirbmon ?)");
            StartCoroutine(EndFight());
            return;
        }

        try{
            var skills_id = MyEirbmonsList[currentPokemon].GetComponent<PokemonObject>().skills_id;
            var SkillsJSON = JSON.Parse(EirbmonSkills);
            
            skill1T = SkillsJSON[skills_id[0]]["name"];
            skill2T = SkillsJSON[skills_id[1]]["name"];
            skill3T = SkillsJSON[skills_id[2]]["name"];

            currentSkillName[0] = skill1T;
            currentSkillName[1] = skill2T;
            currentSkillName[2] = skill3T;

            currentSkillDamage[0] = SkillsJSON[skills_id[0]]["damage"];
            currentSkillDamage[1] = SkillsJSON[skills_id[1]]["damage"];
            currentSkillDamage[2] = SkillsJSON[skills_id[2]]["damage"];

            SetHealthBar(MyEirbmonsList[currentPokemon], "Dresser");
        }catch{
            Debug.LogError("User cannot instantiate the Eirbmon skills. Is the Skill List Database loaded ?");
            StartCoroutine(EndFight());
            return;
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

        }
        catch{
            Debug.LogError("Ennemy cannot choose an Eirbmon for the fight (Did the ennemy have at least 1 Eirbmon ?)");
            StartCoroutine(EndFight());
            return;
        }

        try{
            var skills_id = EnnemyPokemonList[currentEnnemyPokemon].GetComponent<PokemonObject>().skills_id;
            var SkillsJSON = JSON.Parse(EirbmonSkills);

            currentEnnemySkillName[0] = SkillsJSON[skills_id[0]]["name"];
            currentEnnemySkillName[1] = SkillsJSON[skills_id[1]]["name"];
            currentEnnemySkillName[2] = SkillsJSON[skills_id[2]]["name"];

            currentEnnemySkillDamage[0] = SkillsJSON[skills_id[0]]["damage"];
            currentEnnemySkillDamage[1] = SkillsJSON[skills_id[1]]["damage"];
            currentEnnemySkillDamage[2] = SkillsJSON[skills_id[2]]["damage"];

            SetHealthBar(EnnemyPokemonList[currentEnnemyPokemon], "Ennemy");
        }
        catch{
            Debug.LogError("Ennemy cannot instantiate the skills. Is the Skill List Database loaded ?");
            StartCoroutine(EndFight());
            return;
        }

    }     

    IEnumerator EndFight()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().LeaveCombat();
        GameObject.Find("Dresser(Local)").GetComponent<DresserController>().ennemyPNJ = "null";

        var PokemonsJSON = JSON.Parse(PokemonString);
        int N = 0;
        if (PokemonsJSON != null)
            N = PokemonsJSON.Count;

        string EirbmonsString = "{ \"message\": \"end_combat\", \"length\": " + N + ", \"pokemons\": [";

        for (int i = 0; i<N; i++){
            EirbmonsString = EirbmonsString + MyEirbmonsList[i].GetComponent<PokemonObject>().ConvertToString();
            if (i != N-1)
                EirbmonsString = EirbmonsString + ", ";
        }
        EirbmonsString = EirbmonsString + "]}";
        Debug.Log(EirbmonsString);

        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("end_combat");
        GameObject.Find("GameManager").GetComponent<GameManager>().SendMessageToReact("EirbmonsString");

        SceneManager.UnloadSceneAsync("CombatScene");
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
        side.SetText("Sorry, but you lost. Come back with better Eirbmons! You can check the store to get more powerful Eirbmon!");
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(EndFight());
    }

        IEnumerator WinFight()
    {
        int nb_level = 10;
        var PokemonsJSON = JSON.Parse(PokemonString);
        int N = 0;
        if (PokemonsJSON != null)
            N = PokemonsJSON.Count;

        for (int i = 0; i<N; i++){
            nb_level = 10;
            if (isPNJ)
                nb_level = 33;
            MyEirbmonsList[i].GetComponent<PokemonObject>().level = nb_level + MyEirbmonsList[i].GetComponent<PokemonObject>().level;
        }
        side.SetText("Congratulations ! You win the fight, you're a real dresser ! All your Eirbmons gained " + nb_level + " levels. If they reached lv100, they can evolve.");
        yield return new WaitForSeconds(1.1f);
        StartCoroutine(EndFight());
    }
    
}
