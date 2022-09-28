using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iAct
{
    GameCharacter set_target(Team t,Character_Priority priority=null);
    bool Active(GameCharacter chr = null, GameCharacter target=null);

    float init_delay();



}
