﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MainMenu : MonoBehaviour
{
    AudioSource m_MyAudioSource;
    float m_MySliderValue;


    void Start()
    {    
        Screen.SetResolution(Screen.height*5/4*1/2, Screen.height*1/2, false);
    }

    public void PlayGame()
    {
        Debug.Log("Play v1.0");
        SceneManager.LoadScene("MainScene"); 
       // SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));     
        
        //NetworkManager.singleton.StartClient();
        //NetworkManager.singleton.StartServer();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    void OnGUI()
    {
        //m_MySliderValue = GUI.HorizontalSlider(new Rect(25, 25, 200, 60), m_MySliderValue, 0.0F, 1.0F);
        //m_MyAudioSource.volume = m_MySliderValue;
    }
}
