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
    public Image puzzleBW;
    public Transform DragParent;

    [Space]
    public GameObject puzzlePrefab;
    public GameObject PuzzleElementPrefab;
    
    [Space]
    public ImageManager imageManager;
    
    
    [Space]
    public GameObject scrollView;
    public GameObject scrollViewContent;

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
        // определяем размеры пазла
        int w_cell = mainTexture.width / gameSettings.columns;
        int h_cell = mainTexture.height / gameSettings.lines;

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
            var pixel = tex.GetPixels();
            for (var index = 0; index < pixel.Length; index++)
            { 
                pixel[index] = Color.clear;
            }
            tex.SetPixels(pixel);
            tex.Apply();
            foreach (var elementPosition in block)
            {
                var pixels = mainTexture.GetPixels((int) elementPosition.x * w_cell, (int) elementPosition.y * h_cell,
                    h_cell, w_cell);
                tex.SetPixels((int)(elementPosition.x-minX)*w_cell, (int)(elementPosition.y - minY)*h_cell, h_cell, w_cell, pixels);
                
                //tex.SetPixels(mainTexture.GetPixels((int)(elementPosition.x-minX+1)*w_cell, (int)(elementPosition.y - minY+1)*h_cell, h_cell, w_cell));

                Debug.Log("x " + (int)(elementPosition.x-minX)*w_cell);
                Debug.Log("y " +(int)(elementPosition.y - minY)*h_cell);
            }
            tex.Apply();
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            Debug.Log("tex.w = " + tex.width);
            Debug.Log("tex.h = " + tex.height);

            GameObject newPuzzle = Instantiate(PuzzleElementPrefab, blockParent.transform);
            //this.NextFrame(()=>newPuzzle.transform.localScale=Vector3.one);
            newPuzzle.GetComponent<Image>().sprite = sprite;
            newPuzzle.GetComponent<RectTransform>().sizeDelta = new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);
                //newPuzzle.GetComponent<RectTransform>().anchoredPosition = new Vector2((elementPosition.x - minX) * w_cell, (elementPosition.y - minY) * h_cell);
            //newPuzzle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            puzzleBlock.puzzleController = this;
            puzzleBlock.puzzlePosition = new Vector2(minX * w_cell, minY * h_cell);
            
            //puzzlePrefabs.Add(blockParent);
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