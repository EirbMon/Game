using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public GameObject currentInterObj = null;
    public InteractionObject currentInterObjScript = null;
    public Inventory inventory;

    void Update()
    {
        if (Input.GetButtonDown ("Interact") && currentInterObj){    
            if (currentInterObjScript.inventory){
                inventory.AddItem(currentInterObj);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
            if (other.CompareTag("interactionobject")){
                Debug.Log(other.name);
                currentInterObj = other.gameObject;
                currentInterObjScript = currentInterObj.GetComponent <InteractionObject> ();
            }  
    }

        void OnTriggerExit2D(Collider2D other)
    {
            if (other.CompareTag("interactionobject")){
                if (other.gameObject == currentInterObj){
                    currentInterObj = null;
                }
            }
    }

}
