using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float speed;
    public Transform target;
    public float height;
    Transform mob_transform;
    [SerializeField] bool is_attacking;

    [Header("Attack Settings")]
    [SerializeField] int damage_point;
    [SerializeField] Transform attack_point;
    [SerializeField] LayerMask enemy_layers;
    float attack_range = 1.2f;
    float next_attack_time = 0f;
    bool cooldown;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mob_transform = transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(transform.position, target.position) < 15 && Vector2.Distance(transform.position, target.position) > 2.5f && !cooldown && !is_attacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, height), speed * Time.deltaTime); // FOLLOWING THE PLAYER
        }

        if (Vector2.Distance(transform.position, target.position) <= 2.7f && next_attack_time <= Time.time && !is_attacking && !cooldown)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {

    }

    void Attack()
    {
        Collider2D[] hit_player = Physics2D.OverlapCircleAll(attack_point.position, attack_range, enemy_layers); // LIST OF ALL ENEMIES HIT
        foreach (Collider2D player in hit_player) // IF PLAYER TOUCHED
        {
            player.GetComponent<Player_Health>().TakeDamage(damage_point); // TAKES THE TakeDamage(int damage) FUNCTION IN THE ENEMY'S SCRIPT TO GIVE DAMAGE TO THE ENEMY
        }
        next_attack_time = Time.time + 2f; // LIMITS THE NUMBER OF ATTACKS AT 4 PER SECONDS
        cooldown = true;
        is_attacking = false;
    }
}
