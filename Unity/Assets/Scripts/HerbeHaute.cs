using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class HerbeHaute: NetworkBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Haute herbe contact");   
        if (Random.Range(0,15) == 5){
            Debug.Log("Find Fight");   
            other.GetComponent<DresserController>().isInCombat = true;
        }
    }

}
