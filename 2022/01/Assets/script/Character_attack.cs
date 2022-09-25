using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Character_attack : MonoBehaviour
{
    protected int[] action_pattern;
    int pattern_num;
    protected List<iAct> action=new List<iAct>();
    
    Character_Priority c_priority=new Character_Priority();

    float action_delay;

    public GameCharacter target;
    GameCharacter _target;

    Team _t;

    bool action_completed_check;
    public Team T { get { return _t; } set { _t = value; } }
    

    public void attack_enemy(GameCharacter c)
    {

        if (action_delay <= 0)
        {
            Act(c);
            
        }
        else
            action_delay -= Time.deltaTime;
    }


    
    



    void Act(GameCharacter c)
    {
       
        if (action.Count == 0)
            return;
        int a= pattern_num >= action.Count ? 0 : pattern_num;
        _target = action[action_pattern[a]].set_target(T, c_priority);
        if (_target != target)
        {
            target = _target;
            return;
        }
        if (!on_acition)
            StartCoroutine(Act(c, target,action_pattern[a]));
        if (action_completed_check)
        {
            pattern_num++;
            if (pattern_num > action_pattern.Length)
                pattern_num = 0;
            action_completed_check = false;
        }
        
        action_delay = 1.5f;

    }
    bool on_acition;
    IEnumerator Act(GameCharacter chr = null, GameCharacter target = null, int action_num = 0)
    {
      
        act_animation(chr.C_ani);
        on_acition = true;
        yield return new WaitForSeconds(action[action_num].init_delay());
        action[action_num].Active(chr, target);
        action_completed_check = true;
        on_acition = false;
    }
    public void stop_action()
    {
        on_acition = false;
        StopAllCoroutines();
    }

    void act_animation(Character_Animation c = null, int action_num = 0)
    {
        if (c == null)
            return;

        c.action_animation(action_num);
    }

}
