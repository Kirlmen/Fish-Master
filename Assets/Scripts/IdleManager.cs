using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IdleManager : MonoBehaviour
{
    #region Singleton
    public static IdleManager Instance;
    #endregion

    [HideInInspector] public int length;
    [HideInInspector] public int strength;
    [HideInInspector] public int offlineGain;

    [HideInInspector] public int lengthCost;
    [HideInInspector] public int strengthCost;
    [HideInInspector] public int offlineGainCost;

    [HideInInspector] public int wallet;
    [HideInInspector] public int totalGain;

    private int[] costs = new int[]
    {
        120,
        151,
        197,
        250,
        324,
        414,
        537,
        687,
        892,
        1145,
        1484,
        1911,
        2479,
        3196,
        4148,
        5359,
        6954,
        9000,
        11687
    };



    void Awake()
    {
        if (IdleManager.Instance) { UnityEngine.Object.Destroy(gameObject); }
        else { IdleManager.Instance = this; }

        length = -PlayerPrefs.GetInt("Length", 30);
        strength = PlayerPrefs.GetInt("Strength", 3);
        offlineGain = PlayerPrefs.GetInt("Offline", 3);
        lengthCost = costs[-length / 10 - 3]; //upgrading index one by one.
        strengthCost = costs[strength - 3];
        offlineGainCost = costs[offlineGain - 3];
        wallet = PlayerPrefs.GetInt("Wallet", 0);
    }


    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            DateTime now = DateTime.Now;
            PlayerPrefs.SetString("Date", now.ToString());
            MonoBehaviour.print(now.ToString());
        }
        else
        {
            string @string = PlayerPrefs.GetString("Date", string.Empty);
            if (@string != string.Empty)
            {
                DateTime d = DateTime.Parse(@string);
                totalGain = (int)((DateTime.Now - d).TotalMinutes * offlineGain + 1.0);
                UIManager.Instance.ChangeUI(Scenes.Return);
            }
        }
    }


    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }

    public void BuyLength()
    {
        length -= 10;
        wallet -= lengthCost;
        lengthCost = costs[-length / 10 - 3];
        PlayerPrefs.SetInt("Length", -length);
        PlayerPrefs.SetInt("Wallet", wallet);
        UIManager.Instance.ChangeUI(Scenes.Main);
    }


    public void BuyStregth()
    {
        strength++;
        wallet -= strengthCost;
        strengthCost = costs[strength - 3];
        PlayerPrefs.SetInt("Strength", strength);
        PlayerPrefs.SetInt("Wallet", wallet);
        UIManager.Instance.ChangeUI(Scenes.Main);
    }

    public void BuyOfflineGain()
    {
        offlineGain++;
        wallet -= offlineGainCost;
        offlineGainCost = costs[strength - 3];
        PlayerPrefs.SetInt("Offline", offlineGain);
        PlayerPrefs.SetInt("Wallet", wallet);
        UIManager.Instance.ChangeUI(Scenes.Main);
    }

    public void CollectMoney()
    {
        wallet += totalGain;
        PlayerPrefs.SetInt("Wallet", wallet);
        UIManager.Instance.ChangeUI(Scenes.Main);
    }

    public void Collect2xMoney()
    {
        wallet += totalGain * 2;
        PlayerPrefs.SetInt("Wallet", wallet);
        UIManager.Instance.ChangeUI(Scenes.Main);
    }

}
