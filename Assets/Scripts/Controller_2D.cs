using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_2D : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed_horizontal = 1300.0f;
    [SerializeField] float wall_sliding_speed = 2f;
    float horizontal_value;
    int direction;
    Vector2 ref_velocity = Vector2.zero;
    bool facing_right = true;
    float jumpForce = 55f;
    bool is_jumping = false;

    [Header("Status Settings")]
    [SerializeField] bool grounded;
    [SerializeField] Transform ground_check;
    [SerializeField] LayerMask what_is_ground;
    [SerializeField] bool is_touching_front;
    [SerializeField] Transform front_check;
    [SerializeField] bool is_crouching;
    [SerializeField] bool wall_sliding;
    [SerializeField] bool is_attacking;
    [SerializeField] bool is_dashing;
    [SerializeField] bool is_waiting;
    [SerializeField] bool is_invisible;
    float check_radius = 0.5f;

    [Header("Attack Settings")]
    [SerializeField] [Range(0, 1000)] public int gauge;
    [SerializeField] int damage_point = 15;
    [SerializeField] Transform attack_point;
    [SerializeField] LayerMask enemy_layers;
    float attack_range = 1f;
    float next_attack_time = 0f;

    [Header("Dash Settings")]
    [SerializeField] float dashing_velocity = 100f;
    [SerializeField] float dashing_time = 0.2f;
    Vector2 dashing_direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        direction = 1;
        sr.flipX = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_value = Input.GetAxis("Horizontal");

        if (horizontal_value > 0 && !facing_right) Flip(); // PLAYER MOVING TO THE RIGHT
        else if (horizontal_value < 0 && facing_right) Flip(); // PLAYER MOVING TO THE LEFT

        grounded = Physics2D.OverlapCircle(ground_check.position, check_radius, what_is_ground); // IS THE PLAYER GROUNDED ?

        if (Input.GetButtonDown("Jump"))
        {
            is_jumping = true;
            Jump();
        }

        if (Input.GetButtonDown("Attack") && Time.time >= next_attack_time && !wall_sliding && !is_dashing && !is_invisible)
        {
            is_attacking = true;
            Attack();
        }

        if (Input.GetButtonDown("Dash") && gauge >= 200 && !wall_sliding && !is_dashing && !is_waiting)
        {
            is_dashing = true;
            gauge -= 200;
            StartCoroutine(Dash());
        }

        if (is_dashing) rb.velocity = dashing_direction.normalized * dashing_velocity;

        if (Input.GetButtonDown("Invisible") && gauge >= 800 && !is_invisible && !is_waiting)
        {
            is_invisible = true;
            gauge -= 800;
            StartCoroutine(Invisible());
        }

        // CROUCH AND SLIDE
        if (Input.GetButtonDown("Crouch") && grounded);
        {
            // change collider, velocity,
        }

        is_touching_front = Physics2D.OverlapCircle(front_check.position, check_radius, what_is_ground); // IS THE PLAYER TOUCHING A WALL ?

        // WALL SLIDING
        if (is_touching_front && !grounded && horizontal_value != 0)
        {
            wall_sliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, - wall_sliding_speed, float.MaxValue));
        }
        else wall_sliding = false;
    }

    void FixedUpdate()
    {
        Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.deltaTime, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f); // BASIC MOVEMENT
    }

    // JUMP
    void Jump()
    {
        if (is_jumping && grounded) rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // NORMAL JUMP
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

    // DASH
    IEnumerator Dash()
    {
        dashing_direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") / 1.5f); // DIRECTION OF THE DASH
        if (dashing_direction == Vector2.zero) dashing_direction = new Vector2(transform.localScale.x * direction, 0); // IF THE PLAYER IS NOT MOVING, HE WILL DASH WHERE HE'S TURNED
        yield return new WaitForSeconds(dashing_time);
        is_dashing = false;
        is_waiting = true; // COOLDOWN
        yield return new WaitForSeconds(1);
        is_waiting = false;
    }

    // INVISIBLE
    IEnumerator Invisible()
    {
        yield return new WaitForSeconds(3);
        is_invisible = false;
        is_waiting = true; // COOLDOWN
        yield return new WaitForSeconds(1);
        is_waiting = false;
    }
}