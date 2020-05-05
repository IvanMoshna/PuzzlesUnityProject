using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Puzzles.Settings;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    
    
    [Space]
    public GameObject scrollView;
    public GameObject scrollViewContent;

    [Space] 
    public int wCell;
    public int hCell;

    private SettingsGame gameSettings;
    private bool isWin;
   

    void NewGame()
    {
        originalImage.gameObject.SetActive(true);
        Clear();
        Generate();
    }

    void Clear()
    {
        isWin = false;
        /*foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }*/
    }

    private void Generate() //создание пазлов/нарезка текстуры
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
        List<List<Vector2>> puzzle = PuzzlesCreator.CreatePuzzle(gameSettings.lines, gameSettings.columns);

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
            foreach (var elementPosition in block)
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
            
            
            
            foreach (var elementPosition in block)
            {
                
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
                //newPuzzle.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                //newPuzzle.GetComponent<RectTransform>().anchoredPosition = new Vector2((elementPosition.x - minX) * w_cell, (elementPosition.y - minY) * h_cell);
            //newPuzzle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            puzzleBlock.puzzleController = this;
            puzzleBlock.puzzlePosition = new Vector2(minX * w_cell, minY * h_cell);

            
            var puzzleItem = puzzleBlock.GetComponent<PuzzleItem>();
            puzzleItem.backgroundImage.GetComponent<Image>().sprite = bgSprite;
            puzzleItem.backgroundImage.GetComponent<RectTransform>().sizeDelta = new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);
            
          
            var puzzleItemGridImage = puzzleItem.gridImage;
            puzzleItemGridImage.GetComponent<Image>().sprite = gridSprite;
            puzzleItemGridImage.GetComponent<Image>().color = new Color(1,1,1,gameSettings.transparency);
            puzzleItemGridImage.GetComponent<RectTransform>().sizeDelta = new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);
            puzzleItemGridImage.GetComponent<PuzzleShadow>().puzzleController = this;
            
            
            blockParent.GetComponent<RectTransform>().sizeDelta =
                new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);
            blockParent.transform.SetParent(scrollViewContent.transform);
        }

        this.NextFrame(() => SetPreferedContentSize());
        originalImage.gameObject.SetActive(false);
    }

    public void SetPreferedContentSize()
    {
        var layoutGroup = scrollViewContent.GetComponent<HorizontalLayoutGroup>();
        scrollViewContent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(layoutGroup.preferredWidth, layoutGroup.preferredHeight);
        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, layoutGroup.preferredHeight);
    }

    private void Start()
    {
        gameSettings = ToolBox.Get<SettingsGame>();
        NewGame();
        Debug.Log("NewGame!");
    }
}