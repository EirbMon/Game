using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("MainScene");      
        
        //NetworkManager.singleton.StartClient();
        //NetworkManager.singleton.StartServer();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
