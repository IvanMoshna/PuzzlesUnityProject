using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Puzzles.Settings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler,
    IBeginDragHandler
{

    public PuzzleController puzzleController;
    public GameObject ScrollParent;
    public ImageManager imageManager;
    public Vector2 puzzlePosition;
    public GameObject puzzleImage;
    public GameObject backgroundImage;

    
    public ScrollRect scrollRect;

    private bool isDraggable;
    private bool isFramed;
    private RectTransform rt;
    private float startDragTime;
    private float dragDelay;
    private float dragDelta;

    private bool isClicked;
    private Vector2 clickPosition;
    private bool movedAway;
    private SettingsGame settingsGame;
    private Vector2 offsetToCenter;

    private void Start()
    {
        //rt = GetComponent<RectTransform>();
        rt = puzzleImage.GetComponent<RectTransform>();
        offsetToCenter = new Vector2(puzzleImage.GetComponent<RectTransform>().sizeDelta.x/2, puzzleImage.GetComponent<RectTransform>().sizeDelta.y/2);
        settingsGame = ToolBox.Get<SettingsGame>();
        dragDelay = settingsGame.DragDelay;
        dragDelta = settingsGame.DragDelta;
        isDraggable = false;
        scrollRect = puzzleController.scrollView.GetComponent<ScrollRect>();
        transform.localScale=Vector3.one;
    }

    private void Update()
    {
        if (isClicked && !movedAway && Time.time - startDragTime > dragDelay)
        {
            StartDrag();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
        movedAway = false;
        startDragTime = Time.time;
        clickPosition = eventData.position;
    }

    private void StartDrag()
    {
        isClicked = false;
        isDraggable = true;
        isFramed = true;
        //transform.SetParent(puzzleController.DragParent, true);
        puzzleImage.transform.SetParent(puzzleController.DragParent, true);
        /*rt.anchorMax = Vector2.zero;
        rt.anchorMin = Vector2.zero;*/
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        imageManager.FrameOff();

        //rt.anchoredPosition
        Debug.Log("Distance = " + Vector2.Distance(puzzleImage.GetComponent<RectTransform>().position, puzzlePosition));
        
        if (Vector2.Distance(rt.anchoredPosition, puzzlePosition) < settingsGame.puzzleDistance)
        {
            puzzleImage.transform.localPosition = puzzlePosition;
            rt.anchoredPosition = puzzlePosition;
            puzzleImage.GetComponent<Image>().raycastTarget = false;
            backgroundImage.SetActive(false);
            transform.SetParent(puzzleController.DragParent, true);
            puzzleImage.transform.SetParent(this.transform,true);

            //repeat pattern
            foreach (var bg in imageManager.backgroundnPanels)
            {
                Vector3 pos = transform.localPosition;
                GameObject puzzleClone = Instantiate(gameObject, bg.transform);
                puzzleClone.transform.localPosition = pos;
            }

            isDraggable = false;
        }
        else
        {
            puzzleImage.transform.SetParent(this.transform,true);
            puzzleImage.transform.localPosition = Vector3.zero;
        }

        puzzleController.SetPreferedContentSize();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable /*&& !movedAway && Time.time - startDragTime <= dragDelay*/ &&
            Vector2.Distance(clickPosition, eventData.position) > dragDelta)
        {
            movedAway = true;
        }

        if (!isDraggable)
        {
            scrollRect.OnDrag(eventData);
        }
        else
        {
            //transform.position = eventData.position;
            puzzleImage.transform.position = eventData.position-offsetToCenter;
            if (isFramed)
            {
                imageManager.FrameOn();
                isFramed = false;
            }
        }
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.OnEndDrag(eventData);
        isFramed = false;
    }
}