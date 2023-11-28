using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class FlockMovement : MonoBehaviour
{
    [Range(0.0f, 20.0f)]
    public float turningCoefficient = 10f;
    //[Range(0.0f, 20.0f)]
    public float movementSpeed = 5;

    private bool inTween = false;
    public bool inFormation = false;

    private Vector3 cameraOffset;

    private Camera mainCamera;

    private FlockManager flockManager;

    public Transform leadingAnimal;

    public GameObject skyPlaceholder;
    public GameObject underwaterPlaceholder;

    private TweenerCore<float, float, FloatOptions> tween;

    private float _vertical;

    public float ReactionSpeed = 1.0f; // Speed at which the value goes towards 1 or -1
    public float DecaySpeed = 2f;    // Decay speed of returning to zero

    private float currentRotationGuide = 0.0f;

    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.position += new Vector3(0, leadingAnimal.transform.position.y, 0);
        cameraOffset = mainCamera.transform.position - leadingAnimal.transform.position;
        flockManager = GetComponent<FlockManager>();
        tween = DOTween.To(() => movementSpeed, x => movementSpeed = x, movementSpeed * 2, 30);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inTween)
        {
            if (flockManager.isInWater)
            {
                inTween = true;
                inFormation = true;
                StartCoroutine(TweenChecker(1f));
            }
            else
            {
                foreach(GameObject animal in flockManager.flock)
                {
                    animal.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Space");
                }
                inTween = true;
                tween.Pause();
                float prevMovementSpeed = movementSpeed;
                movementSpeed *= 6;
                DOTween.To(() => movementSpeed, x => movementSpeed = x, prevMovementSpeed, 0.5f).OnComplete(() => { tween.Play(); });
                StartCoroutine(TweenChecker(0.5f));
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) && inFormation)
        {
            inFormation = false;
            StartCoroutine(TweenChecker(1f));
        }

        leadingAnimal.GetComponent<AnimalManager>().speed = movementSpeed;

        Vector3 deltaDistance = movementSpeed * leadingAnimal.right * Time.deltaTime;
        leadingAnimal.transform.localPosition += deltaDistance;
        AdjustCamera();
        skyPlaceholder.transform.position += new Vector3(deltaDistance.x, 0, 0);
        underwaterPlaceholder.transform.position += new Vector3(deltaDistance.x, 0, 0);

        ConstrainPositions();

        _vertical = Input.GetAxisRaw("Vertical");
        if (_vertical == 1)
        {
            currentRotationGuide = Mathf.MoveTowards(currentRotationGuide, 1.0f, ReactionSpeed * Time.deltaTime);
        }
        else if (_vertical == -1)
        {
            currentRotationGuide = Mathf.MoveTowards(currentRotationGuide, -1.0f, ReactionSpeed * Time.deltaTime);
        }
        else
        {
            currentRotationGuide = Mathf.MoveTowards(currentRotationGuide, 0.0f, DecaySpeed * Time.deltaTime);
        }
        //if (currentRotationGuide != 0)
        //{
        //    Debug.Log(currentRotationGuide);
        //}
    }

    private void FixedUpdate()
    {
        SetRotation();
    }

    private void AdjustCamera()
    {
        float defaultY = (leadingAnimal.transform.position + cameraOffset).y;
        float modifiedY = (defaultY / (skyPlaceholder.transform.position.y - underwaterPlaceholder.transform.position.y * 0.9f) + 1) / 2;
        float easedY = EaseInOutSigmoid(underwaterPlaceholder.transform.position.y * 0.9f, skyPlaceholder.transform.position.y, modifiedY, 12.5f);
        mainCamera.transform.position = new Vector3(leadingAnimal.transform.position.x + cameraOffset.x, easedY, cameraOffset.z);
    }

    private void SetRotation()
    {
        leadingAnimal.rotation = Quaternion.Euler(0, 0, currentRotationGuide * 90);
    }


    private void SetRotationOld()
    {
        //float verticalInput = Input.GetAxis("Vertical");
        //leadingAnimal.transform.position += verticalInput * Time.fixedDeltaTime * new Vector3(0, 4, 0);

        float verticalInput = Input.GetAxis("Vertical");
        float angle;
        if (leadingAnimal.rotation.eulerAngles.z > 270)
        {
            angle = 360 - leadingAnimal.rotation.eulerAngles.z;
        }
        else
        {
            angle = leadingAnimal.rotation.eulerAngles.z;
        }


        if (verticalInput != 0 && Input.GetButton("Vertical"))
        {
            leadingAnimal.Rotate(0, 0, verticalInput * (1 - (Mathf.Abs(angle % 90)) / 90) * turningCoefficient);
        }
        else
        {
            if (leadingAnimal.rotation.eulerAngles.z <= 5 || leadingAnimal.rotation.eulerAngles.z >= 355)
            {
                leadingAnimal.rotation = Quaternion.identity;
            }
            else if (leadingAnimal.rotation.eulerAngles.z >= 270)
            {
                leadingAnimal.Rotate(0, 0, (1 - (Mathf.Abs(angle % 90)) / 90) * turningCoefficient);
            }
            else if (leadingAnimal.rotation.eulerAngles.z <= 90)
            {
                leadingAnimal.Rotate(0, 0, -(1 - (Mathf.Abs(angle % 90)) / 90) * turningCoefficient);
            }

        }
        if (leadingAnimal.rotation.eulerAngles.z < 270 && leadingAnimal.rotation.eulerAngles.z > 180)
        {
            leadingAnimal.rotation = Quaternion.Euler(0, 0, 271);
        }
        else if (leadingAnimal.rotation.eulerAngles.z > 90 && leadingAnimal.rotation.eulerAngles.z < 180)
        {
            leadingAnimal.rotation = Quaternion.Euler(0, 0, 89);
        }
    }

    private void ConstrainPositions()
    {
        if (leadingAnimal.transform.position.y > skyPlaceholder.transform.position.y + skyPlaceholder.transform.localScale.y / 2)
        {
            leadingAnimal.transform.position = new Vector3(leadingAnimal.transform.position.x, skyPlaceholder.transform.position.y + skyPlaceholder.transform.localScale.y / 2, leadingAnimal.transform.position.z);
        }
        else if (leadingAnimal.transform.position.y < underwaterPlaceholder.transform.position.y - underwaterPlaceholder.transform.localScale.y / 2)
        {
            leadingAnimal.transform.position = new Vector3(leadingAnimal.transform.position.x, underwaterPlaceholder.transform.position.y - skyPlaceholder.transform.localScale.y / 2, leadingAnimal.transform.position.z);
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

    private IEnumerator TweenChecker(float duration)
    {
        yield return new WaitForSeconds(duration);
        inTween = false;
    }

    float EaseInOutSigmoid(float startValue, float endValue, float time, float steepness)
    {
        float difference = endValue - startValue;
        float sigmoid = 1 / (1 + Mathf.Exp(-steepness * (time - 0.5f)));
        float finalValue = sigmoid * difference + startValue;
        return finalValue;
    }
}
