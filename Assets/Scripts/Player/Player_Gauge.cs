using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gauge : MonoBehaviour
{
    public int max_gauge = 900;
    public int current_gauge;
    float next_add_time = 0f;
    public Gauge_Bar gauge_bar;
    
    // Start is called before the first frame update
    void Start()
    {
        current_gauge = max_gauge; // PLAYER GET THE MAX GAUGE AT THE BEGINING
        gauge_bar.SetMaxGauge(max_gauge);
    }

    // Update is called once per frame
    void Update()
    {
        if (current_gauge > max_gauge) // CHECKS AT EVERY UPDATES THE HEALTH OF THE PLAYER, IF IT'S OVER THE MAX HEALTH, IT FIXES THE HEALTH TO MAX HEATH
        {
            current_gauge = max_gauge;
        }

        if (current_gauge < max_gauge && next_add_time >= 2)
        {
            AddGauge(15);
            next_add_time = 0;
        }
        else next_add_time += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gauge_orb"))
        {
            if (current_gauge < max_gauge)
            {
                AddGauge(50);
            }
            Destroy(collision.gameObject);
        }
    }

    //  REDUCE
    public void Reduce(int cost)
    {
        current_gauge -= cost;
        gauge_bar.SetGauge(current_gauge);
    }

    // ADD GAUGE
    public void AddGauge(int gain)
    {
        current_gauge += gain;
        gauge_bar.SetGauge(current_gauge);
    }
}
