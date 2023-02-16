using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBurst : attack_basic
{
    public float LB_animation_time;
    
   virtual public bool set_LB()
    {
        return false;
    }
    public override void Active(GameCharacter chr = null)
    {
       
        on_LB_situaltion(chr);
        base.Active(chr);
        off_LB_situaltion(chr);
     

    }
    //(공격을 주는 필살기의 대상과 주체는 idle로 에니메이션 강제 변경시키기)
    public virtual void on_LB_situaltion(GameCharacter c)
    {

        Time.timeScale = 0;
        Make_time_goes_on_LB(c);
        Make_time_goes_on_LB(_target);
    }
    void Make_time_goes_on_LB(GameCharacter c)
    {
        c.C_ani.change_animator_mode_unscaledTime();//에니메이션이 시간이 멈춰도 재생되게 함
        c.attack.stop_action(c);
        //시간이 멈춰도 행동되게 하고싶으면 Time.unscaleddeltaTime을 사용
    }
    void initalize_time_goes(GameCharacter c)
    {
        c.C_ani.change_animator_mode_normal();

    }
    public virtual void off_LB_situaltion(GameCharacter c)
    {
        
        Time.timeScale = 1;
        initalize_time_goes(c);
        initalize_time_goes(_target);


    }
}
