using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSpawner : MonoBehaviour
{
    [Range(0f, 1f)]
    public float coefficient;
    private Camera cam;

    private float startPosition;

    private GameObject currentBackground;
    private GameObject previousBackground;
    public GameObject[] backgrounds;

    private float prefabWidth;
    private float prefabHeight;
    private float prevCamX = 0f;

    void Start()
    {
        cam = Camera.main;
        prefabWidth = backgrounds[0].GetComponent<SpriteRenderer>().size.x * 0.95f;
        prefabHeight = backgrounds[0].GetComponent<SpriteRenderer>().size.y;
        GameObject toSpawn = backgrounds[Random.Range(0, backgrounds.Length)];
        currentBackground = Instantiate(toSpawn, new Vector3(0, toSpawn.GetComponent<SpriteRenderer>().size.y / 2), Quaternion.identity);
    }

    void Update()
    {
        currentBackground.transform.position += new Vector3((cam.transform.position.x - prevCamX) * coefficient, 0);
        float dist = currentBackground.transform.position.x - cam.transform.position.x;
        if (dist <= 0)
        {
            if (previousBackground != null) Destroy(previousBackground);
            previousBackground = currentBackground;
            GameObject toSpawn = backgrounds[Random.Range(0, backgrounds.Length)];
            currentBackground = Instantiate(toSpawn, new Vector3(previousBackground.transform.position.x + prefabWidth, prefabHeight / 2), Quaternion.identity);
        }
        if (previousBackground != null)
        {
            previousBackground.transform.position = currentBackground.transform.position - new Vector3(prefabWidth, 0);
        }
        prevCamX = cam.transform.position.x;
    }
}
