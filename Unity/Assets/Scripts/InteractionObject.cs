using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{

    public bool inventory; //If true: object can be stored in inventory


    public void DoInteraction(string name)
    {
        Debug.Log("Item added to inventory: " + name);
        gameObject.SetActive(false);
    }

}