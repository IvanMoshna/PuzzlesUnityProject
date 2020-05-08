using Puzzles.Configs;
using UnityEditor;
using UnityEngine;

namespace Puzzles.Configs
{

#if UNITY_EDITOR
    
    [CustomEditor(typeof(SettingsGame))]
    public class SettingsGameEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var component = (SettingsGame) target;

            if (GUILayout.Button("Clear Saves"))
            {
                component.ClearSaves();
            }
        }

    }
    
#endif

}