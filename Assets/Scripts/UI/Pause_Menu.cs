using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pause_menu_ui;
    public Animator music;
    public float wait_time;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (paused)
            {
                Resume();
            }
            else Pause();
        }
    }

    public void Resume()
    {
        pause_menu_ui.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    void Pause()
    {
        pause_menu_ui.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

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
