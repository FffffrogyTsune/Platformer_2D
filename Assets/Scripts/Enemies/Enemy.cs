using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public Controller_2D controller_2d;
    Knight knight;
    Animator anim_controller;
    public int health_max;
    public int health;
    public bool respawning;
    public bool dead;
    public Vector2 respawn_point;

    public Rigidbody2D health_orb;
    public Rigidbody2D gauge_orb;
    public Rigidbody2D coin;
    public int drop_coin;

    [SerializeField] bool facing_right = false;
    public LayerMask player_layers;
    [SerializeField] Transform right_detector;
    [SerializeField] Transform left_detector;
    public Transform detector;
    public LayerMask wall_layer;
    public float detection_range;
    [SerializeField] bool player_right;
    [SerializeField] bool player_left;


    // Start is called before the first frame update
    void Start()
    {
        respawn_point = transform.position;
        knight = GetComponent<Knight>();
        anim_controller = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        health = health_max; // THE ENEMY STARTS WITH A FULL HEALTH
    }

    private void Update()
    {
        rb.velocity = Vector2.zero;
        respawning = false;
        dead = false;

        if (controller_2d.can_ai_respawn)
        {
            respawning = true;
            transform.localPosition = respawn_point;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            health = health_max;
            GetComponent<CapsuleCollider2D>().enabled = true; // REACTIVATE THE BOX COLLIDER 2D
            anim_controller.SetBool("Dead", false);
        }

        if (Physics2D.OverlapCircle(right_detector.position, detection_range, player_layers) && !facing_right && health > 0 && !knight.is_attacking && !knight.stun) Flip(); // DETECTION OF THE PLAYER
        if (Physics2D.OverlapCircle(left_detector.position, detection_range, player_layers) && facing_right && health > 0 && !knight.is_attacking && !knight.stun) Flip(); // DETECTION OF THE PLAYER
    }

    // FLIP
    void Flip()
    {
        facing_right = !facing_right;
        knight.attack_point.localPosition = new Vector2(-knight.attack_point.localPosition.x, knight.attack_point.localPosition.y);
        detector.localPosition = new Vector2(-detector.localPosition.x, detector.localPosition.y);
        sr.flipX = !sr.flipX; // SPRITE
    }

    // TAKING DAMAGE
    public void TakeDamage(int damage)
    {
        rb.velocity = Vector2.zero;
        health -= damage;
        StartCoroutine(Hurt());
        if (health <= 0) // WHEN THE ENEMY HAS NO HEALTH, HE DIES
        {
            Die();
        }
    }
    
    // DYING
    void Die()
    {
        dead = true;
        if (gauge_orb != null && health_orb != null)
        {
            Rigidbody2D H_orb = Instantiate(health_orb, transform.position, transform.rotation);
            H_orb.velocity = new Vector2(Random.Range(-5, 5), 12);
        
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
        anim_controller.SetBool("Dead", true);
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        GetComponent<CapsuleCollider2D>().enabled = false; // DEACTIVATE THE BOX COLLIDER 2D
    }
    public bool Ballz()
    {
        return Physics2D.OverlapCircle(detector.position, 0.3f, wall_layer);
    }

    IEnumerator Hurt()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        sr.color = Color.white;
    }
}
