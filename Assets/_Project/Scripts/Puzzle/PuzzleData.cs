using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PuzzleData 
{
   //public List<Vector2> puzzleData;
   //public Vector2 puzzlePosition;
   public List<SerializableVector> puzzleData;
   public SerializablePosition puzzlePosition;
   public bool isPosed;
   public PuzzleData()
   {
      puzzleData = new List<SerializableVector>();
      //puzzlePosition = new SerializablePosition();
   }
}
