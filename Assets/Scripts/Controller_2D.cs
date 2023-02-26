using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_2D : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    float horizontal_value;
    Vector2 ref_velocity = Vector2.zero;
    float jumpForce = 30f;
    [SerializeField] float moveSpeed_horizontal = 800.0f;
    [SerializeField] bool is_jumping = false;
    [SerializeField] bool grounded = false;
    [Range(0, 1)] [SerializeField] float smooth_time = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_value = Input.GetAxis("Horizontal");

        if (horizontal_value > 0) sr.flipX = false;
        else if (horizontal_value < 0) sr.flipX = true;

        if (Input.GetButtonDown("Jump") && grounded)
        {
            is_jumping = true;
        }
    }

    void FixedUpdate()
    {

        Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.deltaTime, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f);

        // JUMP
        if (is_jumping && grounded)
        {
            is_jumping = false;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            grounded = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        grounded = true;
    }
}