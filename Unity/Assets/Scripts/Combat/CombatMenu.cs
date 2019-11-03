using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatMenu : MonoBehaviour
{

    public void RunAwayCombat()
    {
        Debug.Log("Run Away");
        SceneManager.UnloadSceneAsync("CombatScene");
    }
}
