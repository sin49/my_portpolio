using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AI_06_anim : MonoBehaviour//6번 적 에니메이션 이벤트
{
    E_AI_06 e_01;
    // Start is called before the first frame update
    void Start()
    {
        e_01 = this.transform.parent.GetComponent<E_AI_06>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void e_throw_anim()//공격 생성
    {
        e_01.create_bullet();
    }
}
