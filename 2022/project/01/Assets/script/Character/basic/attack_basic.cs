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

    public virtual void Active(int DMG,List<GameCharacter> l=null)
    {
        _Target_list = l;

            attack_by_type(DMG,_Target_list);

       
    }

    virtual protected void attack_by_type(int ATK, List<GameCharacter>  obj = null)
    {
        _Target_list.Clear();
    }

   
}
