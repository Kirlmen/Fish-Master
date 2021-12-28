using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
    #endregion

    private GameObject currentUI;
    private int gameCount;

    public GameObject endUI, gameUI, mainUI, returnUI;
    public Button lengthButton, strengthButton, offlineButton;
    public Text gameSceneMoney, lengthCostText, lengthValueText, strengthCostText, strengthValueText, offlineCostText, offlineValueText, endUIMoney, returnUIMoney;

    void Awake()
    {
        if (UIManager.Instance)
        {
            Destroy(base.gameObject);
        }
        else { UIManager.Instance = this; }
        currentUI = mainUI;
    }

    private void Start()
    {
        CheckIdle();
        UpdateText();
    }

    public void ChangeUI(Scenes scenes)
    {
        currentUI.SetActive(false);
        switch (scenes)
        {
            case Scenes.Main:
                currentUI = mainUI;
                CheckIdle();
                UpdateText();
                break;
            case Scenes.Game:
                currentUI = gameUI;
                gameCount++;
                break;
            case Scenes.End:
                currentUI = endUI;
                SetEndUIMoney();
                break;
            case Scenes.Return:
                currentUI = returnUI;
                SetReturnUIMoney();
                break;

        }
        currentUI.SetActive(true);
    }

    public void SetEndUIMoney()
    {
        endUIMoney.text = "$" + IdleManager.Instance.totalGain;
    }

    public void SetReturnUIMoney()
    {
        returnUIMoney.text = "$" + IdleManager.Instance.totalGain + " Gained While Offline!";
    }


    public void UpdateText()
    {
        gameSceneMoney.text = "$" + IdleManager.Instance.wallet;
        lengthCostText.text = "$" + IdleManager.Instance.lengthCost;
        lengthValueText.text = -IdleManager.Instance.length + "m";
        strengthCostText.text = "$" + IdleManager.Instance.strengthCost;
        strengthValueText.text = IdleManager.Instance.strength + " fishes.";
        offlineCostText.text = "$" + IdleManager.Instance.offlineGainCost;
        offlineValueText.text = "$" + IdleManager.Instance.offlineGain + "/min";
    }

    public void CheckIdle()
    {
        int lengthCost = IdleManager.Instance.lengthCost;
        int strengthCost = IdleManager.Instance.strengthCost;
        int offlineGainCost = IdleManager.Instance.offlineGainCost;
        int wallet = IdleManager.Instance.wallet;

        if (wallet < lengthCost)
        {
            lengthButton.interactable = false;
        }
        else { lengthButton.interactable = true; }
        if (wallet < strengthCost)
        {
            strengthButton.interactable = false;
        }
        else { strengthButton.interactable = true; }
        if (wallet < offlineGainCost)
        {
            offlineButton.interactable = false;
        }
        else { offlineButton.interactable = true; }

    }
}
