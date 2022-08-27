using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_manager : MonoBehaviour//키 설정 저장 클레스
{

    public enum KeyAction { UP, DOWN, LEFT, RIGHT, ATTACK, JUMP,DASH, INVENTORY, PAUSE }
    public static Dictionary<KeyAction, KeyCode> Keys = new Dictionary<KeyAction, KeyCode>();
 
   
}
