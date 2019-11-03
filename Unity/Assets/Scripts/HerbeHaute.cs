using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class HerbeHaute: NetworkBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("1111");
        if (Random.Range(0,10) == 5){
            Debug.Log("2222");   
            other.GetComponent<DresserController>().isInCombat = true;
        }


        //StartCombat();
        //if (isServer)
        //    RpcStartCombat();
        //else
        //    CmdStartCombat();


    }

    void StartCombat(){
        Debug.Log("HB");
        if (Random.Range(0,10) == 5){
            SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive);                
        }
    }

    [Command]
     void CmdStartCombat()
     {
         //Apply it to all other clients
         Debug.Log("CMD HB - Apply it to all other clients");
         StartCombat();
         RpcStartCombat();
     }
 
     [ClientRpc]
     void RpcStartCombat()
     {
        if (isLocalPlayer)
            return;
         Debug.Log("RPC HB - Apply only to local client");
         StartCombat();
     }
}
