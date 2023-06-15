using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class pulsingEffect : MonoBehaviour
{
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
