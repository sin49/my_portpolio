using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_AI_01_anim : MonoBehaviour//1�� �� ���ϸ��̼� �̺�Ʈ
{
    E_AI_01 e_01;
    // Start is called before the first frame update
    void Start()
    {
        e_01 = this.transform.parent.GetComponent<E_AI_01>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void e_throw_anim()//����ü ����
    {
        e_01.create_bullet();
    }
}
