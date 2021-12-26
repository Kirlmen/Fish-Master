using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class HookHandler : MonoBehaviour
{
    [SerializeField] Transform hookedFish;


    private Camera mainCamera;
    private Collider2D coll;

    private int length;
    private int strength;
    private int fishCount;

    private bool canMove;

    private Tweener cameraTween;


    private void Awake()
    {
        mainCamera = Camera.main;
        coll = GetComponent<Collider2D>();

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
        //idleManager
        length = -50;
        strength = 3;
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

        //game ui
        coll.enabled = false;
        canMove = true;
        //clear

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
            int num = 0;
            //clearing out th ehook from the fishes
            //idlemanager totalgain = num;
            //scenemanager end screen
        });
    }
}
