using System.Collections;
using System.Collections.Generic;
using Common;
using Puzzles.Configs;
using UnityEngine;

public class UIContinueScreen : MonoBehaviour
{
    private SettingsGame settingsGame;
    public PuzzleController controller;
    
    public void Awake()
    {
        settingsGame = ToolBox.Get<SettingsGame>();
    }

    public void OnNewGameButtonClick()
    {
        controller.Clear();
        controller.NewGame();
        controller.UIController.GetComponent<UIController>().ContinueScreen.SetActive(false);
    }
    
    public void OnContinueButtonClick()
    {
        gameObject.SetActive(false);
    }
}
