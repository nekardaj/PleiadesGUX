using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSpawner : MonoBehaviour
{
    // Spawnovat v tomhle poradi (od nejblizsiho po nejvzdalenejsi)
    public GameObject[] firstLayer;
    public GameObject[] secondLayer;
    public GameObject[] thirdLayer;
    public GameObject[] sfumatoLayer;
    public GameObject[] skyLayer;

    public float coefficient = 1.5f;

    private FlockMovement movement;
    private FlockManager manager;

    private float length, startPosition;
    private Camera cam;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        movement = player.GetComponent<FlockMovement>();
        manager = player.GetComponent<FlockManager>();
        cam = Camera.main;
    }

    void Update()
    {

    }
}
