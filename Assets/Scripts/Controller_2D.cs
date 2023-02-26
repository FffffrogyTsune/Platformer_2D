using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_2D : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    float horizontal_value;
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
    
    [SerializeField] float wall_sliding_speed;
    [SerializeField] float x_wall_force;
    [SerializeField] float y_wall_force;
    [SerializeField] float wall_jump_time;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

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

        is_touching_front = Physics2D.OverlapCircle(front_check.position, check_radius, what_is_ground); // IS THE PLAYER TOUCHING A WALL ?

        if (is_touching_front && !grounded && horizontal_value != 0) wall_sliding = true;
        else wall_sliding = false;

        if (wall_sliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, - wall_sliding_speed, float.MaxValue));
        }
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
        else if (is_jumping && wall_sliding) rb.AddForce(new Vector2(x_wall_force * -horizontal_value, y_wall_force), ForceMode2D.Impulse); // WALL JUMP
        is_jumping = false;
    }

    // FLIP (INVERT)
    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        facing_right = !facing_right;
        sr.flipX = !sr.flipX; // SPRITE
    }
}