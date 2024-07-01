using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]

public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapGenerator = (MapGenerator) target;
        base.OnInspectorGUI();
        if (GUILayout.Button("GenreatePoints"))
        {
            mapGenerator.GeneratePointCloud();
        }
    }
}
