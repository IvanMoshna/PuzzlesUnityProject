using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeGenerator : MonoBehaviour
{
    public PuzzleController puzzleController;


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
    }
    
    void SetShape(GameObject go1, GameObject go2, Transform parrent, List<GameObject> gameObjects)
    {
        //Debug.Log("First puzzleID = " + gameObjects[ind1].GetComponent<Puzzle>().puzzleID);
        //Debug.Log("Second puzzleID = " + gameObjects[ind2].GetComponent<Puzzle>().puzzleID);
        
        //ЛИБО СПЕРВА УДАЛЯТЬ С бОльшим ID, ЛИБО ПРИДУМАТЬ ДРУГОЙ СПОСОБ!!! 
        gameObjects.Remove(go2);
        gameObjects.Remove(go1);
        
        go1.transform.SetParent(parrent);
        go2.transform.SetParent(go1.transform);

        go1.transform.localPosition = new Vector3(go1.transform.localPosition.x, go1.transform.localPosition.y, -1);
        isShaped = true;
        Debug.Log("SHAPED!");
    }

    void SetInContent(List<GameObject> gameObjects, Transform content)
    {
        
        foreach (var puzzle in gameObjects)
        {
            //Debug.Log("puzzle parent before: " + puzzle.transform.parent.name);
            puzzle.transform.SetParent(content, true); 
            puzzle.transform.localPosition = new Vector3(puzzle.transform.localPosition.x, puzzle.transform.localPosition.y, -1);
            
            //Debug.Log("puzzle parent after: " + puzzle.transform.parent.name);

        }

        isInContent = true;
    }
    private void Update()
    {
        if (puzzleController.isGenerated && !isShaped && !isInContent)
        {
            /*foreach (var p in puzzleController.puzzlePrefabs)
            {
                Debug.Log("ID " + p.GetComponent<Puzzle>().puzzleID);
            }*/
            Debug.Log("0ID = " + puzzleController.puzzlePrefabs[0].GetComponent<Puzzle>().puzzleID);
           SetFinalPositions(puzzleController.puzzlePrefabs, puzzleController.puzzlePos);
           SetShape(puzzleController.puzzlePrefabs[0], puzzleController.puzzlePrefabs[1], puzzleController.contentBox.transform, puzzleController.puzzlePrefabs);
           SetInContent(puzzleController.puzzlePrefabs, puzzleController.contentBox.transform);
        }
    }
}
    
    
    
    
    
    
    
    
    
    
    

