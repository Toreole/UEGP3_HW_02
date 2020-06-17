using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UEGP3.RandomWalk.Edit
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        MapGenerator generator;

        private void OnEnable()
        {
            generator = target as MapGenerator;    
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(Application.isEditor && Application.isPlaying && generator.IsDone)
            {
                if(GUILayout.Button("Fill Diagonal"))
                    generator.FillHoles(generator.DiagonalCheck);
                if(GUILayout.Button("Fill Vertical"))
                    generator.FillHoles(generator.CardinalCheck);
            }
        }
    }
}