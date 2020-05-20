using System.Collections;
using System.Collections.Generic;
using Common;
using Puzzles.Configs;
using UnityEngine;

public class UIContinueScreen : MonoBehaviour
{
    private SettingsGame settingsGame;
    public PuzzleController controller;
    private SettingsGame gameSettings;
    
    public void Awake()
    {
        settingsGame = ToolBox.Get<SettingsGame>();
        gameSettings = ToolBox.Get<SettingsGame>();
    }

    public void OnNewGameButtonClick()
    {
        Debug.Log("OnContinuePuzzleClick");
        controller.UIController.GetComponent<UIController>().GoToGameScreen();
        PuzzleState puzState = PuzzlesCreator.CreatePuzzle(gameSettings.lines, gameSettings.columns);
        
        ////////
        foreach (var dataItem in controller.DataPuzzleState.puzzleStates)
        {
            if (dataItem.puzzleID == controller.originalImage.sprite.name)
            {
                controller.DataPuzzleState.puzzleStates.Remove(dataItem);
                break;
            }
        }
        ////
        controller.DataPuzzleState.puzzleStates.Add(puzState);
        puzState.puzzleID = controller.originalImage.sprite.name;
        controller.currentState = puzState;
        controller.InitView(puzState.puzzleDatas);
        controller.UpdateProgress();
    }

    public void OnContinueButtonClick()
    {
        Debug.Log("OnContinuePuzzleClick");
        controller.UIController.GetComponent<UIController>().GoToGameScreen();
        controller.InitView(controller.currentState.puzzleDatas);
        controller.UpdateProgress();

    }
}
