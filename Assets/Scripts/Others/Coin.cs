using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    bool unic = false;
    public Coin_Counter coin_counter;
    Rigidbody2D rb;

    private void Start()
    {
        coin_counter = GameObject.Find("Coin_counter").GetComponent<Coin_Counter>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !unic)
        {
            rb.velocity = Vector2.zero;
            unic = true;
            coin_counter.coin_count++;
            coin_counter.SetCoinCounter(coin_counter.coin_count);
            StartCoroutine(Anim());
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

        }
    }

    IEnumerator Anim()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 13), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        Destroy(gameObject);
    }
}
