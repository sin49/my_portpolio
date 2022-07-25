    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect0 : MonoBehaviour//아이템의 효과 처리
{
    public enum status_type { ATK,ATKSPEED,CRITICAL,DEFENSE,HP,MOVESPEED};//아이템의 효과 타입
    public Dictionary<status_type, int> item_type_effect=new Dictionary<status_type, int>();//장착 특수 효과 체크
  //장착시 특수효과 
    public int first_line;//첫번째 효과 적용 요구 수치
    public int second_line;//두번째 효과 적용 요구 수치
    public int third_line;//세번째 효과 적용 요구 수치
    public static ItemEffect0 item0to10;
    bool[] code_effect;
    
    float slot_bonus;
   
    public item_use_effect i;
 /*
  * 아이템 시스템
  * 흭득 시 그 아이템 관련 스텟 상승
  * 플레이어는 아이템을 장착 가능하며
  * 장착시 아이템의 효과가 증폭
  * 같은 타입의 아이템을 장착 했을 때
  * 장착된 수만큼 그 타입 관련 특수효과를 얻는다
  */
    void Awake()
    {
        item0to10 = this;
        i = new item_use_effect();
        code_effect = new bool[10];
        item_type_effect.Add(status_type.ATK, 0);
        item_type_effect.Add(status_type.ATKSPEED, 0);
        item_type_effect.Add(status_type.CRITICAL, 0);
        item_type_effect.Add(status_type.DEFENSE, 0);
        item_type_effect.Add(status_type.HP, 0);
        item_type_effect.Add(status_type.MOVESPEED, 0);

    }
    //방어력 타입 특수효과
    public int def_sp_effect(int i)
    {

        //used_effct_def_effect_check의 값으로 단계를 파악
        int d = i;
        if(Player_status.p_status.used_effct_def_effect_check > 0)//1단계
        {
            d--;//피해량 1감소
        }
        if (Player_status.p_status.used_effct_def_effect_check > 1 & Player_status.p_status.used_effct_def_effect)//2단계부터
        {

            if (Player_status.p_status.used_effct_def_effect_check == 2)//2단계
            {//한번만 피격시 데미지 절반 감소
                d = Mathf.RoundToInt(d * 0.5f);
                Player_status.p_status.used_effct_def_effect = false;
            }
            else if (Player_status.p_status.used_effct_def_effect_check == 3)//3단계
            {//대신 데미지 한번 무효
                d = 0;
                Player_status.p_status.used_effct_def_effect = false;
            }

        }
        return d;
    }
    public void effect(Item i)//아이템ㅇ의 효과를 적용한다
    {
        if (i.Item_Useing)//슬룻에 장착했는지 체크
        {
            slot_bonus = 2;//장착하면 능력치 상승 효과를 2배로 받는다
        }
        else
        {
            slot_bonus = 1;
        }
        switch (i.Foreignkey)//Foreignkey로 아이템의 종류를 파악
        {
            case 5:
                if (!code_effect[0])        //체력 증가
                {
                    Player_status.p_status.set_max_HP((int)(20 * i.num*slot_bonus));
                    Debug.Log("체력적용");
                    if(i.Item_Useing)
                    item_type_effect[status_type.HP] += i.num;//체력 타입 특수효과 활성화 하기 위한 상수를 더한다
                    item_use_case(status_type.HP);//체력 타입 특수효과
                    code_effect[0] = true;
                }
                break;
            case 6:// 구조는 모두 동일(스탯 상승,특수효과 활성화 여부 체크,특수 효과 활성화)
                if (!code_effect[1])    //방어력 증가
                {
                    Player_status.p_status.set_defense_point((int)(1 *i.num * slot_bonus));
                    Debug.Log("방어력적용");
                    if (i.Item_Useing)
                        item_type_effect[status_type.DEFENSE] += i.num;
                    item_use_case(status_type.DEFENSE);
                    code_effect[1] = true;
                }
                break;
            case 7:
                if (!code_effect[2])    //공격력 증가
                {
                    Player_status.p_status.set_atk((int)(1 * i.num * slot_bonus));
                    Debug.Log("공격력적용");
                    if (i.Item_Useing)
                        item_type_effect[status_type.ATK] += i.num;
                    item_use_case(status_type.ATK);
                    code_effect[2] = true;
                }
                break;
            case 8:
                if (!code_effect[3])     //공격속도 증가
                {
                    Player_status.p_status.set_firedelay(0.1f * i.num * slot_bonus);
                    Debug.Log("공격속도적용");
                    if (i.Item_Useing)
                        item_type_effect[status_type.ATKSPEED] += i.num;
                    item_use_case(status_type.ATKSPEED);
                    code_effect[3] = true;
                }
                break;
            case 9:
                if (!code_effect[4])    //이동속도 증가
                {
                    Player_status.p_status.set_speed(1 * i.num * slot_bonus);
                    Debug.Log("이동속도적용");
                    if (i.Item_Useing)
                        item_type_effect[status_type.MOVESPEED] += i.num;
                    item_use_case(status_type.MOVESPEED);
                    code_effect[4] = true;
                }
                break;
            case 10:         //크리티컬 확률 증가
                if (code_effect[5])   
                {
                    Player_status.p_status.set_critical_rate(0.3f*i.num*slot_bonus);
                    if (i.Item_Useing)
                        item_type_effect[status_type.CRITICAL] += i.num;
            
                    item_use_case(status_type.CRITICAL);
                    code_effect[0] = false;
                }
                break;
        }
       
    }
    

    public void uneffect(Item i)//올린 능력치를 원래 값으로 되돌림(슬룻에서 해재 할 때,특수요인으로 아이템이 소실 될 때,능력치를 최신 상황으로 동기화 용도 등등)
    {
        switch (i.Foreignkey)
        {
            case 5:
                if (code_effect[0])        //체력
                {
                    Player_status.p_status.set_max_HP();//체력 보너스 제거
                    Debug.Log("효과해제1");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.HP] -= i.num;//특수효과 활성화 하기 위한 상수 빼기
                    item_use_case(status_type.HP);//특수효과 적용 상태 갱신
                    code_effect[0] = false;
                }
                break;
            case 6://이하 동일
                if (code_effect[1])    //방어력
                {
                    Player_status.p_status.set_defense_point();
                    Debug.Log("효과해제2");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.DEFENSE] -= i.num;
                    item_use_case(status_type.DEFENSE);
                    code_effect[1] = false;
                }
                break;
            case 7:
                if (code_effect[2])    //공격력
                {
                    Player_status.p_status.set_atk();
                    Debug.Log("효과해제3");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.ATK] -= i.num;
                    item_use_case(status_type.ATK);
                    code_effect[2] = false;
                }
                break;
            case 8:
                if (code_effect[3])     //공격속도
                {
                    Player_status.p_status.set_firedelay();
                    Debug.Log("효과해제4");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.ATKSPEED] -= i.num;
                    item_use_case(status_type.ATKSPEED);
                    code_effect[3] = false;
                }
                break;
            case 9:
                if (code_effect[4])    //이동속도
                {
                    Player_status.p_status.set_speed();
                    Debug.Log("효과해제5");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.MOVESPEED] -= i.num;
                    item_use_case(status_type.MOVESPEED);
                    code_effect[4] = false;
                }
                break;
            case 10:         //치명타
                if (code_effect[5])    
                {
                    Player_status.p_status.set_critical_rate();
                    Debug.Log("효과해제1");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.CRITICAL] -= i.num;
                    item_use_case(status_type.CRITICAL);
                    code_effect[0] = false;
                }
                break;
        }
       
    }

    void item_use_case(status_type s)// 조건에 맞을 때 종류별로 특수효과를 활성화 시킨다
    {
        //같은 타입의 아이템을 슬룻에 장착시켜서 나온 합을 기준으로 특수효과를 활성화 한다
        int a=0;
        switch (s)
        {
            case status_type.HP://체력
                a = item_type_effect[status_type.HP];
                if (a >= third_line)//3단계 효과 적용
                {
                    i.set_effect_num(0, 3);
                }else if (a >= second_line)//2단계 효과 적용
                {
                    i.set_effect_num(0, 2);
                }else if (a >= first_line)//1단계 효과 적용
                {
                    i.set_effect_num(0, 1);
                }
                else//미적용
                {
                    i.set_effect_num(0, 0);
                }
                i.HP_effect();
                break;
            case status_type.DEFENSE://방어력
                a = item_type_effect[status_type.DEFENSE];

                if (a >= third_line)
                {
                    i.set_effect_num(1, 3);
                }
                else if (a >= second_line)
                {
                    i.set_effect_num(1, 2);
                }
                else if (a >= first_line)
                {
                    i.set_effect_num(1, 1);
                }
                else
                {
                    i.set_effect_num(1, 0);
                }
                i.Def_effect();
                break;
            case status_type.ATK://공격력
                a = item_type_effect[status_type.ATK];
                if (a >= third_line)
                {
                    i.set_effect_num(2, 3);
                }
                else if (a >= second_line)
                {
                    i.set_effect_num(2, 2);
                }
                else if (a >= first_line)
                {
                    i.set_effect_num(2, 1);
                }
                else
                {
                    i.set_effect_num(2, 0);
                }
                i.attack_effect();

                break;
            case status_type.ATKSPEED://공격속도
                a = item_type_effect[status_type.ATKSPEED];
                if (a >= third_line)
                {
                    i.set_effect_num(3, 3);
                }
                else if (a >= second_line)
                {
                    i.set_effect_num(3, 2);
                }
                else if (a >= first_line)
                {
                    i.set_effect_num(3, 1);
                }
                else
                {
                    i.set_effect_num(3, 0);
                }
                i.A_speed_effect();
                break;
            case status_type.MOVESPEED://이동속도
                a = item_type_effect[status_type.MOVESPEED];
                if (a >= third_line)
                {
                    i.set_effect_num(4, 3);
                }
                else if (a >= second_line)
                {
                    i.set_effect_num(4, 2);
                }
                else if (a >= first_line)
                {
                    i.set_effect_num(4, 1);
                }
                else
                {
                    i.set_effect_num(4, 0);
                }
                i.M_speed_effect();

                break;
            case status_type.CRITICAL://크리티컬
                a = item_type_effect[status_type.CRITICAL];
                if (a >= third_line)
                {
                    i.set_effect_num(5, 3);
                }
                else if (a >= second_line)
                {
                    i.set_effect_num(5, 2);
                }
                else if (a >= first_line)
                {
                    i.set_effect_num(5, 1);
                }
                else
                {
                    i.set_effect_num(5, 0);
                }
                i.Crit_effect();

                break;
        }


        Debug.Log(  "갯수" + a);

    }
    public void attack_spef_1(Transform tp)//공격속도 1단계효과: 공격범위 증가
    {
        if (Player_status.p_status.used_effct_A_SPEED_effect_check >= 1)
        {

            tp.localScale = tp.localScale* 1.1f;//1.1 만큼 확대
        }
    }
    public void double_attack_effect(melee_attack e)//공격속도 2단계 효과 :이단 공격
    {
        if(Player_status.p_status.used_effct_A_SPEED_effect_check >= 2)//난수를 추가하여 이단 공격 효과를 활성화 시킨다
        {
            int rand = Random.Range(0, 100);
            if ((Player_status.p_status.used_effct_A_SPEED_effect_check == 3 && rand < 10) || rand < 5)
            {
                e.Double_attack_on = true;
            }
        }
        else
        {
            e.Double_attack_on = false;
        }
    }
    public void Def_effect_not_damaged()//방어력 특수효과 2단계 이상의 효과를 다시 활성화(2,3  단계:한번만 피해를 대폭 감소 시킨다)
    {
        if (Player_status.p_status.used_effct_def_effect_check > 1 && !Player_status.p_status.used_effct_def_effect)
        {
            Player_status.p_status.used_effct_def_effect = true;
        }
    }
    public void HP_effect_room_HP()//체력 특수 효과 :방에 입장할 때 마다 체력 회복 (2단계:1만큼 3단계:6만큼)
    {
        if (Player_status.p_status.used_effct_HP_effect_check > 1)
        {
            if (Player_status.p_status.used_effct_HP_effect_check == 2)
            {
                Player_status.p_status.set_hp(1);
            }
            else if (Player_status.p_status.used_effct_HP_effect_check == 3)
            {
                Player_status.p_status.set_hp(6);
            }
        }
    }
}
public class item_use_effect {//타입별 특수효과를 관리,적용
    public int attack_effect_num;
    public int attack_bonus;
    public int HP_effect_num;
    public int barrier_bonus;
    public int def_effect_num;
    public int crit_effect_num;
    public float crit_rate_bonus;
    public float crit_damage_bonus;
    public int aspeed_effect_num;
    public int mspeed_effect_num;
    public int dash_bonus;
    public float dash_recover_bonus;
    public void set_effect_num(int a,int b)//특수 효과를 적용
    {
        switch (a)
        {
            case 0:
                HP_effect_num = b;
       
                break;
            case 1:
                def_effect_num = b;
        
                break;
            case 2:
                attack_effect_num = b;
           
                break;
            case 3:
                aspeed_effect_num = b;
    
                break;
            case 4:
                mspeed_effect_num = b;
 
                break;
            case 5:
                crit_effect_num = b;

                break;
        }
    }
    //ㅁㅁㅁㅁ_effect= 특수효과의 적용을 초기화 한 후 입력 값에 따라 다시 효과를 적용시킨다(효과 중복 방지)
    public void attack_effect()//공격력
    {
        int attack_bonus_initialize = -attack_bonus;
        Player_status.p_status.set_atk(Player_status.p_status.get_atk_bonus() + attack_bonus_initialize);
        attack_bonus = 0;
        if (attack_effect_num == 3)
        {
            attack_first_effect();
            attack_secon_effect();
            attack_third_effect();
        }else if (attack_effect_num == 2)
        {
            attack_first_effect();
            attack_secon_effect();
        }
        else if (attack_effect_num == 1)
        {
            attack_first_effect();
        }
        else
        {
            attack_bonus = 0;
        }
        Player_status.p_status.set_atk(Player_status.p_status.get_atk_bonus() + attack_bonus);
    }
    public void attack_first_effect()//공격 타입 첫번째:공격력 보너스 10 추가
    {
        attack_bonus = 10;
        Debug.Log("공격첫번째특수");
    }
    public void attack_secon_effect()//공격 타입 두번째:공격력 보너스 20 추가
    {
        attack_bonus = 20;
        Debug.Log("공격두번째특수");
    }
    public void attack_third_effect()//공격 타입 세번째:현재 적용되는 공격력 보너스 2배
    {
        attack_bonus += (Mathf.Abs(Player_status.p_status.get_atk_bonus()));

        Debug.Log("공격세번째특수");
    }
   public void HP_effect()//체력
    {
         var barrier_initialize = -barrier_bonus;
        Player_status.p_status.set_barrier(Player_status.p_status.get_barrier_bonus() +barrier_initialize);
        Player_status.p_status.used_effct_HP_effect_check = 0;
        barrier_bonus = 0;
        if (HP_effect_num == 3)
        {
            HP_first_effect();
            HP_secon_effect();
            HP_third_effect();
        }
        else if (HP_effect_num == 2)
        {
            HP_first_effect();
            HP_secon_effect();
        }
        else if (HP_effect_num == 1)
        {
            HP_first_effect();
        }
        else
        {
            barrier_bonus = 0;
            Player_status.p_status.used_effct_HP_effect_check = 0;
        }
        Player_status.p_status.set_barrier(Player_status.p_status.get_barrier_bonus() + barrier_bonus);
    }
    public void HP_first_effect()//체력 첫번째 효과:보호막 5 부여
    {
        barrier_bonus = 5;
       
        Debug.Log("체력첫번째특수");
    }
    public void HP_secon_effect()
    {
        Player_status.p_status.used_effct_HP_effect_check = 2;
        Debug.Log("체력두번째특수");
    }
    public void HP_third_effect()
    {
        Player_status.p_status.used_effct_HP_effect_check = 3;
        Debug.Log("체력세번째특수");
    }

    public void Def_effect()//방어력
    {
        Player_status.p_status.used_effct_def_effect_check = 0;
        Player_status.p_status.used_effct_def_effect = false;
        if (def_effect_num == 3)
        {
            Def_first_effect();
            Def_secon_effect();
            Def_third_effect();
        }
        else if (def_effect_num == 2)
        {
            Def_first_effect();
            Def_secon_effect();
        }
        else if (def_effect_num == 1)
        {
            Def_first_effect();
        }
        else
        {
            Player_status.p_status.used_effct_def_effect_check = 0;
        }
    
    }
    public void Def_first_effect()
    {
        Player_status.p_status.used_effct_def_effect_check = 1;
        Debug.Log("방어첫번째특수");
    }
    public void Def_secon_effect()
    {
        Player_status.p_status.used_effct_def_effect_check = 2;
        Player_status.p_status.used_effct_def_effect = true;
        Debug.Log("방어두번째특수");
    }
    public void Def_third_effect()
    {
        Player_status.p_status.used_effct_def_effect_check = 3;
        Debug.Log("방어세번째특수");
    }
    public void Crit_effect()//크리티컬
    {
        var crit_rate_initialize = -crit_rate_bonus;
        var crit_damage_initialize = -crit_damage_bonus;
        Player_status.p_status.set_critical_rate(Player_status.p_status.get_critical_rate_bonus() +crit_rate_bonus);
        Player_status.p_status.set_critical_Damage(Player_status.p_status.get_critical_damage_bonus()+crit_damage_initialize);

        crit_damage_bonus = 0;
        crit_rate_bonus = 0;
        if (crit_effect_num == 3)
        {
            Crit_first_effect();
            Crit_secon_effect();
            Crit_third_effect();
        }
        else if (crit_effect_num == 2)
        {
            Crit_first_effect();
            Crit_secon_effect();
        }
        else if (crit_effect_num == 1)
        {
            Crit_first_effect();
        }
        else
        {
            crit_damage_bonus = 0;
            crit_rate_bonus = 0;
        }
        Player_status.p_status.set_critical_rate(Player_status.p_status.get_critical_rate_bonus() + crit_rate_bonus);
        Player_status.p_status.set_critical_Damage(crit_damage_bonus);
    }
    public void Crit_first_effect()//크리티컬 특수 효과 첫번째:치명타 확률 증가(25%증가)
    {
        crit_rate_bonus = 0.025f;
      
        Debug.Log("치명타첫번째특수");
    }
    public void Crit_secon_effect()//크리티컬 특수 효과 두번째:치명타 배율 증가(20%증가)
    {
        crit_damage_bonus = 0.2f;
   
        Debug.Log("치명타두번째특수");
    }
    public void Crit_third_effect()//크리티컬 특수 효과 세번째:치명타 배율 증가(70%증가)
    {
        crit_damage_bonus = 0.7f;

        Debug.Log("치명타세번째특수");
    }
    public void A_speed_effect()//공격속도
    {
        Player_status.p_status.used_effct_A_SPEED_effect_check = 0;
        if (aspeed_effect_num == 3)
        {
            A_SPEED_first_effect();
            A_SPEED_secon_effect();
            A_SPEED_third_effect();
        }
        else if (aspeed_effect_num == 2)
        {
            A_SPEED_first_effect();
            A_SPEED_secon_effect();
        }
        else if (aspeed_effect_num == 1)
        {
            A_SPEED_first_effect();
        }
        else
        {
            Player_status.p_status.used_effct_A_SPEED_effect_check = 0;
        }

    }
    public void A_SPEED_first_effect()
    {
        Player_status.p_status.used_effct_A_SPEED_effect_check = 1;
        Debug.Log("공격속도첫번째특수");
    }
    public void A_SPEED_secon_effect()
    {
        Player_status.p_status.used_effct_A_SPEED_effect_check = 2;
        Debug.Log("공격속도두번째특수");
    }
    public void A_SPEED_third_effect()
    {
        Player_status.p_status.used_effct_A_SPEED_effect_check = 3;
        Debug.Log("공격속도새번째특수");
    }
    public void M_speed_effect()//이동속도
    {
        var dash_count_initialize = -dash_bonus;
        var dash_recover_initialize = -dash_recover_bonus;
        Player_status.p_status.set_dash_count(Player_status.p_status.get_dash_count_bonus()+dash_count_initialize);
        Player_status.p_status.set_dassh_recover_time(Player_status.p_status.get_dash_recover_time_bonus() + dash_recover_initialize);
        dash_bonus = 0;
        dash_recover_bonus = 0;
        if (mspeed_effect_num == 3)
        {
            M_SPEED_first_effect();
            M_SPEED_secon_effect();
            M_SPEED_third_effect();
        }
        else if (mspeed_effect_num == 2)
        {
            M_SPEED_first_effect();
            M_SPEED_secon_effect();
        }
        else if (mspeed_effect_num == 1)
        {
            M_SPEED_first_effect();
        }
        else
        {
            dash_bonus = 0;
           dash_recover_bonus = 0;
        }
        Player_status.p_status.set_dash_count(Player_status.p_status.get_dash_count_bonus() + dash_bonus);
        Player_status.p_status.set_dassh_recover_time(Player_status.p_status.get_dash_recover_time_bonus() + dash_recover_bonus);
    }
    public void M_SPEED_first_effect()//이동속도 특수 효과 첫번째:대쉬 횟수 1 증가
    {
        dash_bonus = 1;
        
        Debug.Log("이동속도첫번째특수");
    }
    public void M_SPEED_secon_effect()//이동속도 특수 효과 두번째:대쉬 회복속도 감소(25%감소)
    {
        dash_recover_bonus = Player_status.p_status.get_dash_recover_time_original() * 0.25f;
        Debug.Log("이동속도두번째특수");
    }
    public void M_SPEED_third_effect()//이동속도 특수 효과 첫번째:대쉬 횟수 대신 2 증가
    {
        dash_bonus = 2;
        Debug.Log("이동속도세번째특수");
    }
}
