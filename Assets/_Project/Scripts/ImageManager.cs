﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public List<Image> backgroundnPanels;


    public void FrameOn()
    {
        Debug.Log("FrameON");
        foreach (var item in backgroundnPanels)
        {
            item.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 1);
        }
    }

    public void FrameOff()
    {
        Debug.Log("FrameOFF");

        foreach (var item in backgroundnPanels)
        {
            item.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        Debug.Log("color " + backgroundnPanels[1].GetComponent<Image>().color.r);
    }

    public void RepeatPattern(GameObject pattern)
    {
        foreach (var bp in backgroundnPanels)
        {
            
        }
    }
}
