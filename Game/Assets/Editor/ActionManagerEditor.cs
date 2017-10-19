using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionManager))]
public class ActionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Undo"))
        {
            ActionManager.obj.Undo();
        }
        if (GUILayout.Button("Redo"))
        {
            ActionManager.obj.Redo();
        }
    }
}
