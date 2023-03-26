using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Priority
{
    public static Character_Priority instance;
    List<GameCharacter> P;
    List<GameCharacter> E;
    List<GameCharacter> Target;
    protected List<Priority_Member> gc;
   
    public GameCharacter get_character(int n = 0)
    {
        return gc[n].Character;
    }
    public Character_Priority(){
        if (instance == null)
            instance = this;
        if (Stage._stage != null)
        {
            P = Stage._stage.Player_create;
            E = Stage._stage.Enemy_create;
        }
        gc = new List<Priority_Member>();
    
    }
   
   
    public GameCharacter get_enemy_by_distance(Team t,Vector3 pos, int n = 0)
    {

        initialize_List(t);
        if (Target.Count == 0)
            return null;
        foreach(GameCharacter c in Target)
        {
            if (c.object_activasion)
            {
                Distance_Member memeber = new Distance_Member(c, pos);
                gc.Add(memeber);
            }
        }
        gc.Sort();

        if (gc.Count < n && gc.Count - 1! >= 0)
        {
            return gc[gc.Count - 1].Character;
        }
        else if (gc.Count - 1! < 0)
        {
            return null;
        }
        else
        {
            return gc[n].Character;
        }

    }
    public List<GameCharacter> get_enemy_by_distance(Team t, Vector3 pos, List<int> n)
    {

        initialize_List(t);
        if (Target.Count == 0)
            return null;
        for (int i = 0; i < Target.Count; i++)
        {
            Distance_Member memeber = new Distance_Member(Target[i], pos);
            gc.Add(memeber);
        }
        gc.Sort();
        if (gc.Count == 0)
            return null;
        List<GameCharacter> target_list = new List<GameCharacter>();
        foreach(int index in n)
        {
            if (gc.Count <= index)
                target_list.Add(gc[gc.Count - 1].Character);
            else if (index < 0)
                target_list.Add(gc[0].Character);
            else
                target_list.Add(gc[index].Character);
        }
        return target_list;

    }
    void initialize_List(Team t)
    {
        gc.Clear();
        if (t == Team.Player)
            Target = E;
        else
            Target = P;
    }
   
}
