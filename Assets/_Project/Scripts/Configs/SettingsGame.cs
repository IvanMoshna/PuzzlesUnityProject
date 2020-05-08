using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Common;

namespace Puzzles.Configs
{
    [CreateAssetMenu(fileName = "SettingsGame", menuName = "Settings/SettingsGame")]
    public class SettingsGame : ScriptableObject
    {
        public float puzzleDistance;
        
        [Space]
        public int columns;
        public int lines;
        
        public float transparency;
        [Space]
        public float DragDelay;
        public float DragDelta;
        
        public bool isShuffled;

        [Space]
        public bool IsSaveGame = true;
        public string PathSaves;

        public void ClearSaves()
        {
            File.Delete(ToolSaver.PathFor(PathSaves, typeof(PuzzleState)));

            Debug.Log("DONE!");
        }

    }
}