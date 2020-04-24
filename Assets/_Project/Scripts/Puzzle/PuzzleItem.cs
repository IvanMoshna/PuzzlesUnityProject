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
    public GameObject backgroundImage;

    public PuzzleController puzzleController;
    public GameObject ScrollParent;
    public ImageManager imageManager;
    public Vector2 puzzlePosition;
    
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

    private void Start()
    {
        rt = GetComponent<RectTransform>();
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
        transform.SetParent(puzzleController.DragParent, true);
        rt.anchorMax = Vector2.zero;
        rt.anchorMin = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        imageManager.FrameOff();

        if (Vector2.Distance(rt.anchoredPosition, puzzlePosition) < settingsGame.puzzleDistance)
        {
            transform.SetParent(puzzleController.DragParent);
            rt.anchoredPosition = puzzlePosition;
            //  parentPuzzle.GetComponent<PuzzleItem>().backgroundImage.GetComponent<Image>().enabled = false;

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
            transform.SetParent(ScrollParent.transform, false);
        }

        puzzleController.SetPreferedContentSize();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable && !movedAway && Time.time - startDragTime <= dragDelay &&
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
            transform.position = eventData.position;
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