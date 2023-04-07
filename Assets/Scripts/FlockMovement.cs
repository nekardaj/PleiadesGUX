using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlockMovement : MonoBehaviour
{
    public float turningCoefficient = 3;
    private float movementSpeed = 20;

    private Vector3 cameraOffset;

    private Camera mainCamera;

    public GameObject squarePlaceholder;
    public GameObject skyPlaceholder;
    public GameObject underwaterPlaceholder;

    void Start()
    {
        mainCamera = Camera.main;
        cameraOffset = mainCamera.transform.position - transform.position;
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

        if (mainCamera.transform.position.y > skyPlaceholder.transform.position.y)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, skyPlaceholder.transform.position.y, mainCamera.transform.position.z);
        }
        else if (mainCamera.transform.position.y < underwaterPlaceholder.transform.position.y)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, underwaterPlaceholder.transform.position.y, mainCamera.transform.position.z);
        }
    }
}
