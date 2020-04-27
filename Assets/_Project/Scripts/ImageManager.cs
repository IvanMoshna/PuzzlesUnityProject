using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public List<Image> backgroundnPanels;
    
    public void FrameOn()
    {
        foreach (var item in backgroundnPanels)
        {
            item.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 1);
        }
    }

    public void FrameOff()
    {
        foreach (var item in backgroundnPanels)
        {
            item.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
    
}
