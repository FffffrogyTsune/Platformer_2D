using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Music_Secret : MonoBehaviour
{
    public AudioSource secret;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        secret.Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        secret.Stop();
    }
}
