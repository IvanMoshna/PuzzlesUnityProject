using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollviewAdapter : MonoBehaviour
{
    public Image[] availableIcons;
    public RectTransform prefab;
    

    public void UpdateItems()
    {
        
    }

    /*void FetchItemModelsFromServer(int count, Action<ItemModel> onDone)
    {
        yield return new WaitForSeconds(0.1f);

        var results = new ItemModel[count];
        for (int i = 0; i < count; i++)
        {
            results[i] = new ItemModel();
            
        }
    }*/


    public class ItemModel
    {
        public Image backgroungIcon;
    }
}
