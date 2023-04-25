using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour
{
    public Sprite birdSprite;
    public Sprite fishSprite;

    private SpriteRenderer sprite;
    public FlockManager flock;
    public bool isFish;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water") {
            sprite.sprite = fishSprite;
            isFish = true;
        }
        else
        {
            flock.SolveCollisionEnter(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water") {
            isFish = false;
            sprite.sprite = birdSprite;
        }
        else
        {
            flock.SolveCollisionExit(collision);
        }
    }
}
