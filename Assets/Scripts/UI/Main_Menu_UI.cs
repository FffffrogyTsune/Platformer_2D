using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu_UI : MonoBehaviour
{
    public int game_start_scene;

    public void StartGame()
    {
        SceneManager.LoadScene(game_start_scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
