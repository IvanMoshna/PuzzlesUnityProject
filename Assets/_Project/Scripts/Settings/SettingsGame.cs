using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Puzzles.Settings
{
    [CreateAssetMenu(fileName = "SettingsGame", menuName = "Settings/SettingsGame")]
    public class SettingsGame : ScriptableObject
    {
        public float puzzleDistance;
        public int columns;
        public int lines;
        public float transparency;
        public float DragDelay;
        public float DragDelta;
        public bool isShuffled;

    }
}