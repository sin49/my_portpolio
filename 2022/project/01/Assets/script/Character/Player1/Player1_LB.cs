using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//0.2���� ������ �� ���� ����� ������ (300%+25%(������))�� ���ݷ����� ����� (�ִϸ��̼����� 0.3�� �������� �� ������� 1/3�� ������ ����� ���� ǥ��)
public class Player1_LB : LimitBurst
{
    public Player1_LB(){
        _init_delay = 0.2f;
        }
    protected override void attack_by_type(int ATK,List<GameCharacter> obj_list)
    {
        int Total_damage = ATK * 3;
        float anim_time_chk=0;
        for(int i = 0; i < 2; i++)
        {
            anim_time_chk += Time.unscaledDeltaTime;
            if (anim_time_chk >= 0.3f)
            {
                obj_list[0].hitted(Total_damage / 3);
                anim_time_chk = 0;
                i++;
            }
        }
        obj_list[0].get_forced(4, 1.25f);
        base.attack_by_type(ATK, obj_list);
    }
}


//�Ͻ����� ��ư ������ ���� �ϱⰡ �� ����....
//���������� LB���� TIme.timescale����ϸ� �ȵ�(�� �ൿ ������Ű�Ⱑ ����)
