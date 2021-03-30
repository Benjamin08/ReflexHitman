using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RotateWall))]
public class RotateWall_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        RotateWall script = (RotateWall)target;
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("unlockedWithKills"));
        
        if(script.unlockedWithKills)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("killsRequireToActivate"));
        }

        serializedObject.ApplyModifiedProperties();
    }

}