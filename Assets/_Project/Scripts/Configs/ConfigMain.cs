using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles.Configs
{
    [CreateAssetMenu(fileName = "ConfigMain", menuName = "Settings/ConfigMain")]
    public class ConfigMain : ScriptableObject
    {
        public PuzzleState InitState()
        {
            var dataGame = new PuzzleState();
            
            return dataGame;
        }
    }
}
