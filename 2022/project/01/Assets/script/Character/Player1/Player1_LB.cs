using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//0.2초의 딜레이 후 가장 가까운 적에게 (300%+25%(레벨당))의 공격력으로 대미지 (애니메이션으로 0.3초 간격으로 총 대미지의 1/3의 값으로 대미지 값을 표기)
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


//일시정지 버튼 때문에 뭔가 하기가 좀 힘듬....
//내생각에는 LB에는 TIme.timescale사용하면 안됨(걍 행동 고정시키기가 맞음)
