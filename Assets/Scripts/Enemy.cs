using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public GameObject deathEffect;
    public GameObject bullet;
    public Transform bulletPos;
    
    private GameObject player;
    private float timer ;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update ()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        

        if(distance < 10)
        {
            timer += Time.deltaTime;

                if(timer > 2)
            {
                timer = 0;
                Shoot();
            }
        }
    }

    void Shoot ()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    public void TakeDamage (int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
