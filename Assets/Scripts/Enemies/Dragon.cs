using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim_controller;
    Enemy_02 enemy_02;

    public Transform target;
    public float height;
    public bool is_attacking;
    public bool stun = false;

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
        enemy_02 = GetComponent<Enemy_02>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= cooldown)
        {
            stun = false;
            anim_controller.SetBool("Stun", false);
        }

        if (Vector2.Distance(transform.position, target.position) <= 8.5f && Time.time >= next_attack_time && !is_attacking && !stun && !enemy_02.dead)
        {
            print("attack");
            is_attacking = true;
            anim_controller.SetTrigger("Prepare");
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        anim_controller.SetTrigger("Attack_01");
        yield return new WaitForSeconds(1f);
        Collider2D[] hit_player = Physics2D.OverlapCircleAll(attack_point.position, attack_range, player_layers); // DETECTION OF THE PLAYER
        foreach (Collider2D player in hit_player) // IF PLAYER TOUCHED
        {
            player.GetComponent<Player_Health>().TakeDamage(damage_point); // TAKES THE TakeDamage(int damage) FUNCTION IN THE PLAYER'S SCRIPT TO GIVE DAMAGE TO THE ENEMY
        }
        next_attack_time = Time.time + Random.Range(3f, 4.5f);
        stun = true;
        anim_controller.SetBool("Stun", true);
        cooldown = Time.time + cooldown_time;
        is_attacking = false;
    }
}