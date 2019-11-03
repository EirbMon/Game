using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInteract : NetworkBehaviour
{
    public GameObject currentInterObj = null;
    public PokemonObject currentInterObjScript = null;
    public Inventory inventory;
    

    void Update()
    {
        if (!isLocalPlayer)
             return;

        if (Input.GetButtonDown ("Interact") && currentInterObj){
            TakeItem();
            if (isServer)
                RpcTakeItem();
            else
                CmdTakeItem();
        }
        
    }

    [Command]
     void CmdTakeItem()
     {
         //Apply it to all other clients
         Debug.Log("CMD - Apply it to all other clients");
         TakeItem();
         RpcTakeItem();
     }
 
     [ClientRpc]
     void RpcTakeItem()
     {
         Debug.Log("RPC - Apply only to local client");
         if (isLocalPlayer)
             return;
         TakeItem();
     }

    void TakeItem(){
        if (currentInterObjScript.inventory){
                    inventory.AddItem(currentInterObj);
                    currentInterObjScript.visible = false;
                    currentInterObjScript.Deactivate(false);
                    currentInterObjScript.SendPokemonToReact();
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
            if (other.CompareTag("interactionobject")){
                currentInterObj = other.gameObject;
                currentInterObjScript = currentInterObj.GetComponent <PokemonObject> ();
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
