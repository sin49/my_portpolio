using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_status :MonoBehaviour
{
    int ID;

    string Name;

    public attack_type A_type { get; set; }

    float _range;

    public float Distance_number { get { return _range; } set { _range = value; } }

    int LV;

    //status
    int _HP;
    public int HP { get { return _HP; } set { _HP = value; } }
    public int current_HP { get; set; }
    int _ATK;
    public int ATK { get { return _ATK; } set { _ATK = value; } }
    float _movement_speed;
    public float movement_speed { get { return _movement_speed; } set { _movement_speed = value; } }
    float _attack_speed;
    public float attack_speed { get { return _attack_speed; } set { _attack_speed = value; } }
    float _Burst_gauge_raise;
    public float Burst_gauge_raise { get { return _Burst_gauge_raise; } set { _Burst_gauge_raise = value; } }




    Dictionary<pattern, int> _skill_lv = new Dictionary<pattern, int>();

    public int skill_lv(pattern p)
    {
        return _skill_lv[p];
    }
    public Character_status()
    {
        movement_speed = 1;
        attack_speed = 1;
       

    }

    public Character_status(int ID)
    {
        
        this.ID = ID;
        Distance_number = 5;//debug
        HP = 10;
        ATK = 1;
        movement_speed=1;
    }
    Character_status(int ID, int hp, int atk)
    {
        this.ID = ID;
        set_status(hp, atk);

    }
    public void set_status(int hp, int atk)
    {
        HP = hp;
        ATK = atk;
   
    }

    private void Awake()
    {
        current_HP = HP;
    }

}

