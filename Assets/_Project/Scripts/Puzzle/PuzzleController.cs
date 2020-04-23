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
    public Image originalPuzzle; //с него генерим пазлы
    public GameObject puzzleBW;
    public GameObject checkedPuzzles;
    public List<GameObject> puzzlePrefabs = new List<GameObject>();
    public GameObject puzzlePrefab;
    public SettingsGame gameSettings;
    public bool isGenerated;
    public GameObject contentBox;
    public GameObject scrollView;
    public GameObject scrollViewContent;
    public ImageManager imageManager;
    public GameObject PuzzleElementPrefab;

    private int sortingOrder;
    public List<Vector3> puzzlePos = new List<Vector3>();
    private Transform current;
    private Vector3 offset;
    private bool isWin;

    void NewGame()
    {
        isGenerated = false;
        originalPuzzle.gameObject.SetActive(true);
        puzzleBW.SetActive(false);
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
        puzzlePos = new List<Vector3>();
    }

    private void Generate() //создание пазлов/нарезка текстуры
    {
        Texture2D mainTexture = originalPuzzle.mainTexture as Texture2D;
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

            foreach (var elementPosition in block)
            {
                Texture2D tex = new Texture2D(w_cell, h_cell, TextureFormat.ARGB32, false);
                tex.SetPixels(mainTexture.GetPixels((int) elementPosition.x * w_cell, (int) elementPosition.y * h_cell,
                    h_cell, w_cell));
                tex.Apply();

                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));

                GameObject newPuzzle = Instantiate(PuzzleElementPrefab, blockParent.transform);
                newPuzzle.GetComponent<Image>().sprite = sprite;
                newPuzzle.GetComponent<RectTransform>().sizeDelta = new Vector2(w_cell, h_cell);
                newPuzzle.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2((elementPosition.x - minX) * w_cell, (elementPosition.y - minY) * h_cell);
                puzzleBlock.puzzleController = this;
                puzzleBlock.puzzlePosition = new Vector2(minX * w_cell, minY * h_cell);
            }

            puzzlePrefabs.Add(blockParent);
            blockParent.GetComponent<RectTransform>().sizeDelta =
                new Vector2((maxX - minX + 1) * w_cell, (maxY - minY + 1) * h_cell);
            blockParent.transform.SetParent(scrollViewContent.transform);
        }


        this.NextFrame(() => SetPrefferedContentSize());
        isGenerated = true;
        originalPuzzle.gameObject.SetActive(false);
        puzzleBW.SetActive(true);
    }

    public void SetPrefferedContentSize()
    {
        scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(
            scrollViewContent.GetComponent<HorizontalLayoutGroup>().preferredWidth,
            scrollViewContent.GetComponent<HorizontalLayoutGroup>().preferredHeight);
        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(
            0f,
            scrollViewContent.GetComponent<HorizontalLayoutGroup>().preferredHeight);
    }

    private void Start()
    {
        gameSettings = ToolBox.Get<SettingsGame>();
        NewGame();
        Debug.Log("NewGame!");
    }
}