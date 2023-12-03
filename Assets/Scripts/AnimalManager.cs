using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public bool isLeader = false;
    public Sprite birdSprite;
    public Sprite fishSprite;

    private SpriteRenderer sprite;
    private Animator animator;
    public FlockManager manager;
    public FlockMovement movement;

    private float rotationSpeed = 5f;
    public float speed = 5f;

    public float angleTemp = 0f;

    public Vector3 groupCentre;

    private void Start()
    {
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    private void Update()
    {
        if (manager.leadingAnimal == gameObject) return;
        if (movement.inFormation && manager.isInWater)
        {
            Vector3 direction = transform.parent.GetChild(transform.GetSiblingIndex() - 1).position - transform.position;
            if (direction != Vector3.zero)
            {
                angleTemp = Vector2.SignedAngle(transform.right, direction) * rotationSpeed * 5 * Time.deltaTime;
                transform.Rotate(0, 0, angleTemp);
            }
            speed = movement.movementSpeed * direction.magnitude;
            if (speed > movement.movementSpeed * 1.5f)
            {
                speed = movement.movementSpeed * 1.5f;
            }
        }
        else
        {
            groupCentre = Vector3.zero;
            Vector3 avoidVector = Vector3.zero;
            float groupSpeed = 0.01f;
            int groupSize = 0;

            foreach (GameObject animal in manager.flock)
            {
                if (animal != gameObject)
                {
                    groupCentre += animal.transform.position;
                    groupSize++;

                    float distance = Vector3.Distance(transform.position, animal.transform.position);
                    if (distance <= 1.0f)
                    {
                        avoidVector += (transform.position - animal.transform.position) / distance;
                    }

                    groupSpeed += animal.GetComponent<AnimalManager>().speed;
                }
            }

            if (groupSize > 0)
            {
                groupCentre /= groupSize;
                speed = groupSpeed / groupSize;

                Vector3 direction = groupCentre + avoidVector - transform.position;
                float distance = Vector3.Distance(transform.position, manager.leadingAnimal.transform.position);
                if (distance >= 4)
                {
                    speed = movement.movementSpeed * 1.5f;
                    direction = manager.leadingAnimal.transform.position - transform.position;
                    angleTemp = Vector2.SignedAngle(transform.right, direction);
                    transform.Rotate(0, 0, angleTemp);
                }
                if (direction != Vector3.zero)
                {
                    angleTemp = Vector2.SignedAngle(transform.right, direction) * rotationSpeed * Time.deltaTime;
                    transform.Rotate(0, 0, angleTemp);
                }
                //speed = movement.movementSpeed * direction.magnitude;
            }
        }
        //speed /= Mathf.Max(Mathf.Abs(angleTemp), 1);
        Mathf.Clamp(speed, movement.movementSpeed / 2, movement.movementSpeed * 1.5f);
        transform.position += speed * Time.deltaTime * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            manager.isInWater = true;
            animator.enabled = false;
            sprite.sprite = fishSprite;
        }
        else
        {
            if (isLeader)
            {
                manager.SolveCollisionEnter(collision);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            manager.isInWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            manager.isInWater = false;
            sprite.sprite = birdSprite;
            animator.enabled = true;
        }
        else
        {
            manager.SolveCollisionExit(collision);
        }
    }
}
