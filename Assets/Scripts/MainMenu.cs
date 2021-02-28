using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject playButton, backButton, backButton2;
    public GameObject mainMenu, credits, controls;
    public void PlayGame() 
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }
    public void Main()
    {
        mainMenu.SetActive(true);
        controls.SetActive(false);
        credits.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void Controls()
    {
        mainMenu.SetActive(false);
        controls.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton2);
    }

    public void QuitGame () {
        Debug.Log ("QUIT!");
        Application.Quit();
    }

}
