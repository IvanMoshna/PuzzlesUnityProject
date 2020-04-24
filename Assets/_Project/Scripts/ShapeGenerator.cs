using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeGenerator : MonoBehaviour
{
 /*   public PuzzleController puzzleController;


    private bool isShaped;
    private bool isInContent;
    private void Start()
    {
        isShaped = false;
        isInContent = false;
    }

    void SetFinalPositions(List<GameObject> gameObjects, List<Vector3> finalPositions)
    {

        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.position = finalPositions[i];
        }

        isShaped = true;
    }
    
    /*public void SetShape(List <GameObject> puzzles, params GameObject[] pieces)
    {
        GameObject mainParent = pieces[0].GetComponent<PuzzleItem>().imageBox;
        pieces[0].GetComponent<PuzzleItem>().images[0].GetComponent<Image>().raycastTarget = false;   
        
        for (int i = 1; i < pieces.Length; i++)
        {
            GameObject child = pieces[i].GetComponent<PuzzleItem>().images[0].gameObject;
            child.transform.SetParent(mainParent.transform, true);
            mainParent.GetComponent<PuzzleItem>().images.Add(child.GetComponent<Puzzle>());
            child.GetComponent<Image>().raycastTarget = false;
            
            puzzles.Remove(pieces[i]);
            Destroy(pieces[i]);
        }

        int coef = mainParent.transform.childCount;
        Debug.Log("Coef = " + coef);
        if (coef > 1)
        {
            float x=0;
            float y=0;
            int k = 10;
            for (int i = 0; i < coef; i++)
            {
                if (x - mainParent.transform.GetChild(i).transform.position.x < k)
                {
                    x = mainParent.transform.GetChild(i).transform.position.x;
                    mainParent.GetComponent<RectTransform>().sizeDelta = new Vector2(mainParent.GetComponent<RectTransform>().sizeDelta.x + 50, mainParent.GetComponent<RectTransform>().sizeDelta.y);
                    //Debug.Log("X + 50");
                }
                if (y - mainParent.transform.GetChild(i).transform.position.y < k)
                {
                    y = mainParent.transform.GetChild(i).transform.position.y;
                    mainParent.GetComponent<RectTransform>().sizeDelta = new Vector2(mainParent.GetComponent<RectTransform>().sizeDelta.x, mainParent.GetComponent<RectTransform>().sizeDelta.y+50);
                    //Debug.Log("Y + 50");
                }
            }
        }
    }*/
    /*
    void SetInContent(List<GameObject> gameObjects, Transform content)
    {
        
        foreach (var puzzle in gameObjects)
        {
            puzzle.transform.SetParent(content, true); 
            puzzle.transform.localPosition = new Vector3(puzzle.transform.localPosition.x, puzzle.transform.localPosition.y, -1);
        }

        isInContent = true;
    }
    private void Update()
    {
        if (puzzleController.isGenerated && !isShaped && !isInContent)
        { 
            Debug.Log("UPDATE");
            SetFinalPositions(puzzleController.puzzlePrefabs, puzzleController.puzzlePos);
            //SetShape(puzzleController.puzzlePrefabs,puzzleController.puzzlePrefabs[0], puzzleController.puzzlePrefabs[1], puzzleController.puzzlePrefabs[6] );
            SetInContent(puzzleController.puzzlePrefabs, puzzleController.contentBox.transform);
        }
    }*/
}
    
    
    
    
    
    
    
    
    
    
    

