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
    //(������ �ִ� �ʻ���� ���� ��ü�� idle�� ���ϸ��̼� ���� �����Ű��)
    public virtual void on_LB_situaltion(GameCharacter c)
    {

        Time.timeScale = 0;
        Make_time_goes_on_LB(c);
        Make_time_goes_on_LB(_target);
    }
    void Make_time_goes_on_LB(GameCharacter c)
    {
        c.C_ani.change_animator_mode_unscaledTime();//���ϸ��̼��� �ð��� ���絵 ����ǰ� ��
        c.attack.stop_action(c);
        //�ð��� ���絵 �ൿ�ǰ� �ϰ������ Time.unscaleddeltaTime�� ���
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
