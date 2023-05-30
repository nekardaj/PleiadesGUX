using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSpawner : MonoBehaviour
{
    public float coefficient;
    public Camera cam;

    private float startPosition;

    private GameObject currentBackground;
    private GameObject previousBackground;
    public GameObject[] backgrounds;

    private float prefabWidth;
    private float prefabHeight;

    void Start()
    {
        prefabWidth = backgrounds[0].GetComponent<SpriteRenderer>().size.x * 0.95f;
        prefabHeight = backgrounds[0].GetComponent<SpriteRenderer>().size.y;
        GameObject toSpawn = backgrounds[Random.Range(0, backgrounds.Length)];
        currentBackground = Instantiate(toSpawn, new Vector3(0, toSpawn.GetComponent<SpriteRenderer>().size.y / 2), Quaternion.identity);
    }

    void FixedUpdate()
    {
        /*
        float temp = cam.transform.position.x * (1 - coefficient);
        float distance = cam.transform.position.x * coefficient;
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);
        if (temp > startPosition + length) startPosition += length;
        else if (temp < startPosition - length) startPosition -= length;
        */
        currentBackground.transform.position = new Vector3(cam.transform.position.x * coefficient, currentBackground.transform.position.y);
        if (cam.transform.position.x - currentBackground.transform.position.x <= 0)
        {
            previousBackground = currentBackground;
            GameObject toSpawn = backgrounds[Random.Range(0, backgrounds.Length)];
            currentBackground = Instantiate(toSpawn, new Vector3(previousBackground.transform.position.x + prefabWidth / 2, prefabHeight / 2), Quaternion.identity);
        }
        if (previousBackground != null)
        {
            previousBackground.transform.position = new Vector3(cam.transform.position.x * coefficient - prefabWidth, previousBackground.transform.position.y);
        }
    }
}
