using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using DG.Tweening;
using Puzzles.Configs;

public class UIGameScreen : MonoBehaviour
{
    [Space]
    public GameObject topPanel;
    public GameObject lowerPanel;
    public GameObject winPanel;

    [Space]
    public float duration;
    public GameObject topWinPosition;
    public GameObject topGamePosition;

    private PuzzleController controller;
    
    public void SetGamePanelsWinPositions()
    {
        topPanel.transform.DOMove(topWinPosition.transform.position, duration);
        var lowPos = lowerPanel.GetComponent<RectTransform>().sizeDelta.y;
        lowerPanel.transform.DOMoveY( -lowPos, duration);
        winPanel.transform.DOMoveY(0, duration);
    }

    public void SetGamePanelsGamePositions()
    {
        topPanel.transform.DOMove(topGamePosition.transform.position, duration);
        lowerPanel.transform.DOMoveY(0, duration);
        winPanel.transform.DOMoveY(-200, duration);   
    }

    public void RestartOnGameScreen()
    {
        
        var gameSettings = ToolBox.Get<SettingsGame>();

        var controller = GameObject.Find("Puzzle").GetComponent<PuzzleController>();
        
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
        controller.Clear();        
        controller.DataPuzzleState.puzzleStates.Add(puzState);
        puzState.puzzleID = controller.originalImage.sprite.name;
        controller.currentState = puzState;
        controller.InitView(puzState.puzzleDatas);
        controller.UpdateProgress();
        
        SetGamePanelsGamePositions();
    }

}
