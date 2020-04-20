using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ImageBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public List<Puzzle> images;
    public PuzzleController puzzleController;
    public ScrollRect scrollRect;
    public float holdTime = 0.5f;
    public float deltaY;
    public ImageManager imageManager;
    
    private bool isDraggable;
    private bool isFramed;
    private EventSystem es;
    public void OnBeginDrag (PointerEventData eventData)
    {
        scrollRect.OnBeginDrag(eventData);
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //костыль
            if (gameObject.transform.childCount > 1)
            {
                float pivotCoef = 0.25f;
                Vector2 pivotVector =
                    new Vector2(gameObject.transform.GetChild(0).GetComponent<RectTransform>().pivot.x + pivotCoef,
                        gameObject.transform.GetChild(0).GetComponent<RectTransform>().pivot.y + pivotCoef);
                gameObject.GetComponent<RectTransform>().pivot = pivotVector;
            }
            isFramed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        imageManager.FrameOff();
        var puzzlePos = images[0].puzzlePos;
        if (Vector2.Distance(gameObject.transform.position, puzzlePos) < puzzleController.gameSettings.puzzleDistance)
        {
            GameObject parentPuzzle = gameObject.transform.parent.gameObject;
            gameObject.GetComponentInParent<Image>().raycastTarget = false;
            gameObject.transform.position = puzzlePos;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -1);

            parentPuzzle.transform.SetParent(puzzleController.checkedPuzzles.transform);
            parentPuzzle.GetComponent<PuzzleItem>().backgroundImage.GetComponent<Image>().enabled = false;

            //repeat pattern
            foreach (var bg in imageManager.backgroundnPanels)
            {
                Vector3 pos = parentPuzzle.transform.localPosition;
                GameObject puzzleClone = Instantiate(parentPuzzle, bg.transform);
                puzzleClone.transform.localPosition = pos;
            }
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(0,0,0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.delta.y<deltaY && !isDraggable)
            scrollRect.OnDrag(eventData);
        else
        {
            isDraggable = true;
           
            gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
            if (isFramed)
            {
                Debug.Log("is Framed");
                imageManager.FrameOn();
                isFramed = false;
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
        scrollRect.OnEndDrag(eventData);
        isDraggable = false;
        isFramed = false;
    }
}
