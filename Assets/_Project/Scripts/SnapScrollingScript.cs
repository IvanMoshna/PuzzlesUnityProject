
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapScrollingScript : MonoBehaviour {
/*
    [Range(1, 200)]
    [Header("Controllers")]
    //public int panCount; [Range(0, 700)]
    public int panOffset; [Range(0f, 200f)]
    public float snapSpeed; [Range(0f, 5f)]
    //public float scaleOffset; [Range(1f, 20f)]
    //public float scaleSpeed = 0f;
    //public PuzzleScript puzzleController;
    
    [Header("Other Object")]
    public GameObject panPrefab;

    private GameObject[] instPans;
    private Vector2[] pansPos;
    private Vector2[] pansScale;

    private RectTransform contentRect;
    private Vector2 contentVector;
    
    public int selectedPanID;
    public bool isScrolling;

    private int panCount;


    void Start()
    {
        panCount = puzzleController.puzzle.Count;
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];
        for(int i=0; i<panCount; i++)
        {
            //instPans[i] = Instantiate(panPrefab, transform, false);
            
            if (i == 0) continue;
            puzzleController.puzzle[i].transform.localPosition = new Vector2(puzzleController.puzzle[i].transform.localPosition.x  /*+ panPrefab.GetComponent<RectTransform>().sizeDelta.x#1# +panOffset, 
                puzzleController.puzzle[i].transform.localPosition.y);
            pansPos[i] = -puzzleController.puzzle[i].transform.localPosition;
        }
		
	}

    private void FixedUpdate()
    {
        float nearestPos = float.MaxValue;
        for(int i=0; i<panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if(distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
            //float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            //pansScale[i].x = Mathf.SmoothStep(puzzleController.puzzle[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            //pansScale[i].y = Mathf.SmoothStep(puzzleController.puzzle[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            //puzzleController.puzzle[i].transform.localScale = pansScale[i];

        }

        if (isScrolling) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }
*/
}

