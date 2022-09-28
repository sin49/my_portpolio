using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_ai : Character_attack
{
    Melee_attack_normal m= new Melee_attack_normal();
    Player1_Skill1 S1 = new Player1_Skill1();
    public Player1_ai()
    {
        action_pattern = new int[] { 0,1,0 };
        action.Add(m);
    
        action.Add(S1);
    }
   
}
