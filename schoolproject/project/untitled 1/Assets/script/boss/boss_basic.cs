using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boss_basic : MonoBehaviour//보스의 상태와 이등을 다루는 클레스
{
    public float e_hp=200;
    public float e_hp_max;
    public float speed;
    public bool Vcheck = true;
    public bool Hcheck = true;
    public int b_type;
    //b_2
    public float shoot_mode_time;
    //
    public float time;
    public bool teleport_check;
    public GameObject teleport_sprite;
    public bool teleport_check2;
    GameObject player_;
    Transform player_location;
    Vector3 pos3;
    public int b_3_check;
    public GameObject[] lazer;
    public bool die_check;
    public bool lazer_check;
    public bool endure;
    public GameObject clear;
    public bool b_4_movement;
    public bool shoot_mode;
    public bool b3_2_check;
    public GameObject game_manager;
    public Animator ani;
    public bool die_ani_check;
    public bool damage_check;
    public bool teleport_ani_check;
    public bool b_3_2_intro_animation_check;
    public bool effect_check;
    public GameObject effect;
    public bool b_3_die_ani_check;
    // Start is called befor
    // Start is called before the first frame update
    void Start()
    {
        //음악을 보스전 음악으로 전환시킨다
        game_manager = GameObject.Find("GameManager");
        if (b_type != 4)
        {
            game_manager.GetComponent<shooting_GameManager>().audioSource.clip = game_manager.GetComponent<shooting_GameManager>().boss_music;
        }
        else
        {
            game_manager.GetComponent<shooting_GameManager>().audioSource.clip = game_manager.GetComponent<shooting_GameManager>().boss_music2;
        }
        game_manager.GetComponent<shooting_GameManager>().audioSource.Play();
        ani = GetComponent<Animator>();
        e_hp_max = e_hp;
        clear = GameObject.FindWithTag("clear");
        clear.GetComponent<Clear_bullet>().run();
    }

    // Update is called once per frame
    void Update()
    {
        //hp이 음수가 되는 경우를 방지 (보스 체력 ui의 표시가 음수가 되는 것을 막기)
        if (e_hp <= 0)
        {
            e_hp = 0;
        }
        game_manager = GameObject.Find("GameManager");
        player_ = GameObject.FindWithTag("Player");
        //y값이 화면을 벗어나지 않도록
        if (transform.position.y >= 3)
        {
            Vcheck = true;
        }
        else if (transform.position.y <= -3)
        {
            Vcheck = false;
        }
        //b_type의 값을 통해 보스의 이동 ai,기믹을 결정
        if (b_type == 1)
        {
            b_type_1();
        }else if (b_type == 2)
        {
            b_type_2();
        }else if (b_type == 3)
        {
            b_type_3();
        }else if (b_type == 4)
        {
            b_type_4();
        }
        
    }
    public void b_type_1()//안움직임
    {
        //hp이 0이되면 보스 격파
        if (e_hp <= 0)
        {
            
                ani.SetTrigger("die");
            clear.GetComponent<Clear_bullet>().run();
            die_check = true;
            game_manager.GetComponent<shooting_GameManager>().audioSource.Stop();
        }
        //사망 애니메이션 처리 후 파괴 게임메니저를 clear_phase로 전환하도록 한다
        if (die_ani_check)
        {
            Destroy(this.gameObject);
            game_manager.GetComponent<shooting_GameManager>().clear_phase = true;
        }
       

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_clear1"))
        {
            ani.SetTrigger("teleport");
        }
        //공격에니메이션
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_attack1_1"))
        {
            ani.ResetTrigger("attack");
        }

        //보스가 화면 오른족에서 등장
        if (transform.position.x <= 4.8)
        {
            Hcheck = false;
        }
        if (Hcheck)
        {
            transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
        }
        if (!Hcheck)
        {
            gameObject.GetComponent<boss1_bulletmaster>().shoot_mode = true;
        }
    }
    public void b_type_2()//화면 전체를 한바퀴 돌다가 공격을 할때는 멈춘다,
    {
        //이펙트 자괴
        if (effect_check)
        {
            Destroy(effect.gameObject);
        }
        //hp이 0이되면 보스 격파
        if (e_hp <= 0)
        {
            game_manager.GetComponent<shooting_GameManager>().audioSource.Stop();
            ani.SetTrigger("die");
            clear.GetComponent<Clear_bullet>().run();
            die_check = true;
        }

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_clear2"))
        {
            ani.SetTrigger("teleport");
        }
        if (die_ani_check)
        {
            Destroy(this.gameObject);
            game_manager.GetComponent<shooting_GameManager>().clear_phase = true;
        }
        //shoot_mode의 bool값을 통해 보스가 이동과 공격을 반복하게 만든다
        //shoot_mode_time을 통해 일정 시간마다 shoot_mode의 값이 전환된다
        if (!GetComponent<boss2_bulletManager>().shoot_mode)
            shoot_mode_time += Time.deltaTime;
        if (e_hp <= e_hp_max / 2)//체력이 절반이 되면 보스가 더욱 빨라지며 자주 공격하게 바뀐다
        {
            if (e_hp <= 40)//체력이 거의 줄어 들었을 때 공격없이 매우 빠르게 이동한다
            {
                if (e_hp <= 0)
                {
                    speed = 0;
                }
                else
                {
                    speed = 20;
                }
            }
            else
            {
                speed = 15;
                //2초 간격
                if (shoot_mode_time >= 2)
                {
                    GetComponent<boss2_bulletManager>().shoot_mode = true;
                    shoot_mode_time = 0;
                }
            }
        }
        else
        {
            //4초 간격
            if (shoot_mode_time >= 4)
            {
                GetComponent<boss2_bulletManager>().shoot_mode = true;
                shoot_mode_time = 0;
            }
        }
        //보스가 화면의 상하좌우 끝에 닿았는지를 bool값으로 체크해 보스의 이동 방향을 제어한다
        if (transform.position.y >= 2)
        {
            Vcheck = true;
        }
        else if (transform.position.y <= -2)
        {
            Vcheck = false;
        }
        if (transform.position.x >= 4)
        {
            Hcheck = true;
        }else if (transform.position.x <= -4)
        {
            Hcheck = false;
        }
        //보스가 화면에서 벗어나지 않고 반시계방향으로 이동한다
        if (!GetComponent<boss2_bulletManager>().shoot_mode)
        {
            if (Hcheck && Vcheck)
            {
                transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
            }
            else if (!Hcheck && Vcheck)
            {
                transform.Translate(new Vector2(0, -1 * speed * Time.deltaTime));
            }
            else if (!Hcheck && !Vcheck)
            {
                transform.Translate(new Vector2(1 * speed * Time.deltaTime, 0));
            }
            else if (Hcheck && !Vcheck)
            {
                transform.Translate(new Vector2(0, 1 * speed * Time.deltaTime));
            }
        }
        
    }
    public void b_type_3()//보스가 위아래로 이동하며 특정 공격패턴때 순간이동을 한다
    {
        //3스테이지 보스는 endure과 endure_check값을 통해 1페이즈와 버티기 패턴 이후 2페이즈로 구분된다

        if (b_3_die_ani_check)
        {
            
            Destroy(this.gameObject);
            GetComponentInParent<boss3_>().endure_check = true;
        }

        //공격에니메이션
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_1"))
        {
            ani.ResetTrigger("attack");
            GetComponent<boss3_bullet_manager>().attack_ani_check = false;
        }
        //텔레포트 도중에는 피해를 받지 않는다 teleport_ani_check의 값을 통해 보스가 텔레포트하고있는 상태라는 것을 확인한다
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_teleport_reverse"))
        {
            ani.ResetTrigger("teleport2");
            teleport_ani_check = false;
            damage_check = false;
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_teleport"))
        {
            ani.ResetTrigger("teleport");
            teleport_ani_check = true;
        }
        //공격에니메이션
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_2"))
        {
            ani.ResetTrigger("attack2");
            GetComponent<boss3_bullet_manager>().attack_ani_check = false;
        }
        //체력이 1이하가 되면 버티기 패턴 후 2페이즈로 전환한다
        if (e_hp <= 1)
        {
            ani.SetTrigger("endure");
            endure = true;
        }

        if (!endure)
        {
            //순간이동을 하면서 나타나며 위아래로 왕복하며 움직인다
            if (b_3_check == 0)
            {
                teleport3();
            }
            else if (b_3_check == 2)
            {
                moveupdown();
            }
        }
        else
        {
            
            transform.position = new Vector3(5, 0, 10);
            Color color = new Vector4(1, 1, 1, 0.3f);
            transform.GetComponent<SpriteRenderer>().color = color;
        }
    }
    public void teleport1()//보스3 순간이동 특수 패턴에 쓰이는 텔레포트1
    {
        ani.ResetTrigger("idle");
        time += Time.deltaTime;
        if (!teleport_check)
        {
            // teleport_sprite로 텔레포트를 예고한다.
            if (time >= 1)
            {
                damage_check = true;
                ani.SetTrigger("teleport");
                ani.ResetTrigger("teleport2");
                teleport_sprite.gameObject.SetActive(true);
                teleport_sprite.transform.position = transform.position;
                teleport_check = true;
                teleport_check2 = false;
                time = 0;
            }
        }
        else
        {
            //보스를 화면 바깥으로 이동시켜서 사라지게 만든다
            if (time >= 0.3)
            {
                if (!teleport_check2)
                {
                    transform.position = new Vector3(15,0,0);
                    teleport_sprite.gameObject.SetActive(false);
                    teleport_check2 = true;
                    time = 0;
                    teleport_check = false;
                    gameObject.GetComponent<boss3_bullet_manager>().teleport_check2=true;
                }
            }
        }
    }
    public void teleport2()//보스3 순간이동 특수 패턴에 쓰이는 텔레포트2
    {
        ani.ResetTrigger("idle");
        time += Time.deltaTime;
        if (!teleport_check)
        {
            // 플레이어의 위치에 teleport_sprite로 텔레포트를 예고한다.
            if (time >= 1)
            {
                
                if (player_ != null) {
                    player_location = player_.transform;
                }
                else
                {
                    player_location = gameObject.transform;
                }
                teleport_sprite.gameObject.SetActive(true);
                teleport_sprite.transform.position = new Vector3(player_location.position.x, player_location.position.y, 0.5f);
               
                teleport_check = true;
                teleport_check2 = false;
                time = 0;
            }
        }
        else
        {
            //체력이 절반이 됐을 때는 순간이동 위치에 4방향으로 레이저를 쏜다
            if (gameObject.GetComponent<boss3_bullet_manager>().rage)
            {
                if (!lazer_check)
                {
                    switch (gameObject.GetComponent<boss3_bullet_manager>().teleport_check)
                    {
                        case 0:
                            for (int i = 0; i <= 3; i++)
                            {
                                GameObject lazer1 = Instantiate(lazer[i], teleport_sprite.transform.position, teleport_sprite.transform.rotation);
                                lazer1.transform.rotation = Quaternion.Euler(0, 0, 90 * i);
                            }
                            
                            break;
                        case 1:
                            for (int i = 0; i <= 3; i++)
                            {
                                GameObject lazer2 = Instantiate(lazer[i], teleport_sprite.transform.position, teleport_sprite.transform.rotation);
                                lazer2.transform.rotation = Quaternion.Euler(0, 0, 90 * i + 45);
                            }
                            break;
                        case 2:
                            for (int i = 0; i <= 3; i++)
                            {
                                GameObject lazer1 = Instantiate(lazer[i], teleport_sprite.transform.position, teleport_sprite.transform.rotation);
                                lazer1.transform.rotation = Quaternion.Euler(0, 0, 90 * i);
                                lazer1.GetComponentInChildren<e_lazer_orbit>().color_time = 0.01f;
                            }
                            for (int i = 0; i <= 3; i++)
                            {
                                GameObject lazer2 = Instantiate(lazer[i], teleport_sprite.transform.position, teleport_sprite.transform.rotation);
                                lazer2.transform.rotation = Quaternion.Euler(0, 0, 90 * i + 45);
                                lazer2.GetComponentInChildren<e_lazer_orbit>().color_time = 0.01f;
                            }
                            break;
                    }
                    lazer_check = true;
                }
            }
            if (time >= 1)
            {
                // teleport_sprite위치로 순간이동 한다
                //teleport_check로 순간이동 횟수를 기록한다
                if (!teleport_check2)
                {
                    ani.SetTrigger("teleport2");
                    ani.ResetTrigger("teleport");
                    transform.position = teleport_sprite.transform.position+new Vector3 (0,0,9.5f);
                    teleport_sprite.gameObject.SetActive(false);
                    teleport_check2 = true;
                    time = 0;
                    teleport_check = false;
                    gameObject.GetComponent<boss3_bullet_manager>().teleport_check2 = false;
                    gameObject.GetComponent<boss3_bullet_manager>().teleport_check++;
                    lazer_check = false;
                }
            }
        }
    }
    public void teleport3()//보스3 개막 순간이동(원위치 순간이동)
    {
        time += Time.deltaTime;
        if (!teleport_check)
        {
            if (time >= 1)
            {
                damage_check = true;
                ani.SetTrigger("teleport");
                teleport_sprite.gameObject.SetActive(true);
                teleport_sprite.transform.position = new Vector3(5f, 0, 10);
                teleport_check = true;
                teleport_check2 = false;
                time = 0;
            }
        }
        else
        {
            if (time >= 1)
            {
                if (!teleport_check2)
                {
                    ani.ResetTrigger("teleport");
                    ani.SetTrigger("teleport2");
                    transform.position = teleport_sprite.transform.position;
                    teleport_sprite.gameObject.SetActive(false);
                    teleport_check2 = true;
                    time = 0;
                    teleport_check = false;
                    b_3_check = 1;
                    ani.SetTrigger("idle");
                }
            }
        }
    }
    public void moveupdown()//보스3 이동 화면을 위아래로 반복하며 이동한다
    {
        if (transform.position.y >= 3)
        {
            Vcheck = true;
        }
        else if (transform.position.y <= -3)
        {
            Vcheck = false;
        }
        if (Vcheck == true)
        {
            transform.Translate(new Vector2(0, -1 * speed * Time.deltaTime));
        }
        else
        {
            transform.Translate(new Vector2(0, 1 * speed * Time.deltaTime));
        }
    }
    public void b_type_4()//3스테이지 보스 2페이즈 이동
    {
        //hp이 0이되면 격파
        if (e_hp <= 0)
        {
            ani.SetTrigger("die");
            clear.GetComponent<Clear_bullet>().run();
            die_check = true;
            game_manager.GetComponent<shooting_GameManager>().audioSource.Stop();
        }
        if (die_ani_check)
        {
            Destroy(this.gameObject);
            game_manager.GetComponent<shooting_GameManager>().clear_phase = true;
        }
        //1페이즈와 연결시켜 보스의 hp가 다시 차오르는 연출을 표현한다
        if (!b3_2_check)
        {
            if (e_hp != 200)
            {
                e_hp+=0.5f;
            }
            else
            {
                b3_2_check = true;
            }

        }
        if (effect_check)
        {
            Destroy(effect.gameObject);
        }
        //2페이즈 등장 연출이 끝나면
        if (b_3_2_intro_animation_check)
        {
            
            ani.SetTrigger("idle");
            e_hp_max = 1000;
            //특정 공격 패턴을 위해 플레이어의 y값을 쫓는 b_4_movement2를 제외하면 위아래로 변칙적인 속도로 움직인다
            if (!b_4_movement)
            {
                b_4_movement1();
            }
            else
            {
                b_4_movement2();
            }
        }
    }
    public void b_4_movement1()//위아래로 왕복 또한 속도가 수시로 바뀐다
    {
        moveupdown();
        speed = Random.Range(-1, 40);
    }
    public void b_4_movement2()//플레이어의 y값으로 보스가 움직인다. 
    {
        // 빠른속도로 플레이어가 못 벗어나도록 공격을 시작할때는 속도가 느리게 되서 플레이어가 벗어날 수 있게
        if (!shoot_mode)
        {
            speed = 15;
        }
        else
        {
            speed = 0.5f;
        }
        //플레이어의 y값을 쫓아간다 플레이어가 없다면 가만히 있는다.
        if (GameObject.FindWithTag("Player")!=null){
            if(transform.position.y<= player_.transform.position.y)
            {
                transform.Translate(new Vector2(0, 1 * speed * Time.deltaTime));
            }
            if (transform.position.y >= player_.transform.position.y)
            {
                transform.Translate(new Vector2(0, -1 * speed * Time.deltaTime));
            }
        }
        else
        {
                speed = 0;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //플레이어의 공격에 3스테이지 2페이즈를 제외한 모든 보스는 피해를 받으며 플레이어의 파워 게이지를 채운다
        if (col.CompareTag("bullet"))
        {
            if (b_type != 4)
            {
                if (!endure)
                {
                    if (!damage_check)
                    {
                        if (!col.GetComponent<Bullet>().hit_check)
                        {
                            e_hp--;
                            if (!player_.GetComponent<shooting_player>().special_power)
                            {
                                player_.GetComponent<shooting_player>().power_gauge++;
                            }
                        }
                        col.GetComponent<Bullet>().hit_animation();
                        col.GetComponent<Bullet>().speed = 0;
                    }
                }
            }
            else// 3스테이지 2페이즈 보스는 피해를 직접적으로 받지는 안지만 파워게이지는 채운다
            {
                if (!player_.GetComponent<shooting_player>().special_power)
                {
                    player_.GetComponent<shooting_player>().power_gauge++;
                }
                col.GetComponent<Bullet>().hit_animation();
                col.GetComponent<Bullet>().speed = 0;
            }

        }
        if (col.CompareTag("ex_bullet"))//특수탄 피해 처리
        {
            if (!endure)
            {
                if (!damage_check)
                {
                    e_hp -= 50;
                }
            }
            

        }
    }
}
