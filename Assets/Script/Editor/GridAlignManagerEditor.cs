using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridAlignManager))]
[CanEditMultipleObjects]
public class GridAlignManagerEditor : Editor {

    private SerializedProperty rowNo;
    private SerializedProperty columNo;
    private SerializedProperty sizeScale;

    private readonly int maxRow = 15;
    private readonly int minRow = 3;

    private readonly int maxColum = 13;
    private readonly int minColum = 3;


    private readonly float maxSize = 0.1f;
    private readonly float minSize = 0.01f;

    private GridAlignManager gridAlignManager;

    private bool updateReady;

    void OnEnable()
    {
        gridAlignManager = (GridAlignManager)target;
        rowNo = serializedObject.FindProperty("rowNo");
        columNo = serializedObject.FindProperty("columnNo");
        sizeScale = serializedObject.FindProperty("sizeScale");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        if (GUILayout.Button("Destroy GridNodes"))
        {
            gridAlignManager.DestroyGridNode();
        }
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Slider(sizeScale, minSize, maxSize, new GUIContent("Size Change"));

        if (EditorGUI.EndChangeCheck())
        {

            gridAlignManager.UpdateSize();

        }

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.IntSlider(rowNo, minRow, maxRow, new GUIContent("Row Count"));
        EditorGUILayout.IntSlider(columNo, minColum, maxColum, new GUIContent("Column Count"));
        

        if (EditorGUI.EndChangeCheck())
        {
            updateReady = true;
           
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        if (updateReady)
        {
            gridAlignManager.Refresh();
            updateReady = false;
        }
    }
}


