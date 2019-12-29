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

    public GameObject EirbmonPodium = null;
    public GameObject DresserPodium = null;

    public GameObject[] MyEirbmonsList = new GameObject[6];
    public int currentEirbmon = 0;
    public int nbMyEirbmon = 0;

    public string JSONString = null;
    public string EirbmonString = null;
    public string EnnemyEirbmonString = null;

    public bool waiting_react_response = false;
    public bool isPNJ = false;
    public bool lockRound = false;
    bool dev;

    public int currentSelection;

    [Space(10)]
    [Header("Main")]
    public TextMeshProUGUI bag;
    private string bagT;


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
    public TextMeshProUGUI Eirbmon1;
    private string Eirbmon1T;
    public TextMeshProUGUI Eirbmon2;
    private string Eirbmon2T;
    public TextMeshProUGUI Eirbmon3;
    private string Eirbmon3T;
    public TextMeshProUGUI Eirbmon4;
    private string Eirbmon4T;
    public TextMeshProUGUI Eirbmon5;
    private string Eirbmon5T;
    public TextMeshProUGUI Eirbmon6;
    private string Eirbmon6T;
    public TextMeshProUGUI Retour;


    private void Start()
    {
        
        waiting_react_response = true;

        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        EirbmonString = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().MyEirbmons;

        InitiateEirbmon();
    }

    private void Update()
    {

        if (waiting_react_response)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
 
            HealAllEirbmon();
        }

    }

    public void HealAllEirbmon()
    {


        for (int i = 0; i < nbMyEirbmon; i++)
        {

            MyEirbmonsList[i].GetComponent<PokemonObject>().FullHeal();

        }
    }

    public void InitiateEirbmon()
    {

        var PokemonsJSON = JSON.Parse(EirbmonString);
        int N = 0;
        if (PokemonsJSON != null)
            N = PokemonsJSON.Count;

        nbMyEirbmon = N;

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
