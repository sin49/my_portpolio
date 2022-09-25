using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : GameCharacter
{
    public Player1()
    {
      
        if(status==null)
   status = new Player1_status();
        if (attack == null)
    attack = new Player1_ai();
}
   
}
