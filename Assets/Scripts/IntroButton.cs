using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. SceneManagement;

public class IntroButton : MonoBehaviour {

    public FadeManager manager;
    public void Play()
    {
        manager.FadeToLevel(1);
        Debug.Log($"[log] Started game with version {ParametersManager.GetParameterSettings()}");
        //Time.timeScale = 1f;
        //SceneManager.LoadScene("MovementScene");
        //Debug.Log("Play");
    }


}
