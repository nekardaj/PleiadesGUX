using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    private GameObject flock;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        flock = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene(0);
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();
        }
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    flock.GetComponent<FlockMovement>().turningCoefficient++;
        //    Mathf.Clamp(flock.GetComponent<FlockMovement>().turningCoefficient, 1, 10);
        //}
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    flock.GetComponent<FlockMovement>().turningCoefficient--;
        //    Mathf.Clamp(flock.GetComponent<FlockMovement>().turningCoefficient, 1, 10);
        //}
    }
}
