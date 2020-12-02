using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuScreen : MonoBehaviour
{
    public GameObject continueScreen;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (continueScreen.activeSelf)
                continueScreen.SetActive(false);
            else
                Application.Quit();
        }
    }
}
