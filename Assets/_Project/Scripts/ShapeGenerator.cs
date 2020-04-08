using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeGenerator : MonoBehaviour
{
    public PuzzleController puzzleController;


    private bool isShaped;
    private void Start()
    {
        
    }

    void GetShape(GameObject go1, GameObject go2)
    {
        go2.transform.SetParent(go1.transform);
        isShaped = true;
    }

    private void Update()
    {
        if (puzzleController.isGenerated && !isShaped)
        {
            List<GameObject> quadPuzzles = puzzleController.puzzlePrefabs;
            GetShape(quadPuzzles[0], quadPuzzles[1]);
        }
    }
}
    
    
    
    
    
    
    
    
    
    
    

