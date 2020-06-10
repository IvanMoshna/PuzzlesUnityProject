using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Puzzles.Configs;
using Puzzles.Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler,
    IBeginDragHandler
{

    public PuzzleController puzzleController;
    public GameObject ScrollParent;
    public ImageManager imageManager;
    //public Vector2 puzzlePosition;
    public GameObject puzzleImage;
    public GameObject backgroundImage;
    public GameObject gridImage;
    public GameObject spriteImage;

    public PuzzleData puzzleData;
    
    public ScrollRect scrollRect;
    public Canvas canvas;

    public List<Vector2> elementPositions;
    public int progressItemCount;
    
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
        
            //spriteImage.SetActive(false);

        if (puzzleData.isPosed)
        {
            Vector2 puzDataPos = new Vector2(puzzleData.puzzlePosition.x, puzzleData.puzzlePosition.y);
            rtPuzzleImage.transform.SetParent(puzzleController.DragParent, false);  
            
            puzzleImage.transform.localPosition = puzDataPos;
            rtPuzzleImage.anchoredPosition = puzDataPos;
            
            puzzleImage.GetComponent<Image>().raycastTarget = false;
            backgroundImage.SetActive(false);
            gridImage.SetActive(false);
            
            RepeatPattern(imageManager.backgroundPanels, rtPuzzleImage.gameObject);

        }
        puzzleController.SetAlphaToPuzzles(puzzleController.progressCount, puzzleController.winCount);
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
        Vector3 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(rtPuzzleImage, eventData.position, null, out localPoint);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rtPuzzleImage, eventData.position, null, out localPoint);
        offsetToCenter = localPoint;
        Debug.Log("OFFSET = " +offsetToCenter);
    }

    private void StartDrag()
    {
       
        isClicked = false;
        isDraggable = true;
        isFramed = true;
        //transform.SetParent(puzzleController.DragParent, true);
        gridImage.transform.SetParent(puzzleController.DragParent, true);
        puzzleImage.transform.SetParent(puzzleController.DragParent, true);
        Debug.Log("ANCHORED: " + rtPuzzleImage.anchoredPosition);
        spriteImage.transform.SetParent(puzzleController.SpritesParent, false);
        /*rt.anchorMax = Vector2.zero;
        rt.anchorMin = Vector2.zero;*/
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        imageManager.FrameOff();
        

        //rt.anchoredPosition
        //Debug.Log("Distance = " + Vector2.Distance(puzzleImage.GetComponent<RectTransform>().position, puzzlePosition));
        
        Vector2 puzDataPos = new Vector2(puzzleData.puzzlePosition.x, puzzleData.puzzlePosition.y);
        
        if (Vector2.Distance(rtPuzzleImage.anchoredPosition, puzDataPos) < settingsGame.puzzleDistance && rtPuzzleImage.parent == puzzleController.DragParent)
        {
            Debug.Log("ДО");
            Debug.Log("PUZZLE IMAGE: " + puzzleImage.transform.position);
            Debug.Log("SPRITE IMAGE: " + spriteImage.transform.position);
            
            //Debug.Log("Distance = " + Vector2.Distance(rtPuzzleImage.anchoredPosition, puzDataPos));
            puzzleImage.transform.localPosition = puzDataPos;
            //spriteImage.transform.position = new Vector3(puzzleImage.transform.localPosition.x, puzzleImage.transform.localPosition.y, 1);
            Debug.Log("после дата поз");

            Debug.Log("PUZZLE IMAGE: " + puzzleImage.transform.position);
            Debug.Log("SPRITE IMAGE: " + spriteImage.transform.position);
            //spriteImage.transform.position = Camera.main.ScreenToWorldPoint(rtPuzzleImage.anchoredPosition);
           
            spriteImage.transform.position = puzzleImage.transform.position;
            
            /*puzzleImage.transform.SetParent(puzzleController.IntermediateParent, true);
            spriteImage.transform.position = Camera.main.ScreenToWorldPoint(puzzleImage.transform.position);*/
            
            
            
            /*Vector3[] v = new Vector3[4];
            rtPuzzleImage.GetWorldCorners(v);

            v[0].y -= Camera.main.orthographicSize/2;
            v[0].x -= Camera.main.aspect * Camera.main.orthographicSize;
            spriteImage.transform.position = v[0];
         
            Debug.Log("ANCHORED: " + v[0]);*/
            Debug.Log("после всей хуйни");

            Debug.Log("PUZZLE IMAGE: " + puzzleImage.transform.position);
            Debug.Log("SPRITE IMAGE: " + spriteImage.transform.position);
            /*Debug.Log("IMAGE");
            Debug.Log("Anchor = "+ puzzleImage.GetComponent<RectTransform>().anchoredPosition);
            Debug.Log("Local = "+ puzzleImage.GetComponent<RectTransform>().localPosition);
            Debug.Log("Position = "+ puzzleImage.GetComponent<RectTransform>().position);
            Debug.Log("SPRITE");
            //Debug.Log("Anchor = "+ spriteImage.GetComponent<RectTransform>().anchoredPosition);
            Debug.Log("Local = "+ spriteImage.transform.localPosition);
            Debug.Log("Position = "+ spriteImage.transform.position);
            
            Debug.Log("Camera Pos" + Camera.main.ScreenToWorldPoint(puzDataPos));*/

            
            rtPuzzleImage.anchoredPosition = puzDataPos;
            puzzleImage.GetComponent<Image>().raycastTarget = false;
            backgroundImage.SetActive(false);
            gridImage.SetActive(false);
            
       
            //puzzleImage.transform.SetParent(this.transform,true);
            //transform.SetParent(puzzleController.DragParent, true);
            //spriteImage.transform.SetParent(puzzleController.DragParent, false);
            puzzleImage.SetActive(false);

            RepeatPattern(imageManager.backgroundPanels, gameObject);
            
            foreach (var pos in elementPositions)
            {
                puzzleController.posedPositions.Add(new Vector2(pos.x*128, pos.y*128));//добавляем позиции поставленного пазла    
            }
            
            isDraggable = false;
            puzzleData.isPosed = true;
            
            puzzleController.allTransparentPuzzlesList.Add(rtPuzzleImage.gameObject);
            puzzleController.UpdateProgress();
            puzzleController.SetAlphaToPuzzles(puzzleController.progressCount, puzzleController.winCount);
            
            if (puzzleController.progressCount == puzzleController.winCount)
            {
                var gameScreen = GameObject.Find("GameScreen").GetComponent<UIGameScreen>();
                gameScreen.SetGamePanelsWinPositions();
                puzzleController.currentState.isCollected = true;
                Debug.Log("U WIN!!!!");
            }
            //TODO: ПОФИКСИТЬ ВОЗМОЖНЫЕ ПОЗИЦИИ ТЕНЕЙ, КОГДА ДРУГОЙ ПАЗЛ УЖЕ ПОСТАВЛЕН
            /*var shadowsList = puzzleController.shadowsList;// список теней
            foreach (var shadow in shadowsList)
            {
                var shadowElPos = shadow.GetComponent<PuzzleShadow>().shadowPositions; //список позиций в каждой тени
                var hFigure = shadow.GetComponent<RectTransform>().sizeDelta.x / 128;
                var wFigure = shadow.GetComponent<RectTransform>().sizeDelta.y / 128;
                Vector2 removedPosition = new Vector2();
                Vector2 removedPosition2 = new Vector2();
                
                List<int> removedList = new List<int>();
                foreach (var shadowPos in puzzleController.posedPositions)
                {
                    for (int i = 1; i <= hFigure; i++)
                    {
                        for (int j = 1; j <= wFigure; j++)
                        {
                            //Vector2 removedPosition = new Vector2(shadowPos.x * i, shadowPos.y * j);
                            removedPosition.x = shadowPos.x * i;
                            removedPosition.y = shadowPos.y * j;
                            //Vector2 removedPosition2 = new Vector2(shadowPos.x - 128*i, shadowPos.y -128*j);
                            removedPosition2.x = shadowPos.x - 128 * i;
                            removedPosition2.y = shadowPos.y - 128 * j;
                            
                            //Debug.Log("shadowElPos = "+shadowElPos.Count);
                            if (shadowElPos.Contains(removedPosition))
                            {
                                removedList.Add(shadowElPos.IndexOf(removedPosition));
                            }
                            if (shadowElPos.Contains(removedPosition2))
                            {
                                removedList.Add(shadowElPos.IndexOf(removedPosition2));
                            }
                        }
                    }
                }
                
                for (int k = shadowElPos.Count; k >= 0; k--)
                {
                    if (removedList.Contains(k))
                    {

                        shadowElPos.RemoveAt(k);
                    }
                }
            }*/
        }
        else
        {
            gridImage.transform.SetParent(this.transform,true);
            puzzleImage.transform.SetParent(this.transform,true);
            spriteImage.transform.SetParent(this.transform, false);
            puzzleImage.transform.localPosition = Vector3.zero;
            gridImage.transform.localPosition = Vector3.zero;
            puzzleImage.GetComponent<Image>().enabled = true;
                //spriteImage.SetActive(false);
        }
        puzzleController.SetPreferedContentSize();
    }

    public void RepeatPattern(List<Image> images, GameObject go)
    {
        //Debug.Log("REPEAT");
        foreach (var bg in images)
        {
            Vector3 pos = go.transform.localPosition;
            GameObject puzzleClone = Instantiate(go, bg.transform);
            puzzleClone.transform.localPosition = pos;
            var puzItemScript = go.GetComponent<PuzzleItem>();
            if(puzItemScript == null)
                puzzleController.allTransparentPuzzlesList.Add(puzzleClone);
            else
            {
                puzzleController.allTransparentPuzzlesList.Add(puzzleClone.GetComponent<PuzzleItem>().puzzleImage);
            }
        }
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
            //puzzleImage.transform.position = eventData.position-offsetToCenter;
            Vector3 pos = new Vector3(eventData.position.x, eventData.position.y, 1);
            
            puzzleImage.transform.position = Camera.main.ScreenToWorldPoint(pos);
            puzzleImage.GetComponent<Image>().enabled = false;
            spriteImage.SetActive(true);
            spriteImage.transform.position = puzzleImage.transform.position;
            
            SetPuzzlePosOnGridImage(rtPuzzleImage.anchoredPosition, gridImage, gridImage.GetComponent<PuzzleShadow>().shadowPositions);

            if (isFramed)
            {
                imageManager.FrameOn();
                isFramed = false;
            }
        }
       
    }

    private void SetPuzzlePosOnGridImage(Vector2 puzzle, GameObject gridImage, List<Vector2> puzzlePozList)
    {
       //Debug.Log("GRID POSITIONS");
        
        float nearestPos=1000f;
        foreach (var pos in puzzlePozList)
        {
            //TODO: проверять стоит ли пазл 
            /*var isPosed = false;
            foreach (var p in elementPositions)
            {
                if (puzzleController.posedPositions.Contains(p))
                {
                    elementPositions.Remove(p);
                    isPosed = true;
                    Debug.Log("ХУЙ");
                }
            }*/
            if (!(Vector2.Distance(puzzle, pos) < nearestPos)) continue;
            nearestPos = Vector2.Distance(puzzle, pos);
            gridImage.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.OnEndDrag(eventData);
        isFramed = false;
    }
}