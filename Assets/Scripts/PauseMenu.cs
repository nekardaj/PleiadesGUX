using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    private AudioSource backgroundMusic;

    void Start()
    {
        backgroundMusic = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }  

    public void Resume () 
          
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    } 

    void Pause () 
          
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    } 

    public void Restart()
    {  
        Time.timeScale = 1f;
        SceneManager.LoadScene("MovementScene");
        Debug.Log("Loading menu...");
    }

    public void QuitGame()
    {  
        Debug.Log("Quitting game...");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
	    Application.Quit();
#endif

    }

    public void MusicSetter()
    {
        if (backgroundMusic.volume == 1)
        {
            backgroundMusic.volume = 0;
        }
        else
        {
            backgroundMusic.volume = 1;
        }
    }


}
