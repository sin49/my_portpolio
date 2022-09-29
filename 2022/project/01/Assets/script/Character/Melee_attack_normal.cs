using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_attack_normal : attack_basic
{
    
   
    protected override void attack_by_type( int damage, GameCharacter obj)
    {
     
      if(obj!=null)
           obj.hitted(damage);
    }
}
