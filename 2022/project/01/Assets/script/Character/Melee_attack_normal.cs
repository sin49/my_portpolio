using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_attack_normal : attack_basic
{
    
   public Melee_attack_normal()
    {
       
    }
    protected override void attack_by_type( int ATK, List<GameCharacter> obj_list)
    {
        obj_list[0].hitted(ATK);
        base.attack_by_type(ATK, obj_list);
    }
}
