﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DamageZone : MonoBehaviour
{

    void Start(){
        NetworkManager.singleton.StartClient();
        //NetworkManager.singleton.StartServer();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DresserController controller = other.GetComponent<DresserController >();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}