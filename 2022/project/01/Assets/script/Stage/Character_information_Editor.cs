using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
[CustomEditor(typeof(round_info))]
public class Character_information_Editor : Editor
{
    ReorderableList list;
    ReorderableList list2;
    private void OnEnable()
    {
        list = new ReorderableList(
            serializedObject, serializedObject.FindProperty("round_chr"), true, true, true, true);
        
        list.drawElementCallback = (rect, index,active,focused) =>
        {
            rect.y += 2;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            var basewidth = (rect.width / 3) - 5;
            //EditorGUI.PropertyField(new Rect(rect.x,rect.y,basewidth,EditorGUIUtility.singleLineHeight),element.)
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
