using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class HookHandler : MonoBehaviour
{
    [SerializeField] Transform hookedTransform;


    private Camera mainCamera;
    private Collider2D coll;
    private int length;
    private int strength;
    private int fishCount;
    private bool canMove;
    private Tweener cameraTween;
    private List<FishHandler> hookedFishes;


    private void Awake()
    {
        mainCamera = Camera.main;
        coll = GetComponent<Collider2D>();
        hookedFishes = new List<FishHandler>();

    }


    void Update()
    {
        if (canMove && Input.GetMouseButton(0))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;

            position.x = worldPos.x;
            transform.position = position;
        }
    }
    public void StartFishing()
    {

        length = IdleManager.Instance.length - 20;
        strength = IdleManager.Instance.strength;
        fishCount = 0;

        float time = (-length) * 0.1f;
        cameraTween = mainCamera.transform.DOMoveY(length, 1 + time * 0.25f, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y <= -11)
            {
                transform.SetParent(mainCamera.transform); // hook became camera's child.
            }
        }).OnComplete(delegate
        {
            coll.enabled = true; //hook's trigger.
            cameraTween = mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
            {
                if (mainCamera.transform.position.y >= -25f)
                    StopFishing();
            });
        });

        UIManager.Instance.ChangeUI(Scenes.Game);
        coll.enabled = false;
        canMove = true;
        hookedFishes.Clear();

    }

    private void StopFishing()
    {
        canMove = false;
        cameraTween.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            coll.enabled = true;
            int priceNum = 0;
            for (int i = 0; i < hookedFishes.Count; i++)
            {
                hookedFishes[i].transform.SetParent(null); //once it's hooked.
                hookedFishes[i].ResetFish();
                priceNum += hookedFishes[i].Type.price;
            }
            IdleManager.Instance.totalGain = priceNum;
            UIManager.Instance.ChangeUI(Scenes.End);
        });
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish") && fishCount != strength) //strength = max carry fish count
        {
            fishCount++;
            FishHandler fishHandler = other.GetComponent<FishHandler>();
            fishHandler.FishHooked();
            hookedFishes.Add(fishHandler);
            other.transform.SetParent(transform); //hook becomes the parent so it can move with it
            other.transform.position = hookedTransform.position;
            other.transform.rotation = hookedTransform.rotation;
            other.transform.localScale = Vector2.one;

            other.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                other.transform.rotation = Quaternion.identity;
            });

            if (fishCount == strength)
            {
                StopFishing();
            }
        }
    }
}
