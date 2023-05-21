using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Controller_2D controller_2d;
    SpriteRenderer sr;
    public Sprite activated;

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
            
            Debug.Log("check");
            Destroy(GetComponent<Checkpoint>());
        }
    }
}
