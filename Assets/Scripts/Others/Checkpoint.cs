using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Controller_2D controller_2d;
    SpriteRenderer sr;
    public Sprite activated;
    [SerializeField] GameObject[] enemies;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            controller_2d.respawn_point = transform.position;
            sr.sprite = activated;
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Enemy>().health <=0) Destroy(enemy);
            }
            Destroy(GetComponent<Checkpoint>());
        }
    }
}
