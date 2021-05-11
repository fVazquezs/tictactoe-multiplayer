using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    public Text VictorLabel;

    private void OnEnable()
    {
        VictorLabel.text = GameModeController.Instance.GameVictor.ToString();
    }

    public void GoToMenu()
    {
        UIController.Instance.GoToScreen(EUIScreen.Title);
    }
}
