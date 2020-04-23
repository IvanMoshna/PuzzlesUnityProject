using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Puzzles.Settings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler
{
    public GameObject backgroundImage;

    public PuzzleController puzzleController;
    public GameObject ScrollParent;
    public ImageManager imageManager;
    public Vector2 puzzlePosition;

    private bool isDraggable;
    private bool isFramed;
    private RectTransform rt;
    private float startDragTime;
    private float dragDelay;

    private bool isClicked;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        isDraggable = false;
        dragDelay=ToolBox.Get<SettingsGame>().DragDelay;
    }

    private void Update()
    {
        if (!isClicked)
        {
            //ScrollParent.GetComponent<ScrollRect>().
        }
        else
        {
            if (Time.time - startDragTime >= dragDelay)
            {
                StartDrag();
                isClicked = false;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
        startDragTime = Time.time;
    }

    private void StartDrag()
    {
        isFramed = true;
        transform.SetParent(puzzleController.checkedPuzzles.transform, true);
        rt.anchorMax = Vector2.zero;
        rt.anchorMin = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        imageManager.FrameOff();

        if (Vector2.Distance(rt.anchoredPosition, puzzlePosition) < puzzleController.gameSettings.puzzleDistance)
        {
            transform.SetParent(puzzleController.checkedPuzzles.transform);
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

        puzzleController.SetPrefferedContentSize();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        transform.position = eventData.position;
        if (isFramed)
        {
            imageManager.FrameOn();
            isFramed = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isFramed = false;
    }
}