using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnterInside : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other)
    {
        DresserController controller = other.GetComponent<DresserController>();

        if (controller != null)
        {
            controller.Teleport(-56.5f,-2.00f);
        }
    }
}
