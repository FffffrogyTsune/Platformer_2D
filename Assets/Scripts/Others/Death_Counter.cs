using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Death_Counter : MonoBehaviour
{
    public TextMeshProUGUI text_mesh_pro;
    public int death_count = 0;

    private void Start()
    {
        text_mesh_pro.GetComponent<TextMeshPro>();
    }

    public void SetHitCounter(int count)
    {
        text_mesh_pro.SetText(count.ToString());
    }
}
