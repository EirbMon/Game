using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
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

    GameManager manager = null;
    GameObject[] MyEirbmonsList = new GameObject[6];
    bool inHealArea = false;


    void Start()
    {
        //manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //PokemonString = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().MyEirbmons;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        inHealArea = true;     
    }

    void OnTriggerExit2D(Collider2D other)
    {
        inHealArea = false;    
    }

    void Update()
    {
        if (inHealArea)  // Input.GetKeyDown(KeyCode.Return)
        {
            HealAllEirbmon();
        }

    }

    public void HealAllEirbmon()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        MyEirbmonsList = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().inventory.inventory;
        int N = GameObject.Find("Dresser(Local)").GetComponent<DresserController>().inventory.N;

        Debug.Log("Inventory number of object: N");

        for (int i = 0; i < N; i++)
        {
            Debug.Log("Healing Eirbmon from Unity n°" + i);
            try{
            MyEirbmonsList[i].GetComponent<PokemonObject>().FullHeal();
            } catch {
                Debug.Log("Error when healing number" + i);
            }
        }

        manager.SendMessageToReact(manager.FormatMessage("heal", N, MyEirbmonsList));
    }
}
