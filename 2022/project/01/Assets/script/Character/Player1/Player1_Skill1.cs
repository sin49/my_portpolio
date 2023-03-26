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
        GameCharacter chr = obj_list[0];

        chr.execute_Hit_handler(ATK*2);
        chr.forced(chr.transform.forward*3, 1.25f);
        base.attack_by_type(ATK, obj_list);
    }

}
