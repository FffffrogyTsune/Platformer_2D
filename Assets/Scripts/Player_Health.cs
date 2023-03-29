using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    public int max_health = 100;
    public int current_health;
    public Health_Bar health_bar;

    int damage = 10; // TEST

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
        if (current_health > max_health) // CHECKS AT EVERY UPDATES THE HEALTH OF THE PLAYER, IF IT'S OVER THE MAX HEALTH, IT FIXES THE HEALTH TO MAX HEATH
        {
            current_health = max_health;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Health_orb") && current_health < max_health)
        {
            GetHealth(5);
            Destroy(collision.gameObject);
        }
    }

    //  TAKE DAMAGE
    public void TakeDamage(int damage)
    {
        current_health -= damage;
        health_bar.SetHealth(current_health);
    }

    public void GetHealth(int health)
    {
        current_health += health;
        health_bar.SetHealth(current_health);
    }
}
