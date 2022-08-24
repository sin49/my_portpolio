using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_status : MonoBehaviour//플레이어의 능력치를 가지는 클레스
{
    //싱글톤패턴
    public static Player_status p_status;

    public int layout_num;//레이아웃 숫자
    
    [SerializeField]
    private int original_MaX_HP;//최대 체력 원본
    private int MaX_HP_bonus;//최대 체력 가산점
    private int MaX_HP_spbonus;//최대 체력 가산점-특수아이템
    [SerializeField]
    private int HP;//현재 체력
    private int Barrier;
    private int original_Barrier;
    private int Barrier_Bouns;
    public int Money;
    private int original_Defense;//방어력 원본
    private int Defense_bonus;//방어력 가산점
    private float original_untouchable_time;//무적시간 원본 
    private float untouchable_time_bonus;//무적시간 보너스

    float original_speed;
    float speed_bonus;

    [Header("jump")]
    private float original_jump_force;//점프높이 원본
    float jump_force_bonus;//점프높이 가산점
    private int original_max_jump_count;//점프 횟수 원본
    int max_jump_count_bonus;//점프횟수 가산점

    [Header("gun")]
    private float original_firedelay;//원본 발사 속도
    private float firedelay_bonus;//발사속도 보너스
    private int original_Gun_Atk;//총 공격력 원본
    private int Gun_Atk_bonus;//총 공격력 가산점
    private int Gun_Atk_spbonus;//총 공격력 가산점-특수아이템

    private float original_bullet_speed;//총알 속도 원본
    private float bullet_speed_bonus;//총알 속도 가산점
    private bool volly;//연발,단발

    [Header("dash")]
    [SerializeField]
    float original_dash_force;//대쉬길이 원본
    float dash_force_bonus;//대쉬길이 가산점
    int original_max_dash_count = 1;//대쉬횟수 원본
    int max_dash_count_bonus;//대쉬횟수 가산점

    int attack_num=1;
    int attack_num_bonus;

    public bool spawn_check;
    bool created_hp_chk;
    //치명타
    float original_critical_rate;
    float critical_rate_bonus;
    float critical_damage;
    float critical_damage_bonus;

    float critical_rate;
    float critical_rate_num;
    //공중 공격
    public int air_attack_num;
    public int air_attack_num_orignal;
    public int air_attack_num_bonus;
    //대쉬회복
    float dash_recover_timer_check = 1f;
    float dash_recover_time_bouns;
    //특수능력 보유 여부
    public int used_effct_def_effect_check;//0,1,2,3
    public bool used_effct_def_effect;
    public int used_effct_HP_effect_check;//0,1,2
    public bool used_effct_HP_effect;
    public int used_effct_A_SPEED_effect_check;//0,1,2
    public bool used_effct_A_SPEED_effect;

    //능력치의 값을 get,set을 통해 받고 설정한다
    //크리티컬 관련 get()
    public float get_critical_damage()// 종합 크리티컬 데미지
    {
        return critical_damage+ critical_damage_bonus;
    }
    public float get_critical_damage_bonus()//크리티컬 데미지 보너스
    {
        return critical_damage_bonus;
    }
    public float get_origin_critical_rate()//종합 크리티컬 확률
    {
        return original_critical_rate + critical_rate_bonus;
    }
    public float get_critical_rate()//크리티컬 확률
    {
        return critical_rate;
    }
    public float get_critical_rate_bonus()////크리티컬 확률 보너스
    {
        return critical_rate_bonus;
    }
    //크리티컬 관련 set
    public void set_critical_rate(float a=0)
    {
        critical_rate_bonus = a;
    }
    public void set_critical_Damage(float a = 0)
    {
        critical_damage_bonus = a;
    }

    public void critical_rate_plus()//크리티컬 확률 보정이 점점 높아진다
    {
        critical_rate_num = critical_rate / 2;
        critical_rate += critical_rate_num;
    }
    public void reset_critical_rate()//크리티컬 보정을 초기화한다
    {
        critical_rate = get_origin_critical_rate();
    }
    public bool critical()//크리티컬 시스템
    {
        float crit = critical_rate;
        Debug.Log(critical_rate);
        float rand = Random.Range(0.00f, 1.00f);//(0~1)
        //크리티컬 확률이 랜덤값보다 높을 때 크리티컬
        if (crit >= rand)
        {
            //크리티컬이 발생하면 크리티컬 보정을 초기화
            reset_critical_rate();
            Debug.Log("크리티컬!" + crit + "," + rand);
            //반환값 true
            return true;
        }
        else
        {
            //크리티컬이 발생하지않으면 확률에 보정을 넣어 점점 크리티컬이 터지기 쉽게 만든다
            critical_rate_plus();
   
            return false;
        }
    }

    public void set_layout()//플레이어의 초기 능력치를 설정한다
    {
        //resources 파일의 엑셀을 데이터베이스로 사용
        //데이터베이스 구축+ 연결은 자신이 아닌 같은 팀의 다른 프로그래머가 담당
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
    //체력 보호막 구현
    public void make_barrier()//베리어의 최종 수치를 정하고 베리어 생성
    {
        Barrier = original_Barrier + Barrier_Bouns;
    }
    public void set_barrier(int a)//베리어의 수치 보너스 조정( set)
    {
         Barrier_Bouns=a;
    }
    
    public int get_present_barrier()//현재 베리어의 수치를 가져온다
    {
        return Barrier;
    }
    public int get_barrier()//최대 베리어의 수치를 가져온다
    {
        return original_Barrier + Barrier_Bouns;
    }
    public int get_barrier_bonus()//베리어의 수치 보너스의 값을 가져옴(get)
    {
        return  Barrier_Bouns;
    }
    

    //공격력
    //get
    public int get_atk()  //최종 공격력     
    {
        return original_Gun_Atk + Gun_Atk_bonus+AchievementsManage.achievementsManage.Ach_Damge+Gun_Atk_spbonus;
    }
    public int get_atk_bonus()// 공격력  보너스
    {
        return Gun_Atk_bonus;
    }
    public int get_original_atk()//초기 공격력 
    {
        return original_Gun_Atk;
    }
    public int get_spatk()//특수능력을 통한 공격력 보너스
    {
        return Gun_Atk_spbonus;
    }

    //set
    public void set_atk(int p=0)   //공격력 보너스 
    {
        Gun_Atk_bonus = p;
    }

    public void set_spatk(int p = 0)//특수능력을 통한 공격력 보너스
    {
        Gun_Atk_spbonus = p;
    }

  
    //총 관련(공격 기획이 원거리->근거리로 변경됨으로써 사용안함)
    public float get_bullet_speed()// 총알 속도
    {
        return original_bullet_speed + bullet_speed_bonus;
    }
    public float get_bullet_speed_bonus()// 총알 속도 보너스
    {
        return bullet_speed_bonus;
    }
    public void set_bullet_speed(float i=0)// 총알 속도 보너스 값 조정
    {
        bullet_speed_bonus=i;
    }
    //공격 속도
    public float get_firedelay()//종합 공속
    {
        return original_firedelay + firedelay_bonus;
    }
    public float get_firedelay_bonus()//보너스
    {
        return firedelay_bonus;
    }
    public void set_firedelay(float i=0)//공격속도 조정
    {
        firedelay_bonus=i;
    }


    //단발
    public bool get_volly()
    {
        return volly;
    }

    //데미지 테스트
    public void DamgeTest(int i)
    {
        HP -= i;
    }

    //이동관련
    public void set_speed(float p=0)//이동속도 조정
    {
        speed_bonus = p;
    }
    public float get_speed()//최종 속도 값
    {
        return original_speed + speed_bonus+ AchievementsManage.achievementsManage.Ach_Speed;
    }
    public float get_speed_bonus()//속도 보너스 값
    {
        return speed_bonus;
    }
    //대쉬관련
    public float get_dash_force()//대쉬시 받는 힘 
    {
        return original_dash_force + dash_force_bonus;
    }
    //대쉬 다시 대쉬할 수 있기까지의 회복시간
    public void set_dassh_recover_time(float a)//회복시간 설정
    {
        dash_recover_time_bouns = a;
    }
    public float get_dash_recover_time()//회복시간 받기
    {
        return dash_recover_timer_check - dash_recover_time_bouns;
    }
    public float get_dash_recover_time_bonus()//회복시간 보너스
    {
        return dash_recover_time_bouns;
    }
    public float get_dash_recover_time_original()//원본 회복시간
    {
        return dash_recover_timer_check;
    }
    void Awake()
    {
       
        p_status = this;
        
    }

    //체력
    public int get_max_hp()//최대 체력
    {
        return original_MaX_HP+MaX_HP_bonus+ AchievementsManage.achievementsManage.Ach_MaxHp+MaX_HP_spbonus;
    }
    public int get_hp()//현재 체력
    {
        return HP;
    }
    public int get_max_Hp_bonus()//최대채력 보너스
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
    public void set_hp(int i)//현재 채력을 회복한다
    {
        HP += i;
        int n = get_max_hp() / i;
        Gamemanager.GM.Player_obj.GetComponent<PlayerCharacter>().Player_heal_cross_particle(n);//회복 이펙트 파티클 생성
        if(HP>get_max_hp())//최대 체력 이상이면 최대채력으로
        {
            HP = get_max_hp();
        }
    }

    
  
    public float get_untouchable_time()//무적시간
    {
        return original_untouchable_time + untouchable_time_bonus;
    }

//한번에 대쉬가능한 횟수
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
    //점프 횟수
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

    //방어력
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
   
    //보호막이 있을시 데미지 계산식
    public void damage_present_barrier(int a, GameObject DNP, Transform Tr)
    {
        float damage_lose = get_defense_point() / 100;//방어력을 통해 대미지 감소
        int damage = Mathf.RoundToInt(a * (1.0f - damage_lose));//데미지 계산식
        if (used_effct_def_effect_check > 0)//특수 능력 활성화시
        {
            damage = damage - 1;//데미지 -1
        }
        if (damage <= 0)//데미지가 0이하 일때
        {
            damage = 1;//무조건 1로
        }
        Barrier -= damage;//베리어에 데미지 값을 빼기
       
        
        if (Barrier < 0)//배리어의 값이 음수일때(배리어 초과 데미지)
        {
            //초과 데미지를 HP에다가 준다
            Font_manager.DN.SpawnNumber(1, Barrier+damage, Tr);//데미지 폰트 생성(데미지 폰트는 에셋)
            damage_hp(-1 * Barrier, Gamemanager.GM.Player_obj.GetComponent<PlayerCharacter>().DNP, Gamemanager.GM.Player_obj.transform);
            Barrier = 0;//베리어는 파괴
        }
        else
        {
            Font_manager.DN.SpawnNumber(1, damage, Tr);//데미지 폰트 생성(데미지 폰트는 에셋)

        }
    }
    public void lose_hp(int i)//계산식을 거치지 않고 피해를 줌
    {
        HP -= i;
    }
    public void damage_hp(int i, GameObject DNP, Transform Tr)//체력에 피해를 주기
    {
        float damage_lose = get_defense_point() / 100;//방어력을 통해 대미지 감소
        int damage = Mathf.RoundToInt(i * (1.0f - damage_lose));//데미지 계산식
        if (used_effct_def_effect_check > 0)
        {
            damage = damage - 1;
        }
        if (damage <= 0)
        {
            damage = 1;
        }

   
        Font_manager.DN.SpawnNumber(6, damage, Tr);
        //체력을 데미지만큼 뺀다
        HP -= damage;
        if (HP < 0)//체력이 음수가 된다면 0으로 만든다
        {
            HP = 0;
        }

    }
}
