using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iAct
{
    void Active(int ATK, List<GameCharacter> chr = null);

    float init_delay();
    


}
