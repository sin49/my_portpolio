using System.Collections.Generic;
using UnityEngine;


public enum class_type { Attacker, Defenser, Supporter, Healer }
public enum attack_type { Melee, range };
public enum pattern { attack = 0, skill1 = 1, skill2 = 2, Burst = 3 };
public enum Team { Player, Enemy };
public class GameCharacter : MonoBehaviour
{

    public int ID;


    Rigidbody rgd;

    


    public GameCharacter target;
    Vector3 target_direction;
    float target_distance;


    public Character_status status;
    public Character_attack attack;
    public Character_Animation C_ani;

    public Team T { get { return attack.T; } set {  attack.T=value; } }

   void chase_enemy()
    {
        
      
        rgd.velocity = target_direction * 10 * status.movement_speed;
    }

 
    private void Awake()
    {
        
        rgd = GetComponent<Rigidbody>();
        C_ani = this.GetComponent<Character_Animation>();
    }
  
    // Update is called once per frame
    void FixedUpdate()
    {
        target = attack.target;
        target_direction = target==null?transform.position:get_target_direction();
        target_distance = target == null ? 0 : (target.transform.position - transform.position).magnitude;

        if (target_distance > status.Distance_number)
        {
        
            chase_enemy();
        }
        else
        {
            rgd.velocity = rgd.velocity * 0.01f;
          attack.attack_enemy(this);
        }
    }

    public void get_damage(int a)
    {
        
        status.current_HP -= a;
        if (status.current_HP < 0)
            status.current_HP = 0;
        Stage._stage.creaate_damagee_font(a, this.transform);
        if (C_ani == null)
            return;
        if (status.current_HP < 0)
            C_ani.change_hitted_event();
        C_ani.hitted_animation();
    }

    Vector3 get_target_direction()
    {
      Vector3 v =(target.transform.position - transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(v);
        transform.rotation = q;
        return v;
    }




}

