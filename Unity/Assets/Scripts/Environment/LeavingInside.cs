using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeavingInside : MonoBehaviour
{
    public EnterInside entry;
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

            float offsetX = controller.transform.position.x - this.transform.position.x; 
            float offsetY = controller.transform.position.y - this.transform.position.y;
            float pos_x = entry.transform.position.x;
            float pos_y = entry.transform.position.y;
            entry.GetComponent<EnterInside>().first = true;
            controller.Teleport(pos_x + offsetX, pos_y + offsetY);
        }
    }
}
