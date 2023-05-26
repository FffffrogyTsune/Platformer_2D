using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : MonoBehaviour
{
    public Enemy enemy;
    BoxCollider2D col;
    Animator animator;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.health <= 0)
        {
            Opening();
        }
    }

    void Opening()
    {
        animator.SetTrigger("Opening");
        col.enabled = false;
    }
}
