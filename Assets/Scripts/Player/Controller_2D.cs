using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_2D : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    CapsuleCollider2D cap;
    Animator anim_controller;
    public Dash_UI dash_ui;
    public Player_Health player_health;
    public Health_Bar health_bar;
    public Player_Gauge player_gauge;
    public Gauge_Bar gauge_bar;
    public Death_Counter death_counter;

    [Header("Movement Settings")]
    public float moveSpeed_horizontal = 450;
    [SerializeField] float wall_sliding_speed;
    [SerializeField] float horizontal_value;
    int direction;
    Vector2 ref_velocity = Vector2.zero;
    bool facing_right = true;
    float jumpForce = 23.5f;
    float coyote_time = 0.12f;
    float coyote_time_counter;
    bool is_jumping = false;

    [Header("Status Settings")]
    public Vector2 respawn_point = new Vector2 (-71f, -4.5f);
    [SerializeField] bool grounded;
    [SerializeField] Transform ground_check;
    [SerializeField] LayerMask what_is_ground;
    [SerializeField] bool is_touching_front;
    [SerializeField] Transform front_check;
    [SerializeField] bool is_touching_up;
    [SerializeField] Transform up_check;
    [SerializeField] bool wall_sliding;
    [SerializeField] bool is_attacking;
    [SerializeField] bool is_dashing;
    [SerializeField] bool is_waiting;
    [SerializeField] bool is_invincible;
    [SerializeField] bool is_dying;
    public bool can_ai_respawn;
    bool is_holding_jump;
    float check_radius = 0.1f;

    [Header("Attack Settings")]
    [SerializeField] int damage_point = 15;
    [SerializeField] Transform attack_point;
    [SerializeField] Transform special_attack_point;
    [SerializeField] LayerMask enemy_layers;
    [SerializeField] LayerMask box_layers;
    float attack_range = 0.7f;
    float next_attack_time = 0f;
    float next_combo_time = 0f;
    [SerializeField] int combo = 0;

    [Header("Dash Settings")]
    [SerializeField] float dashing_velocity = 70f;
    [SerializeField] float dashing_time = 0.1f;
    [SerializeField] bool cooling;
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

        if (is_dying) horizontal_value = 0;

        if (next_combo_time >= Time.time) moveSpeed_horizontal = 220;
        else moveSpeed_horizontal = 450;

        if (horizontal_value > 0 && !facing_right) Flip(); // PLAYER MOVING TO THE RIGHT
        else if (horizontal_value < 0 && facing_right) Flip(); // PLAYER MOVING TO THE LEFT

        anim_controller.SetFloat("Speed", Mathf.Abs(horizontal_value));
        anim_controller.SetFloat("Vel_Y", rb.velocity.y);

        if (rb.velocity.y < 0 && is_holding_jump) rb.gravityScale = 7;
        else rb.gravityScale = 6;

        grounded = Physics2D.OverlapCircle(ground_check.position, check_radius, what_is_ground); // IS THE PLAYER GROUNDED ?
        is_touching_front = Physics2D.OverlapCircle(front_check.position, check_radius, what_is_ground); // IS THE PLAYER TOUCHING A WALL ?
        is_touching_up = Physics2D.OverlapCircle(up_check.position, check_radius, what_is_ground); // IS THE PLAYER TOUCHING A CEILING ?
        is_holding_jump = Input.GetButton("Jump");

        if (player_health.current_health <= 0 && !is_dying)
        {
            is_dying = true;
            StartCoroutine(PlayerDie());
            death_counter.death_count += 1;
            death_counter.SetHitCounter(death_counter.death_count);
        }

        if (grounded)
        {
            coyote_time_counter = coyote_time;
        }
        else
        {
            coyote_time_counter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && coyote_time_counter > 0f && !is_touching_up)
        {
            is_jumping = true;
            anim_controller.SetTrigger("Ascent");
            Jump();
        }
        
        if (Time.time > next_combo_time || combo == 2)
        {
            combo = 0;
        }

        if (Input.GetButtonDown("Attack") && Time.time >= next_attack_time && !wall_sliding && !is_dashing)
        {
            is_attacking = true;
            combo += 1;
            next_combo_time = Time.time;
            anim_controller.SetTrigger("Attack_0" + combo.ToString());
            Attack();
        }


        if (Input.GetButtonDown("Dash") && horizontal_value != 0 && player_gauge.current_gauge >= 300 && !wall_sliding && !is_dashing && !is_waiting)
        {
            is_dashing = true;
            player_gauge.Reduce(300);
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

        if (player_gauge.current_gauge >= 300 && !cooling) dash_ui.SetTrue();
    }

    void FixedUpdate()
    {
        Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.deltaTime, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f); // BASIC MOVEMENT

        if (!is_holding_jump && !grounded)
        {
            rb.AddForce(new Vector2(0, -50), ForceMode2D.Force);
        }
        else coyote_time_counter = 0f;

        if (is_dashing)
        {
            rb.velocity = dashing_direction.normalized * dashing_velocity; // DASH
            StartCoroutine(dash_ui.DashCooldown());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !is_dashing && player_health.current_health > 0 && !is_invincible)
        {
            player_health.TakeDamage(20);
            StartCoroutine(Invincible());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    // JUMP
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // NORMAL JUMP
        is_jumping = false;
    }

    // FLIP
    void Flip()
    {
        front_check.localPosition = new Vector2(-front_check.localPosition.x, front_check.localPosition.y);
        attack_point.localPosition = new Vector2(-attack_point.localPosition.x, attack_point.localPosition.y);
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
        Collider2D[] hit_box = Physics2D.OverlapCircleAll(attack_point.position, attack_range, box_layers); // LIST OF ALL BOX HIT
        foreach (Collider2D box in hit_box) // FOR EACH BOX HIT
        {
            box.GetComponent<Box>().Break(); // TAKES THE Break() FUNCTION IN THE BOX'S SCRIPT TO BREAK THE BOX
        }
        next_attack_time = Time.time + 0.2f; // LIMITS THE NUMBER OF ATTACKS PER SECONDS
        next_combo_time = Time.time + 0.5f;
        is_attacking = false;
    }

    // DASH
    IEnumerator Dash()
    {
        cooling = true;
        dashing_direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0); // DIRECTION OF THE DASH
        yield return new WaitForSeconds(dashing_time);
        if (dashing_direction == Vector2.zero) dashing_direction = new Vector2(transform.localScale.x * direction, 0); // IF THE PLAYER IS NOT MOVING, HE WILL DASH WHERE HE'S TURNED
        Collider2D[] hit_enemies = Physics2D.OverlapCircleAll(attack_point.position, attack_range, enemy_layers); // LIST OF ALL ENEMIES HIT
        foreach (Collider2D enemy in hit_enemies) // FOR EACH ENEMIES HIT
        {
            enemy.GetComponent<Enemy>().TakeDamage(50); // TAKES THE TakeDamage(int damage) FUNCTION IN THE ENEMY'S SCRIPT TO GIVE DAMAGE TO THE ENEMY
        }
        Collider2D[] hit_box = Physics2D.OverlapCircleAll(attack_point.position, attack_range, box_layers); // LIST OF ALL BOX HIT
        foreach (Collider2D box in hit_box) // FOR EACH BOX HIT
        {
            box.GetComponent<Box>().Break(); // TAKES THE Break() FUNCTION IN THE BOX'S SCRIPT TO BREAK THE BOX
        }
        is_dashing = false;
        is_waiting = true; // COOLDOWN
        yield return new WaitForSeconds(3);
        is_waiting = false;
        cooling = false;
    }

    // INVINCIBILITY
    IEnumerator Invincible()
    {
        is_invincible = true;
        yield return new WaitForSeconds(1.5f);
        is_invincible = false;
    }

    // DEATH
    IEnumerator PlayerDie()
    {
        anim_controller.SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        transform.position = respawn_point;
        player_health.current_health = player_health.max_health - 30;
        health_bar.SetHealth(player_health.current_health);
        player_gauge.current_gauge = 300;
        gauge_bar.SetGauge(player_gauge.current_gauge);
        anim_controller.SetTrigger("Revive");
        is_dying = false;
        can_ai_respawn = true;
        yield return new WaitForSeconds(0.1f);
        can_ai_respawn = false;
        
    }
}