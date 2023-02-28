using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_01 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("test");
            Destroy(GameObject.Find("Gate_01"));
            Destroy(GameObject.Find("Gate_02"));
        }
    }
}
