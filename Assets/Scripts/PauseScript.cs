using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool paused = false;
    public Text pauseText;
    public GameObject pauseFirstButton, restartButton, menuButton, slider;

    public float timer = 0;

    private bool gameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            PauseGame();
        }

        timer += Time.deltaTime;
    }
    public void PauseGame() //simple pause toggle
    {
        if (paused && !gameOver) //stop from unpausing during gameOver
        {
            pauseMenu.gameObject.SetActive(false);
            paused = false;
            Time.timeScale = 1;
        } else //not paused
        {
            pauseMenu.gameObject.SetActive(true);
            paused = true;
            Time.timeScale = 0;

            pauseText.text = "Paused\nTime: " + Mathf.Round(timer) + " Seconds";

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
    }

    public void ReloadLevel() //reloads the current level
    {
        if (paused)
            PauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        if (paused)
            PauseGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoseGame()
    {        
        pauseFirstButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        pauseText.text = "You Got Caught!\nTime: " + Mathf.Round(timer) + " Seconds";

        //PauseGame();
        
        pauseMenu.gameObject.SetActive(true);
        paused = true;
        gameOver = true;
        Time.timeScale = 0;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restartButton);
    }

    public void WinGame()
    {
        pauseFirstButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        pauseText.text = "You Escaped!\nTime: " + Mathf.Round(timer) + " Seconds";

        pauseMenu.gameObject.SetActive(true);
        paused = true;
        gameOver = true;
        Time.timeScale = 0;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuButton);
    }


}
