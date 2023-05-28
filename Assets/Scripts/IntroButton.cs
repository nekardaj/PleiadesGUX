using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. SceneManagement;

public class IntroButton : MonoBehaviour {

    public void Play()
    {  
        Time.timeScale = 1f;
        SceneManager.LoadScene("MovementScene");
        Debug.Log("Play");
    }


}
