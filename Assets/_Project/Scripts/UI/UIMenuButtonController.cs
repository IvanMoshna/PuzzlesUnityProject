using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuButtonController : MonoBehaviour
{
    [Space]
    public PuzzleController puzzleController;
    public UIController uiController;

    [Space] 
    public Image icon;
    public Image backgrounIcon;
    
    
    public void OnPuzzleChoseClick()
    {
        uiController.GoToGameScreen();
        puzzleController.originalImage.sprite = icon.sprite;
        puzzleController.backgroundImage.sprite = backgrounIcon.sprite;
        puzzleController.NewGame();
    }
    
}
