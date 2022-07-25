using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_animator : MonoBehaviour//플레이어의 에니메이션을 관리하고 실행하는 함수
{
    
    private Animator m_animator;
    public int anim_state;
    private int m_currentAttack = 0;
    public int m_currentAttack_2 = 0;
    public Rigidbody2D rgd;
    public bool death_state;
    public bool Hit_state;
    public bool jump_state;
    public bool attack_state;
    public bool crouch_state;
    public bool move_state;
    public bool ground_anim_chk;
    public bool m_crouch;
    public bool death_chk;
    public bool sword_delay;
    Material m;

    public void resurrection()//부활 에니메이션 실행
    {
        m_animator.SetTrigger("resurrection");
    }
    public void dash()//대쉬 에니메이션 실행
    {
        m_animator.SetBool("mustbeing", true);//다른 에니메이션을 무시하고 강제 실행
        m_animator.SetBool("dash", true);
        melee_initialize();//근접 콤보 초기화
    }
    public void dash_end()//대쉬 종료
    {
        m_animator.SetBool("mustbeing", false);
        m_animator.SetBool("dash", false);
    }
    void Start()
    {
       
        m_animator = GetComponent<Animator>();
        rgd = GetComponentInParent<Rigidbody2D>();
    }
    private void Update()
    {


        if (rgd.velocity.y < -9)//착지시 흙먼지 발생 여부(일정 높이일 때 발새)
        {
            ground_anim_chk = true;
        }
        set_airspeed();
        m_animator.SetInteger("HP", Player_status.p_status.get_hp());
        m_animator.SetFloat("move_speed", Player_status.p_status.get_speed() * 0.1f);
        m_animator.SetFloat("Attackspeed", Player_status.p_status.get_firedelay());

        //플레이어의 에니메이션의 우선 순위를 결정한다(죽음>피격>이동>idle)
        if (death_state)
        {
            death_anim();
        }else if (Hit_state)
        {
            Hit_anim();
        }
        else if (move_state)
        {
            move_anim();
        }
        else
        {
            idle_anim();
        }
    }
    public void melee_initialize()//근접 3타 콤보를 멈추고 초기화 한다
    {
        this.GetComponent<Player_anim_event>().Melee_1_off();
        this.GetComponent<Player_anim_event>().Melee_2_off();
        this.GetComponent<Player_anim_event>().Melee_3_off();
        this.GetComponent<Player_anim_event>().air_Melee_off();
    }
    public void hitted_intialize(){//피격 에니메이션을 마무라한다

        melee_initialize();
        this.transform.parent.GetComponent<PlayerCharacter>().can_move = true;
        sword_delay = false;
        m_currentAttack_2 = 0;
    }
    public void mustbeing_false()//set
    {
        m_animator.SetBool("mustbeing", false);
    }
  public void sword_delay_on()//근접공격의 딜레이를 준다
    {
        this.transform.parent.GetComponent<PlayerCharacter>().can_move = false;
       
       sword_delay = true;
       // rgd.velocity = Vector3.zero;
    }
    public void sword_delay_off()//근접 공격의 딜레이를 끝낸다
    {
       
        m_currentAttack_2++;
        if (m_currentAttack_2 > 2)
            m_currentAttack_2 = 0;
        sword_delay = false;
    }
    public void idle_anim()//idle 애니
    {
        m_animator.SetInteger("AnimState", 0);

    }
    public void move_anim()//이동 애니
    {
        m_animator.SetInteger("AnimState", 1);

    }
    public void jump_anim()//점프 애니
    {
        m_animator.SetTrigger("Jump");
        m_animator.SetBool("Grounded", false);
    }
    public void sword_attack_anim()//근접 공격 애니
    {
        if (!sword_delay)
        {

           
            
           
            m_animator.SetInteger("sword_count", m_currentAttack_2);
            m_animator.SetTrigger("swordattack" + 1);
        }



    }
    public void sword_anim_start()//근접 공격 에니메이션 진행 중 변경된 설정을 초기화 한다(콤보 도중 끊기는 경우에 대응)
    { this.transform.parent.GetComponent<PlayerCharacter>().can_move = true;//이동을 조작 가능하게
        m_animator.SetInteger("sword_count", 0);//몇번째 타격 에니메이션인지를 초기화
        m_currentAttack_2 = 0;
        sword_delay = false;//공격 딜레이
    }
  
    public void set_ground(bool a)//플레이어가 땅에 닿았음을 에니메이터에게 알린다
    {
        if (!a)//공중에 있을 때
        {
            if(rgd.velocity.y > 3 || rgd.velocity.y < -3)
                m_animator.SetBool("Grounded", a);
        }
        else//땅에 있는 상태일 때
        {
            m_animator.SetBool("Grounded", a);
        }
        //점프를 시작할 때와 공중에 있을 때를 구분짓기
    }

    public void set_airspeed()//추락 속도를 받는다(빠르게 추락한다->높은 곳에서 떨어진다->흙먼지를 일으킬지 판단한다)
    {
        m_animator.SetFloat("AirSpeedY", rgd.velocity.y);
    }
    public void death_anim()//사망에니메이션을 진행한다
    {
        m_animator.SetBool("noBlood", true);//에셋 관련
        m_animator.SetTrigger("Death");
        death_state = false;
    }
    public void Hit_anim()//피격 에니메이션을 진행한다
    {
        m_animator.SetBool("mustbeing", true);//현재 행동을 취소하고 강제 진행
        m_animator.SetTrigger("Hurt");
       
    }
  
    public void air_attack_anim()//공중 공격 에니메이션을 진행한다(오른쪽 방향)
    {
    


           
                m_animator.SetTrigger("AirAttack");
           
     


    }
    public void air_attack_anim_mirror()//공중 공격 에니메이션을 진행한다(왼쪽 방향)
    {
      
        

            m_animator.SetTrigger("AirAttack_mirror");
          
        

    }
    public void attack_anim()//공격 에니메이션을 진행한다(오른쪽 방향)
    {
        
        {
            m_currentAttack++;//현재 진행해야할 타수 에니메이션

   
            if (m_currentAttack > 2)//3타 콤보로 이루어짐
                m_currentAttack = 1;
            m_animator.SetTrigger("Attack" + m_currentAttack);//그 타수에 맞는 에니메이션을 진행한다
        }
    }
    public void attack_anim_mirror()//공격 에니메이션을 진행한다(왼쪽 방향)
    {
        m_currentAttack++;

        // Loop back to one after second attack
        if (m_currentAttack > 2)
            m_currentAttack = 1;
        m_animator.SetTrigger("Attack" + m_currentAttack+"_mirror");
    }
}
