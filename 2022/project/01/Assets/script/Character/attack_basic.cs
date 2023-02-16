using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_basic : MonoBehaviour, iAct
{
    protected float _init_delay = 0.1f;
    public float init_delay()
    {
        return _init_delay;
    }
   protected GameCharacter _target;
    //chr=본인 ,_target=대상
    protected List<GameCharacter> _Target_list = new List<GameCharacter>();

    public virtual void Active(GameCharacter chr = null)
    {

        set_target(chr.attack);
            attack_by_type(chr.status.ATK,_Target_list);

       
    }

    virtual protected void set_target(Character_attack priority = null)
    {
        _target= priority.get_enemy_by_distance(priority.T,0);
        _Target_list.Add(_target);
    }

    virtual protected void attack_by_type(int ATK, List<GameCharacter>  obj = null)
    {
        _Target_list = new List<GameCharacter>();
    }


  
}
