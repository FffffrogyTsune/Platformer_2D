using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Orb : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] bool grounded;
    [SerializeField] Transform ground_check;
    [SerializeField] LayerMask what_is_ground;
    float check_radius = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(ground_check.position, check_radius, what_is_ground); // IS THE ORB GROUNDED ?

        if (grounded) rb.bodyType = RigidbodyType2D.Static; // THE ORB STAY STATIC WHEN IT TOUCHES THE GROUND
    }
}
