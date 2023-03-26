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


    //필요한거 animator받기
    //핸들러에 에니메이션 실행 기능 추가하기

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
    //에니메이션 속도 조절
    public void set_animation_speed(float s)
    {
        ani.speed = s;
    }
    //현재 에니메이션을 멈춘다
    public void stop_current_animation()
    {
      
        string s = ani.GetCurrentAnimatorStateInfo(0).ToString();
        float t = ani.GetCurrentAnimatorStateInfo(0).normalizedTime;
        ani_buffer.animation_name=s;
        ani_buffer.animation_length = t;
        reset_parameter(s);
        ani.Play("idle",-1,0);
    }
    //멈췄던 에니메이션을 다시 재생한다.
    public void resume_animation()
    {
        if (ani_buffer.animation_name == null)
            return;

        ani.Play(ani_buffer.animation_name, -1, ani_buffer.animation_length);
        ani_buffer.animation_name =null;
    }
    //특정 파라미터를 리셋한다
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
