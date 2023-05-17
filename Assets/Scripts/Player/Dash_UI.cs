using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_UI : MonoBehaviour
{
    public Slider slider;
    public Player_Gauge player_gauge;

    public IEnumerator DashCooldown()
    {
        slider.interactable = false;
        yield return new WaitForSeconds(3);
        if (player_gauge.current_gauge >= 300) slider.interactable = true;
    }

    public void SetTrue()
    {
        slider.interactable = true;
    }
}
