using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void HelpClean()
    {
        SceneManager.LoadScene(4);
    }
    public void Story()
    {
        SceneManager.LoadScene(4);
    }

    public void Sandbox()
    {
        SceneManager.LoadScene(1);
    }

    public void Campaign()
    {
        SceneManager.LoadScene(7);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }



}
