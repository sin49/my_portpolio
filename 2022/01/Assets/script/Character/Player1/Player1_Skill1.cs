using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_Skill1 : attack_basic
{
    public Player1_Skill1()
    {
        _init_delay = 0.2f;
    }
    protected override void attack_by_type(int ATK, GameCharacter obj = null)
    {
       
        obj.hitted(ATK * 2);
        obj.get_forced(4, 1.25f);
    }

}
