﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DresserController : NetworkBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    
    public int health {  get { return currentHealth; }}
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;
    public bool isInCombat = false;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    Rigidbody2D rigidbody2d;

    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (!isLocalPlayer) 
        {
        this.transform.Find("CM vcam1").gameObject.SetActive(false);
        this.transform.Find("InventoryCanvas").gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer){
            return;
        }

        if (isInCombat){
            Debug.Log("Start Combat Scene");
            SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive);   
            isInCombat = false;
        }

        if (SceneManager.GetSceneByName("CombatScene").isLoaded){
            return;
        }


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
                        
        Vector2 move = new Vector2(horizontal, vertical);
                
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
                
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
                
        Vector2 position = rigidbody2d.position;
                
        position = position + move * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    public void Teleport(float x, float y)
    {
        transform.position = new Vector2(x, y);
        rigidbody2d.MovePosition(transform.position);
    }

    public void CombatMode(bool mode){
        isInCombat = mode;

    }
}