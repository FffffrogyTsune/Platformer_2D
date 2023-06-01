using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Rigidbody2D coin;
    public GameObject particle;
    public int drop_coin;

    public void Break()
    {
        for (int i = 0; i < drop_coin; i++)
        {
            Rigidbody2D Coin_0 = Instantiate(coin, transform.position + new Vector3(0, 1f), transform.rotation);
            Coin_0.velocity = new Vector2(Random.Range(-4, 4), 7);
        }

        GetComponent<BoxCollider2D>().enabled = false; // DEACTIVATE THE BOX COLLIDER 2D
        this.enabled = false; // DEACTIVATE THIS SCRIPT
        Destroy(gameObject);
    }
}
