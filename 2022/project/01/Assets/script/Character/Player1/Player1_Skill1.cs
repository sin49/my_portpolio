using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_Skill1 : attack_basic
{
    //���� ����� ��󿡰� ���ݷ��� 125%+(������ 8%)�� ���ظ� ��
    public Player1_Skill1()
    {
        _init_delay = 0.2f;
    }
    protected override void attack_by_type(int ATK, List<GameCharacter> obj_list)
    {

        obj_list[0].hitted(ATK * 2);
        obj_list[0].get_forced(4, 1.25f);
        base.attack_by_type(ATK, obj_list);
    }

}
