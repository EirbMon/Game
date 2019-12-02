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


    void Start(){
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
        Debug.Log("Start: version 1.3");
    }

    public void SendMessageToReact(string message){
        try{
            DoInteraction(message);
        }
        catch{
            Debug.Log("DoInteraction fail");
        }
    }
    
}
