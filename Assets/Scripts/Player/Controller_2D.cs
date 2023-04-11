using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_2D : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    CapsuleCollider2D cap;
    Animator anim_controller;
    public Player_Health player_health;
    public Health_Bar health_bar;
    public Player_Gauge player_gauge;
    public Gauge_Bar gauge_bar;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed_horizontal = 650;
    [SerializeField] float wall_sliding_speed;
    float horizontal_value;
    int direction;
    Vector2 ref_velocity = Vector2.zero;
    bool facing_right = true;
    float jumpForce = 23.5f;
    bool is_jumping = false;

    [Header("Status Settings")]
    Vector2 respawn_point = new Vector2 (-7, 2.5f);
    [SerializeField] bool grounded;
    [SerializeField] Transform ground_check;
    [SerializeField] LayerMask what_is_ground;
    [SerializeField] bool is_touching_front;
    [SerializeField] Transform front_check;
    [SerializeField] bool is_touching_up;
    [SerializeField] Transform up_check;
    [SerializeField] bool wall_sliding;
    [SerializeField] bool is_attacking;
    [SerializeField] bool is_guarding;
    [SerializeField] bool is_dashing;
    [SerializeField] bool is_waiting;
    float check_radius = 0.1f;

    [Header("Attack Settings")]
    [SerializeField] int damage_point = 15;
    [SerializeField] Transform attack_point;
    [SerializeField] Transform special_attack_point;
    [SerializeField] LayerMask enemy_layers;
    float attack_range = 1f;
    float next_attack_time = 0f;

    [Header("Dash Settings")]
    [SerializeField] float dashing_velocity = 90f;
    [SerializeField] float dashing_time = 0.2f;
    Vector2 dashing_direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cap = GetComponent<CapsuleCollider2D>();
        anim_controller = GetComponent<Animator>();

        direction = 1;
        sr.flipX = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_value = Input.GetAxis("Horizontal");

        if (horizontal_value > 0 && !facing_right) Flip(); // PLAYER MOVING TO THE RIGHT
        else if (horizontal_value < 0 && facing_right) Flip(); // PLAYER MOVING TO THE LEFT

        anim_controller.SetFloat("Speed", Mathf.Abs(horizontal_value));
        anim_controller.SetFloat("Vel_Y", rb.velocity.y);

        if (rb.velocity.y < 0) rb.gravityScale = 7;
        else rb.gravityScale = 6;

        grounded = Physics2D.OverlapCircle(ground_check.position, check_radius, what_is_ground); // IS THE PLAYER GROUNDED ?
        is_touching_front = Physics2D.OverlapCircle(front_check.position, check_radius, what_is_ground); // IS THE PLAYER TOUCHING A WALL ?
        is_touching_up = Physics2D.OverlapCircle(up_check.position, check_radius, what_is_ground); // IS THE PLAYER TOUCHING A CEILING ?

        if (player_health.current_health <= 0)
        {
            StartCoroutine(PlayerDie());
        }

        if (Input.GetButtonDown("Jump") && grounded && !is_touching_up)
        {
            is_jumping = true;
            Jump();
        }

        if (Input.GetButtonDown("Attack") && Time.time >= next_attack_time && !wall_sliding && !is_guarding && !is_dashing && !is_waiting)
        {
            is_attacking = true;
            Attack();
        }

        if (Input.GetButton("Shield"))
        {
            is_guarding = true;
        }
        else is_guarding = false;

        if (Input.GetButtonDown("Special") && player_gauge.current_gauge >= 400 && !wall_sliding && !is_dashing && !is_waiting)
        {
            is_attacking = true;
            player_gauge.Reduce(400);
            StartCoroutine(Special());
        }

        if (Input.GetButtonDown("Dash") && player_gauge.current_gauge >= 200 && !wall_sliding && !is_dashing && !is_waiting)
        {
            is_dashing = true;
            player_gauge.Reduce(200);
            StartCoroutine(Dash());
        }

        // WALL SLIDING
        if (is_touching_front && !grounded && horizontal_value != 0)
        {
            wall_sliding = true;
            anim_controller.SetBool("Wall_Slide", true);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wall_sliding_speed, float.MaxValue));
        }
        else
        {
            wall_sliding = false;
            anim_controller.SetBool("Wall_Slide", false);
        }
    }

    void FixedUpdate()
    {
        Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.deltaTime, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f); // BASIC MOVEMENT

        // GUARD
        if (is_guarding && !wall_sliding)
        {
            anim_controller.SetBool("Guard", true);
            moveSpeed_horizontal = 250;
        }
        else
        {
            anim_controller.SetBool("Guard", false);
            moveSpeed_horizontal = 650;
        }

        if (is_dashing) rb.velocity = dashing_direction.normalized * dashing_velocity; // DASH
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !is_dashing && player_health.current_health > 0)
        {
            player_health.TakeDamage(20);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint") && player_health.current_health > 0)
        {
            respawn_point = collision.gameObject.transform.position;
        }
    }

    // JUMP
    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // NORMAL JUMP
        is_jumping = false;
    }

    // FLIP
    void Flip()
    {
        front_check.localPosition = -front_check.localPosition;
        attack_point.localPosition = -attack_point.localPosition;
        facing_right = !facing_right;
        direction = -direction;
        sr.flipX = !sr.flipX; // SPRITE
    }

    // ATTACK
    void Attack()
    {
        Collider2D[] hit_enemies = Physics2D.OverlapCircleAll(attack_point.position, attack_range, enemy_layers); // LIST OF ALL ENEMIES HIT
        foreach(Collider2D enemy in hit_enemies) // FOR EACH ENEMIES HIT
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage_point); // TAKES THE TakeDamage(int damage) FUNCTION IN THE ENEMY'S SCRIPT TO GIVE DAMAGE TO THE ENEMY
        }
        next_attack_time = Time.time + 0.25f; // LIMITS THE NUMBER OF ATTACKS AT 4 PER SECONDS
        is_attacking = false;
    }

    // SPECIAL ATTACK
    IEnumerator Special() // A CHANGER VOIR GDOC
    {
        yield return new WaitForSeconds(0.5f);
        Collider2D[] hit_enemies = Physics2D.OverlapCircleAll(special_attack_point.position, attack_range * 10, enemy_layers); // LIST OF ALL ENEMIES HIT
        foreach (Collider2D enemy in hit_enemies) // FOR EACH ENEMIES HIT
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage_point * 5); // TAKES THE TakeDamage(int damage) FUNCTION IN THE ENEMY'S SCRIPT TO GIVE DAMAGE TO THE ENEMY
        }
        is_waiting = true; // COOLDOWN
        yield return new WaitForSeconds(2);
        is_waiting = false;
        is_attacking = false;
    }

    // DASH
    IEnumerator Dash()
    {
        dashing_direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0); // DIRECTION OF THE DASH
        if (dashing_direction == Vector2.zero) dashing_direction = new Vector2(transform.localScale.x * direction, 0); // IF THE PLAYER IS NOT MOVING, HE WILL DASH WHERE HE'S TURNED
        yield return new WaitForSeconds(dashing_time);
        is_dashing = false;
        is_waiting = true; // COOLDOWN
        yield return new WaitForSeconds(1);
        is_waiting = false;
    }

    // DEATH
    IEnumerator PlayerDie()
    {
        anim_controller.SetBool("Death", true);
        yield return new WaitForSeconds(1.5f);
        anim_controller.SetBool("Death", false);
        player_health.current_health = player_health.max_health - 30;
        health_bar.SetHealth(player_health.current_health);
        player_gauge.current_gauge = 300;
        gauge_bar.SetGauge(player_gauge.current_gauge);
        transform.position = respawn_point;
        yield return new WaitForSeconds(0.1f);
    }
}