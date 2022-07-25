using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Event//게임 중 일어나는 이벤트의 발생을 확인하고 그에 반응하는 행동을 수행한다
{
    public float l;
    public int resurrection_hp;
    public void when_camera_move(float a)//카메라가 이동했을 때
    {
        //이동값
        l = a;
        //카메라의 이동에 따른 스크롤을 처리하는 용도
    }
    public bool when_player_get_bad_status()//플레이어가 상태이상에 걸렸을 때
    {
      
       return Sp_ItemEffect.sp_itemeffect.Sp_3();
    }
    public int when_get_money(int a)//플레이어가 돈을 얻었을 때
    {

        int n = a;
        Player_status.p_status.Money += n;
        Gamemanager.GM.lastest_get_money = n;
        Gamemanager.GM.get_money_chk = true;
        return n;
    }
    public int when_lose_money(int a)//플레이어가 돈을 잃었을 때
    {
        int n = a;
        Player_status.p_status.Money -= n;
        Gamemanager.GM.lastest_lose_money = n;
        Gamemanager.GM.lose_money_chk = true;
        return n;
    }
    public void when_sp_item_will_get(Item i)//아이템을 먹기전에
    {
        Gamemanager.GM.loot_box.Add(i.CreateItem());
    }
    public int when_P_A_Key_input(int i)//플레이어가 공격키를 누르면
    {
        int dmg = i;
        Sp_ItemEffect.sp_itemeffect.Sp_0(dmg);
        return dmg;
    }
    public void when_P_Attack_effect(melee_attack m)//플레이어가 공격할 때
    {
       
        ItemEffect0.item0to10.attack_spef_1(m.transform);
        ItemEffect0.item0to10.double_attack_effect(m);
    }
    public void P_Attack_col_effect(Collider2D EA)//플레이어 공격이 충돌
    {
        Sp_ItemEffect.sp_itemeffect.Sp_1(EA);
    }
    public void when_dash_key_input(Transform pos)//대쉬키를 눌렀을 때
    {
        Sp_ItemEffect.sp_itemeffect.SP_11(pos);
    }
    public void when_player_hitted(Transform pos)//플레이어가 피격 당했을 때
    {
        Sp_ItemEffect.sp_itemeffect.SP_11(pos);
    }
    public void On_Dash_col_effect(Collider2D col)//플레이어가 대쉬 중 적과 충돌
    {
        Sp_ItemEffect.sp_itemeffect.Sp_2(col);
    }
    public void when_Enemy_hitted(int dmg,Unit enemy)//적이 피해를 입었을 때
    {
        Sp_ItemEffect.sp_itemeffect.Sp_4(dmg);
        Sp_ItemEffect.sp_itemeffect.Sp_8(enemy);
    }
  
    public void Enemy_death(Transform t)//적이 죽었을 때
    {
        Gamemanager.GM.lastest_enemy_point = t.position;
        Sp_ItemEffect.sp_itemeffect.Sp_9();
    }
    public void Dash_End_effect()//플레이어 대쉬가 끝났을 때
    {
        Sp_ItemEffect.sp_itemeffect.Sp_2_reset_list();
    }
   public bool is_Player_get_bad_status()//플레이어가 상태이상을 받을지 안받을지
    {
        return Sp_ItemEffect.sp_itemeffect.Sp_3();
    }
    public void when_room_clear()//방을 클리어 했을 때
    {
      
        
    }
    public void when_room_enter()//방에 입장했을 때
    {
        Player_status.p_status.make_barrier();
        ItemEffect0.item0to10.Def_effect_not_damaged();
        ItemEffect0.item0to10.HP_effect_room_HP();
        Sp_ItemEffect.sp_itemeffect.SP_11_room_chk();
    }
    public void when_player_get_item(Item i)//아이템을 얻을 때
    {
        Gamemanager.GM.loot_box.Add(i.CreateItem());
        Sp_ItemEffect.sp_itemeffect.Sp_6(i);
    }
    public bool is_player_death()//플레이어가 죽을지 안죽을지
    {
        if (Sp_ItemEffect.sp_itemeffect.SP_5())
        {
            return true;
        }
        else
        {
            switch (Sp_ItemEffect.sp_itemeffect.used_revive_item_num)
            {
                case 5:
                    resurrection_hp = Player_status.p_status.get_max_hp();
                    Sp_ItemEffect.sp_itemeffect.sp5_used = true;
                    break;

            }
            return false;
        }
    }
}
