using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_status : MonoBehaviour
{
    public static Player_status p_status;
    public int layout_num;//���̾ƿ� ����
    
    [SerializeField]
    private int original_MaX_HP;//�ִ� ü�� ����
    private int MaX_HP_bonus;//�ִ� ü�� ������
    private int MaX_HP_spbonus;//�ִ� ü�� ������-Ư��������
    [SerializeField]
    private int HP;//���� ü��
    private int Barrier;
    private int original_Barrier;
    private int Barrier_Bouns;
    public int Money;
    private int original_Defense;//���� ����
    private int Defense_bonus;//���� ������
    private float original_untouchable_time;//�����ð� ���� 
    private float untouchable_time_bonus;//�����ð� ���ʽ�

    float original_speed;
    float speed_bonus;

    [Header("jump")]
    private float original_jump_force;//�������� ����
    float jump_force_bonus;//�������� ������
    private int original_max_jump_count;//���� Ƚ�� ����
    int max_jump_count_bonus;//����Ƚ�� ������

    [Header("gun")]
    private float original_firedelay;//���� �߻� �ӵ�
    private float firedelay_bonus;//�߻�ӵ� ���ʽ�
    private int original_Gun_Atk;//�� ���ݷ� ����
    private int Gun_Atk_bonus;//�� ���ݷ� ������
    private int Gun_Atk_spbonus;//�� ���ݷ� ������-Ư��������

    private float original_bullet_speed;//�Ѿ� �ӵ� ����
    private float bullet_speed_bonus;//�Ѿ� �ӵ� ������
    private bool volly;//����,�ܹ�

    [Header("dash")]
    [SerializeField]
    float original_dash_force;//�뽬���� ����
    float dash_force_bonus;//�뽬���� ������
    int original_max_dash_count = 1;//�뽬Ƚ�� ����
    int max_dash_count_bonus;//�뽬Ƚ�� ������

    int attack_num=1;
    int attack_num_bonus;
    public bool spawn_check;
    bool created_hp_chk;
    float original_critical_rate;
    float critical_rate_bonus;
    float critical_damage;
    float critical_damage_bonus;

    float critical_rate;
    float critical_rate_num;

    public int air_attack_num;
    public int air_attack_num_orignal;
    public int air_attack_num_bonus;

    float dash_recover_timer_check = 1f;
    float dash_recover_time_bouns;

    public int used_effct_def_effect_check;//0,1,2,3
    public bool used_effct_def_effect;
    public int used_effct_HP_effect_check;//0,1,2
    public bool used_effct_HP_effect;
    public int used_effct_A_SPEED_effect_check;//0,1,2
    public bool used_effct_A_SPEED_effect;


    public float get_critical_damage()
    {
        return critical_damage+ critical_damage_bonus;
    }
    public float get_critical_damage_bonus()
    {
        return critical_damage_bonus;
    }
    public float get_origin_critical_rate()
    {
        return original_critical_rate + critical_rate_bonus;
    }
    public float get_critical_rate()
    {
        return critical_rate;
    }
    public float get_critical_rate_bonus()
    {
        return critical_rate_bonus;
    }
    public void set_critical_rate(float a=0)
    {
        critical_rate_bonus = a;
    }
    public void set_critical_Damage(float a = 0)
    {
        critical_damage_bonus = a;
    }
    public void critical_rate_plus()
    {
        critical_rate_num = critical_rate / 2;
        critical_rate += critical_rate_num;
    }
    public void reset_critical_rate()
    {
        critical_rate = get_origin_critical_rate();
    }
    public bool critical()
    {
        float crit = critical_rate;
        Debug.Log(critical_rate);
        float rand = Random.Range(0.00f, 1.00f);
        if (crit >= rand)
        {
            reset_critical_rate();
            Debug.Log("ũ��Ƽ��!" + crit + "," + rand);
            return true;
        }
        else
        {
            critical_rate_plus();
            Debug.Log("ũ��Ƽ��! �� �ƴϾ���?"+crit+","+rand);
            return false;
        }
    }
    //List<Dictionary<string, object>> Data = CSVReader.Read("playerStatus");
    //csv��  original �� ���ϸ� �ɵ�?
    public void set_layout()
    {
        air_attack_num_orignal = 1;
        air_attack_num = air_attack_num_orignal;
        List<Dictionary<string, object>> Data = CSVReader.Read("Player");
        //Debug.Log("�ƴ� ����" + Data[0]["Max_Hp"]);

        original_MaX_HP = int.Parse(Data[0]["Max_Hp"].ToString());
        if (!created_hp_chk)
        {
            HP = get_max_hp();
            created_hp_chk = true;
        }
        else
        {
            HP = get_hp();
        }
       
        original_Defense = int.Parse(Data[0]["Defense"].ToString());
        original_speed = float.Parse(Data[0]["Move_Speed"].ToString());
        original_untouchable_time = float.Parse(Data[0]["Untouchable_Time"].ToString());
        original_jump_force = float.Parse(Data[0]["Jump_Force"].ToString());
        original_max_jump_count = int.Parse(Data[0]["Jump_Count"].ToString());
        original_firedelay = float.Parse(Data[0]["FireDelay"].ToString());
        original_Gun_Atk = int.Parse(Data[0]["Gun_Force"].ToString());
        original_bullet_speed = float.Parse(Data[0]["Bullet_Speed"].ToString());
        volly = bool.Parse(Data[0]["Volly"].ToString());
        original_dash_force = float.Parse(Data[0]["Desh_Force"].ToString());
        original_max_dash_count = int.Parse(Data[0]["Desh_Count"].ToString());
        original_critical_rate=float.Parse(Data[0]["Critical_Rate"].ToString());
        critical_rate = original_critical_rate;
        critical_damage= float.Parse(Data[0]["Critical_Damage"].ToString());
        spawn_check = true;
        //original_MaX_HP = 100;
        //HP = get_max_hp();
        //original_Defense = 0;
        //original_untouchable_time = 1;
        //original_jump_force = 20;
        //original_max_jump_count = 1;
        //original_firedelay = 0.2f;
        //original_Gun_Atk = 10;
        //original_bullet_speed = 10;
        //volly = true;
        //original_dash_force = 20;
        //original_max_dash_count = 1;
        //original_speed = 10;

        
    }
    public void make_barrier()
    {
        Barrier = original_Barrier + Barrier_Bouns;
    }
    public void set_barrier(int a)
    {
         Barrier_Bouns=a;
    }
    
    public int get_present_barrier()
    {
        return Barrier;
    }
    public int get_barrier()
    {
        return original_Barrier + Barrier_Bouns;
    }
    public int get_barrier_bonus()
    {
        return  Barrier_Bouns;
    }
    public int get_attack_num()
    {
        return attack_num+attack_num_bonus;
    }
    public void set_attack_num(int a)
    {
        attack_num_bonus = a;
    }
    

    //���ݷ�
    public int get_atk()       
    {
        return original_Gun_Atk + Gun_Atk_bonus+AchievementsManage.achievementsManage.Ach_Damge+Gun_Atk_spbonus;
    }
    public int get_atk_bonus()
    {
        return Gun_Atk_bonus;
    }
    public int get_original_atk()
    {
        return original_Gun_Atk;
    }
    public int get_atk_no_sp()
    {
         return  original_Gun_Atk + Gun_Atk_bonus+AchievementsManage.achievementsManage.Ach_Damge;
    }
    public void set_atk(int p=0)    
    {
        Gun_Atk_bonus = p;
    }

    public void set_spatk(int p = 0)
    {
        Gun_Atk_spbonus = p;
    }

    public int get_spatk()
    {
        return Gun_Atk_spbonus;
    }

    //�� ����
    public float get_bullet_speed()
    {
        return original_bullet_speed + bullet_speed_bonus;
    }
    public float get_bullet_speed_bonus()
    {
        return bullet_speed_bonus;
    }
    public void set_bullet_speed(float i=0)
    {
        bullet_speed_bonus=i;
    }
    public float get_firedelay()
    {
        return original_firedelay + firedelay_bonus;
    }
    public float get_firedelay_bonus()
    {
        return firedelay_bonus;
    }
    public void set_firedelay(float i=0)
    {
        firedelay_bonus=i;
    }


    //�ܹ�
    public bool get_volly()
    {
        return volly;
    }

    //�׽�Ʈ
    public void DamgeTest(int i)
    {
        HP -= i;
    }

    //�̵�����
    public void set_speed(float p=0)
    {
        speed_bonus = p;
    }
    public float get_speed()
    {
        return original_speed + speed_bonus+ AchievementsManage.achievementsManage.Ach_Speed;
    }
    public float get_speed_bonus()
    {
        return speed_bonus;
    }

    public float get_dash_force()
    {
        return original_dash_force + dash_force_bonus;
    }
    public void set_dassh_recover_time(float a)
    {
        dash_recover_time_bouns = a;
    }
    public float get_dash_recover_time()
    {
        return dash_recover_timer_check - dash_recover_time_bouns;
    }
    public float get_dash_recover_time_bonus()
    {
        return dash_recover_time_bouns;
    }
    public float get_dash_recover_time_original()
    {
        return dash_recover_timer_check;
    }
    void Awake()
    {
       
        p_status = this;
        
    }

    //ü��
    public int get_max_hp()
    {
        return original_MaX_HP+MaX_HP_bonus+ AchievementsManage.achievementsManage.Ach_MaxHp+MaX_HP_spbonus;
    }
    public int get_hp()
    {
        return HP;
    }
    public int get_max_Hp_bonus()
    {
        return MaX_HP_bonus;
    }
    public void set_max_HP(int i=0)
    {
        MaX_HP_bonus = i;
    }
    public void set_max_spHP(int i = 0)
    {
        MaX_HP_spbonus = i;
    }
    public void set_hp(int i)
    {
        HP += i;
        int n = get_max_hp() / i;
        Gamemanager.GM.Player_obj.GetComponent<PlayerCharacter>().Player_heal_cross_particle(n);
        if(HP>get_max_hp())
        {
            HP = get_max_hp();
        }
    }


  
    public float get_untouchable_time()
    {
        return original_untouchable_time + untouchable_time_bonus;
    }

    //ĳ���� ������
    public int get_dash_count()
    {
        return original_max_dash_count+max_dash_count_bonus;
    }
    public int get_dash_count_bonus()
    {
        return max_dash_count_bonus;
    }
    public void set_dash_count(int a)
    {
        max_dash_count_bonus=a;
    }
    public int get_jump_count()
    {
        return original_max_jump_count + max_jump_count_bonus;
    }
    public float get_jump_force()
    {
        return original_jump_force + jump_force_bonus;
    }
    public float get_jump_force_bonus()
    {
        return jump_force_bonus;
    }

    //����
    public int get_defense_point()
    {
        return original_Defense + Defense_bonus+ AchievementsManage.achievementsManage.Ach_Defense;
    }
    public int get_defense_bonus()
    {
        return Defense_bonus;
    }
    public void set_defense_point(int def=0)
    {
        Defense_bonus = def;
    }
   
    public void damage_present_barrier(int a, GameObject DNP, Transform Tr)
    {
        float damage_lose = get_defense_point() / 100;
        int damage = Mathf.RoundToInt(a * (1.0f - damage_lose));//������ ����
        if (used_effct_def_effect_check > 0)
        {
            damage = damage - 1;
        }
        if (damage <= 0)
        {
            damage = 1;
        }
        Barrier -= damage;
       
        
        if (Barrier < 0)
        {
            Font_manager.DN.SpawnNumber(1, Barrier+damage, Tr);
            damage_hp(-1 * Barrier, Gamemanager.GM.Player_obj.GetComponent<PlayerCharacter>().DNP, Gamemanager.GM.Player_obj.transform);
            Barrier = 0;
        }
        else
        {
            Font_manager.DN.SpawnNumber(1, damage, Tr);

        }
    }
    public void lose_hp(int i)//���� �ƴ�
    {
        HP -= i;
    }
    public void damage_hp(int i, GameObject DNP, Transform Tr)
    {
        float damage_lose = get_defense_point() / 100;
        int damage = Mathf.RoundToInt(i * (1.0f - damage_lose));//������ ����
        if (used_effct_def_effect_check > 0)
        {
            damage = damage - 1;
        }
        if (damage <= 0)
        {
            damage = 1;
        }
        Debug.Log(damage);
        //HP -= i-i*get_defense_point();
        Font_manager.DN.SpawnNumber(6, damage, Tr);
        HP -= damage;
        if (HP < 0)
        {
            HP = 0;
        }

    }
}
