﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeavingInside : MonoBehaviour
{
    public EnterInside entry;
    public bool first = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        DresserController controller = other.GetComponent<DresserController>();

        if (controller != null)
        {
            if (first){
                first = false;
                return;
            }

            float pos_x = entry.transform.position.x;
            float pos_y = entry.transform.position.y;
            entry.GetComponent<EnterInside>().first = true;
            controller.Teleport(pos_x ,pos_y);
        }
    }
}
