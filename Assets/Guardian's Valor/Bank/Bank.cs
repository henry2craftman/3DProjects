using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150; // 이후 설계자가 균형을 맞추면됨

    // 공용으로 놔둘 경우 모든 것이 필요시 접근해서 수정 가능 -> 끔찍
    // -> 잔액이 어디서 업데이트 되는지 제어권을 스크립트 밖에서 접근할 필요 -> 밖에서 접근 가능하지만 직접적으로 설정 불가 -> Property
    [SerializeField] int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    [SerializeField] TextMeshProUGUI displayBalance;

    private void Awake()
    {
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    // 입금하고 인출
    // amount가 -값이 되면 안됨
    // 실수로 양수여야하는 값이 음수로 되는 것을 방지
    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateDisplay();
    }

    // 출금
    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateDisplay();

        if (currentBalance < 0)
        {
            // 패배 했습니다.
            ReloadScene();
        }
    }

    void UpdateDisplay()
    {
        displayBalance.text = "Gold: " + currentBalance;
    }

    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
