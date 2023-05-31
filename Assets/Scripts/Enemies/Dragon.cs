using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim_controller;
    public int health_max;
    public int health;
    public Vector2 respawn_point;
    public Rigidbody2D health_orb;
    public Rigidbody2D gauge_orb;
    public Rigidbody2D coin;
    public int drop_coin;
    [SerializeField] bool facing_right = false;
    public LayerMask player_layers;
    [SerializeField] Transform right_detector;
    [SerializeField] Transform left_detector;
    public float detection_range;
    [SerializeField] bool player_right;
    [SerializeField] bool player_left;

    // Start is called before the first frame update
    void Start()
    {
        respawn_point = transform.position;
        anim_controller = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        health = health_max; // THE ENEMY STARTS WITH A FULL HEALTH
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
