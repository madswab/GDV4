using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor {

    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        LevelGenerator LG = (LevelGenerator)target;

        EditorGUILayout.Space();

        GUILayout.Label("PSD size: 13 - 12 pixels.\n" +
            "Use the pencil tool, size 1px.\n" +
            "Save as PSD en set in Unity filter mode to Point\n" +
            "and compression to none and in advanced Read/Write enabled");
        if (GUILayout.Button("Build Levels", GUILayout.Height(50))){
            Debug.Log("Build done");
            LG.RemoveLevel();
            LG.BuildLevel();
        }
    }
}
