using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{

    public bool inventory; //If true: object can be stored in inventory


    public void DoInteraction(string name)
    {
        Debug.Log("J'ENVOIE: " + name);
        gameObject.SetActive(false);
    }

}