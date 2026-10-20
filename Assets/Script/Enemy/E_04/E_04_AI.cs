using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_04_AI : MonoBehaviour//플레이어를 향해 돌진하는 적
{
    //돌진
    Vector2 dir;
    public int level;
    private Quaternion rotation;
    public GameObject Attackrange;
    public float attack_time;
    bool attack_status;
    Vector3 player_transform_buffer;
    public float tr_buffertime=0.25f;
    float tr_buffertimer = 0;
    float attack_weight;
    Unit unit;
    GameObject Player;
    Enemy_status E_Status;
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
  float attack_speed;
    public float attack_speed_num;
    Collider2D sensitive;
    public e_04_attack_rang attack_range;
    Animator e_ani;
    public bool hitted_chk;
    public float attack_delay;
    private bool on_attack;

    public float rance_speed_down;
    public float rancer_delay_time;
    float rancer_delay_timer;

    // Start is called before the first frame update
    void Start()
    {
       
        attack_speed = attack_speed_num;
        tr_buffertimer = tr_buffertime;
       
            Player = this.transform.GetComponent<Unit>().Player;
        unit = this.GetComponent<Unit>();
        unit.size_x = enemy_size_x;
        unit.size_y = enemy_size_y;
        attack_status = true;
        e_ani = this.transform.GetChild(1).GetComponent<Animator>();
        E_Status = this.gameObject.GetComponent<Enemy_status>();
        if (level == 1)
        {
            E_Status.set_layout(7);
        }
        else
        {
            E_Status.set_layout(1);
        }
    
        unit.can_hitted_ani = true;
        unit.max_hp = E_Status.get_max_hp();
        unit.Health_point = E_Status.get_hp();
        unit.Defense_point = E_Status.get_defense_point();
        unit.move_speed = E_Status.get_speed();
        unit.Attack_point = E_Status.get_atk();
    }
    //
    void ray_to_player()//플레이어가 공격범위 안에 들어왔는지 감지
    {
        float length = Mathf.Log(Mathf.Pow(18, 2) + Mathf.Pow(28, 2)) * 2;

        //attack_range를 통해 공격범위 안의 플레이어를 감지한다
        if ( attack_range.on_attack&&unit.manner_time>0)
        {

            unit. can_attack = true;
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
       
        if (unit.Health_point > 0)
        {
           //플레이어 감지
            dir = player_transform_buffer - this.transform.position;
            
            ray_to_player();


            if (attack_delay > 0)//공격 후 딜레이
            {
                attack_delay -= Time.deltaTime;
                  attack_speed= attack_speed_num;
            }
            if (rancer_delay_timer > 0)//돌진 후 경직
            {
                rancer_delay_timer -= Time.deltaTime;
            }
            if (level == 1)//스테이지 2:돌진 공격 존재
            {
                if (!hitted_chk)
                {
                    if (unit.can_attack || on_attack)//플레이어가 공격 범위 안에 있을 때
                    {
                        if (attack_delay <= 0)//공격
                        {
                            attack_ai_0();
                            StopCoroutine("idle");
                        }
                        else//공격 후 대기(딜레이)
                        {
                            if (rancer_delay_timer > 0)
                            {
                                StartCoroutine("idle");//대기  상태
                            }
                            else//
                            {
                                move_ai_0();
                            }
                        }
                    }
                    else//공격 조건을 달성핮지 못하면 이동
                    {
                        if (rancer_delay_timer > 0)
                        {
                            StartCoroutine("idle");//대기  상태
                        }
                        else
                        {//이동
                            StopCoroutine("idle");
                            move_ai_0();
                        }
                    }
                }
            }
            else if(level==0)//스테이지 1:공격 없음
            {
                if (!hitted_chk)//피격외에는 계속 이동
                {
                    move_ai_0();
                }
                else
                {
                    StartCoroutine("idle");//대기  상태
                }
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
        
    }





  




   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new Vector2(enemy_size_x, enemy_size_y));
    }
    //벽에 닿거나 낭떠러지 앞까지 계속 이동한다
    void move_ai_0()
    {
        if (unit.can_move)
        {
            e_ani.SetBool("Walk", true);
            unit.can_forced = true;
            unit.can_hitted_ani = true;
            transform.Translate(unit.direction*Vector3.left  * unit.move_speed * Time.deltaTime);//정면으로 움직임

            move_distance += unit.move_speed * Time.deltaTime;
            //정면에 레이캐스트로 벽감지
            Debug.DrawLine((transform.position + Vector3.up * enemy_size_y * 0.1f), (transform.position + Vector3.up * enemy_size_y * 0.1f) - (new Vector3(enemy_size_x * 0.5f+0.4f, 0, 0)) * unit.direction, Color.red);
            Debug.DrawLine((transform.position- Vector3.up * enemy_size_y * 0.1f), (transform.position - Vector3.up * enemy_size_y * 0.1f) - (new Vector3(enemy_size_x * 0.5f + 0.4f, 0, 0)) * unit.direction, Color.green);
            var wall_ray = Physics2D.Raycast(transform.position + Vector3.up * enemy_size_y * 0.1f, Vector3.left * unit.direction, enemy_size_x * 0.5f + 0.4f, LayerMask.GetMask("platform_can't_pass"));
            var wall_ray2 = Physics2D.Raycast(transform.position - Vector3.up * enemy_size_y * 0.1f, Vector3.left * unit.direction, enemy_size_x * 0.5f + 0.4f, LayerMask.GetMask("platform_can't_pass"));
            if (wall_ray.collider != null|| wall_ray2.collider != null)//벽에 닿을시 반대 방향
            {
                Debug.Log("벽에의해");
                unit.direction_change_spr();
                move_distance = 0;
            }

            //앞에 플랫폼이 없으면 방향 바꿈
            var bottom_ray = Physics2D.Raycast(transform.position + Vector3.left * (enemy_size_x / 2) * unit.direction, Vector3.down, enemy_size_y / 2 + 0.2f, LayerMask.GetMask("platform_can't_pass"));
           var bottom_ray_2 = Physics2D.Raycast(transform.position + Vector3.left * (enemy_size_x / 2) * unit.direction, Vector3.down, enemy_size_y / 2 + 0.2f, LayerMask.GetMask("platform_can_pass"));
            Debug.DrawLine(transform.position + Vector3.left * (enemy_size_x / 2) * unit.direction, transform.position + Vector3.left * (enemy_size_x / 2) * unit.direction + (Vector3.down * (enemy_size_y / 2) + new Vector3(0, 0.4f)), Color.blue);
            if (bottom_ray.collider == null&& bottom_ray_2.collider == null)
            {
                Debug.Log("바닥에의해");
                unit.direction_change_spr();
                move_distance = 0;
            }
        }
    }
    //공격:속도를 순간적으로 높여 돌진한다
    void attack_ai_0()
    {
        if (unit.can_move)
        {

            unit.can_hitted_ani = false;
            if (attack_speed > 0)
            {
                on_attack = true;
                //돌진에니메이션으로 변경
                e_ani.SetBool("Walk", false);
                e_ani.SetBool("Attack 0", true);
                transform.Translate(Vector3.left * unit.direction * (unit.move_speed + attack_speed) * Time.deltaTime);//정면으로 움직임+attack_speed로 가속

                //벽에 닿으면 반대 방향으로
                var wall_ray = Physics2D.Raycast(transform.position, Vector3.left * unit.direction, enemy_size_x / 2 + 0.2f, LayerMask.GetMask("platform_can't_pass"));
                if (wall_ray.collider != null)
                {
                    Debug.DrawLine(transform.position, transform.position + (new Vector3(0.2f, 0, 0) + new Vector3(enemy_size_x / 2, 0, 0)) * unit.direction, Color.green);
                    unit.direction_change_spr();
                    move_distance = 0;
                }
               //피격 당해도 밀려나지 않음
                unit.can_forced = false;
                //서서히 감속
                attack_speed -= rance_speed_down;
            }
            else//돌진이 끝나면 돌진 공격 중 변경된 값을 초기화 하고 딜레이
            {
                unit.can_forced = true;//넉백 여부
                unit.can_hitted_ani = true;
               rancer_delay_timer = rancer_delay_time;//딜레이
                on_attack = false;
                e_ani.SetBool("Walk", true);//이동에니메이션으로 복구
                e_ani.SetBool("Attack 0", false);
                attack_delay = 2f;
            }
        }
        }
        
    

    
    
    IEnumerator idle()
    {
        var wait = new WaitForSeconds(idle_time);
        e_ani.SetBool("Walk", false);
        idle_corutine_check = true;
        yield return wait;
        move_corutine_check = true;
        idle_corutine_check = false;
    }

}


