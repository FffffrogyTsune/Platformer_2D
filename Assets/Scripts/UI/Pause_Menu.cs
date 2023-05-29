using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pause_menu_ui;
    public GameObject settings_menu;
    public static bool settings_menu_activated;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (paused || settings_menu_activated)
            {
                Resume();
            }
            else Pause();
        }
    }

    public void Resume()
    {
        pause_menu_ui.SetActive(false);
        settings_menu.SetActive(false);
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
        SceneManager.LoadScene(0);
    }
}
