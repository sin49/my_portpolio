using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_manager : MonoBehaviour
{

    public enum KeyAction { UP, DOWN, LEFT, RIGHT, ATTACK, JUMP,DASH, INVENTORY, PAUSE }
    public static Dictionary<KeyAction, KeyCode> Keys = new Dictionary<KeyAction, KeyCode>();
    // Start is called before the first frame update
    //Ű������ �����ͼ� ����
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
    /* Keys.Add(KeyAction.UP, KeyCode.UpArrow);
        Keys.Add(KeyAction.DOWN, KeyCode.DownArrow);
       Keys.Add(KeyAction.LEFT, KeyCode.LeftArrow);
        Keys.Add(KeyAction.RIGHT, KeyCode.RightArrow);
       Keys.Add(KeyAction.ATTACK, KeyCode.Space);
     Keys.Add(KeyAction.ACTIVE, KeyCode.E);
       Keys.Add(KeyAction.INVENTORY, KeyCode.Tab);
        Keys.Add(KeyAction.PAUSE, KeyCode.Escape);*/
}
