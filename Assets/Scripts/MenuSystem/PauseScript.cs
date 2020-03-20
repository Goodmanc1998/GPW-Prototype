using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static bool paused = false;

    public GameObject pauseUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) Unpause();
            else Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        paused = true;
        pauseUI.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        paused = false;
        pauseUI.SetActive(false);
    }

    public void OnMainMenu()
    {
        Unpause();
        SceneManager.LoadScene(0);
    }

    public void OnQuit()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
