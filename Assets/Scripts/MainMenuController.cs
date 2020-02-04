using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject helpMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void showHelp()
    {
        helpMenu.SetActive(true);
    }

    public void hideHelp()
    {
        helpMenu.SetActive(false);
    }
}
