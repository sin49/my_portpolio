using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_basic : MonoBehaviour, iAct
{
    float _init_delay = 0.3f;
    public float init_delay()
    {
        return _init_delay;
    }
    bool action_completed_check;
    int action_num;
   
    public bool Active(GameCharacter chr = null, GameCharacter target = null)
    {

        if (target != null)
            attack_by_type(chr.status.ATK, target);

        return action_completed_check;
    }
   
 
    virtual public GameCharacter set_target(Team t,Character_Priority priority = null)
    {
        return priority.get_enemy_by_distance(t,0);
    }

    virtual protected void attack_by_type(int ATK, GameCharacter obj = null)
    {
        action_completed_check = true;
    }


  
}
