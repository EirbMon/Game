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
        Debug.Log("Start: version 1.5");
        SendMessageToReact("eirbmon_skills");
    }

    public void SendMessageToReact(string message){
        try{
            DoInteraction(message);
        }
        catch{
            Debug.LogError("Communication Unity -> React has failed for: " + message);
            if (message == "eirbmon_skills")
                return;
            GameObject.Find("Dresser(Local)").GetComponent<DresserController>().waiting_react_response = false;
        }
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
