using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Character_Animation
{
    public Character_Animation(GameCharacter gc)
    {
       ani = gc.GetComponent<Animator>();
        if (gc.this_ani_controller != null)
            ani.runtimeAnimatorController = gc.this_ani_controller;
    }
    Animator ani;
    
    struct animation_buffer{
      
       public string animation_name;
        public float animation_length;
        }
    animation_buffer ani_buffer;


    //�ʿ��Ѱ� animator�ޱ�
    //�ڵ鷯�� ���ϸ��̼� ���� ��� �߰��ϱ�

   public void initialize_animation()
    {
        ani_buffer = new animation_buffer();
        reset_all_parameter();
        ani.Play("idle", -1, 0);

    }
    public void active_stun_timer_animator(float f)
    {
        ani.SetFloat("Stun", f);
    }
   public void active_death_animation()
    {
        ani.SetBool("Die", true);
    }
    //���ϸ��̼� �ӵ� ����
    public void set_animation_speed(float s)
    {
        ani.speed = s;
    }
    //���� ���ϸ��̼��� �����
    public void stop_current_animation()
    {
      
        string s = ani.GetCurrentAnimatorStateInfo(0).ToString();
        float t = ani.GetCurrentAnimatorStateInfo(0).normalizedTime;
        ani_buffer.animation_name=s;
        ani_buffer.animation_length = t;
        reset_parameter(s);
        ani.Play("idle",-1,0);
    }
    //����� ���ϸ��̼��� �ٽ� ����Ѵ�.
    public void resume_animation()
    {
        if (ani_buffer.animation_name == null)
            return;

        ani.Play(ani_buffer.animation_name, -1, ani_buffer.animation_length);
        ani_buffer.animation_name =null;
    }
    //Ư�� �Ķ���͸� �����Ѵ�
    void reset_parameter(string s)
    {
        switch (s)
        {
            case "Stun":
                ani.SetFloat("Stun", 0);
                break;
            case "Die":
                ani.SetBool("Die", false);
                break;
            default:
                break;
        }
    }
    void reset_all_parameter()
    {
        ani.SetFloat("Stun", 0);
        ani.SetBool("Die", false);
    }
    public void active_action_animation(Character_action s)
    {
        ani.Play(s.action_name, -1, 0);
    }
 
}
