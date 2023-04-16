using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour
{
    public Sprite BirdSprite;
    public Sprite FishSprite;

    private SpriteRenderer spr;
    public bool isFish;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water") {
            spr.sprite = FishSprite;
            isFish = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water") {
            isFish = false;
            spr.sprite = BirdSprite;
        }
    }
}
