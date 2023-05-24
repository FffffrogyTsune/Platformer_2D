using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Rigidbody2D coin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Break()
    {
        for (int i = 0; i <= 1; i++)
        {
            Rigidbody2D Coin_0 = Instantiate(coin, transform.position, transform.rotation);
            Coin_0.velocity = new Vector2(Random.Range(-4, 4), 12);
        }
    }
    
}
