using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Character_attack : Character_Priority
{
    protected int[] action_pattern;
    int pattern_num;
    protected List<iAct> action=new List<iAct>();

    LimitBurst limitburst;
    bool on_LB;
    [HideInInspector]
    public bool is_LB_can_use_anywhere;

    float action_delay=1f;
    float delay_time;

    public GameCharacter target;
    GameCharacter _target;
    Team _t;

    bool on_corutine;
    public Team T { get { return _t; } set { _t = value; } }

   Coroutine act_corutine;

    float corutine_timer;
    private void Start()
    {
        if (limitburst != null)
            is_LB_can_use_anywhere = limitburst.set_LB();
        _target= get_enemy_by_distance(T, 0);
    }
    public void attack_enemy(GameCharacter c)
    {
        if (on_corutine)
        {
            corutine_timer += Time.deltaTime;
        }
         else if (delay_time <= 0)
        {
            Act(c);
            
        }
        else
            delay_time -=Time.deltaTime;
    }

    void Act(GameCharacter c)
    {
        delay_time =action_delay;
        
        if (action.Count == 0)
            return;
      
        _target = get_enemy_by_distance(T, 0);
        if (_target != target)
        {
            target = _target;
           
            return;
        }
        if (target == null)
            return;
        int a = pattern_num >= action.Count ? 0 : pattern_num;
        act_corutine = StartCoroutine(Act(c,  action_pattern[a]));
            
       
      
        
     

    }
  public bool Active_LB(GameCharacter c)
    {

        if (limitburst != null)
        {
            stop_action(c);
            StartCoroutine(Act_LB(c));
            return true;
        }
        else
            return false;
    }
    IEnumerator Act(GameCharacter chr = null, int action_num = 0)
    {
        if (!act_animation(chr.C_ani, action_num))
            stop_action(chr);
        on_corutine = true;
        yield return new WaitUntil(()=>corutine_timer>= action[action_num].init_delay());
        corutine_timer = 0;
        action[action_num].Active(chr);
        pattern_num++;
        if (pattern_num > action_pattern.Length)
            pattern_num = 0;
        chr.gain_LBgauge();
        on_corutine = false;
    }
    IEnumerator Act_LB(GameCharacter c)
    {
        on_corutine = true;
        yield return new WaitForSeconds(limitburst.init_delay());
        limitburst.Active(c);
        on_corutine = false;
        delay_time = action_delay;
        c.End_LB();
    }
    public void stop_action(GameCharacter c)
    {
        StopCoroutine(act_corutine);
    
        if(pattern_num<action_pattern.Length)
            c.C_ani.stop_animation(action_pattern[pattern_num]);
    }

    bool act_animation(Character_Animation c = null, int action_num = 0)
    {
        if (c == null)
            return true;
        
        return c.action_animation(action_num);
    }
    public void initalize()
    {
        pattern_num = 0;
        on_corutine = false;
        action_delay = 0;
  
    }
}
