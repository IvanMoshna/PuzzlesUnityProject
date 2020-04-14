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
    
    /*void SetShape(GameObject go1, GameObject go2, Transform parrent, List<GameObject> gameObjects)
    {
        //Debug.Log("First puzzleID = " + gameObjects[ind1].GetComponent<Puzzle>().puzzleID);
        //Debug.Log("Second puzzleID = " + gameObjects[ind2].GetComponent<Puzzle>().puzzleID);
        
        gameObjects.Remove(go2);
        gameObjects.Remove(go1);
        
        go1.transform.SetParent(parrent);
        go2.transform.SetParent(go1.transform);

        go1.transform.localPosition = new Vector3(go1.transform.localPosition.x, go1.transform.localPosition.y, -1);
        isShaped = true;
        Debug.Log("SHAPED!");
    }*/

    public void SetShape(List <GameObject> puzzles, params GameObject[] pieces)
    {
        GameObject mainParent = pieces[0].GetComponent<PuzzleItem>().imageBox;
        pieces[0].GetComponent<PuzzleItem>().imageBox.GetComponent<ImageBox>().images[0].GetComponent<Image>().raycastTarget = false;   
        
        for (int i = 1; i < pieces.Length; i++)
        {
            GameObject child = pieces[i].GetComponent<PuzzleItem>().imageBox.GetComponent<ImageBox>().images[0].gameObject;
            child.transform.SetParent(mainParent.transform, true);
            mainParent.GetComponent<ImageBox>().images.Add(child.GetComponent<Puzzle>());
            child.GetComponent<Image>().raycastTarget = false;
            
            puzzles.Remove(pieces[i]);
            Destroy(pieces[i]);
        }

        
        
        
        
        //mainParent.transform.SetParent(puzzleController.contentBox.transform);
        //return rez;
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
            SetFinalPositions(puzzleController.puzzlePrefabs, puzzleController.puzzlePos);
            SetShape(puzzleController.puzzlePrefabs,puzzleController.puzzlePrefabs[0], puzzleController.puzzlePrefabs[1], puzzleController.puzzlePrefabs[6] );
            SetInContent(puzzleController.puzzlePrefabs, puzzleController.contentBox.transform);
        }
    }
}
    
    
    
    
    
    
    
    
    
    
    

