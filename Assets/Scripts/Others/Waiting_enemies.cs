using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiting_enemies : MonoBehaviour
{
    public Moving_Platform moving_platform;
    public BoxCollider2D col;
    [SerializeField] GameObject[] enemies;
    bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        moving_platform.enabled = false;
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        int length = enemies.Length;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<CapsuleCollider2D>().enabled == false) length--;
        }

        if (length <= 0)
        {
            ready = true;
            col.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            col.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && ready)
        {
            moving_platform.enabled = true;
            this.enabled = false;
        }
    }
}
