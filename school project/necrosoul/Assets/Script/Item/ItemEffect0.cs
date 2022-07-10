    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect0 : MonoBehaviour
{
    public enum status_type { ATK,ATKSPEED,CRITICAL,DEFENSE,HP,MOVESPEED};
    public Dictionary<status_type, int> item_type_effect=new Dictionary<status_type, int>();
  
    public int first_line;
    public int second_line;
    public int third_line;
    public static ItemEffect0 item0to10;
    bool[] code_effect;
    GameObject Player;
    GameObject p_bullet;
    float slot_bonus;
    public GameObject doubleattack_prefab;
    public item_use_effect i;
    // Start is called before the first frame update
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

    public int def_sp_effect(int i)
    {
        int d = i;
        if(Player_status.p_status.used_effct_def_effect_check > 0)
        {
            d--;
        }
        if (Player_status.p_status.used_effct_def_effect_check > 1 & Player_status.p_status.used_effct_def_effect)
        {

            if (Player_status.p_status.used_effct_def_effect_check == 2)
            {
                d = Mathf.RoundToInt(d * 0.5f);
                Player_status.p_status.used_effct_def_effect = false;
            }
            else if (Player_status.p_status.used_effct_def_effect_check == 3)
            {
                d = 0;
                Player_status.p_status.used_effct_def_effect = false;
            }

        }
        return d;
    }
    public void effect(Item i)
    {
        if (i.Item_Useing)
        {
            slot_bonus = 2;
        }
        else
        {
            slot_bonus = 1;
        }
        switch (i.Foreignkey)
        {
            case 5:
                if (!code_effect[0])        //ü��
                {
                    Player_status.p_status.set_max_HP((int)(20 * i.num*slot_bonus));
                    Debug.Log("ü������");
                    if(i.Item_Useing)
                    item_type_effect[status_type.HP] += i.num;
                    item_use_case(status_type.HP);
                    code_effect[0] = true;
                }
                break;
            case 6:
                if (!code_effect[1])    //����
                {
                    Player_status.p_status.set_defense_point((int)(1 *i.num * slot_bonus));
                    Debug.Log("��������");
                    if (i.Item_Useing)
                        item_type_effect[status_type.DEFENSE] += i.num;
                    item_use_case(status_type.DEFENSE);
                    code_effect[1] = true;
                }
                break;
            case 7:
                if (!code_effect[2])    //���ݷ�
                {
                    Player_status.p_status.set_atk((int)(1 * i.num * slot_bonus));
                    Debug.Log("���ݷ�����");
                    if (i.Item_Useing)
                        item_type_effect[status_type.ATK] += i.num;
                    item_use_case(status_type.ATK);
                    code_effect[2] = true;
                }
                break;
            case 8:
                if (!code_effect[3])     //���ݼӵ�
                {
                    Player_status.p_status.set_firedelay(0.1f * i.num * slot_bonus);
                    Debug.Log("���ݼӵ�����");
                    if (i.Item_Useing)
                        item_type_effect[status_type.ATKSPEED] += i.num;
                    item_use_case(status_type.ATKSPEED);
                    code_effect[3] = true;
                }
                break;
            case 9:
                if (!code_effect[4])    //�̵��ӵ�
                {
                    Player_status.p_status.set_speed(1 * i.num * slot_bonus);
                    Debug.Log("�̵��ӵ�����");
                    if (i.Item_Useing)
                        item_type_effect[status_type.MOVESPEED] += i.num;
                    item_use_case(status_type.MOVESPEED);
                    code_effect[4] = true;
                }
                break;
            case 10:         //ȸ����
                if (code_effect[5])   
                {
                    Player_status.p_status.set_critical_rate(0.3f*i.num*slot_bonus);
                    if (i.Item_Useing)
                        item_type_effect[status_type.CRITICAL] += i.num;
                    Debug.Log("ȿ������1");
                    item_use_case(status_type.CRITICAL);
                    code_effect[0] = false;
                }
                break;
        }
       
    }
    

    public void uneffect(Item i)
    {
        switch (i.Foreignkey)
        {
            case 5:
                if (code_effect[0])        //ü��
                {
                    Player_status.p_status.set_max_HP();
                    Debug.Log("ȿ������1");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.HP] -= i.num;
                    item_use_case(status_type.HP);
                    code_effect[0] = false;
                }
                break;
            case 6:
                if (code_effect[1])    //����
                {
                    Player_status.p_status.set_defense_point();
                    Debug.Log("ȿ������2");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.DEFENSE] -= i.num;
                    item_use_case(status_type.DEFENSE);
                    code_effect[1] = false;
                }
                break;
            case 7:
                if (code_effect[2])    //���ݷ�
                {
                    Player_status.p_status.set_atk();
                    Debug.Log("ȿ������3");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.ATK] -= i.num;
                    item_use_case(status_type.ATK);
                    code_effect[2] = false;
                }
                break;
            case 8:
                if (code_effect[3])     //���ݼӵ�
                {
                    Player_status.p_status.set_firedelay();
                    Debug.Log("ȿ������4");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.ATKSPEED] -= i.num;
                    item_use_case(status_type.ATKSPEED);
                    code_effect[3] = false;
                }
                break;
            case 9:
                if (code_effect[4])    //�̵��ӵ�
                {
                    Player_status.p_status.set_speed();
                    Debug.Log("ȿ������5");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.MOVESPEED] -= i.num;
                    item_use_case(status_type.MOVESPEED);
                    code_effect[4] = false;
                }
                break;
            case 10:         //ġ��Ÿ
                if (code_effect[5])    //�̵��ӵ�
                {
                    Player_status.p_status.set_critical_rate();
                    Debug.Log("ȿ������1");
                    if (!i.Item_Useing)
                        item_type_effect[status_type.CRITICAL] -= i.num;
                    item_use_case(status_type.CRITICAL);
                    code_effect[0] = false;
                }
                break;
        }
       
    }

    void item_use_case(status_type s)
    {
        int a=0;
        switch (s)
        {
            case status_type.HP:
                a = item_type_effect[status_type.HP];
                if (a >= third_line)
                {
                    i.set_effect_num(0, 3);
                }else if (a >= second_line)
                {
                    i.set_effect_num(0, 2);
                }else if (a >= first_line)
                {
                    i.set_effect_num(0, 1);
                }
                else
                {
                    i.set_effect_num(0, 0);
                }
                i.HP_effect();
                break;
            case status_type.DEFENSE:
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
            case status_type.ATK:
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
            case status_type.ATKSPEED:
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
            case status_type.MOVESPEED:
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
            case status_type.CRITICAL:
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


        Debug.Log(  "����" + a);

    }
    public void attack_spef_1(Transform tp)
    {
        if (Player_status.p_status.used_effct_A_SPEED_effect_check >= 1)
        {

            tp.localScale = tp.localScale* 1.1f;
        }
    }
    public void double_attack_effect(melee_attack e)
    {
        if(Player_status.p_status.used_effct_A_SPEED_effect_check >= 2)
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
    public void Def_effect_not_damaged()
    {
        if (Player_status.p_status.used_effct_def_effect_check > 1 && !Player_status.p_status.used_effct_def_effect)
        {
            Player_status.p_status.used_effct_def_effect = true;
        }
    }
    public void HP_effect_room_HP()
    {
        if (Player_status.p_status.used_effct_HP_effect_check > 0)
        {
            if (Player_status.p_status.used_effct_HP_effect_check == 1)
            {
                Player_status.p_status.set_hp(1);
            }
            else if (Player_status.p_status.used_effct_HP_effect_check == 2)
            {
                Player_status.p_status.set_hp(6);
            }
        }
    }
}
public class item_use_effect {
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
    public void set_effect_num(int a,int b)
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
   
        public void attack_effect()
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
    public void attack_first_effect()
    {
        attack_bonus = 10;
        Debug.Log("����ù��°Ư��");
    }
    public void attack_secon_effect()
    {
        attack_bonus = 20;
        Debug.Log("���ݵι�°Ư��");
    }
    public void attack_third_effect()
    {
        attack_bonus = 20 + (Player_status.p_status.get_atk() - Player_status.p_status.get_original_atk());
        
        Debug.Log("���ݼ���°Ư��");
    }
   public void HP_effect()
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
    public void HP_first_effect()
    {
        barrier_bonus = 5;
       
        Debug.Log("ü��ù��°Ư��");
    }
    public void HP_secon_effect()
    {
        Player_status.p_status.used_effct_HP_effect_check = 1;
        Debug.Log("ü�µι�°Ư��");
    }
    public void HP_third_effect()
    {
        Player_status.p_status.used_effct_HP_effect_check = 2;
        Debug.Log("ü�¼���°Ư��");
    }

    public void Def_effect()
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
        Debug.Log("���ù��°Ư��");
    }
    public void Def_secon_effect()
    {
        Player_status.p_status.used_effct_def_effect_check = 2;
        Player_status.p_status.used_effct_def_effect = true;
        Debug.Log("���ι�°Ư��");
    }
    public void Def_third_effect()
    {
        Player_status.p_status.used_effct_def_effect_check = 3;
        Debug.Log("����°Ư��");
    }
    public void Crit_effect()
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
    public void Crit_first_effect()
    {
        crit_rate_bonus = 0.025f;
      
        Debug.Log("ġ��Ÿù��°Ư��");
    }
    public void Crit_secon_effect()
    {
        crit_damage_bonus = 0.2f;
   
        Debug.Log("ġ��Ÿ�ι�°Ư��");
    }
    public void Crit_third_effect()
    {
        crit_damage_bonus = 0.7f;

        Debug.Log("ġ��Ÿ����°Ư��");
    }
    public void A_speed_effect()
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
        Debug.Log("���ݼӵ�ù��°Ư��");
    }
    public void A_SPEED_secon_effect()
    {
        Player_status.p_status.used_effct_A_SPEED_effect_check = 2;
        Debug.Log("���ݼӵ��ι�°Ư��");
    }
    public void A_SPEED_third_effect()
    {
        Player_status.p_status.used_effct_A_SPEED_effect_check = 3;
        Debug.Log("���ݼӵ�����°Ư��");
    }
    public void M_speed_effect()
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
    public void M_SPEED_first_effect()
    {
        dash_bonus = 1;
        
        Debug.Log("�̵��ӵ�ù��°Ư��");
    }
    public void M_SPEED_secon_effect()
    {
        dash_recover_bonus = Player_status.p_status.get_dash_recover_time_original() * 0.25f;
        Debug.Log("�̵��ӵ��ι�°Ư��");
    }
    public void M_SPEED_third_effect()
    {
        dash_bonus = 2;
        Debug.Log("�̵��ӵ�����°Ư��");
    }
}
