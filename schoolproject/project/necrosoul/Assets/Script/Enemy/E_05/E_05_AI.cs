using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_05_AI : MonoBehaviour//공중에서 좌우로 왕복하는 적
{
    Vector2 dir;
    private Quaternion rotation;
    Enemy_status E_Status;
    public float attack_time;
    bool attack_status;
    public bool can_attack;
    public float bullet_size;
    float attack_weight;
    Unit unit;
    GameObject Player;

    float move_distance;
    public float move_distance_max;
    public float enemy_size_x;
    public float enemy_size_y;
    public float moving_buffer;
    float moving_weight;
    public float range_distance;
    bool move_corutine_check;
    bool idle_corutine_check;
    bool moving_status;
    public float idle_time;
    public float random_num;
    public float random_num2;
    public Vector2 sentinal_size;
    private Collider2D sensitive;
    public int upvector = 1;
    Animator e_ani;
   
    float t;
    // Start is called before the first frame update
    void Start()
    {
       
        Player = GameObject.FindGameObjectWithTag("Player");
        unit = this.GetComponent<Unit>();
        unit.size_x = enemy_size_x;
        unit.size_y = enemy_size_y;
        attack_status = true;
        e_ani = this.transform.GetChild(1).GetComponent<Animator>();
        E_Status = this.gameObject.GetComponent<Enemy_status>();
        E_Status.set_layout(2);
        unit.can_hitted_ani = true;
        unit.max_hp = E_Status.get_max_hp();
        unit.Health_point = E_Status.get_hp();
        unit.Defense_point = E_Status.get_defense_point();
        unit.move_speed = E_Status.get_speed();
        unit.Attack_point = E_Status.get_atk();
    }
   
    void FixedUpdate()
    {
        brain();
    }
    void brain()//ai 정리
    {
        if (unit.Health_point > 0)
        {
            if (unit.onGround)
            {
                unit.onGround = false;
            }
            e_ani.SetBool("Move", unit.can_move);

            Player = this.transform.GetComponent<Unit>().Player;
            
               
               //계속 이동
                    move_ai_0();
                
       
        }
        else
        {
            die();
        }
    }

    void die()
    {
        unit.HpCheack();

    }














    void move_ai_0()
    {


       
        if (unit.can_move)
        {
          
            transform.Translate(Vector3.left * unit.direction * unit.move_speed * Time.deltaTime);



            e_ani.SetBool("move", true);

           
            //정면에 레이캐스트로 벽감지
            Debug.DrawLine(transform.position, transform.position - (new Vector3(0.2f, 0, 0) + new Vector3(enemy_size_x / 2, 0, 0)) * unit.direction, Color.blue);
           
            var wall_ray = Physics2D.Raycast(transform.position + Vector3.up * enemy_size_y * 0.5f, Vector3.left * unit.direction, enemy_size_x / 1.5f + 0.4f, LayerMask.GetMask("platform_can't_pass"));
            var wall_ray2 = Physics2D.Raycast(transform.position - Vector3.up * enemy_size_y * 0.5f, Vector3.left * unit.direction, enemy_size_x / 1.5f + 0.4f, LayerMask.GetMask("platform_can't_pass"));
            //벽에 닿을시 반대 방향으로
            if (wall_ray.collider != null||wall_ray2.collider!=null)
            {
               
                unit.direction_change_spr();
                Debug.Log("col");
            }
          
        }
        }
  

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new Vector2(enemy_size_x,enemy_size_y));
    }
  
   
}
