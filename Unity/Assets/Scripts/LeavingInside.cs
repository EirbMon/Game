using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingInside : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        DresserController controller = other.GetComponent<DresserController>();

        if (controller != null)
        {
            controller.Teleport(-2.39f,1.50f);
        }
    }
}
