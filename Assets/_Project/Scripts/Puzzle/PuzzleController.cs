using System;
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
using UnityEngine.tvOS;
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
    public BasicGridAdapter osa;

    [Space] 
    public int wCell;
    public int hCell;
    public int progressCount;
    public int winCount;
    public float startTransparency;
    
    public List<Vector2> posedPositions;
    public List<GameObject> shadowsList;
    public List<GameObject> allPuzzlesList;

    [Space] 
    public Content content;
    
    [Space]
    public PuzzleStateList DataPuzzleState;
    public PuzzleState currentState;

    private SettingsGame gameSettings;
    private bool isWin;
    

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
        currentState = null;
        allPuzzlesList.Clear();
    }

    public void InitView(List<PuzzleData> puzzle) //нарезка текстуры
    {
        Debug.Log("INIT VIEW");
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
        var shuffledList = new List<GameObject>();
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
            
            if(gameSettings.isTransparency)
                newPuzzle.GetComponent<Image>().color = new Color(1,1,1,startTransparency);//сетаем начальную прозрачность пазла
            
            newPuzzle.GetComponent<RectTransform>().sizeDelta = new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);

            puzzleBlock.puzzleController = this;
            puzzleBlock.puzzleData = block;
            puzzleBlock.puzzleData.puzzlePosition = new SerializablePosition(minX * w_cell, minY * h_cell);

            var puzzleItem = puzzleBlock.GetComponent<PuzzleItem>();
            puzzleItem.backgroundImage.GetComponent<Image>().sprite = bgSprite;
            puzzleItem.backgroundImage.GetComponent<RectTransform>().sizeDelta = new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);
            puzzleItem.canvas = this.canvas;
            puzzleItem.progressItemCount = puzzleItem.elementPositions.Count; 
            
            var puzzleItemGridImage = puzzleItem.gridImage;
            shadowsList.Add(puzzleItemGridImage);
            puzzleItemGridImage.GetComponent<Image>().sprite = gridSprite;
            puzzleItemGridImage.GetComponent<Image>().color = new Color(1,1,1,gameSettings.transparency);
            puzzleItemGridImage.GetComponent<RectTransform>().sizeDelta = new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);
            puzzleItemGridImage.GetComponent<PuzzleShadow>().puzzleController = this;
            
            blockParent.GetComponent<RectTransform>().sizeDelta =
                new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);

            shuffledList.Add(blockParent);
        }
        
        if(gameSettings.isShuffled)
            shuffledList.Shuffle();
        foreach (var item in shuffledList)
        {
            allPuzzlesList.Add(item.GetComponent<PuzzleItem>().puzzleImage);
            if (item.GetComponent<PuzzleItem>().puzzleData.isPosed)
            {
                item.transform.SetParent(DragParent);
            }
            else
            {
                item.transform.SetParent(scrollViewContent.transform);
            }
        }
        
        this.NextFrame(SetPreferedContentSize);
        originalImage.gameObject.SetActive(false);
    }

    public void SetAlphaToPuzzles(int progress, int winProgress)
    {
        foreach (var item in allPuzzlesList)
        {
            float ratio =(float) ((double)progress/(double)winProgress);
            float itemAlpha = 0.5f + ratio*0.5f;
            var colorToItem = new Color(1,1,1,itemAlpha);
            item.GetComponent<Image>().color = colorToItem;
        }
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
        winCount = gameSettings.columns * gameSettings.lines;
        InitGameState();
    }
    
    public void NewGame()
    {
        
        //InitGameState();

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
            currentState = puzzleState;
            
            Debug.Log("puzzleCreator");
            InitView(puzzleState.puzzleDatas);
            UIController.GetComponent<UIController>().GoToGameScreen();
        }
        else
        {
            bool isInited = false;
            foreach (var pID in DataPuzzleState.puzzleStates)
            {
                if (pID.puzzleID == ID)
                {
                    currentState = pID;
                    UIController.GetComponent<UIController>().OnContinueScreen();
                    //затем смотрим чо нажмет и только потом вызываем InitView
                    Debug.Log(pID.puzzleID);
                    //InitView(pID.puzzleDatas);
                    isInited = true;
                    break;
                }
            }

            if (!isInited)
            {
                PuzzleState puzState = PuzzlesCreator.CreatePuzzle(gameSettings.lines, gameSettings.columns);
                puzState.puzzleID = ID;
                currentState = puzState;
                DataPuzzleState.puzzleStates.Add(puzState);
                InitView(puzState.puzzleDatas);
                UIController.GetComponent<UIController>().GoToGameScreen();
            }

            /*progressCount = InitProgress(currentState);
            currentState.progressCount = progressCount;*/
            
        }
        //UpdateProgress();
        //InitView(DataPuzzleState.puzzleStates);
        //SetAlphaToPuzzles(progressCount);
    }

    public int InitProgress(PuzzleState puzzleState)
    {
        int counter=0;

            foreach (var pData in puzzleState.puzzleDatas)
            {
                if (pData.isPosed)
                {
                    counter += pData.puzzleData.Count;
                }
            }
            return counter;
    }

    public void UpdateProgress()
    {
        Debug.Log("Update Progress");
        progressCount = InitProgress(currentState);
        currentState.progressCount = progressCount;
        //SetAlphaToPuzzles(progressCount, winCount);
    }
    
    private void InitGameState()
    {
        DataPuzzleState = ToolSaver.Instance.Load<PuzzleStateList>(gameSettings.PathSaves);
    }

    public void SaveGameState()
    {
        Debug.Log("SAVE");
        if (currentState != null)
        {
            foreach (var dataItems in DataPuzzleState.puzzleStates)
            {
                if (dataItems.puzzleID == currentState.puzzleID)
                {
                    //TODO: что то не то он удаляет
                    DataPuzzleState.puzzleStates.Remove(dataItems);
                    Debug.Log("Remove");
                    break;
                }
            }
            
            DataPuzzleState.puzzleStates.Add(currentState);
            currentState = null;
        }

        if (gameSettings.IsSaveGame)
        {
            ToolSaver.Instance.Save(gameSettings.PathSaves, DataPuzzleState);
        }
    }
    
    public void RefreshData(PuzzleState currentData, PuzzleStateList dataList)
    {
        foreach (var dataItem in dataList.puzzleStates)
        {
            if (dataItem.puzzleID == currentData.puzzleID)
            {
                dataList.puzzleStates.Remove(dataItem);
                break;
            }
        }
        dataList.puzzleStates.Add(currentData);
        //currentData = null;
    }
    private void OnApplicationQuit() { SaveGameState(); }
}