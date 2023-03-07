using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int health_max = 50;
    [SerializeField] int health;

    // Start is called before the first frame update
    void Start()
    {
        health = health_max; // THE ENEMY STARTS WITH A FULL HEALTH
    }

    // TAKING DAMAGE
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) // WHEN THE ENEMY HAS NO HEALTH, HE DIES
        {
            Die();
        }
    }
    
    // DYING
    void Die()
    {
        this.enabled = false; // DEACTIVATE THIS SCRIPT
        GetComponent<BoxCollider2D>().enabled = false; // DEACTIVATE THE BOX COLLIDER 2D
        GameObject.Find("Player").GetComponent<Controller_2D>().gauge += 5;
    }
}
