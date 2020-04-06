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

public class PuzzleController : MonoBehaviour
{

    public MeshRenderer originalPuzzle; //с него генерим пазлы
    public MeshRenderer puzzleBW;
    public float targetDistance; //дистанция от точки своего назначения, чем больше, тем больше допустимая неточность изображения
    public string puzzleTag = "GameController";
    /*public int columns;
    public int lines;*/
    public float smooth; //сглаживание всех пазлов во время соединения
    //public float puzzleDistance;
    public GameObject tookenPuzzle;
    public ScrollRect scrollRect;


    private int puzzleCounter;
    private int sortingOrder;
    public List<SpriteRenderer> puzzle = new List<SpriteRenderer>();
    private List<Vector3> puzzlePos = new List<Vector3>();
    private Transform current;
    private Vector3 offset;
    private bool isWin;
    private Vector3 scrollPosition;

    private SettingsGame gameSettings;

    //public SnapScrollingScript snapScrolling;

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
        
        puzzle = new List<SpriteRenderer>();
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


	    float distance = -250f;
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

			    // создание нового объекта и настройка его позиции
			    GameObject obj = new GameObject("Puzzle: " + puzzleCounter);
			    obj.transform.parent = transform;
			    position = new Vector3(((int) (position.x * 100f)) / 100f, ((int) (position.y * 100f)) / 100f, 0);
			    puzzlePos.Add(position);
			    Debug.Log("puzzlePos " + position);
			    
			    position = new Vector3(  distance,0, 0);
			    obj.transform.localPosition = position;
			    //obj.transform.position = position;
			    Debug.Log("localPosition" + obj.transform.position);

			    // конвертируем текстуру в спрайт
			    SpriteRenderer ren = obj.AddComponent<SpriteRenderer>();
			    int unit = Mathf.RoundToInt((float) Screen.height /
			                                (Camera.main.orthographicSize *
			                                 2f)); // формула расчета размера спрайта (только для режима камеры Оrthographic)
			    ren.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), unit);
			    ren.sortingOrder = 1;
			    obj.AddComponent<BoxCollider2D>();
			    obj.tag = puzzleTag;
				
			    
			   
			    puzzle.Add(ren);
			    puzzleCounter++;
			    distance += 200f;
		    }

	    }

	    originalPuzzle.gameObject.SetActive(false);
	    puzzleBW.gameObject.SetActive(true);
	    int g = 0;
	    Debug.Log("SetPos: " + g++);
    }


    private void Awake()
    {
	    gameSettings = ToolBox.Get<SettingsGame>();
	    NewGame();
	    Debug.Log("NewGame!");

    }
    void GetPuzzle()
    {
	   
	    Debug.Log("SCROLL POSITION: " + scrollPosition);
	    // массив рейкаста, чтобы фильтровать спрайты по глубине Z (тот что ближе, будет первым элементом массива)
	    RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
	    if(hit.Length > 0 && hit[0].transform.tag == puzzleTag)
	    {
		    offset = hit[0].transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);;
		    current = hit[0].transform;
		    sortingOrder = current.GetComponent<SpriteRenderer>().sortingOrder;
		    current.GetComponent<SpriteRenderer>().sortingOrder = puzzleCounter + 1;
		    //Debug.Log("currentName = " + current.name);
		    //current.SetParent(tookenPuzzle.transform);//чтобы потом не скролилось все
		    Debug.Log("position " + current.transform.position);
		    scrollPosition = current.transform.position;
	    }
    }
    
    int CheckPuzzle(float distance) // проверка всех пазлов, относительно точек назначения
    {
	    int i = 0;
	    for(int j = 0; j < puzzle.Count; j++)
	    {
		    if(Vector2.Distance(puzzle[j].transform.position, puzzlePos[j]) < distance)
		    {
			    Debug.Log("Puzzle position: " + puzzle[j].transform.position);
			    Debug.Log("PuzzlePos position: " + puzzlePos[j]);
			    i++;
			    puzzle[j].GetComponent<BoxCollider2D>().enabled = false;
		    }
		    
	    }
	    return i;
    }

    void SetPosition()
    {
	    float distance = 0f;

	    foreach(SpriteRenderer p in puzzle)
	    {
		    p.transform.position = new Vector3(0,distance , 0);
		    distance += 10f;
		    Debug.Log("distance = " + distance);
		    Debug.Log("puzzleName = " + p.name);

	    }
    }
    
    void Update()
    {
	    if(isWin)
	    {
		    if(CheckPuzzle(gameSettings.puzzleDistance) == puzzle.Count)
		    {
			    Clear();
			    originalPuzzle.gameObject.SetActive(true);
		    }
		    else
		    {
			    for(int j = 0; j < puzzle.Count; j++)
			    {
				    puzzle[j].transform.position = Vector3.Lerp(puzzle[j].transform.position, puzzlePos[j], smooth * Time.deltaTime);
				    Debug.Log("ХУЙ!");
			    }
		    }
	    }
	    else
	    {
		    if(Input.GetMouseButtonDown(0))
		    {
			    GetPuzzle();
			    scrollRect.horizontal = false;

		    }
		    else if(Input.GetMouseButtonUp(0))
		    {
			    current.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

			    if(CheckPuzzle(targetDistance) == puzzle.Count)
			    {
				    isWin = true;
				    Debug.Log("!WIN!");
			    }
			    else
			    {
				    current.transform.position = scrollPosition;
			    }
			    
			    current = null;
			    scrollRect.horizontal = true;
		    }	
		    
	    }

	    if(current)
	    {
		    Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    current.position = new Vector3(position.x, position.y, current.position.z) + new Vector3(offset.x, offset.y, 0);
	    }
    }
}
    
    
    
    
    
    
    
    
    
    
    
    
    
    

