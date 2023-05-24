using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int health_max;
    [SerializeField] int health;

    public Rigidbody2D health_orb;
    public Rigidbody2D gauge_orb;
    public Rigidbody2D coin;

    [SerializeField] bool facing_right = true;
    [SerializeField] Transform attack_point;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = health_max; // THE ENEMY STARTS WITH A FULL HEALTH
    }

    private void Update()
    {
        if (rb.velocity.x > 0 && !facing_right) Flip(); // PLAYER MOVING TO THE RIGHT
        else if (rb.velocity.x < 0 && facing_right) Flip(); // PLAYER MOVING TO THE LEFT
    }

    // FLIP
    void Flip()
    {
        attack_point.localPosition = -attack_point.localPosition;
        facing_right = !facing_right;
        //sr.flipX = !sr.flipX; // SPRITE
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
            H_orb.velocity = new Vector2(Random.Range(-10,10), 28);
        }

        for (int i = 0; i <= Random.Range(0, 2); i++)
        {
            Rigidbody2D G_orb = Instantiate(gauge_orb, transform.position, transform.rotation);
            G_orb.velocity = new Vector2(Random.Range(-10, 10), 28);
        }

        for (int i = 0; i <= 2; i++)
        {
            Rigidbody2D Coin_0 = Instantiate(coin, transform.position, transform.rotation);
            Coin_0.velocity = new Vector2(Random.Range(-5, 5), 15);
        }

        GetComponent<CapsuleCollider2D>().enabled = false; // DEACTIVATE THE BOX COLLIDER 2D
        this.enabled = false; // DEACTIVATE THIS SCRIPT
        Destroy(gameObject);
    }
}
