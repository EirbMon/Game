using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnterInside : MonoBehaviour
{
    public LeavingInside exit;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        DresserController controller = other.GetComponent<DresserController>();

        if (controller != null)
        {
            float pos_x = exit.transform.position.x;
            float pos_y = exit.transform.position.y;
            controller.Teleport(pos_x , pos_y + 1);
        }
    }
}
