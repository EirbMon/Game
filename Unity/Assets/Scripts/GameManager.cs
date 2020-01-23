﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Globalization;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;


public class GameManager : NetworkBehaviour
{


    [DllImport("__Internal")]
    private static extern void DoInteraction(string message);
    public string EirbmonSkills = null;

    public bool dev = false;


    void Start(){

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
        Application.targetFrameRate = 300;
        Debug.Log("Start: version 1.7");
        SendMessageToReact(FormatMessage("eirbmon_skills"));
    }

    public void SendMessageToReact(string message){
        try{
            DoInteraction(message);
        }
        catch{
            Debug.LogError("Communication Unity -> React has failed for: " + message);
            if (message == "{ \"message\": \"eirbmon_skills\", \"length\": 0, \"pokemons\": []}")
                return;
            GameObject.Find("Dresser(Local)").GetComponent<DresserController>().waiting_react_response = false;
        }
    }

    public string FormatMessage(string message, int N=0, GameObject[] MyEirbmonsList=null){
        string EirbmonsString = "{ \"message\": \"" + message + "\", \"length\": " + N + ", \"pokemons\": [";

        for (int i = 0; i<N; i++){
            EirbmonsString = EirbmonsString + MyEirbmonsList[i].GetComponent<PokemonObject>().ConvertToString();
            if (i != N-1)
                EirbmonsString = EirbmonsString + ", ";
        }
        EirbmonsString = EirbmonsString + "]}";
        return EirbmonsString;
    }

    public void SetEirbmonSkills(string ReactJSON){
        try{
            EirbmonSkills = ReactJSON;
        }
        catch{
            Debug.LogError("Erreur, REACT n'a pas envoyé la liste des skills (Format string ? ie JSON.Stringify() ) " + ReactJSON);
        }
    }
    
}
