using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Microsoft.SqlServer.Server;

// This class customizes what is visible in the editor of any given class. 
// Remember, the naming convention is MyClassEdior.
// Also, re-start Unity in case you get an error in the inspector that goes "Multi-object editing not supported"
[CustomEditor(typeof(CustomAction))]
public class CustomActionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CustomAction customAction = (CustomAction)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("actionType"));

        // This is to switch between two types of interactions.... if you absolutely need to have them in one single script.... but separate, somehow...
        // Best to have separate scripts for them, but this more of a Justin Case kind of exercise.
        switch (customAction.actionType)
        {
            case CustomAction.ActionType.ChatBubble:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("chatBubbleParams").FindPropertyRelative("chatBubble"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("chatBubbleParams").FindPropertyRelative("text"));
                if (GUILayout.Button("Talk!"))
                {
                    customAction.ExecuteAction();
                }
                break;
            case CustomAction.ActionType.Move:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moveParams"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
            
        
    }

    public void OnSceneGUI()
    {
        CustomAction customAction = (CustomAction)target;

        switch (customAction.actionType)
        {
            case CustomAction.ActionType.Move:

                // This is to properly update the transform handle for the move position.
                EditorGUI.BeginChangeCheck();
                Vector3 newMovePos = Handles.PositionHandle(customAction.moveParams.movePosition, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(customAction, "Change Move Position");
                    customAction.moveParams.movePosition = newMovePos;
                    serializedObject.Update();
                }
                break;
        }
    }
}
