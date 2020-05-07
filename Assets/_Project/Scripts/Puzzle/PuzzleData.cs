using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleData 
{
   public List<Vector2> puzzleData;
   public Vector2 puzzlePosition;
   public bool isPosed;
   public PuzzleData()
   {
      puzzleData = new List<Vector2>();
      puzzlePosition = new Vector2();
   }
}
