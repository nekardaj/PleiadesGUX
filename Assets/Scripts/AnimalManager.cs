using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public Sprite birdSprite;
    public Sprite fishSprite;

    private SpriteRenderer sprite;
    public FlockManager manager;
    public FlockMovement movement;

    private float rotationSpeed = 10f;
    public float speed = 5f;

    private GameObject cube;

    public Vector3 groupCentre;

    private void Start()
    {
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    private void Update()
    {
        if (manager.leadingAnimal == gameObject) return;

        groupCentre = Vector3.zero;
        Vector3 avoidVector = Vector3.zero;
        float groupSpeed = 0.01f;
        int groupSize = 0;

        foreach(GameObject animal in manager.flock)
        {
            if (animal != gameObject)
            {
                groupCentre += animal.transform.position;
                groupSize++;

                float distance = Vector3.Distance(transform.position, animal.transform.position);
                if (distance < 3.0f)
                {
                    avoidVector += transform.position - animal.transform.position;
                }

                groupSpeed += animal.GetComponent<AnimalManager>().speed;
            }
        }

        if (groupSize > 0)
        {
            groupCentre /= groupSize;
            //groupCentre += (manager.flock[0].transform.position - new Vector3(3, 0, 0)) - transform.position;
            speed = Mathf.Min(groupSpeed / groupSize, movement.movementSpeed * 1.1f);

            Vector3 direction = groupCentre + avoidVector - transform.position;
            float distance = Vector3.Distance(transform.position, manager.leadingAnimal.transform.position);
            if (distance > 6)
            {
                speed = movement.movementSpeed * 1.5f;
                direction = manager.leadingAnimal.transform.position - transform.position;
            }

            //cube.transform.position = transform.position + direction;
            if (direction != Vector3.zero)
            {
                //transform.LookAt(transform.position + direction);
                //transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }

        transform.position += speed * Time.deltaTime * transform.forward;
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water") {
            manager.isInWater = true;
            sprite.sprite = fishSprite;
        }
        else
        {
            manager.SolveCollisionEnter(collision);
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
        if (collision.gameObject.tag == "Water") {
            manager.isInWater = false;
            sprite.sprite = birdSprite;
        }
        else
        {
            manager.SolveCollisionExit(collision);
        }
    }
}
