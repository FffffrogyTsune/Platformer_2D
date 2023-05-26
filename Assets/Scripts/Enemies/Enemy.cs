using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    public Controller_2D controller_2d;
    [SerializeField] int health_max;
    public int health;
    [SerializeField] Vector2 respawn_point;

    public Rigidbody2D health_orb;
    public Rigidbody2D gauge_orb;
    public Rigidbody2D coin;
    public int drop_coin;

    [SerializeField] bool facing_right = true;

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

        if (controller_2d.can_ai_respawn)
        {
            transform.localPosition = respawn_point;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            health = health_max;
            GetComponent<CapsuleCollider2D>().enabled = true; // REACTIVATE THE BOX COLLIDER 2D
        }
    }

    // FLIP
    void Flip()
    {
        facing_right = !facing_right;
        //sr.flipX = !sr.flipX; // SPRITE
    }

    // TAKING DAMAGE
    public void TakeDamage(int damage)
    {
        rb.velocity = Vector2.zero;
        health -= damage;
        if (health <= 0) // WHEN THE ENEMY HAS NO HEALTH, HE DIES
        {
            Die();
        }
    }
    
    // DYING
    void Die()
    {
        if (gauge_orb != null && health_orb != null)
        {
            for (int i = 0; i <= Random.Range(0, 2); i++)
            {
                Rigidbody2D H_orb = Instantiate(health_orb, transform.position, transform.rotation);
                H_orb.velocity = new Vector2(Random.Range(-5, 5), 12);
            }
            for (int i = 0; i <= Random.Range(0, 2); i++)
            {
                Rigidbody2D G_orb = Instantiate(gauge_orb, transform.position, transform.rotation);
                G_orb.velocity = new Vector2(Random.Range(-5, 5), 12);
            }
        }  

        for (int i = 0; i < drop_coin; i++)
        {
            Rigidbody2D Coin_0 = Instantiate(coin, transform.position + new Vector3(0, 1f), transform.rotation);
            Coin_0.velocity = new Vector2(Random.Range(-4, 4), 7);
        }

        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        GetComponent<CapsuleCollider2D>().enabled = false; // DEACTIVATE THE BOX COLLIDER 2D
    }
}
