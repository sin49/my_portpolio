using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
[CustomEditor(typeof(chracter_list))]
public class test_Editor2 : Editor
{
    ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(
            serializedObject, serializedObject.FindProperty("chr1"), true, true, true, true);
        /* list.drawElementCallback = (rect, index, active, focused) =>
         {
             rect.y += 2;

             var element = list.serializedProperty.GetArrayElementAtIndex(index);
             var basewidth = (rect.width / 3) - 5;
             //EditorGUI.PropertyField(new Rect(rect.x,rect.y,basewidth,EditorGUIUtility.singleLineHeight),element.)
         };*/
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

