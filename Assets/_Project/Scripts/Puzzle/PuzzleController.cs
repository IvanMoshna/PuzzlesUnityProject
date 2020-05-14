﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Puzzles.Configs;
using Puzzles.Configs;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Your.Namespace.Here20May12044942767.Grids;
using Image = UnityEngine.UI.Image;

public class PuzzleController : MonoBehaviour
{
    public Image originalImage; //с него генерим пазлы
    public Image backgroundImage;
    public Image gridImage;
    public Transform DragParent;

    [Space]
    public GameObject puzzlePrefab;
    public GameObject PuzzleElementPrefab;
    
    [Space]
    public ImageManager imageManager;

    public GameObject UIController;
    
    [Space]
    public GameObject scrollView;
    public GameObject scrollViewContent;
    public Canvas canvas;

    [Space] 
    public int wCell;
    public int hCell;

    public List<Vector2> posedPositions;
    public List<GameObject> shadowsList;

    [Space] 
    public Content content;
    
    private SettingsGame gameSettings;
    private bool isWin;
    private PuzzleStateList DataPuzzleState;

    public void Clear()
    {
        var contentList = scrollViewContent.transform;
        foreach (Transform cl in contentList)
        {
            Destroy(cl.gameObject);
        }

        var dragParentList = DragParent.transform;
        foreach (Transform dP in dragParentList)
        {
            Destroy(dP.gameObject);
        }

        foreach (var bp in imageManager.backgroundPanels)
        {
            foreach (Transform bpImage in bp.transform)
            {
                Destroy(bpImage.gameObject);
            }
        }
    }

    private void InitView(List<PuzzleData> puzzle) //нарезка текстуры
    {
        Texture2D mainTexture = originalImage.mainTexture as Texture2D;
        Texture2D backgroundTexture = backgroundImage.mainTexture as Texture2D;
        Texture2D gridTexture = gridImage.mainTexture as Texture2D;
        /*var gridPixels = gridTexture.GetPixels();
        for (int i = 0; i < gridPixels.Length; i++)
        {
            gridPixels[i] = Color.gray;
        }
        gridTexture.SetPixels(gridPixels);
        gridTexture.Apply();
        Sprite gridSprite = Sprite.Create(gridTexture, new Rect(0,0, gridTexture.width, gridTexture.height), new Vector2(.5f, .5f));*/

        // определяем размеры пазла
        int w_cell = mainTexture.width / gameSettings.columns;
        int h_cell = mainTexture.height / gameSettings.lines;

        wCell = w_cell;
        hCell = h_cell;
        //List<PuzzleData> puzzle = PuzzlesCreator.CreatePuzzle(gameSettings.lines, gameSettings.columns);
        //PuzzleState puzzleState = new PuzzleState(puzzle);

        foreach (var block in puzzle)
        {
            GameObject blockParent = Instantiate(puzzlePrefab, transform, false);
            var puzzleBlock = blockParent.GetComponent<PuzzleItem>();
            puzzleBlock.ScrollParent = scrollViewContent;

            puzzleBlock.imageManager = imageManager;
            int minX = Int32.MaxValue;
            int maxX = 0;
            int minY = Int32.MaxValue;
            int maxY = 0;
            foreach (var elementPosition in block.puzzleData)
            {
                if (elementPosition.x < minX) minX = (int) elementPosition.x;
                if (elementPosition.x > maxX) maxX = (int) elementPosition.x;
                if (elementPosition.y < minY) minY = (int) elementPosition.y;
                if (elementPosition.y > maxY) maxY = (int) elementPosition.y;
            }
            
            
            Texture2D tex = new Texture2D((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell, TextureFormat.ARGB32, false);
            Texture2D backgroundTex = new Texture2D((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell, TextureFormat.ARGB32, false);
            Texture2D gridTex = new Texture2D((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell, TextureFormat.ARGB32, false);
            
            var clearPixels = tex.GetPixels();
            var bp = backgroundTex.GetPixels();
            var gp = gridTex.GetPixels();
            for (var index = 0; index < clearPixels.Length; index++)
            { 
                clearPixels[index] = Color.clear;
                bp[index] = Color.clear;
                gp[index] = Color.clear;
            }
            tex.SetPixels(clearPixels);
            backgroundTex.SetPixels(bp);
            gridTex.SetPixels(gp);
            tex.Apply();
            backgroundTex.Apply();
            gridTex.Apply();


            var shadowElPos = puzzleBlock.gridImage.GetComponent<PuzzleShadow>().shadowElementPositions;
            foreach (var eP in block.puzzleData)
            {
                Vector2 elementPosition = new Vector2(eP.x, eP.y);
                puzzleBlock.elementPositions.Add(elementPosition);
                shadowElPos.Add(new Vector2(elementPosition.x*128, elementPosition.y*128));
                var pixels = mainTexture.GetPixels((int) elementPosition.x * w_cell, (int) elementPosition.y * h_cell,
                    h_cell, w_cell);
                var backgroundPixels = backgroundTexture.GetPixels((int) elementPosition.x * w_cell, (int) elementPosition.y * h_cell,
                    h_cell, w_cell);
                var gridPixels = gridTexture.GetPixels((int) elementPosition.x * w_cell, (int) elementPosition.y * h_cell,
                    h_cell, w_cell);
                tex.SetPixels((int)(elementPosition.x-minX)*w_cell, (int)(elementPosition.y - minY)*h_cell, h_cell, w_cell, pixels);
                backgroundTex.SetPixels((int)(elementPosition.x-minX)*w_cell, (int)(elementPosition.y - minY)*h_cell, h_cell, w_cell, backgroundPixels);
                gridTex.SetPixels((int)(elementPosition.x-minX)*w_cell, (int)(elementPosition.y - minY)*h_cell, h_cell, w_cell, gridPixels);
            }
            tex.Apply();
            backgroundTex.Apply();
            gridTex.Apply();
            
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            Sprite bgSprite = Sprite.Create(backgroundTex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            Sprite gridSprite = Sprite.Create(gridTex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));

            GameObject newPuzzle = Instantiate(PuzzleElementPrefab, blockParent.transform);
            puzzleBlock.puzzleImage = newPuzzle;
            //this.NextFrame(()=>newPuzzle.transform.localScale=Vector3.one);
            newPuzzle.GetComponent<Image>().sprite = sprite;
            newPuzzle.GetComponent<RectTransform>().sizeDelta = new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);

            puzzleBlock.puzzleController = this;
            puzzleBlock.puzzleData = block;
            puzzleBlock.puzzleData.puzzlePosition = new SerializablePosition(minX * w_cell, minY * h_cell);

            var puzzleItem = puzzleBlock.GetComponent<PuzzleItem>();
            puzzleItem.backgroundImage.GetComponent<Image>().sprite = bgSprite;
            puzzleItem.backgroundImage.GetComponent<RectTransform>().sizeDelta = new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);
            puzzleItem.canvas = this.canvas;
            
            var puzzleItemGridImage = puzzleItem.gridImage;
            shadowsList.Add(puzzleItemGridImage);
            puzzleItemGridImage.GetComponent<Image>().sprite = gridSprite;
            puzzleItemGridImage.GetComponent<Image>().color = new Color(1,1,1,gameSettings.transparency);
            puzzleItemGridImage.GetComponent<RectTransform>().sizeDelta = new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);
            puzzleItemGridImage.GetComponent<PuzzleShadow>().puzzleController = this;
            
            blockParent.GetComponent<RectTransform>().sizeDelta =
                new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);

            if (puzzleBlock.puzzleData.isPosed)
            {
                
                blockParent.transform.SetParent(DragParent);
                
            }
            else
            {
                blockParent.transform.SetParent(scrollViewContent.transform);
            }

        }
        this.NextFrame(SetPreferedContentSize);
        originalImage.gameObject.SetActive(false);
    }

    public void SetPreferedContentSize()
    {
        var layoutGroup = scrollViewContent.GetComponent<HorizontalLayoutGroup>();
        scrollViewContent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(layoutGroup.preferredWidth, layoutGroup.preferredHeight);
        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, layoutGroup.preferredHeight);
    }

    private void Awake()
    {
        content = Content.Load();
        ToolBox.Add(content);
    }

    private void Start()
    {
        gameSettings = ToolBox.Get<SettingsGame>();



        /*originalImage.gameObject.SetActive(true);
        //Clear();
        InitGameState();
        if (DataPuzzleState == null)
        {
            //DataPuzzleState = PuzzlesCreator.CreatePuzzle(gameSettings.lines, gameSettings.columns);
            NewGame();
        }
        else
        {
            UIController.GetComponent<UIController>().ContinueScreen.SetActive(true);
            InitView(DataPuzzleState.puzzleDatas);
        }*/
        //InitView(DataPuzzleState.puzzleDatas);

    }
    
    public void NewGame()
    {
        
        InitGameState();

        string ID = originalImage.sprite.name;
        
        foreach (var panel in imageManager.backgroundPanels)
        {
            panel.sprite = backgroundImage.sprite;
        }

        if (DataPuzzleState == null)
        {
            PuzzleState puzzleState = PuzzlesCreator.CreatePuzzle(gameSettings.lines, gameSettings.columns);
            puzzleState.puzzleID = ID;
            DataPuzzleState = new PuzzleStateList();
            DataPuzzleState.puzzleStates.Add(puzzleState);
            
            Debug.Log("puzzleCreator");
            InitView(puzzleState.puzzleDatas);
        }
        else
        {
            foreach (var pID in DataPuzzleState.puzzleStates)
            {
                if (pID.puzzleID == ID)
                {
                    //UIController.GetComponent<UIController>().OnContinueScreen();
                    Debug.Log(pID.puzzleID);
                    InitView(pID.puzzleDatas);
                    break;
                }
            }
            PuzzleState puzState = PuzzlesCreator.CreatePuzzle(gameSettings.lines, gameSettings.columns);
            puzState.puzzleID = ID;
            DataPuzzleState.puzzleStates.Add(puzState);
            InitView(puzState.puzzleDatas);
        }
        //InitView(DataPuzzleState.puzzleStates);
    }
    
    
    private void InitGameState()//надо делать вообще где нить в Awake
    {
        DataPuzzleState = ToolSaver.Instance.Load<PuzzleStateList>(gameSettings.PathSaves);
    }

    public void SaveGameState()
    {
        if (gameSettings.IsSaveGame)
        {
            ToolSaver.Instance.Save(gameSettings.PathSaves, DataPuzzleState);
        }
    }
    
    private void OnApplicationQuit() { SaveGameState(); }
}