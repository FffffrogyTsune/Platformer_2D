using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Next_Scene : MonoBehaviour
{
    [SerializeField] Coin_Counter coin_counter;

    private void Start()
    {
        coin_counter.coin_count = Coin_Counter.total_coin;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Coin_Counter.total_coin = coin_counter.coin_count;
            SceneManager.LoadScene(2);
        }
    }
}
