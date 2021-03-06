﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingInside : MonoBehaviour
{
    public EnterInside entry;

    void OnTriggerEnter2D(Collider2D other)
    {
        DresserController controller = other.GetComponent<DresserController>();

        if (controller != null)
        {
            float pos_x = entry.transform.position.x;
            float pos_y = entry.transform.position.y;
            controller.Teleport( pos_x , pos_y - 1);
        }
    }
}
