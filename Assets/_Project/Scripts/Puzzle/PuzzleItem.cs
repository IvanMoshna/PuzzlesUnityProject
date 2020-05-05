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
    public GameObject gridImage;

    
    public ScrollRect scrollRect;
    public Canvas canvas;

    private bool isDraggable;
    private bool isFramed;
    private RectTransform rtPuzzleImage;
    private float startDragTime;
    private float dragDelay;
    private float dragDelta;

    private bool isClicked;
    private Vector2 clickPosition;
    private bool movedAway;
    private SettingsGame settingsGame;
    private Vector2 offsetToCenter;
    private Vector2 offsetToShadow;

    private void Start()
    {
        //rt = GetComponent<RectTransform>();
        rtPuzzleImage = puzzleImage.GetComponent<RectTransform>();
        //offsetToCenter = new Vector2(puzzleImage.GetComponent<RectTransform>().sizeDelta.x/2, puzzleImage.GetComponent<RectTransform>().sizeDelta.y/2);
        offsetToShadow = new Vector2(puzzleController.wCell, puzzleController.hCell);
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
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rtPuzzleImage, eventData.position, null, out localPoint);
        Debug.Log("localpoint = " + localPoint);
        offsetToCenter = localPoint;
    }

    private void StartDrag()
    {
       
        isClicked = false;
        isDraggable = true;
        isFramed = true;
        //transform.SetParent(puzzleController.DragParent, true);
        gridImage.transform.SetParent(puzzleController.DragParent, true);
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
        
        if (Vector2.Distance(rtPuzzleImage.anchoredPosition, puzzlePosition) < settingsGame.puzzleDistance)
        {
            puzzleImage.transform.localPosition = puzzlePosition;
            rtPuzzleImage.anchoredPosition = puzzlePosition;
            puzzleImage.GetComponent<Image>().raycastTarget = false;
            backgroundImage.SetActive(false);
            gridImage.SetActive(false);
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
            gridImage.transform.SetParent(this.transform,true);
            puzzleImage.transform.SetParent(this.transform,true);
            puzzleImage.transform.localPosition = Vector3.zero;
            gridImage.transform.localPosition = Vector3.zero;
            
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
          

            
            
            puzzleImage.transform.position = eventData.position-offsetToCenter;
                
                
            SetPuzzlePosOnGridImage(rtPuzzleImage.anchoredPosition, gridImage, gridImage.GetComponent<PuzzleShadow>().shadowPositions);

            if (isFramed)
            {
                imageManager.FrameOn();
                isFramed = false;
            }
        }
       
    }

    public void SetPuzzlePosOnGridImage(Vector2 puzzle, GameObject gridImage, List<Vector2> puzzlePozList)
    {
        float nearestPos=1000f;
        foreach (var pos in puzzlePozList)
        {
            if (Vector2.Distance(puzzle, pos) < nearestPos)
            {
                nearestPos = Vector2.Distance(puzzle, pos);
                gridImage.GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.OnEndDrag(eventData);
        isFramed = false;
    }
}