using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    #region Lose
    public GameObject Lose;
    public TextMeshProUGUI LoseText;
    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        HideUI();
    }

    public void HideUI()
    {
        Lose.SetActive(false);
    }
    public void ShowLoseMenu()
    {
        HideUI();
        Lose.SetActive(true);
        LoseText.text = Constants.LoseText + GameManager.instance.CurrentLevelManager.Score;
    }
    public void Restart()
    {
        HideUI();
        GameManager.instance.LoadLevel(GameManager.instance.Level);
    }
}