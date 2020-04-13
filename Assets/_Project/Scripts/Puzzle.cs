using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Puzzle : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IPointerUpHandler, IDragHandler
{
    public int puzzleID;
    public PuzzleController puzzleController;
    public Vector2 puzzlePos;

    public void OnPointerDown(PointerEventData eventData)
    {
        //gameObject.transform.SetParent(puzzleController.checkedPuzzles.transform, true);
        ///Debug.Log("PARENT = " + gameObject.transform.parent);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On Begin Drag");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Distance = " + Vector2.Distance(gameObject.transform.position, puzzlePos));
        if (Vector2.Distance(gameObject.transform.position, puzzlePos) < puzzleController.gameSettings.puzzleDistance)
        {
            GameObject parentPuzzle = gameObject.transform.parent.gameObject;
            gameObject.GetComponentInParent<Image>().raycastTarget = false;
            gameObject.transform.position = puzzlePos;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -1);

            parentPuzzle.transform.SetParent(puzzleController.checkedPuzzles.transform);
            parentPuzzle.GetComponent<Image>().enabled = false;
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
