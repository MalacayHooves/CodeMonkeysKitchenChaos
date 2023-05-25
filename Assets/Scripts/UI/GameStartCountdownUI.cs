using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        GameManager.instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            countdownText.text = Mathf.Ceil(GameManager.instance.GetCountdownToStartTimer()).ToString();
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
