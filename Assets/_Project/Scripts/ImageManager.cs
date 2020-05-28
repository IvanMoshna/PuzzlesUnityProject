using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    [FormerlySerializedAs("backgroundnPanels")] 
    public List<Image> backgroundPanels;
    
    public void FrameOn()
    {
        foreach (var item in backgroundPanels)
        {
            item.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
        }
    }

    public void FrameOff()
    {
        foreach (var item in backgroundPanels)
        {
            item.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
    
}
