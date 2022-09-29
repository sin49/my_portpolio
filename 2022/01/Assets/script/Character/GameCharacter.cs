
using System.Collections.Generic;
using UnityEngine;


public enum attack_type { Melee, range };

public class GameCharacter : MonoBehaviour, Character
{

    public int ID;
    public int index;

    Rigidbody rgd;

    Stage s;

    List<Character_observer> MyObserver = new List<Character_observer>();
    public GameCharacter target;

    Vector3 target_direction;
    
    float target_distance;
    int _current_hp;
    public int current_hp { get { return _current_hp; }set { _current_hp = value; } }

    public Character_status status;
    public Character_attack attack;
    public Character_Animation C_ani;
    Quaternion q;
    public Team T { get { return attack.T; } set {  attack.T=value; } }

   float _deadbody_duration = 3f;

    public float deadbody_duration { get { return _deadbody_duration; } set { _deadbody_duration = value; } }

   

    bool forced;
    float forced_timer;

    void chase_enemy()
    {
        if (target == null)
            return;
   
        rgd.velocity = transform.forward * 10 * status.movement_speed;

    }
    private void OnDisable()
    {
        initialize_chracter();
    }
   
    private void Awake()
    {
        
        rgd = GetComponent<Rigidbody>();
        C_ani = this.GetComponent<Character_Animation>();
        s = Stage._stage;
        if (status != null)
            current_hp = status.HP;
        _deadbody_duration = 3f;

    }
  
    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
        {
            target = attack.target;   
       
        }
        target_direction = target==null?transform.position:get_target_direction();
        target_distance = target == null ? 0 : (target.transform.position - transform.position).magnitude;
    
       
        if (current_hp <= 0)
        {
            
            deadbody_duration -= Time.deltaTime;
            if (deadbody_duration <= 0)
                this.gameObject.SetActive(false);
            return;
        }

        Snap_ViewPort();
        if (forced&&forced_timer>0 )
        {
            forced_timer -= Time.deltaTime;

            if (rgd.velocity.magnitude < 0.1f && forced_timer <= 0)
            {
                C_ani.resumre_animation();
                forced = false;
            }
            else
                return;
        }
       
        if (target != null)
        {
            transform.LookAt(target.transform);
            if (target.current_hp <= 0)
                initialize_target();

        }
        if (target_distance > status.Distance_number)
            {
                /*q = Quaternion.LookRotation(target_direction);
                transform.rotation = q;*/
                
                chase_enemy();
            }
            else
            {
                
                rgd.velocity = Vector3.zero;
              
                attack.attack_enemy(this);
            
            }
        }

       
          
        void initialize_target()
    {
        attack.target = null;
        target = null;
    }
        
    
    public void hitted(int a)
    {
        if (get_damage(a))
        {
            
            
            if (C_ani == null)
                return;
            C_ani.hitted_animation();
        }
      
    }
  
    bool get_damage(int a)
    {
        
        current_hp -= a;
        notifyObserver();
        
        if(s!=null)
             s.creaate_damagee_font(a, this.transform);
        if (current_hp <= 0)
        {
            current_hp = 0;
            Die();
         
            return false;
        }
        else
        {
            return true;
        }
       
      
    }

    Vector3 get_target_direction()
    {
      Vector3 v =(target.transform.position - transform.position).normalized;
       /* Quaternion q = Quaternion.LookRotation(v);
        transform.rotation = q;*/
        return v;
    }

    void Die()
    {

        attack.stop_action(this);
        rgd.freezeRotation = false;
       
        rgd.velocity = Vector3.zero;
        get_forced(Random.Range(8, 16),0);
        if (C_ani == null)
            return;
        C_ani.die_animation();


    }
    public void get_forced(float force,float force_time)
    {
        attack.stop_action(this);
        rgd.velocity = Vector3.zero;
        Vector3 pow = -1 * (transform.forward + new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2))) * force;
        rgd.AddForce(pow, ForceMode.Impulse);
        forced_timer = force_time;
        forced = true;
  
    }

    public void AddObserver(Character_observer s)
    {
        MyObserver.Add(s);
        s.update_information(current_hp, this);
    }

    public void DeleteObserver(Character_observer s)
    {
        MyObserver.Remove(s);
    }
    void DeleteAllObserver()
    {
     
        for (int i = MyObserver.Count-1; i >0; i--)
        {
            MyObserver.RemoveAt(i);
        }
    }
    public void notifyObserver()
    {
       foreach(Character_observer s in MyObserver)
        {
            s.update_information(current_hp, this);
        }
    }

  public void initialize_chracter()
    {
        if (status != null)
            current_hp = status.HP;
        _deadbody_duration = 3f;
        if (C_ani != null)
            C_ani.initialize_animation();
        rgd.freezeRotation = true;
        initialize_target();
        attack.initalize();

    }
    Vector3 viewport_position;
    void Snap_ViewPort()
    {
        viewport_position = Camera.main.WorldToViewportPoint(transform.position);
        if (viewport_position.x>0.95f)
        {
            viewport_position.x = 0.95f;
        }
        else if(viewport_position.x < 0.05f)
        {
            viewport_position.x = 0.05f;
        }
        if (viewport_position.y > 0.95f)
        {
            viewport_position.y = 0.95f;
        }
        else if (viewport_position.y < 0.05f)
        {
            viewport_position.y = 0.05f;
        }
        transform.position = Camera.main.ViewportToWorldPoint(viewport_position);
    }
    
}
