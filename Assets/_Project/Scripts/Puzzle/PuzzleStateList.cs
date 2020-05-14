using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PuzzleStateList
{
    public List<PuzzleState> puzzleStates;


    public PuzzleStateList()
    {
        puzzleStates = new List<PuzzleState>();
    }

    public PuzzleStateList(PuzzleState pState, string ID)
    {
        pState.puzzleID = ID;
        puzzleStates.Add(pState);
    }
}
