using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_anim_event : MonoBehaviour//플레이어의 에니메이션을 통해 일어나는 이벤트 클레스
{
    [Header("Effects")]
    public float melee_force;
    public float melee_1_anim_time;
    public float melee_2_anim_time;
    public float melee_3_anim_time;
    bool landing_ef_chk;
    public AnimationClip melee_1_clip;
    public AnimationClip melee_1_hold_clip;
    public AnimationClip melee_2_clip;
    public AnimationClip melee_2_hold_clip;
    public AnimationClip melee_3_clip;
    public AnimationClip melee_3_hold_clip;
    public AnimationClip air_melee_clip;
    public AnimationClip air_melee_hold_clip;
    public GameObject Player_ghost;
    public List<GameObject> Player_ghost_instansi=new List<GameObject>();
    public Transform created_list;
    public GameObject m_RunStopDust;
    public GameObject m_JumpDust;
    public GameObject m_LandingDust;
    public GameObject m_DodgeDust;
    public GameObject m_WallSlideDust;
    public GameObject m_WallJumpDust;
    public GameObject m_AirSlamDust;
    public GameObject m_ParryEffect;
    public GameObject melee_1;
    public GameObject melee_1_instani;
    public GameObject melee_2;
    public GameObject melee_2_instani;
    public GameObject melee_3;
    public GameObject melee_3_instani;
    public GameObject air_melee_;
    public GameObject air_melee_instani;
    public float melee_1_reaction;
    public float melee_2_reaction;
    public float melee_3_reaction;
    public int melee_dmg;
    private AudioManager_PrototypeHero m_audioManager;
    public PlayerCharacter m_player;
    melee_attack index;
    public Player_animator p_anim;

    void AE_set_melee_attack_dmg()//근접 공격력을 플레이어의 공격력과 같게한다
    {

        melee_dmg= Gamemanager.GM.game_ev.when_P_A_Key_input(Player_status.p_status.get_atk());//게임 이벤트 클레스에 공격키를 눌렸음을 호출한다
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++)//플레이어의 환영(잔상) 효과 /생성 후 활성화.비활성화 하여 사용(풀링)
        {
            //셍성 후 풀링 리스트에 넣고 비활성화 시킨다
            var a= Instantiate(Player_ghost);
            a.transform.GetChild(0).GetComponent<Player_shadow>().p_created = true;
            Player_ghost_instansi.Add(a);
            //생성한 환영은 풀링 관리용 오브젝트에 자식으로 들어간다
            a.transform.SetParent(created_list);
            a.SetActive(false);
        }
        m_player= GetComponentInParent<PlayerCharacter>();
        p_anim = GetComponent<Player_animator>();
        m_audioManager = AudioManager_PrototypeHero.instance;
    }
 
    GameObject player_ghost_pulling()//생성시킨 환영 리스트에 비활성화 된 환영의 게임오브젝트를 올림차순으로 검사하여 반환한다(풀링)
    {
        int index=0;
        for(int i = 0; i < Player_ghost_instansi.Count; i++)
        {
            if (!Player_ghost_instansi[i].activeSelf)//활성화됬는지 여부
            {
                index = i;
            }
        }
        return Player_ghost_instansi[index];//비활성화된 환영 반환(모두 활성화 사태면 0번을 반환)
    }
    //player_ghost_attack_effect(1,2,3,air)
    //이단 공격이 발생했을 때 현재 자신이 실해하고 있는 공격 에니메이션과 같은 에니메이션을 실행하는 환영을 플레이어의 위치 살짝 뒤쪽에 소환한다(연출)
    //환영은 풀링시켜서 사용되며 지속시간은 실행하는 에니메이션의 길이를 따라간다. 지속시간이 지나면 다시 비활성화한다.
    //플레이어의 공격 에니메이션과 환영의 에니메이션이 겹치면서 연달아 공격하는 연출을 만든다
    void player_ghost_attack_1_effect()
    {
        if (melee_1_instani.GetComponent<melee_attack>().Double_attack_on)//이단 공격 여부
        {

                var a = player_ghost_pulling();//비활성화된 환영을 가져온다
                
            //환영의 정보를 설정한다(실행시킬 에니메이션,환영의 지속시간,환영의 특수성 여부)
                var b = a.transform.GetChild(0).GetComponent<Player_shadow>();
                b.shadow_original_timer = melee_1_clip.length ;
                b.shadow_time = melee_1_clip.length;
                b.once_chk = true;
                b.anim_chk = false;
                b.shadow_type = true;
                b.animation_level = 3;
            //설정된 환영을 활성화 한다
                a.SetActive(true);
  
          
        }
    }
    void player_ghost_attack_2_effect()
    {
        if (melee_2_instani.GetComponent<melee_attack>().Double_attack_on)
        {
            var a = player_ghost_pulling();

            var b = a.transform.GetChild(0).GetComponent<Player_shadow>();
            b.shadow_original_timer = melee_1_clip.length;
            b.shadow_time = melee_1_clip.length;
            b.once_chk = true;
            b.anim_chk = false;
            b.shadow_type = true;
            b.animation_level = 1;
            a.SetActive(true);
        }
    }
    
    void player_ghost_attack_3_effect()
    {
        if (melee_3_instani.GetComponent<melee_attack>().Double_attack_on)
        {
            var a = player_ghost_pulling();

            var b = a.transform.GetChild(0).GetComponent<Player_shadow>();
            b.shadow_original_timer = melee_1_clip.length;
            b.shadow_time = melee_1_clip.length;
            b.once_chk = true;
            b.anim_chk = false;
            b.shadow_type = true;
            b.animation_level = 2;
            a.SetActive(true);
        }
    }
    void player_air_ghost_attack_effect()
    {
        if (air_melee_instani.GetComponent<melee_attack>().Double_attack_on)
        {
            var a = player_ghost_pulling();

            var b = a.transform.GetChild(0).GetComponent<Player_shadow>();
            b.shadow_original_timer = melee_1_clip.length;
            b.shadow_time = melee_1_clip.length;
            b.once_chk = true;
            b.anim_chk = false;
            b.shadow_type = true;
            b.animation_level = 11;
            a.SetActive(true);
        }
    }
    //플레이어 사망에니메이션이 끝나면 사망 처리하는 함수를 실행시킨다
    void player_died()
    {
        m_player.death();
    }
    //부활이 가능하다면 부활처리하는 함수를 실행시킨다(오류 있음)
    void player_resurrection()
    {

        m_player.ressurection();
    }
    //공격을 실행할때 반동으로 플레이어가 살짝씩 앞으로 나오는 것을 구현
    void hitted_push(float x)
    {
        //addforce.impulse를 이용해 플레이어가 순간적으로 짧은 거리로 조금씩 나온다(공격반동)
        var p = this.transform.parent.GetComponent<Rigidbody2D>();
       
        Vector3 v3 = new Vector3(x, 0, 0);

        p.AddForce(v3*m_player.direction, ForceMode2D.Impulse);
    }
    //velocity의 x값을 0으로 만들어 공격 반동을 끝내고 앞으로 나오지 못하게 한다
    void hitted_push_end()
    {
        var p = this.transform.parent.GetComponent<Rigidbody2D>();
        p.velocity = new Vector3(0, p.velocity.y);
    }
    //melee_on(1,2,3,air)
    //공격 에니메이션 실행 시 플레이어의 공격 판정(오브젝트)를 활성화 시켜 플레이어의 공격을 구현한다
    //한번도 플렝이어의 공격이 생성되지않았다면 생성 후 그 오브젝트를 받는다
    //그 후의 공격에서는 생성한 오브젝트를 다시 활성화 시킨다(풀링)
    //melee_off(1,2,3,air)
    //공격 에니메이션이 끝나면 공격반동을 없애고 공격 판정을 비활성화 시켜서 플레이어의 공격을 마무리 짓는다
    void Melee_1_on()
    {
            hitted_push(3f);//공격 반동으로 살짝 앞으로 나온다
        
        if (melee_1_instani == null)//생성된 적이 없다면
        {
           //생성 후 그 공격의 정보를 설정한다
            var a = Instantiate(melee_1, melee_1.transform.position, Quaternion.identity);
            index = a.GetComponent<melee_attack>();
            index.melee_force = melee_force*3;
            index.set_effect_rotate = true;
            index.effect_rotation = 0;
            a.transform.rotation = this.transform.parent.rotation;
            index.Damage = melee_dmg;
            //생성한 공격은 풀링 관리용 오브젝트에 자식으로 들어간다
            a.transform.SetParent(created_list);
            //플레이어가 공격한다는 게임 이벤트가 발생했음을 알린다
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);

            //공격을 활성화 하고 이단공격이 발생했다면 이단공격 연출을 실행한다
            a.SetActive(true);
            melee_1_instani = a;
            player_ghost_attack_1_effect();
        }
        else//생성한 적이 있다면 생성한 공격을 사용한다( 실행 내용은 생성하지 않는 점만 빼면 거의 같다)
        {
            //공격 정보 설정
            melee_1_instani.transform.position = melee_1.transform.position;
            melee_1_instani.transform.rotation = this.transform.parent.rotation;
            index = melee_1_instani.GetComponent<melee_attack>();
            index.melee_force = melee_force*3;
            index.on_crit = false;
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            //disable_hit을 통해 공격 판정을 다시 활성화 시킨다
            index.disable_hit = false;
            index.Damage = melee_dmg;
            //공격 활성화
            melee_1_instani.SetActive(true);
            player_ghost_attack_1_effect();
        }
     
    }
    public void Melee_1_off()
    {

        if (melee_1_instani!=null)
            melee_1_instani.SetActive(false);//공격을 비활성화 한다

        hitted_push_end();//공격 반동을 제거한다
    }


    //공중공격에 한정해서 반동이 존재하지 않는다
    void air_melee_on()
    {
       
        
        if (air_melee_instani == null)
        {
           
            var a = Instantiate(air_melee_, air_melee_.transform.position, Quaternion.identity);

            index = a.GetComponent<melee_attack>();
            index.melee_force = melee_force * 2;
            a.transform.rotation = this.transform.parent.rotation;
            index.Damage = melee_dmg;
            index.set_effect_rotate = true;
            index.effect_rotation = 0;
            a.transform.SetParent(created_list);
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            air_melee_instani = a;
   
            a.SetActive(true);

           
            
            player_air_ghost_attack_effect();
         
        }
        else
        {
           air_melee_instani.transform.position = air_melee_.transform.position;
            air_melee_instani.transform.rotation = this.transform.parent.rotation;
            index = air_melee_instani.GetComponent<melee_attack>();
            index.melee_force = melee_force*2;
            index.on_crit = false;
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            index.disable_hit = false;
            index.Damage = melee_dmg;
            air_melee_instani.SetActive(true);
            player_air_ghost_attack_effect();
        }
    }
    public void air_Melee_off()
    {
        if (air_melee_instani != null)
            air_melee_instani.SetActive(false);

      
    }
    void Melee_2_on()
    {

        if (m_player.on_rush)
        {
            hitted_push(2.5f);
            m_player.on_rush = false;
        }
        else
        {

            hitted_push(1f);
        }

       
        if (melee_2_instani == null)
        {
            
         
            var a = Instantiate(melee_2, melee_2.transform.position, Quaternion.identity);
            index = a.GetComponent<melee_attack>();
            index.melee_force = melee_force;
            index.Damage = melee_dmg;
            index.set_effect_rotate = true;
            a.transform.rotation = this.transform.parent.rotation;
            index.effect_rotation = -0.2f;
            a.transform.SetParent(created_list);
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            a.SetActive(true);
            melee_2_instani = a;
            // melee_2.SetActive(true);
            player_ghost_attack_2_effect();
        }
        else
        {
            melee_2_instani.transform.position = melee_2.transform.position;
            melee_2_instani.transform.rotation = this.transform.parent.rotation;
            index = melee_2_instani.GetComponent<melee_attack>();
            index.melee_force = melee_force;
            index.on_crit = false;
            index.Damage = melee_dmg;
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            index.disable_hit = false;
            
            melee_2_instani.SetActive(true);
            player_ghost_attack_2_effect();
        }
    }
    public void Melee_2_off()
    {
        if (melee_2_instani != null)
            melee_2_instani.SetActive(false);
        hitted_push_end();
    }
    void Melee_3_on()
    {
        hitted_push(1f);
        if (melee_3_instani == null)
        {
            
            var a = Instantiate(melee_3, melee_3.transform.position, Quaternion.identity);
            index = a.GetComponent<melee_attack>();
            index.melee_force = melee_force;
            index.Damage = melee_dmg;
            a.transform.rotation = this.transform.parent.rotation;
            index.set_effect_rotate = true;
            index.effect_rotation = 90;
            a.transform.SetParent(created_list);
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            a.SetActive(true);
            melee_3_instani = a;
            player_ghost_attack_3_effect();
        }
        else
        {
            melee_3_instani.transform.position = melee_3.transform.position;
            melee_3_instani.transform.rotation = this.transform.parent.rotation;
            index = melee_3_instani.GetComponent<melee_attack>();
            index.melee_force = melee_force;
            index.on_crit = false;
            index.Damage = melee_dmg;
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            index.disable_hit = false;
            melee_3_instani.SetActive(true);
            player_ghost_attack_3_effect();
        }
        // melee_3.SetActive(true);
    }
    public void Melee_3_off()
    {
        if (melee_3_instani != null)
            melee_3_instani.SetActive(false);
        hitted_push_end();
    }
    //플레이어의 점프 가능 횟수를 초기화한다
    void jump_return()
    {
        //player_status =플레이어의 정보값을 관리하는 클레스
        if (m_player.jump_count != Player_status.p_status.get_jump_count())
            m_player.jump_count = Player_status.p_status.get_jump_count();
    }





    //AE_XXXXX
    //에니메이션에 따라 사운드를 재생하고 이펙트를 생성시킨다
    //AE_XXXXX는 자신(김규태)이 코드를 만들지 않고 다른 사람이 만들었다.
    void AE_runStop()
    {
        m_audioManager.PlaySound("RunStop");
        float dustXOffset = 0.6f;
        float dustYOffset = 0.078125f;
        m_player.SpawnDustEffect(m_RunStopDust, dustXOffset, dustYOffset);
    }

    void AE_footstep()
    {
        jump_return();
        m_audioManager.PlaySound("Footstep");
    }

    void AE_Jump()
    {
        m_audioManager.PlaySound("Jump");

      
        
            float dustYOffset = 0.078125f;
            m_player.SpawnDustEffect(m_JumpDust, 0.0f, dustYOffset);
        
        
    }
   
    void AE_Landing()
    {
        if (m_player.landing_chk)
        {
            jump_return();
            p_anim.ground_anim_chk = false;
            if (p_anim.ground_anim_chk)
            {
                m_audioManager.PlaySound("Landing");
                float dustYOffset = 0.078125f;
                m_player.SpawnDustEffect(m_LandingDust, 0.0f, dustYOffset);
                p_anim.ground_anim_chk = false;
            }
        }
    }

 

    void AE_AttackAirLanding()
    {
        m_audioManager.PlaySound("AirSlamLanding");
        float dustYOffset = 0.078125f;
        m_player.SpawnDustEffect(m_AirSlamDust, 0.0f, dustYOffset);

    }

    void AE_Hurt()
    {
        m_audioManager.PlaySound("Hurt");
        p_anim.Hit_state = false;
    }

    void AE_Death()
    {
        m_audioManager.PlaySound("Death");
    }

    void AE_SwordAttack()
    {
        m_audioManager.PlaySound("SwordAttack");
    }

    void AE_SheathSword()
    {
        m_audioManager.PlaySound("SheathSword");
    }

  
}
