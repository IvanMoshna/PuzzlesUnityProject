using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{

    public GameObject GameScreen;
    public GameObject MenuScreen;
    public GameObject ContinueScreen;
    public GameObject LoadingScreen;

    private void DisableAll()
    {
        ContinueScreen.SetActive(false);
        GameScreen.SetActive(false);
        MenuScreen.SetActive(false);
        LoadingScreen.SetActive(false);
    }
    private void Awake()
    {
        DisableAll();
        MenuScreen.SetActive(true);
    }

    public void GoToGameScreen()
    {
        OnLoadingScreen();
    }

    public void OnLoadingScreen()
    {
        DisableAll();
        LoadingScreen.SetActive(true);
        StartCoroutine(StartTimer(1f));
    }
    IEnumerator StartTimer(float time)
    {
        GameScreen.SetActive(true);
        yield return new WaitForSeconds(time);
        LoadingScreen.SetActive(false);
    }
    
    public void OnContinueScreen()
    {
        ContinueScreen.SetActive(true);
        
    }
    public void GoToMenuScreen()
    {
        var controller = GameObject.Find("Puzzle").GetComponent<PuzzleController>();
        controller.progressCount = 0;
        controller.SaveGameState();
        controller.Clear();
        DisableAll();
        MenuScreen.SetActive(true);
        GameScreen.GetComponent<UIGameScreen>().SetGamePanelsGamePositions();

    }

    public void OnPrivacyPolicyButtonClick()
    {
        Application.OpenURL("https://docs.google.com/document/d/1BIH9rky4C1LIo8PCVuTKyCEF2Z_1GEiOMMgn_97Z3zE/edit?usp=sharing");
    }
}
