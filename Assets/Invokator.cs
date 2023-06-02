using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invokator : MonoBehaviour
{
    public Enemy_02 enemy_02;
    public Player_Health player_health;
    float next_invoke = 0f;
    public float enemy_height;
    [SerializeField] Rigidbody2D current_enemy;
    public Rigidbody2D knight;

    // Start is called before the first frame update
    void Start()
    {
        next_invoke = Time.time + 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (current_enemy != null)
        {
            if (current_enemy.GetComponent<Enemy>().health <= 0 || player_health.current_health <= 0 || enemy_02.health <= 0)
            {
                Destroy(current_enemy.gameObject);
                current_enemy = null;
            }
        }
        
        if (Time.time >= next_invoke && current_enemy != null) next_invoke = Time.time + 15f;
        if (Time.time >= next_invoke && enemy_02.health >= 0 && current_enemy == null)
        {
            Rigidbody2D enemy = Instantiate(knight, transform.position, transform.rotation);
            enemy.GetComponent<Knight>().height = enemy_height;
            enemy.GetComponent<Knight>().boss_battle = true;
            current_enemy = enemy;
            next_invoke = Time.time + 15f;
        }
    }
}
