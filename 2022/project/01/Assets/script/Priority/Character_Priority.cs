using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Priority:MonoBehaviour
{
    List<GameCharacter> P;
    List<GameCharacter> E;
    List<GameCharacter> Target;
    protected List<Priority_Member> gc;
   
    public GameCharacter get_character(int n = 0)
    {
        return gc[n].Character;
    }
    public Character_Priority(){
        if (Stage._stage != null)
        {
            P = Stage._stage.Player_create;
            E = Stage._stage.Enemy_create;
        }
        gc = new List<Priority_Member>();
    
    }
   
    public GameCharacter get_enemy_by_distance(Team t, int n = 0)
    {
       
        initialize_List(t);
        if (Target.Count == 0)
            return null;
        for (int i = 0; i < Target.Count; i++)
        {
            Distance_Member memeber = new Distance_Member(Target[i]);
            gc.Add(memeber);
        }
        gc.Sort();
        
        if (gc.Count < n&&gc.Count-1!>=0)
        {
            return gc[gc.Count-1].Character;
        }else if (gc.Count - 1! < 0)
        {
            return null;
        }
        else
        {
            return gc[n].Character;
        }
     
    }
   
    void initialize_List(Team t)
    {
        for(int i = 0; i < gc.Count; i++)
        {
            gc.RemoveAt(0);
        }
        if (t == Team.Player)
            Target = E;
        else
            Target = P;
    }
   
}
