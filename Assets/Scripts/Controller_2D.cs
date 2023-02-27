using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_2D : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D co;

    float horizontal_value;
    int direction;
    [SerializeField] float moveSpeed_horizontal = 800.0f;
    Vector2 ref_velocity = Vector2.zero;
    bool facing_right = true;
    float jumpForce = 30f;
    bool is_jumping = false;

    float check_radius = 0.5f;
    [SerializeField] bool grounded;
    [SerializeField] Transform ground_check;
    [SerializeField] LayerMask what_is_ground;
    [SerializeField] bool is_touching_front;
    [SerializeField] Transform front_check;
    [SerializeField] bool wall_sliding;
    [SerializeField] bool is_attacking;

    [SerializeField] float wall_sliding_speed;
    [SerializeField] float x_wall_force;
    [SerializeField] float y_wall_force;
    [SerializeField] float wall_jump_time;

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

        if (Input.GetButtonDown("Fire1"))
        {
            is_attacking = true;
            Attack();
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
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f);
    }

    // JUMP
    void Jump()
    {
        if (is_jumping && grounded) rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // NORMAL JUMP
        else if (is_jumping && is_touching_front) rb.AddForce(new Vector2(x_wall_force * -direction, y_wall_force - rb.velocity.y), ForceMode2D.Impulse); // WALL JUMP
        is_jumping = false;
    }

    // FLIP (INVERT)
    void Flip()
    {
        front_check.localPosition = -front_check.localPosition;
        facing_right = !facing_right;
        direction = -direction;
        sr.flipX = !sr.flipX; // SPRITE
    }

    // ATTACK
    void Attack()
    {
        if (is_attacking && !wall_sliding)
        {
            Debug.Log("test");
        }
        is_attacking = false;
    }
}