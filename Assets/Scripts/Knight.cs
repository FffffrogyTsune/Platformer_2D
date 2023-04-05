using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float speed;
    Transform mob_transform;
    float horizontal_value = 1f;
    Vector2 ref_velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mob_transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 target_velocity = new Vector2(horizontal_value * speed * Time.deltaTime, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f); // BASIC MOVEMENT
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        horizontal_value = -horizontal_value;
    }
}
