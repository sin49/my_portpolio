using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_manager : MonoBehaviour//키 설정 저장 클레스
{

    public enum KeyAction { UP, DOWN, LEFT, RIGHT, ATTACK, JUMP,DASH, INVENTORY, PAUSE }
    public static Dictionary<KeyAction, KeyCode> Keys = new Dictionary<KeyAction, KeyCode>();
 
    private void Awake()
    {
    
    }
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*기본값
     *Keys.Add(KeyAction.UP, KeyCode.UpArrow);
        Keys.Add(KeyAction.DOWN, KeyCode.DownArrow);
       Keys.Add(KeyAction.LEFT, KeyCode.LeftArrow);
        Keys.Add(KeyAction.RIGHT, KeyCode.RightArrow);
       Keys.Add(KeyAction.ATTACK, KeyCode.Space);
     Keys.Add(KeyAction.ACTIVE, KeyCode.E);
       Keys.Add(KeyAction.INVENTORY, KeyCode.Tab);
        Keys.Add(KeyAction.PAUSE, KeyCode.Escape);*/
}
