using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public List<Puzzle> images;
    public PuzzleController puzzleController;
 

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var puzzlePos = images[0].puzzlePos;
        Debug.Log("Distance = " + Vector2.Distance(gameObject.transform.position, puzzlePos));
        if (Vector2.Distance(gameObject.transform.position, puzzlePos) < puzzleController.gameSettings.puzzleDistance)
        {
            GameObject parentPuzzle = gameObject.transform.parent.gameObject;
            gameObject.GetComponentInParent<Image>().raycastTarget = false;
            gameObject.transform.position = puzzlePos;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -1);

            parentPuzzle.transform.SetParent(puzzleController.checkedPuzzles.transform);
            parentPuzzle.GetComponent<PuzzleItem>().backgroundImage.GetComponent<Image>().enabled = false;
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(0,0,0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }
}
