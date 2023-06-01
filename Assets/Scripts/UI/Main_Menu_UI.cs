using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu_UI : MonoBehaviour
{
    public int game_start_scene;
    public Animator music;
    public Animator screen;
    public float wait_time;

    public void StartGame()
    {
        StartCoroutine(ChangeScene());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator ChangeScene()
    {
        music.SetTrigger("Fade_Out");
        screen.SetTrigger("End");
        yield return new WaitForSeconds(wait_time);
        SceneManager.LoadScene(game_start_scene);
    }
}
