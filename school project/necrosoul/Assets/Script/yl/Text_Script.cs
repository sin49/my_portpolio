using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Script : MonoBehaviour
{
    public Text Yes;
    public Text No;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Yes.text = "Ȯ��"+"\n"+Key_manager.Keys[Key_manager.KeyAction.ATTACK].ToString();
        No.text = "���" + "\n" + Key_manager.Keys[Key_manager.KeyAction.JUMP].ToString();
    }
}
