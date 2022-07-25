using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_07_AI : MonoBehaviour//공중에서 플레이어를 추격하여 근접공격하는 적
{

    public List<node> path;
    Enemy_status E_Status;
    Vector2 dir;
    private Quaternion rotation;
    public List<Transform> create_position = new List<Transform>();
    public float attack_time;
    bool attack_status;

    public float bullet_size;
    float attack_weight;
    Unit unit;
    Rigidbody2D rgd;
    public GameObject Player;
    public GameObject attack_range;//if attack_range.bool=true ->attack
    public List<GameObject> create_object = new List<GameObject>();
    public List<GameObject> created_object = new List<GameObject>();
    public E_07_range E_range;
    Pathfinding_E_07 p_e_07;
    float move_distance;
    public float move_distance_max;
    public float enemy_size_x;
    public float enemy_size_y;
    public float moving_buffer;
    float moving_weight;
    public E_07_chase_range range_distance;
    public bool can_chase;

    bool moving_status;
    public float idle_time;
    Animator e_ani;
    public bool move_strict;
    public float attack_delay;
    Vector2 node_dir;
    public float wall_bounce_force;
    public float move_force;
    public bool on_attack;
    float s_ran;
    int num;
    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new Vector2(enemy_size_x, enemy_size_y));
    }
    void Start()
    {
        p_e_07 = this.GetComponent<Pathfinding_E_07>();
        rgd = GetComponent<Rigidbody2D>();
        s_ran = Random.Range(-0.5f, 0.5f);
        unit = this.GetComponent<Unit>();
        attack_status = true;
        e_ani = this.transform.GetChild(1).GetComponent<Animator>();
        E_Status = this.gameObject.GetComponent<Enemy_status>();
        E_Status.set_layout(4);
        unit.can_hitted_ani = false;
        unit.max_hp = E_Status.get_max_hp();
        unit.Health_point = E_Status.get_hp();
        unit.Defense_point = E_Status.get_defense_point();
        unit.move_speed = E_Status.get_speed();
        unit.Attack_point = E_Status.get_atk();
        unit.size_x = enemy_size_x;
        unit.size_y = enemy_size_y;
    }
    //플레이어를 감지
    void ray_to_player()
    {
        //인식 범위 안의 플레이어를 감지
        if (range_distance.on_player)
        {//인식시 추격
            can_chase = true;
        }
        else
        {
            can_chase = false;
        }
        //공격 범위 안의 플레이어를 감지
        if (E_range.on_player)
        {//공격 할 수 있다면 공격
            unit.can_attack = true;

        }
        else
        {

            unit.can_attack = false;

        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        brain();
    }
    void brain()//ai 정리
    {
        if (unit.onGround)//공중에 있다
        {
            unit.onGround = false;
        }
        if (rgd.velocity.magnitude > 5)//최대 가속도를 제한한다
        {
            rgd.velocity *= 0.95f;
        }
        if (unit.can_forced)//밀려나지 안흠
        {
            unit.can_forced = false;
        }
        if (unit.Health_point > 0)
        {
            //플레이어 감지
            ray_to_player();
            if (on_attack)
            {
                rgd.velocity = Vector3.zero;
            }
           

            if (unit.can_attack)//공격 가능 할 때 공격
            {

                if (attack_delay <= 0)
                {
                    attack();
                    E_range.on_player = false;
                }
                else
                {
                    
                }
               

            }
            else
            {
                
                if (!on_attack|| attack_delay > 0)
                {
                    //플레이어를 인식했다면 추격
                    if (can_chase)
                    {

                        chase_player();
            
                    }
                    else//아닐 시 왕복하며 이동
                    {


                        move_ai_0();

                    }
                }
               


            }
            //공격 후 딜레이
            if (attack_delay > 0)
            {
                

                attack_delay -= Time.deltaTime;
                e_ani.ResetTrigger("attack");
                E_range.on_player = false;
                rgd.bodyType = RigidbodyType2D.Kinematic;
            }
            else
            {

                e_ani.SetBool("attack_delay", false);
                rgd.bodyType = RigidbodyType2D.Dynamic;
            }
        }
        else
        {
            die();
        }

    }

    void die()
    {
        unit.HpCheack();
        e_ani.SetTrigger("death");
        for (int i = 0; i < created_object.Count; i++)
        {
            Destroy(created_object[i]);
        }
    }

  





    //플레이어를 추격한다
    //추격 할 때는 a*알고리즘을 이용한 길찿기 알고리즘을 이용한다
    void chase_player()
    {
        if (!move_strict&&unit.can_move)
        {

            e_ani.SetBool("move", true);
            e_ani.SetBool("attack_delay", true);
            //길찿기 벡터의 리스트의 위치를 이동방향으로 설정한다
            if (path.Count - 5 >= 0)
            {
                //적 오브젝트의 크기를 고려해 [5]번 위치를 기준으로 방향을 정한다
                node_dir = path[5].pos - (Vector2)this.transform.position;
            }
            //이동
            rgd.AddForce(node_dir.normalized* (move_force+s_ran));
            //이동 방향에 따라 스프라이트 방향 변경
            if (node_dir.x <= 0)
            {
                if (unit.direction == -1)
                {
                    unit.direction_change_spr();
                }
            }
            else
            {
                if (unit.direction == 1)
                {
                    unit.direction_change_spr();
                }
            }
            //이동을 멈췄을 때= 장애물에 끼였을 때
            if (rgd.velocity.magnitude == 0)
            {
                //반대 방향으로 밀어내 새로운 길 탐색
                p_e_07.find_not_stuckpath(path[0]);
            }
            

        }
    }




    //이동 4번적과 동일
        void move_ai_0()
        {
            if (!move_strict && unit.can_move)
            {
                transform.Translate(Vector3.left * unit.direction * unit.move_speed * Time.deltaTime);//정면으로 움직임

                move_distance += unit.move_speed * Time.deltaTime;
                //정면에 레이캐스트로 벽감지
                Debug.DrawLine(transform.position, transform.position - (new Vector3(0.2f, 0, 0) + new Vector3(enemy_size_x / 2, 0, 0)) * unit.direction, Color.green);
                var wall_ray = Physics2D.Raycast(transform.position, Vector3.left * unit.direction, enemy_size_x / 2 + 0.2f, LayerMask.GetMask("platform_can't_pass"));
                if (wall_ray.collider != null)
                {
                    unit.direction_change_spr();
                    move_distance = 0;
                }
            }
        

        }
    //공격 에니메이션 실행
        void attack()
        {
        rgd.velocity = Vector3.zero;

            
     
        e_ani.SetBool("move", false);
            e_ani.SetTrigger("attack");
       
    }
   
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            Debug.Log("충돌");
            //node_dir *= -1;
           

        }
    }
    

    }



