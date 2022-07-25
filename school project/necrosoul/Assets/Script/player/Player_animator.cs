using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_animator : MonoBehaviour//�÷��̾��� ���ϸ��̼��� �����ϰ� �����ϴ� �Լ�
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

    public void resurrection()//��Ȱ ���ϸ��̼� ����
    {
        m_animator.SetTrigger("resurrection");
    }
    public void dash()//�뽬 ���ϸ��̼� ����
    {
        m_animator.SetBool("mustbeing", true);//�ٸ� ���ϸ��̼��� �����ϰ� ���� ����
        m_animator.SetBool("dash", true);
        melee_initialize();//���� �޺� �ʱ�ȭ
    }
    public void dash_end()//�뽬 ����
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


        if (rgd.velocity.y < -9)//������ ����� �߻� ����(���� ������ �� �߻�)
        {
            ground_anim_chk = true;
        }
        set_airspeed();
        m_animator.SetInteger("HP", Player_status.p_status.get_hp());
        m_animator.SetFloat("move_speed", Player_status.p_status.get_speed() * 0.1f);
        m_animator.SetFloat("Attackspeed", Player_status.p_status.get_firedelay());

        //�÷��̾��� ���ϸ��̼��� �켱 ������ �����Ѵ�(����>�ǰ�>�̵�>idle)
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
    public void melee_initialize()//���� 3Ÿ �޺��� ���߰� �ʱ�ȭ �Ѵ�
    {
        this.GetComponent<Player_anim_event>().Melee_1_off();
        this.GetComponent<Player_anim_event>().Melee_2_off();
        this.GetComponent<Player_anim_event>().Melee_3_off();
        this.GetComponent<Player_anim_event>().air_Melee_off();
    }
    public void hitted_intialize(){//�ǰ� ���ϸ��̼��� �������Ѵ�

        melee_initialize();
        this.transform.parent.GetComponent<PlayerCharacter>().can_move = true;
        sword_delay = false;
        m_currentAttack_2 = 0;
    }
    public void mustbeing_false()//set
    {
        m_animator.SetBool("mustbeing", false);
    }
  public void sword_delay_on()//���������� �����̸� �ش�
    {
        this.transform.parent.GetComponent<PlayerCharacter>().can_move = false;
       
       sword_delay = true;
       // rgd.velocity = Vector3.zero;
    }
    public void sword_delay_off()//���� ������ �����̸� ������
    {
       
        m_currentAttack_2++;
        if (m_currentAttack_2 > 2)
            m_currentAttack_2 = 0;
        sword_delay = false;
    }
    public void idle_anim()//idle �ִ�
    {
        m_animator.SetInteger("AnimState", 0);

    }
    public void move_anim()//�̵� �ִ�
    {
        m_animator.SetInteger("AnimState", 1);

    }
    public void jump_anim()//���� �ִ�
    {
        m_animator.SetTrigger("Jump");
        m_animator.SetBool("Grounded", false);
    }
    public void sword_attack_anim()//���� ���� �ִ�
    {
        if (!sword_delay)
        {

           
            
           
            m_animator.SetInteger("sword_count", m_currentAttack_2);
            m_animator.SetTrigger("swordattack" + 1);
        }



    }
    public void sword_anim_start()//���� ���� ���ϸ��̼� ���� �� ����� ������ �ʱ�ȭ �Ѵ�(�޺� ���� ����� ��쿡 ����)
    { this.transform.parent.GetComponent<PlayerCharacter>().can_move = true;//�̵��� ���� �����ϰ�
        m_animator.SetInteger("sword_count", 0);//���° Ÿ�� ���ϸ��̼������� �ʱ�ȭ
        m_currentAttack_2 = 0;
        sword_delay = false;//���� ������
    }
  
    public void set_ground(bool a)//�÷��̾ ���� ������� ���ϸ����Ϳ��� �˸���
    {
        if (!a)//���߿� ���� ��
        {
            if(rgd.velocity.y > 3 || rgd.velocity.y < -3)
                m_animator.SetBool("Grounded", a);
        }
        else//���� �ִ� ������ ��
        {
            m_animator.SetBool("Grounded", a);
        }
        //������ ������ ���� ���߿� ���� ���� ��������
    }

    public void set_airspeed()//�߶� �ӵ��� �޴´�(������ �߶��Ѵ�->���� ������ ��������->������� ����ų�� �Ǵ��Ѵ�)
    {
        m_animator.SetFloat("AirSpeedY", rgd.velocity.y);
    }
    public void death_anim()//������ϸ��̼��� �����Ѵ�
    {
        m_animator.SetBool("noBlood", true);//���� ����
        m_animator.SetTrigger("Death");
        death_state = false;
    }
    public void Hit_anim()//�ǰ� ���ϸ��̼��� �����Ѵ�
    {
        m_animator.SetBool("mustbeing", true);//���� �ൿ�� ����ϰ� ���� ����
        m_animator.SetTrigger("Hurt");
       
    }
  
    public void air_attack_anim()//���� ���� ���ϸ��̼��� �����Ѵ�(������ ����)
    {
    


           
                m_animator.SetTrigger("AirAttack");
           
     


    }
    public void air_attack_anim_mirror()//���� ���� ���ϸ��̼��� �����Ѵ�(���� ����)
    {
      
        

            m_animator.SetTrigger("AirAttack_mirror");
          
        

    }
    public void attack_anim()//���� ���ϸ��̼��� �����Ѵ�(������ ����)
    {
        
        {
            m_currentAttack++;//���� �����ؾ��� Ÿ�� ���ϸ��̼�

   
            if (m_currentAttack > 2)//3Ÿ �޺��� �̷����
                m_currentAttack = 1;
            m_animator.SetTrigger("Attack" + m_currentAttack);//�� Ÿ���� �´� ���ϸ��̼��� �����Ѵ�
        }
    }
    public void attack_anim_mirror()//���� ���ϸ��̼��� �����Ѵ�(���� ����)
    {
        m_currentAttack++;

        // Loop back to one after second attack
        if (m_currentAttack > 2)
            m_currentAttack = 1;
        m_animator.SetTrigger("Attack" + m_currentAttack+"_mirror");
    }
}
