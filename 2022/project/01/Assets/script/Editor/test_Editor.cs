using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(test))]
public class test_Editor : Editor
{

    //Ŀ���� �������� ���õ��� ������...
    //�׷��� �ϴ� ���� ����Ʈ�� �ƴ� �迭�̳� ����Ʈ�� �ٲ㼭 �����ϱ�� ����->
    //reorderableList�� ���� �ʴ� �������� ����
    //�� �� �Ẹ��
    //�Ƚᵵ ������? �ϴ� �غ��� �˵�
    ReorderableList test_list;
    test t;
    int a;
    private void OnEnable()
    {

        t = target as test;



        test_list = new ReorderableList(serializedObject, serializedObject.FindProperty("bb"),true,true,true,true);
        test_list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {//����Ʈ ���
            var element = test_list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 240, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("chr"), GUIContent.none);
            EditorGUI.LabelField(new Rect(rect.x+240+10, rect.y, 60, EditorGUIUtility.singleLineHeight), "LV");
            
            EditorGUI.PropertyField(new Rect(rect.x+270, rect.y, 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("LV"), GUIContent.none);
            /*if (GUI.Button(new Rect(rect.x + 335, rect.y, 40, EditorGUIUtility.singleLineHeight), "�߰�"))
            {
                t.Add_T_List_index(t.cc[array_index],t.bb[index] );
            };*/
           
        };
        test_list.drawHeaderCallback = (Rect rect) =>//���
          {
              EditorGUI.LabelField(rect, "���� ĳ����");
          };
        test_list.onAddCallback = (ReorderableList a) =>//�ʱ�ȭ
          {
              var index = a.serializedProperty.arraySize;
              a.serializedProperty.arraySize++;
              a.index = index;
              var element = a.serializedProperty.GetArrayElementAtIndex(index);
              element.FindPropertyRelative("chr").objectReferenceValue = null;
              element.FindPropertyRelative("LV").intValue = 1;
          };
    }
    string[] n=new string[1] {"0" };
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        test_list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("���� ����");
        EditorGUI.BeginChangeCheck();
        t.round_num = EditorGUILayout.IntField(serializedObject.FindProperty("round_num").intValue);
        EditorGUILayout.EndHorizontal();
        t.dynamic_stage_list(t.round_num);
        //�� End Ǯ���
        if (EditorGUI.EndChangeCheck())
        {
            Debug.Log(t.cc.Count);
            n = set_popup_string_array(t.cc.Count);
          
        }


        EditorGUILayout.LabelField(t.cc.Count.ToString());
        if (t.cc.Count==0)
            return;
        array_index = EditorGUILayout.Popup("����", array_index, n);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("�̸�");
        EditorGUILayout.LabelField("����");
        EditorGUILayout.LabelField("");
        EditorGUILayout.EndHorizontal();
        for (int i = 0; i < t.cc[array_index].Count; i++)
            {
                
                Debug.Log(t.cc[array_index][i].number);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("(" + t.cc[array_index][i].chr.LV + ")" + t.cc[array_index][i].chr.chr.name);
                t.cc[array_index][i].number = EditorGUILayout.IntField(t.cc[array_index][i].number);
                if (GUILayout.Button("����"))
                {
                    t.delete_T_List_index(t.cc[array_index], i);   
                }
                EditorGUILayout.EndHorizontal();
            }
        

    }
    int array_index=0;
    string[] set_popup_string_array(int n)
    {
        string[] str = new string[n];
        for(int i = 0; i < n; i++)
        {
            str[i] = i.ToString();
        }
        return str;
    }
    
}
