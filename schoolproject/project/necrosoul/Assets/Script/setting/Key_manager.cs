using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_manager : MonoBehaviour//Ű ���� ���� Ŭ����
{

    public enum KeyAction { UP, DOWN, LEFT, RIGHT, ATTACK, JUMP,DASH, INVENTORY, PAUSE }
    public static Dictionary<KeyAction, KeyCode> Keys = new Dictionary<KeyAction, KeyCode>();
 
   
}
