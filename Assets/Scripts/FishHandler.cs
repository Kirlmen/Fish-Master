using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class FishHandler : MonoBehaviour
{
    private FishHandler.FishType type;
    private CircleCollider2D coll;
    private SpriteRenderer spriteRenderer;
    private float screenLeft;
    private Tweener tweener;

    public FishHandler.FishType Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            coll.radius = type.colliderRadius;
            spriteRenderer.sprite = type.sprite;
        }
    }


    void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }

    public void ResetFish()
    {
        if (tweener != null)
        {
            tweener.Kill(false);
        }

        //spawn
        float num = UnityEngine.Random.Range(type.minLength, type.maxLength); //random spawn deep
        coll.enabled = true;
        Vector3 position = transform.position;

        //where to go
        position.y = num;
        position.x = screenLeft;
        transform.position = position;

        //turn the fish when it reaches border of the screen
        float num2 = 1;
        float y = UnityEngine.Random.Range(num - num2, num + num2);
        Vector2 v = new Vector2(-position.x, y);

        float num3 = 3;
        float delay = UnityEngine.Random.Range(0, 2 * num3);
        tweener = transform.DOMove(v, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        });

    }


    public void FishHooked()
    {
        coll.enabled = false;
        tweener.Kill(false);
    }

    [Serializable]
    public class FishType
    {
        public int price;
        public float fishCount;
        public float minLength;
        public float maxLength;
        public float colliderRadius;
        public Sprite sprite;

    }
}
