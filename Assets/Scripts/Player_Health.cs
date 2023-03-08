using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    public int max_health = 100;
    public int current_health;

    public Health_Bar health_bar;

    int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health; // PLAYER GET THE MAX HEATH AT THE BEGINING
        health_bar.SetMaxHealth(max_health);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && current_health >= damage) // TEST
        {
            TakeDamage(damage);
            Debug.Log("-10 point !");
        }
    }

    //  TAKE DAMAGE
    public void TakeDamage(int damage)
    {
        current_health -= damage;
        health_bar.SetHealth(current_health);
    }
}
