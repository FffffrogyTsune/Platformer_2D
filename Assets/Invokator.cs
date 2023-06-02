using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invokator : MonoBehaviour
{
    float next_invoke = 0f;
    public Rigidbody2D knight;
    [SerializeField] GameObject[] current_enemies;

    // Start is called before the first frame update
    void Start()
    {
        next_invoke = Time.time + 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= next_invoke)
        {
            Rigidbody2D enemy = Instantiate(knight, transform.position + new Vector3(0, 5.5f), transform.rotation);
            enemy.GetComponent<Knight>().height = -1;
            next_invoke = Time.time + 20f;
        }
    }
}
