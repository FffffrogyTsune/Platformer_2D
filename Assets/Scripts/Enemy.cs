using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health_max;
    [SerializeField] int health;

    public Rigidbody2D health_orb;
    public Rigidbody2D gauge_orb;

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
        for (int i = 0; i <= Random.Range(0, 2); i++)
        {
            Rigidbody2D H_orb = Instantiate(health_orb, transform.position, transform.rotation);
            H_orb.velocity = new Vector2(Random.Range(-10,10), 30);
        }

        for (int i = 0; i <= Random.Range(0, 2); i++)
        {
            Rigidbody2D G_orb = Instantiate(gauge_orb, transform.position, transform.rotation);
            G_orb.velocity = new Vector2(Random.Range(-10, 10), 30);
        }

        GetComponent<BoxCollider2D>().enabled = false; // DEACTIVATE THE BOX COLLIDER 2D
        this.enabled = false; // DEACTIVATE THIS SCRIPT
        Destroy(gameObject);
    }
}
