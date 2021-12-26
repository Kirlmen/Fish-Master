using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] FishHandler fishPrefab;
    [SerializeField] FishHandler.FishType[] fishTypes;

    void Awake()
    {
        for (int i = 0; i < fishTypes.Length; i++)
        {
            int num = 0;
            while (num < fishTypes[i].fishCount)
            {
                FishHandler fish = UnityEngine.Object.Instantiate<FishHandler>(fishPrefab);
                fish.Type = fishTypes[i];
                fish.ResetFish();
                num++;
            }
        }
    }


}
