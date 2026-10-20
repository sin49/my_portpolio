using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_shadow : MonoBehaviour//플레이어의 환영,그림자 클레스(튜토리얼,이단 공격 이펙트 등으로 사용한다)
{
    public PlayerCharacter p;
    public bool p_created;
    public int attack_num;
    public Vector2 dash_direction;
    public float dash_time = 1f;
    public float dash_timer = 0;
    public Player_shadow_animator p_anim;
    public float dash_force;
    public int animation_level;
    public bool shadow_type;
    public float shadow_time;//지속 시간
    private bool on_dash;
    private Vector3 move_vector;
    private Vector2 direct_vector;
    private float move_speed;
    private float move_weight;
    public int direction;
    
    private int jump_count;
    private bool dash_recover_check;
    private Rigidbody2D rgd;
    private bool raycheck;
    private bool onground;
    private int air_attack_num;
    private float jumpbuffertimer;
    private float hangTimer;
    private float jumpbuffertime;
    private bool on_corutine_1;
    private bool on_platform;
    Color c;
    float c_alpha;
    public float shadow_original_timer;
    public float Downbuffertime;
    public float Downbuffertimer;
    public bool once_chk;
    public bool anim_chk;
    SpriteRenderer s_r;
    public float Player_Y;
    internal bool can_move;
    float once_timer;
    public void Start()
    {
        //환영의 rgba 값을 결정한다
        s_r = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        c = s_r.color;
        c_alpha = c.a;
        p_anim = this.transform.GetChild(0).GetComponent<Player_shadow_animator>();
        rgd = this.GetComponent<Rigidbody2D>();
        // make_stat();
    }
    public void make_stat()//환영의 능력치를 결정한다
    {
        
        if(p==null)
            p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        Player_Y = p.Player_Y;
        dash_time = p.dash_time;
        dash_force = p.dash_force;
        move_speed = p.move_speed;
        direction = 1;
        /*if (p.direction == -1)
            direction_change();*/
        jump_count = 1;

    }
    private void FixedUpdate()
    {
        
        if (p == null)//플레이어가 지정되있다면 플레이어의 능력치를 따라간다
        {
            make_stat();
        }
        if (p_created)//플레이어를 통해 생성됬다면 플레이어 위치에 있는다
        {
            this.transform.position = p.transform.position;
        }
        if (on_dash)//대쉬중에는 중력을 받지 않는다
        {
            dash_timer -= Time.deltaTime;
            rgd.gravityScale = 0;
            if (dash_timer < 0)
            {
                dash_end();
               
            }
        }

        //shadow_type의 값에 따라 두가지 유형으로 갈린다
        //true일 경우 지속시간이 지나면 자동으로 파괴된다
        //false일 경우 지속시간이 존재하지 않는다
        if (shadow_type)
        {
            if (shadow_time >= 0)//shadow_time 동안 생존
            {
                c.a =c_alpha* shadow_time / shadow_original_timer;//점점 흐릿해진다
                s_r.color = c;
                shadow_time -= Time.deltaTime;
                animation_work();
            }
            else
            {
                this.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            animation_work();
        }

        if (rgd.velocity.y != 0)//플랫폼 위에 있는지 체크한다(점프 에니메이션,공중 공격 에니메이션)
        {
            if (onground)
            {

                onground = false;
            }
        }
        else
        {
            if (!onground)
            {

                onground = true;
            }
        }
        p_anim.set_ground(onground);//onground 값을 통해 에니메이터에게 플랫폼 위에 있는지 없는지 전달


        if (rgd.velocity.y <= 0 && !raycheck)//공중에 있을 때 땅에 닿으면 점프 횟수 회복
        {
            //공중에 있을 때 자신의 아랫쪽에  ray(스프라이트의 길이의 절반보다 살짝 길다)를 쏜다(플랫폼 종류별만큼 ray를 쏜다)
            Debug.DrawRay(this.transform.position, Vector2.down * Player_Y, Color.red);
            //일반 플랫폼
            RaycastHit2D down_ray_1 = Physics2D.Raycast(this.transform.position - Vector3.up, Vector2.down, Player_Y, LayerMask.GetMask("platform_can't_pass"));
            //양방향 플랫폼
            RaycastHit2D down_ray_2 = Physics2D.Raycast(this.transform.position - Vector3.up, Vector2.down, Player_Y, LayerMask.GetMask("platform_can_pass"));
            //ray에 닿았다면 땅에 닿았음을 알리고 점프를 회복
            if (down_ray_1.collider)
            {
                groundcollision(down_ray_1.transform.gameObject);

                Debug.Log("플랫폼");
            }
            if (down_ray_2.collider)
            {
                groundcollision(down_ray_2.transform.gameObject);

                Debug.Log("단방향");
            }
            else
            {

            }
        }
    }
    public void animation_work()//환영의 액션을 다룬다
    {
        if (once_chk)//액션을 한번만 실행할지 계속 실행할지의 여부를 체크한다
        {
            if (!anim_chk)//anim_chk로 액션을 실행했는지 체크한다
            {
                switch (animation_level)//animation_level의 값에 따라 지정된 액션을 실행한다
                {
                    case 1:
                        attack(1);
                        break;
                    case 2:
                        attack(2);
                        break;
                    case 3:
                        attack(3);
                        break;
                    case 4:
                        if (onground)
                        {
                            jump();
                        }


                        break;
                    case 5:
                        move_left();
                        break;
                    case 6:
                        Move_right();
                        break;
                    case 7:
                        StartCoroutine(downplatform());
                        break;
                    case 8:
                        if (!on_dash)
                        {
                            dash();
                        }

                        break;
                    case 9:
                        attack_combo();
                        break;
                    case 10:
                        if (!on_dash)
                        {
                            dash_mirror();
                        }
                        break;
                    case 11:
                        air_attack();
                        break;
                }
               
            }
        }
        else//지속시간동안 계속 실행
        {
            switch (animation_level)
            {
                case 1:
                    attack(1);
                    break;
                case 2:
                    attack(2);
                    break;
                case 3:
                    attack(3);
                    break;
                case 4:
                    if (onground)
                    {
                        jump();
                    }


                    break;
                case 5:
                    move_left();
                    break;
                case 6:
                    Move_right();
                    break;
                case 7:
                    StartCoroutine(downplatform());
                    break;
                case 8:
                    if (!on_dash)
                    {
                        dash();
                    }
                    break;
                case 9:
                    attack_combo();
                    break;
                case 10:
                    if (!on_dash)
                    {
                        dash_mirror();
                    }
                    break;
                case 11:
                    air_attack();
                    break;
            }
        }
    }
    //공중 공격 액션 실행
    private void air_attack()
    {
        anim_chk = true;
        //공중공격 에니메이션 실행
        p_anim.air_attack_anim();
    }
    //대쉬 액션 실행(오른방향)
    void dash()
    {
        //플레이어의 대쉬구조와 동일
        
            rgd.velocity = Vector2.zero;


            p_anim.dash();
        // 순간적으로 rgd의 값에 힘을 가한다(ForceMode2D.Impulse)
        //그 후 짧은 시간(dash_timer)동안 계속 날라간다
        //dash_timer가 끝나면 velocity의 값을 0으로 만들어 대쉬를 끝낸다
        on_dash = true;
            dash_timer = dash_time;

            //dash_direction =  Vector2.right*direction * Player_status.p_status.get_dash_force();
            dash_direction = Vector2.right * direction * dash_force;
            Debug.Log(dash_direction);
            rgd.AddForce(dash_direction * Time.deltaTime, ForceMode2D.Impulse);
  

            dash_recover_check = false;
        

    }
    //대쉬 액션 실행(왼방향)
    void dash_mirror()
    {


        rgd.velocity = Vector2.zero;


        p_anim.dash();

        on_dash = true;
        dash_timer = dash_time;
        if (this.transform.GetChild(0).transform.rotation.y != 180)
        {
            
            this.transform.GetChild(0).transform.Rotate(0, 180, 0);
        }
        direction = -1;
        //dash_direction =  Vector2.right*direction * Player_status.p_status.get_dash_force();
        dash_direction = Vector2.right * direction * dash_force;
        Debug.Log(dash_direction);
        rgd.AddForce(dash_direction * Time.deltaTime, ForceMode2D.Impulse);


        dash_recover_check = false;


    }
    //dash_timer가 다됐다면 velocity의 값을 0으로 만들어 정지시키고 중력의 영향을 받게만드러 대쉬를 끝낸다
    void dash_end()
    {
        rgd.velocity = Vector2.zero;

        p_anim.dash_end();

        anim_chk = true;
        rgd.gravityScale = 1;
        on_dash = false;
    }
    // 방향키 아래를 두번 연달아 누름으로써 양방향 플랫폼 아래로 떨어질수 있다
    public void down_platform()
    {

        //아래 키를 버퍼로 받으면서 키를 한번 누른 상태를 확인한다
        if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.DOWN]) && on_platform && Downbuffertimer <= 0 && !p_anim.sword_delay && !on_dash)
        {
            Downbuffertimer = Downbuffertime;
        }
        //버퍼가 유지되어있는 동안 아래 키를 한번 더 누르는 것으로 레이어를 변경시키는 코루틴을 시작하여 양방향 플랫폼을 통과하게 한다
        if (Downbuffertimer > 0)
        {
            Downbuffertimer -= Time.deltaTime;
            if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.DOWN]) && on_platform && Downbuffertimer >= 0 && !p_anim.sword_delay && !on_dash)
            {
                //layer_change_timer = layer_change_time;
                if (!on_corutine_1)
                    StartCoroutine(downplatform());
            }
        }
    }
    //양방향 플랫폼을 통과하는 코루틴
    IEnumerator downplatform()
    { 
        var wait = new WaitForSeconds(Downbuffertime);
        //양방향 플랫폼을 통과할수있는 레이어로 변경한다
        on_corutine_1 = true;
        this.gameObject.layer = 10;
        //짧은 시간을 진행한 후 기존의 레이어로 복귀한다
        yield return wait;
        on_corutine_1 = false;
        this.gameObject.layer = 6;
     
    }
    //이동 액션(오른쪽)
    public void Move_right()
    {
        p_anim.move_state = true;
        //스프라이트가 반대 방향이라면 스프라이트의 좌우를 뒤집고 direction을 갱신한다
        if (direction == -1)
        {
            direction_change();
        }
        //이동 벡터를 설정
        move_vector = new Vector3(direction * move_speed , 0, 0);

        //자신의 이동경로에 벽이 존재하는지를 ray로 확인한다
        RaycastHit2D wall_ray = Physics2D.Raycast(this.transform.position, Vector3.right, p.Player_X, LayerMask.GetMask("platform_can't_pass"));

        Debug.DrawRay(this.transform.position, Vector3.right * p.Player_X, Color.red);
        //ray가 닿음으로써 벽과 접촉했다면 이동 중향을 조절해 이동속도를 낮춤으로써 벽을 통과하는 현상을 방지한다
        if (wall_ray.collider)
        {

            move_weight *= 0.93f;
        }
        else
        {
            move_weight = 1;
        }

        //캐릭터가 이동
        transform.Translate(move_vector * move_weight * Time.deltaTime);
    }
    //플레이어의 3타 콤보 액션을 실행
    public void attack_combo()
    {
        if (onground)//땅에 있을때만 실행
        {
            p_anim.attack_combo();
           
        }
    }
    //이동 액션(왼쪽)
    public void move_left()
    {
        p_anim.move_state = true;
        if (direction == 1)
        {
            direction_change();
        }

        move_vector = new Vector3(direction * move_speed * Time.deltaTime * -1, 0, 0);
        direct_vector = new Vector2(-1, direct_vector.y);


        RaycastHit2D wall_ray = Physics2D.Raycast(this.transform.position, Vector3.left, p.Player_X, LayerMask.GetMask("platform_can't_pass"));
        Debug.DrawRay(this.transform.position, Vector3.left * p.Player_X, Color.red);
        if (wall_ray.collider)
        {

            move_weight *= 0.93f;
        }
        else
        {
            move_weight = 1;
        }


        transform.Translate(move_vector * move_weight);

    }
 //점프 액션 실행
    public void jump()
    {
        //점프 횟수가 남아있을 때
        if (jump_count > 0)
        {
            //윗쪽 벡터로 힘을 순간적으로 가하고 애니멩이션 실행
            jump_count--;
            p_anim.jump_anim();
            rgd.AddForce(new Vector2(rgd.velocity.x, Player_status.p_status.get_jump_force()), ForceMode2D.Impulse);
            raycheck = false;
            anim_chk = true;
        }
    }
//공격 액션 한번만 실행
    void attack()
    {
       

        

                if (onground)
                    p_anim.sword_attack_anim();
                else if (!onground && air_attack_num > 0)
                {
                    p_anim.air_attack_anim();
                   air_attack_num--;
                }
                
            

        }
    //지정된 공격 콤보 액션 한부분을 실행
    void attack(int i)
    {




        if (onground)
            p_anim.sword_attack_anim(i-1);
       



    }
    //땅에 닿았다며 점프 횟수를 회복
    public void groundcollision(GameObject a)
    {
        if (a.layer != 11)//일반 플랫폼이라면
        {
            //광선에 닿는 즉시 점프 회복
            
            jump_count = Player_status.p_status.get_jump_count();
            dash_recover_check = true;
            onground = true;
          air_attack_num = Player_status.p_status.air_attack_num_orignal + Player_status.p_status.air_attack_num_bonus;
            raycheck = true;

        }
        else//양방향 플랫폼일시
        {

            if (onground)//플랫폼 위에 있을 때만 점프 회복
            {
                //양방향 플랫폼은 플레이어가 통과하므로 플랫폼 통과 중에 플레이어가 점프 회수를 회복하는 상황을 막는다
                
                jump_count = Player_status.p_status.get_jump_count();
                dash_recover_check = true;
                onground = true;
               air_attack_num = Player_status.p_status.air_attack_num_orignal + Player_status.p_status.air_attack_num_bonus;
                raycheck = true;

            }



        }

    }
    //이동 액션을 멈춘다
    public void stop_move()
    {
        direct_vector = new Vector2(0, direct_vector.y);
        p_anim.move_state = false;
    }
    //방향을 바꾼다(direction 1=오른쪽 -1=왼쪽)
    void direction_change()
    {
        direction *= -1;
        transform.Rotate(0, 180, 0);
        if (transform.rotation.y > 360)
            transform.Rotate(0, -360, 0);
    }
  
}
