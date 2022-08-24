using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_status : MonoBehaviour//�÷��̾��� �ɷ�ġ�� ������ Ŭ����
{
    //�̱�������
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
    //ġ��Ÿ
    float original_critical_rate;
    float critical_rate_bonus;
    float critical_damage;
    float critical_damage_bonus;

    float critical_rate;
    float critical_rate_num;
    //���� ����
    public int air_attack_num;
    public int air_attack_num_orignal;
    public int air_attack_num_bonus;
    //�뽬ȸ��
    float dash_recover_timer_check = 1f;
    float dash_recover_time_bouns;
    //Ư���ɷ� ���� ����
    public int used_effct_def_effect_check;//0,1,2,3
    public bool used_effct_def_effect;
    public int used_effct_HP_effect_check;//0,1,2
    public bool used_effct_HP_effect;
    public int used_effct_A_SPEED_effect_check;//0,1,2
    public bool used_effct_A_SPEED_effect;

    //�ɷ�ġ�� ���� get,set�� ���� �ް� �����Ѵ�
    //ũ��Ƽ�� ���� get()
    public float get_critical_damage()// ���� ũ��Ƽ�� ������
    {
        return critical_damage+ critical_damage_bonus;
    }
    public float get_critical_damage_bonus()//ũ��Ƽ�� ������ ���ʽ�
    {
        return critical_damage_bonus;
    }
    public float get_origin_critical_rate()//���� ũ��Ƽ�� Ȯ��
    {
        return original_critical_rate + critical_rate_bonus;
    }
    public float get_critical_rate()//ũ��Ƽ�� Ȯ��
    {
        return critical_rate;
    }
    public float get_critical_rate_bonus()////ũ��Ƽ�� Ȯ�� ���ʽ�
    {
        return critical_rate_bonus;
    }
    //ũ��Ƽ�� ���� set
    public void set_critical_rate(float a=0)
    {
        critical_rate_bonus = a;
    }
    public void set_critical_Damage(float a = 0)
    {
        critical_damage_bonus = a;
    }

    public void critical_rate_plus()//ũ��Ƽ�� Ȯ�� ������ ���� ��������
    {
        critical_rate_num = critical_rate / 2;
        critical_rate += critical_rate_num;
    }
    public void reset_critical_rate()//ũ��Ƽ�� ������ �ʱ�ȭ�Ѵ�
    {
        critical_rate = get_origin_critical_rate();
    }
    public bool critical()//ũ��Ƽ�� �ý���
    {
        float crit = critical_rate;
        Debug.Log(critical_rate);
        float rand = Random.Range(0.00f, 1.00f);//(0~1)
        //ũ��Ƽ�� Ȯ���� ���������� ���� �� ũ��Ƽ��
        if (crit >= rand)
        {
            //ũ��Ƽ���� �߻��ϸ� ũ��Ƽ�� ������ �ʱ�ȭ
            reset_critical_rate();
            Debug.Log("ũ��Ƽ��!" + crit + "," + rand);
            //��ȯ�� true
            return true;
        }
        else
        {
            //ũ��Ƽ���� �߻����������� Ȯ���� ������ �־� ���� ũ��Ƽ���� ������ ���� �����
            critical_rate_plus();
   
            return false;
        }
    }

    public void set_layout()//�÷��̾��� �ʱ� �ɷ�ġ�� �����Ѵ�
    {
        //resources ������ ������ �����ͺ��̽��� ���
        //�����ͺ��̽� ����+ ������ �ڽ��� �ƴ� ���� ���� �ٸ� ���α׷��Ӱ� ���
        air_attack_num_orignal = 1;
        air_attack_num = air_attack_num_orignal;
        List<Dictionary<string, object>> Data = CSVReader.Read("Player");
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
       

        
    }
    //ü�� ��ȣ�� ����
    public void make_barrier()//�������� ���� ��ġ�� ���ϰ� ������ ����
    {
        Barrier = original_Barrier + Barrier_Bouns;
    }
    public void set_barrier(int a)//�������� ��ġ ���ʽ� ����( set)
    {
         Barrier_Bouns=a;
    }
    
    public int get_present_barrier()//���� �������� ��ġ�� �����´�
    {
        return Barrier;
    }
    public int get_barrier()//�ִ� �������� ��ġ�� �����´�
    {
        return original_Barrier + Barrier_Bouns;
    }
    public int get_barrier_bonus()//�������� ��ġ ���ʽ��� ���� ������(get)
    {
        return  Barrier_Bouns;
    }
    

    //���ݷ�
    //get
    public int get_atk()  //���� ���ݷ�     
    {
        return original_Gun_Atk + Gun_Atk_bonus+AchievementsManage.achievementsManage.Ach_Damge+Gun_Atk_spbonus;
    }
    public int get_atk_bonus()// ���ݷ�  ���ʽ�
    {
        return Gun_Atk_bonus;
    }
    public int get_original_atk()//�ʱ� ���ݷ� 
    {
        return original_Gun_Atk;
    }
    public int get_spatk()//Ư���ɷ��� ���� ���ݷ� ���ʽ�
    {
        return Gun_Atk_spbonus;
    }

    //set
    public void set_atk(int p=0)   //���ݷ� ���ʽ� 
    {
        Gun_Atk_bonus = p;
    }

    public void set_spatk(int p = 0)//Ư���ɷ��� ���� ���ݷ� ���ʽ�
    {
        Gun_Atk_spbonus = p;
    }

  
    //�� ����(���� ��ȹ�� ���Ÿ�->�ٰŸ��� ��������ν� ������)
    public float get_bullet_speed()// �Ѿ� �ӵ�
    {
        return original_bullet_speed + bullet_speed_bonus;
    }
    public float get_bullet_speed_bonus()// �Ѿ� �ӵ� ���ʽ�
    {
        return bullet_speed_bonus;
    }
    public void set_bullet_speed(float i=0)// �Ѿ� �ӵ� ���ʽ� �� ����
    {
        bullet_speed_bonus=i;
    }
    //���� �ӵ�
    public float get_firedelay()//���� ����
    {
        return original_firedelay + firedelay_bonus;
    }
    public float get_firedelay_bonus()//���ʽ�
    {
        return firedelay_bonus;
    }
    public void set_firedelay(float i=0)//���ݼӵ� ����
    {
        firedelay_bonus=i;
    }


    //�ܹ�
    public bool get_volly()
    {
        return volly;
    }

    //������ �׽�Ʈ
    public void DamgeTest(int i)
    {
        HP -= i;
    }

    //�̵�����
    public void set_speed(float p=0)//�̵��ӵ� ����
    {
        speed_bonus = p;
    }
    public float get_speed()//���� �ӵ� ��
    {
        return original_speed + speed_bonus+ AchievementsManage.achievementsManage.Ach_Speed;
    }
    public float get_speed_bonus()//�ӵ� ���ʽ� ��
    {
        return speed_bonus;
    }
    //�뽬����
    public float get_dash_force()//�뽬�� �޴� �� 
    {
        return original_dash_force + dash_force_bonus;
    }
    //�뽬 �ٽ� �뽬�� �� �ֱ������ ȸ���ð�
    public void set_dassh_recover_time(float a)//ȸ���ð� ����
    {
        dash_recover_time_bouns = a;
    }
    public float get_dash_recover_time()//ȸ���ð� �ޱ�
    {
        return dash_recover_timer_check - dash_recover_time_bouns;
    }
    public float get_dash_recover_time_bonus()//ȸ���ð� ���ʽ�
    {
        return dash_recover_time_bouns;
    }
    public float get_dash_recover_time_original()//���� ȸ���ð�
    {
        return dash_recover_timer_check;
    }
    void Awake()
    {
       
        p_status = this;
        
    }

    //ü��
    public int get_max_hp()//�ִ� ü��
    {
        return original_MaX_HP+MaX_HP_bonus+ AchievementsManage.achievementsManage.Ach_MaxHp+MaX_HP_spbonus;
    }
    public int get_hp()//���� ü��
    {
        return HP;
    }
    public int get_max_Hp_bonus()//�ִ�ä�� ���ʽ�
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
    public void set_hp(int i)//���� ä���� ȸ���Ѵ�
    {
        HP += i;
        int n = get_max_hp() / i;
        Gamemanager.GM.Player_obj.GetComponent<PlayerCharacter>().Player_heal_cross_particle(n);//ȸ�� ����Ʈ ��ƼŬ ����
        if(HP>get_max_hp())//�ִ� ü�� �̻��̸� �ִ�ä������
        {
            HP = get_max_hp();
        }
    }

    
  
    public float get_untouchable_time()//�����ð�
    {
        return original_untouchable_time + untouchable_time_bonus;
    }

//�ѹ��� �뽬������ Ƚ��
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
    //���� Ƚ��
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
   
    //��ȣ���� ������ ������ ����
    public void damage_present_barrier(int a, GameObject DNP, Transform Tr)
    {
        float damage_lose = get_defense_point() / 100;//������ ���� ����� ����
        int damage = Mathf.RoundToInt(a * (1.0f - damage_lose));//������ ����
        if (used_effct_def_effect_check > 0)//Ư�� �ɷ� Ȱ��ȭ��
        {
            damage = damage - 1;//������ -1
        }
        if (damage <= 0)//�������� 0���� �϶�
        {
            damage = 1;//������ 1��
        }
        Barrier -= damage;//����� ������ ���� ����
       
        
        if (Barrier < 0)//�踮���� ���� �����϶�(�踮�� �ʰ� ������)
        {
            //�ʰ� �������� HP���ٰ� �ش�
            Font_manager.DN.SpawnNumber(1, Barrier+damage, Tr);//������ ��Ʈ ����(������ ��Ʈ�� ����)
            damage_hp(-1 * Barrier, Gamemanager.GM.Player_obj.GetComponent<PlayerCharacter>().DNP, Gamemanager.GM.Player_obj.transform);
            Barrier = 0;//������� �ı�
        }
        else
        {
            Font_manager.DN.SpawnNumber(1, damage, Tr);//������ ��Ʈ ����(������ ��Ʈ�� ����)

        }
    }
    public void lose_hp(int i)//������ ��ġ�� �ʰ� ���ظ� ��
    {
        HP -= i;
    }
    public void damage_hp(int i, GameObject DNP, Transform Tr)//ü�¿� ���ظ� �ֱ�
    {
        float damage_lose = get_defense_point() / 100;//������ ���� ����� ����
        int damage = Mathf.RoundToInt(i * (1.0f - damage_lose));//������ ����
        if (used_effct_def_effect_check > 0)
        {
            damage = damage - 1;
        }
        if (damage <= 0)
        {
            damage = 1;
        }

   
        Font_manager.DN.SpawnNumber(6, damage, Tr);
        //ü���� ��������ŭ ����
        HP -= damage;
        if (HP < 0)//ü���� ������ �ȴٸ� 0���� �����
        {
            HP = 0;
        }

    }
}
