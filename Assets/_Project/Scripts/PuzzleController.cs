using System;
using System.Collections;
using System.Collections.Generic;
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

    public MeshRenderer originalPuzzle; //с него генерим пазлы
    public MeshRenderer puzzleBW;
    public GameObject checkedPuzzles;
    public List<GameObject> puzzlePrefabs = new List<GameObject>();
    public GameObject puzzlePrefab;
    public SettingsGame gameSettings;

    private int puzzleCounter;
    private int sortingOrder;
    private List<Vector3> puzzlePos = new List<Vector3>();
    private Transform current;
    private Vector3 offset;
    private bool isWin;

    void NewGame()
    {
	    originalPuzzle.gameObject.SetActive(true);
	    puzzleBW.gameObject.SetActive(false);
        Clear();
	    StartCoroutine(Generate());
    }
    void Clear()
    {
        isWin = false;
        puzzleCounter = 0;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        puzzlePos = new List<Vector3>();
    }

    IEnumerator Generate() //создание пазлов/нарезка текстуры
    {
	    Debug.Log("Хоп, генерейт!");
	    // переносим размеры холста в пространство экрана
	    Vector3 posStart =
		    Camera.main.WorldToScreenPoint(new Vector3(originalPuzzle.bounds.min.x, originalPuzzle.bounds.min.y,
			    originalPuzzle.bounds.min.z));
	    Vector3 posEnd =
		    Camera.main.WorldToScreenPoint(new Vector3(originalPuzzle.bounds.max.x, originalPuzzle.bounds.max.y,
			    originalPuzzle.bounds.min.z));

	    int width = (int) (posEnd.x - posStart.x);
	    int height = (int) (posEnd.y - posStart.y);

	    // определяем размеры пазла
	    int w_cell = width / gameSettings.columns;
	    int h_cell = height / gameSettings.lines;

	    // учитываем рамку, т.е. неиспользуемое пространство вокруг холста
	    int xAdd = (Screen.width - width) / 2;
	    int yAdd = (Screen.height - height) / 2;

	    yield return new WaitForEndOfFrame();

	    for (int y = 0; y < gameSettings.lines; y++)
	    {
		    for (int x = 0; x < gameSettings.columns; x++)
		    {
			    // делаем снимок части экрана
			    Rect rect = new Rect(0, 0, w_cell, h_cell);
			    rect.center = new Vector2((w_cell * x + w_cell / 2) + xAdd, (h_cell * y + h_cell / 2) + yAdd);
			    Vector3 position = Camera.main.ScreenToWorldPoint(rect.center);
			    Texture2D tex = new Texture2D(w_cell, h_cell, TextureFormat.ARGB32, false);
			    tex.ReadPixels(rect, 0, 0);
			    tex.Apply();

			    //настройка позиции
			    position = new Vector3(((int) (position.x * 100f)) / 100f, ((int) (position.y * 100f)) / 100f, 0);
			    puzzlePos.Add(position);

			    // конвертируем текстуру в спрайт
			    GameObject instPuzzle = Instantiate(puzzlePrefab, transform, false);
			    SpriteRenderer ren = instPuzzle.AddComponent<SpriteRenderer>();
				int unit = Mathf.RoundToInt((float) Screen.height /
				                            (Camera.main.orthographicSize *
				                             2f)); // формула расчета размера спрайта (только для режима камеры Оrthographic)
			    ren.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), unit);
			    ren.sortingOrder = 1;
			    
			    instPuzzle.GetComponent<Image>().sprite = ren.sprite;
			    instPuzzle.GetComponent<RectTransform>().sizeDelta = new Vector2(w_cell, h_cell);

			    Puzzle instPiece = instPuzzle.GetComponent<Puzzle>();
			    instPiece.puzzleController = this;
			    instPiece.puzzlePos = position;
			    puzzlePrefabs.Add(instPuzzle);

			    puzzleCounter++;
		    }

	    }

	    originalPuzzle.gameObject.SetActive(false);
	    puzzleBW.gameObject.SetActive(true);
    }

    private void Awake()
    {
	    gameSettings = ToolBox.Get<SettingsGame>();
	    NewGame();
	    Debug.Log("NewGame!");

    }

}
    
    
    
    
    
    
    
    
    
    
    
    
    
    

