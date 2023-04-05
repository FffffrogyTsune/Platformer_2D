using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge_Bar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxGauge(int gauge)
    {
        slider.maxValue = gauge;
        slider.value = gauge;
    }

    public void SetGauge(int gauge)
    {
        slider.value = gauge;
    }
}
