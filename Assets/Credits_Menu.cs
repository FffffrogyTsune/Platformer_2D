using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits_Menu : MonoBehaviour
{
    public Animator music;
    public float wait_time;

    public void Menu()
    {
        Time.timeScale = 1f;
        StartCoroutine(ChangeScene());
    }
    IEnumerator ChangeScene()
    {
        music.SetTrigger("Fade_Out");
        yield return new WaitForSeconds(wait_time);
        SceneManager.LoadScene(0);
    }
}
