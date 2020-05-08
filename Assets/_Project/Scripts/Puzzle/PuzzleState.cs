using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PuzzleState
{
    public List<PuzzleData> puzzleDatas;

    public PuzzleState()
    {
        puzzleDatas = new List<PuzzleData>();
    }

    public PuzzleState(List<PuzzleData> pd)
    {
        puzzleDatas = pd;
    }
}
