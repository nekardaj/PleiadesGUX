using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMovement : MonoBehaviour
{
    public float turningCoefficient = 3;
    private float movementSpeed = 20;

    private int last10X = -2;

    private Vector3 cameraOffset;

    private Camera camera;

    public GameObject squarePlaceholder;
    public GameObject skyPlaceholder;
    public GameObject underwaterPlaceholder;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        cameraOffset = camera.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)transform.position.x / 10 > last10X)
        {
            SpawnEnvironmentPlaceholder();
            last10X++;
        }

        SetRotation();

        Vector3 deltaDistance = movementSpeed * transform.right * Time.deltaTime;
        transform.position += deltaDistance;
        camera.transform.position = transform.position + cameraOffset;
        skyPlaceholder.transform.position += new Vector3(deltaDistance.x, 0, 0);
        underwaterPlaceholder.transform.position += new Vector3(deltaDistance.x, 0, 0);

        ConstrainPositions();
    }

    private void SpawnEnvironmentPlaceholder()
    {
        GameObject square = Instantiate(squarePlaceholder, new Vector3(transform.position.x, 0, 0) + Vector3.right * 50, Quaternion.identity);
        StartCoroutine(RemoveEnvironmentPlaceholder(square));
    }

    IEnumerator RemoveEnvironmentPlaceholder(GameObject square)
    {
        yield return new WaitForSeconds(10f);
        Destroy(square);
    }

    private void SetRotation()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput != 0 && Input.GetButton("Vertical"))
        {
            transform.Rotate(0, 0, verticalInput * turningCoefficient);
        }
        else
        {
            if (transform.rotation.eulerAngles.z >= 270)
            {
                transform.Rotate(0, 0, turningCoefficient);
            }
            else if (transform.rotation.eulerAngles.z <= 90)
            {
                transform.Rotate(0, 0, -turningCoefficient);
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

        if (camera.transform.position.y > skyPlaceholder.transform.position.y)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, skyPlaceholder.transform.position.y, camera.transform.position.z);
        }
        else if (camera.transform.position.y < underwaterPlaceholder.transform.position.y)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, underwaterPlaceholder.transform.position.y, camera.transform.position.z);
        }
    }
}
