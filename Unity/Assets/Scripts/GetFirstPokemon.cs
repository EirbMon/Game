using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFirstPokemon : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null)
        {
            Debug.Log("JAPPUIS SUR X LOL");
        }
    }

}