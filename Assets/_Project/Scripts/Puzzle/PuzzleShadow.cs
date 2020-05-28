using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PuzzleShadow : MonoBehaviour
{
   public PuzzleController puzzleController;
   public List<Vector2> shadowPositions;
   public List<Vector2> shadowElementPositions;

   /*private void Start()
   {
      int x = 0;
      Vector2 possiblePos;
      while (x + transform.GetComponent<RectTransform>().sizeDelta.x <=
                  puzzleController.originalImage.GetComponent<RectTransform>().sizeDelta.x)
      {
         int y = 0;
         while (y + transform.GetComponent<RectTransform>().sizeDelta.y <=
                puzzleController.originalImage.GetComponent<RectTransform>().sizeDelta.y)
         {
            possiblePos = new Vector2(x, y);
            shadowPositions.Add(possiblePos);
            y += puzzleController.wCell;
         }

         x += puzzleController.hCell;
      }
   }*/
}
