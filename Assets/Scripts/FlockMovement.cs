using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;

public class FlockMovement : MonoBehaviour
{
    public float turningCoefficient = 1.5f;
    public float movementSpeed = 10;

    private Vector3 cameraOffset;

    private Camera mainCamera;

    public Transform leadingBird;

    public GameObject skyPlaceholder;
    public GameObject underwaterPlaceholder;

    void Start()
    {
        mainCamera = Camera.main;
        cameraOffset = mainCamera.transform.position - transform.position;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            movementSpeed = 60;
            DOTween.To(() => movementSpeed, x => movementSpeed = x, 10, 0.5f);
        }

        SetRotation();

        Vector3 deltaDistance = movementSpeed * leadingBird.right * Time.deltaTime;
        transform.position += deltaDistance;
        mainCamera.transform.position = transform.position + cameraOffset;
        skyPlaceholder.transform.position += new Vector3(deltaDistance.x, 0, 0);
        underwaterPlaceholder.transform.position += new Vector3(deltaDistance.x, 0, 0);

        ConstrainPositions();
    }


    private void SetRotation()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float angle = leadingBird.rotation.eulerAngles.z > 270 ? 360 - leadingBird.rotation.eulerAngles.z : leadingBird.rotation.eulerAngles.z;
        if (verticalInput != 0 && Input.GetButton("Vertical"))
        {
            leadingBird.Rotate(0, 0, verticalInput * (1 - (Mathf.Abs(angle % 90)) / 90) /*turningCoefficient*/);
        }
        else
        {
            if (leadingBird.rotation.eulerAngles.z >= 270)
            {
                leadingBird.Rotate(0, 0, 1 - (Mathf.Abs(angle % 90)) / 90);
            }
            else if (leadingBird.rotation.eulerAngles.z <= 90)
            {
                leadingBird.Rotate(0, 0, -(1 - (Mathf.Abs(angle % 90)) / 90));
            }

            if (leadingBird.rotation.eulerAngles.z <= 3 || leadingBird.rotation.eulerAngles.z >= 357)
            {
                leadingBird.rotation = Quaternion.identity;
            }
        }
        if (leadingBird.rotation.eulerAngles.z < 270 && leadingBird.rotation.eulerAngles.z > 180)
        {
            leadingBird.rotation = Quaternion.Euler(0, 0, -89);
        }
        else if (leadingBird.rotation.eulerAngles.z > 90 && leadingBird.rotation.eulerAngles.z < 180)
        {
            leadingBird.rotation = Quaternion.Euler(0, 0, 89);
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
