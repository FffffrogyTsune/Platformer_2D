using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gauge : MonoBehaviour
{
    public int max_gauge = 1000;
    public int current_gauge;

    public Gauge_Bar gauge_bar;


    // Start is called before the first frame update
    void Start()
    {
        current_gauge = max_gauge; // PLAYER GET THE MAX GAUGE AT THE BEGINING
        gauge_bar.SetMaxGauge(max_gauge);
    }

    //  REDUCE
    public void Reduce(int cost)
    {
        current_gauge -= cost;
        gauge_bar.SetGauge(current_gauge);
    }
}
