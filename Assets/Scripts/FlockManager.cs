using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public float turningCoefficient = 1.5f;
    private float movementSpeed = 20;

    private int flockCount = 1;

    private Vector3 cameraOffset;

    private Camera mainCamera;

    public GameObject skyPlaceholder;
    public GameObject underwaterPlaceholder;
    public GameObject birdPrefab;

    private List<GameObject> flock;

    void Start()
    {
        mainCamera = Camera.main;
        cameraOffset = mainCamera.transform.position - transform.position;
        flock.Add(transform.GetChild(0).gameObject);
    }

    void Update()
    {

        SetRotation();

        Vector3 deltaDistance = movementSpeed * transform.right * Time.deltaTime;
        transform.position += deltaDistance;
        mainCamera.transform.position = transform.position + cameraOffset;
        skyPlaceholder.transform.position += new Vector3(deltaDistance.x, 0, 0);
        underwaterPlaceholder.transform.position += new Vector3(deltaDistance.x, 0, 0);

        ConstrainPositions();
    }


    private void SetRotation()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float angle = transform.rotation.eulerAngles.z > 270 ? 360 - transform.rotation.eulerAngles.z : transform.rotation.eulerAngles.z;
        if (verticalInput != 0 && Input.GetButton("Vertical"))
        {
            transform.Rotate(0, 0, verticalInput * (1 - (Mathf.Abs(angle % 90)) / 90) /*turningCoefficient*/);
        }
        else
        {
            if (transform.rotation.eulerAngles.z >= 270)
            {
                transform.Rotate(0, 0, 1 - (Mathf.Abs(angle % 90)) / 90);
            }
            else if (transform.rotation.eulerAngles.z <= 90)
            {
                transform.Rotate(0, 0, -(1 - (Mathf.Abs(angle % 90)) / 90));
            }

            if (transform.rotation.eulerAngles.z <= 3 || transform.rotation.eulerAngles.z >= 357)
            {
                transform.rotation = Quaternion.identity;
            }
        }
        if (transform.rotation.eulerAngles.z < 270 && transform.rotation.eulerAngles.z > 180)
        {
            transform.rotation = Quaternion.Euler(0, 0, -89);
        }
        else if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 180)
        {
            transform.rotation = Quaternion.Euler(0, 0, 89);
        }
    }

    private void ConstrainPositions()
    {
        if (transform.position.y > skyPlaceholder.transform.position.y + skyPlaceholder.transform.localScale.y / 2)
        {
            transform.position = new Vector3(transform.position.x, skyPlaceholder.transform.position.y + skyPlaceholder.transform.localScale.y / 2, transform.position.z);
        }
        else if (transform.position.y < underwaterPlaceholder.transform.position.y - underwaterPlaceholder.transform.localScale.y / 2)
        {
            transform.position = new Vector3(transform.position.x, underwaterPlaceholder.transform.position.y - skyPlaceholder.transform.localScale.y / 2, transform.position.z);
        }

        if (mainCamera.transform.position.y > skyPlaceholder.transform.position.y)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, skyPlaceholder.transform.position.y, mainCamera.transform.position.z);
        }
        else if (mainCamera.transform.position.y < underwaterPlaceholder.transform.position.y)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, underwaterPlaceholder.transform.position.y, mainCamera.transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Star")
        {
            AddAnimalToFlock();
            Destroy(other.gameObject);
        }
    }

    private void AddAnimalToFlock()
    {
        flockCount++;
        GameObject newAnimal = Instantiate(birdPrefab, transform.position + new Vector3(0, Random.Range(), 0), transform.rotation, transform);
    }
}
