using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Content 
{
    public List<PuzzleSource> puzzleSources = new List<PuzzleSource>();
    public static Content Load()
    {
        string jsonStr = Resources.Load<TextAsset>("content").text;
        Content c = JsonUtility.FromJson<Content>(jsonStr);
        return c;
    }
}
