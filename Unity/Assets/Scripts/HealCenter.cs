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

public class HealCenter : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DoInteraction(string message);

    public GameManager manager = null;

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




    void Start()
    {
        waiting_react_response = true;
        Debug.Log("heyhey");
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("Salut" + GameObject.Find("Dresser(Local)").GetComponent<DresserController>());
        
        PokemonString = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().MyEirbmons;
        

        InitiateEirbmon();
    }

    void Update()
    {

        //if (waiting_react_response)
          //  return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
 
            HealAllEirbmon();
        }

    }

    public void HealAllEirbmon()
    {


        for (int i = 0; i < nbMyPokemon; i++)
        {

            MyEirbmonsList[i].GetComponent<PokemonObject>().FullHeal();

        }
    }

    public void InitiateEirbmon()
    {

        var PokemonsJSON = JSON.Parse(PokemonString);
        int N = 0;
        if (PokemonsJSON != null)
            N = PokemonsJSON.Count;

        nbMyPokemon = N;

        for (int i = 0; i < N; i++)
        {
            var pokemon_position = DresserPodium.transform.position;
            var pokemon_prefab = Resources.Load(PokemonsJSON[i]["type"], typeof(GameObject));
            GameObject Pokemon = (GameObject)Instantiate(pokemon_prefab, pokemon_position, Quaternion.identity) as GameObject;

            Pokemon.GetComponent<PokemonObject>().Initiate(PokemonsJSON[i]);
            Pokemon.transform.localScale += new Vector3(4f, 4f, 0f);
            Pokemon.SetActive(false);

            MyEirbmonsList[i] = Pokemon;
        }

    }
}
