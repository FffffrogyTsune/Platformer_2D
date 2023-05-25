using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    bool wesh = false;
    public Coin_Counter coin_counter;

    private void Start()
    {
        coin_counter = GameObject.Find("Coin_counter").GetComponent<Coin_Counter>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !wesh)
        {
            wesh = true;
            coin_counter.coin_count++;
            coin_counter.SetCoinCounter(coin_counter.coin_count);
            Destroy(gameObject);
        }
    }
}
