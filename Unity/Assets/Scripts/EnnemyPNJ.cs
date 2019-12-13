using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyPNJ : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;

    Rigidbody2D rigidbody2D;
    float timer;
    public bool busy;
    int direction = 1;
    public float timeDeactivate = 5.0f;
    float DeactivateTimer;
    public string EirbmonPNJ;
    bool isDeactivate = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isDeactivate){
            DeactivateTimer -= Time.deltaTime;
            if (DeactivateTimer < 0){
                isDeactivate = false;
                busy = false;
            }
            return;
        }

        if (busy)
            return ;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
        
        
        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
        }
 
        rigidbody2D.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            busy = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if (other.CompareTag("Player")){
            if (isDeactivate)
                return;          
            isDeactivate = true;
            DeactivateTimer = timeDeactivate;
        }
    }

}