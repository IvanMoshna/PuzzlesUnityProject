using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuItemUniversal : MonoBehaviour
{
    public GameObject Title;
    public GameObject Item;
    public GameObject LastItem;


    public Image backgroundIcon;
    public Image pattenIcon;
    public Image currentProgress;
    
    
    public void DisableLayouts()
    {
        Debug.Log("DISABLE");
        Title.SetActive(false);
        Item.SetActive(false);
        LastItem.SetActive(false);
    }
    
    
    
}

