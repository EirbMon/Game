using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnterInside : MonoBehaviour
{
    public LeavingInside exit;
    public bool first = false;
    public bool disabled = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        DresserController controller = other.GetComponent<DresserController>();

        if (controller != null && !disabled)
        {
            if (first){
                first = false;
                return;
            }

            float pos_x = exit.transform.position.x;
            float pos_y = exit.transform.position.y;
            exit.GetComponent<LeavingInside>().first = true;
            controller.Teleport(pos_x , pos_y);
        }
    }
}
