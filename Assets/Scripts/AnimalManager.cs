using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public Sprite birdSprite;
    public Sprite fishSprite;

    private SpriteRenderer sprite;
    public FlockManager flock;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water") {
            flock.isInWater = true;
            sprite.sprite = fishSprite;
        }
        else
        {
            flock.SolveCollisionEnter(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water") {
            flock.isInWater = false;
            sprite.sprite = birdSprite;
        }
        else
        {
            flock.SolveCollisionExit(collision);
        }
    }
}
