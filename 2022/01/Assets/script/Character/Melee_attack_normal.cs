using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_attack_normal : attack_basic
{
    /*public override GameCharacter set_target(Character_Priority priority = null)
    {
        return priority.get_enemy_by_distance(0);
    }*/
  
   
    protected override void attack_by_type( int damage, GameCharacter obj)
    {
     
      if(obj!=null)
           obj.hitted(damage);
    }
}
