using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_ai : Character_attack
{
    Melee_attack_normal m;
    public Player1_ai()
    {
        action_pattern = new int[] { 0 };
        m = new Melee_attack_normal();
        action.Add(m);
    }
   
}
