using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingInside : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.Teleport(-2.39f,1.50f);
            Debug.Log("Leaving the building");
        }
    }
}
