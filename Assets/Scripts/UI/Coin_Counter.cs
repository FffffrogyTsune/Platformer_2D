using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Coin_Counter : MonoBehaviour
{
    public TextMeshProUGUI text_mesh_pro;
    public int coin_count = 0;

    private void Start()
    {
        text_mesh_pro.GetComponent<TextMeshPro>();
    }

    public void SetCoinCounter(int count)
    {
        text_mesh_pro.SetText(count.ToString());
    }
}
