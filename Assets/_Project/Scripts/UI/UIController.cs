using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject GameScreen;
    public GameObject MenuScreen;
    public GameObject ContinueScreen;


    private void DisableAll()
    {
        ContinueScreen.SetActive(false);
        GameScreen.SetActive(false);
        MenuScreen.SetActive(false);
    }


    private void Awake()
    {
        DisableAll();
        MenuScreen.SetActive(true);
    }
}
