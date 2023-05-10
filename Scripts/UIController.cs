using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyLabel;
    [SerializeField] TextMeshProUGUI goalLabel;
    [SerializeField] TextMeshProUGUI timeLabel;
    [SerializeField] TextMeshProUGUI wonLabel;


    public void ChangeMoneyLabel(int money)
    {
        moneyLabel.text = "Money: $" + money.ToString();
    }

    public void ChangeGoalLabel(int goal)
    {
        goalLabel.text = "Goal: $" + goal.ToString();
    }

    public void ChangeTimeLabel(float time)
    {
        string temp = (int)(time % 60) < 10 ? "0" : "";
        timeLabel.text = "Time: " + ((int)(time / 60)).ToString() + ":" + temp + ((int)(time % 60)).ToString();
    }

    public void SetWonLabelText(string text)
    {
        wonLabel.text = text;
    }
}
