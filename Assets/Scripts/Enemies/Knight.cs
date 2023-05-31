using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim_controller;
    Enemy enemy;

    [SerializeField] float speed;
    public Transform target;
    public float height;
    public bool is_attacking;
    [SerializeField] bool stun = false;

    [Header("Attack Settings")]
    [SerializeField] int damage_point;
    public Transform attack_point;
    public LayerMask player_layers;
    public float attack_range;
    public float cooldown_time;
    float next_attack_time = 0f;
    float cooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim_controller = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= cooldown)
        {
            stun = false;
            anim_controller.SetBool("Stun", false);
        }

        if (enemy.health > 0)
        {
            if (Vector2.Distance(transform.position, target.position) <= 11 && Vector2.Distance(transform.position, target.position) > 1.5f && !stun && !is_attacking)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, height), speed * Time.deltaTime); // FOLLOWING THE PLAYER
            }

            if (Vector2.Distance(transform.position, target.position) <= 1.7f && Time.time >= next_attack_time && !is_attacking && !stun)
            {
                is_attacking = true;
                anim_controller.SetTrigger("Prepare");
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.3f);
        anim_controller.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);
        Collider2D[] hit_player = Physics2D.OverlapCircleAll(attack_point.position, attack_range, player_layers); // DETECTION OF THE PLAYER
        foreach (Collider2D player in hit_player) // IF PLAYER TOUCHED
        {
            player.GetComponent<Player_Health>().TakeDamage(damage_point); // TAKES THE TakeDamage(int damage) FUNCTION IN THE PLAYER'S SCRIPT TO GIVE DAMAGE TO THE ENEMY
        }
        next_attack_time = Time.time + Random.Range(1.75f, 2.5f);
        stun = true;
        anim_controller.SetBool("Stun", true);
        cooldown = Time.time + cooldown_time;
        is_attacking = false;
    }
}
