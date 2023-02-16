using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Character_Animation : MonoBehaviour
{
    Animator ani;
    public delegate void active_animation();
    public delegate bool _action_animation(int n);
    public active_animation hitted_animation;
    public active_animation die_animation;
    ChracterShader Shader;
    public _action_animation action_animation;

    float hitted_animation_time=0.2f;
    GameCharacter this_chr;
    private void Awake()
    {
        this_chr = this.GetComponent<GameCharacter>();
        Shader = this.GetComponent<ChracterShader>();
        Shader.deadbody_duration = this_chr.deadbody_duration;
        ani = this.GetComponent<Animator>();
       
        hitted_animation += Shader.change_hitted_emission;
  
        //hitted_animation += active_hitted_animation;
        die_animation += Shader.active_death_Shader;
        die_animation += active_death_animation;
        action_animation += active_action_animation;
        
    }
    public void change_animator_mode_unscaledTime()
    {
        ani.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    public void change_animator_mode_normal()
    {
        ani.updateMode = AnimatorUpdateMode.Normal;
    }

    void FixedUpdate()
    {
        
        if (this_chr.T == Team.Enemy)
            Shader.use_enemy_emission();
        else
            Shader.use_Player_emission();
    }
    
   public void initialize_animation()
    {
        if(Shader!=null)
        Shader.initialize_shader();
    }
    void active_death_animation()
    {
        ani.SetBool("Die", true);
    }
    public void set__animation_speed(float a)
    {
        ani.speed = a;
    }
    public void stop_animation(int n)
    {
        ani.SetTrigger("Stop");
        ani.ResetTrigger("action" + n);
    }
    public void resumre_animation()
    {
        ani.ResetTrigger("Stop");
    }
    bool active_action_animation(int n)
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            
            ani.SetTrigger("action" + n);
            return true;
        }
        else
            return false;
    }
 
}
