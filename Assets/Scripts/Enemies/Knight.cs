using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    Rigidbody2D rb;
    Enemy enemy;

    [SerializeField] float speed;
    public Transform target;
    public float height;
    [SerializeField] bool is_attacking;
    [SerializeField] bool stun = false;

    [Header("Attack Settings")]
    [SerializeField] int damage_point;
    [SerializeField] Transform attack_point;
    [SerializeField] LayerMask player_layers;
    public float attack_range;
    public float cooldown_time;
    float next_attack_time = 0f;
    float cooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= cooldown)
        {
            stun = false;
        }

        if (enemy.health > 0)
        {
            if (Vector2.Distance(transform.position, target.position) < 15 && Vector2.Distance(transform.position, target.position) > 2.5f && !stun && !is_attacking)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, height), speed * Time.deltaTime); // FOLLOWING THE PLAYER
            }

            if (Vector2.Distance(transform.position, target.position) <= 2.7f && Time.time >= next_attack_time && !is_attacking && !stun)
            {
                is_attacking = true;
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.7f);
        Collider2D[] hit_player = Physics2D.OverlapCircleAll(attack_point.position, attack_range, player_layers); // DETECTION OF THE PLAYER
        foreach (Collider2D player in hit_player) // IF PLAYER TOUCHED
        {
            player.GetComponent<Player_Health>().TakeDamage(damage_point); // TAKES THE TakeDamage(int damage) FUNCTION IN THE PLAYER'S SCRIPT TO GIVE DAMAGE TO THE ENEMY
        }
        next_attack_time = Time.time + 3f;
        stun = true;
        cooldown = Time.time + cooldown_time;
        is_attacking = false;
    }
}
