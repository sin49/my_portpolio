using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boss_basic : MonoBehaviour
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
        if (e_hp <= 0)
        {
            e_hp = 0;
        }
        game_manager = GameObject.Find("GameManager");
        player_ = GameObject.FindWithTag("Player");
        if (transform.position.y >= 3)
        {
            Vcheck = true;
        }
        else if (transform.position.y <= -3)
        {
            Vcheck = false;
        }
        
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
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_clear1"))
        {
            ani.SetTrigger("teleport");
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_attack1_1"))
        {
            ani.ResetTrigger("attack");
        }
        
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
    public void b_type_2()//맵을 크게 한바퀴 돌다가 공격을 할때는 멈춘다,공격을 하는 순간은 일정 시간마다
    {
        if (effect_check)
        {
            Destroy(effect.gameObject);
        }
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
        if (!GetComponent<boss2_bulletManager>().shoot_mode)
            shoot_mode_time += Time.deltaTime;
        if (e_hp <= e_hp_max / 2)
        {
            if (e_hp <= 40)
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
                if (shoot_mode_time >= 2)
                {
                    GetComponent<boss2_bulletManager>().shoot_mode = true;
                    shoot_mode_time = 0;
                }
            }
        }
        else
        {
            if (shoot_mode_time >= 4)
            {
                GetComponent<boss2_bulletManager>().shoot_mode = true;
                shoot_mode_time = 0;
            }
        }
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
    public void b_type_3()
    {
        if (b_3_die_ani_check)
        {
            
            Destroy(this.gameObject);
            GetComponentInParent<boss3_>().endure_check = true;
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_1"))
        {
            ani.ResetTrigger("attack");
            GetComponent<boss3_bullet_manager>().attack_ani_check = false;
        }
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
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("boss_2"))
        {
            ani.ResetTrigger("attack2");
            GetComponent<boss3_bullet_manager>().attack_ani_check = false;
        }
        
        if (e_hp <= 1)
        {
            ani.SetTrigger("endure");
            endure = true;
        }
        if (!endure)
        {
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
    public void teleport1()//보스3 순간이동3번 패턴에 쓰이는 텔레포트1
    {
        ani.ResetTrigger("idle");
        time += Time.deltaTime;
        if (!teleport_check)
        {
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
    public void teleport2()//보스3 순간이동3번 패턴에 쓰이는 텔레포트2
    {
        ani.ResetTrigger("idle");
        time += Time.deltaTime;
        if (!teleport_check)
        {
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
    public void moveupdown()//보스3 패턴에 쓰임 위아래 이동
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
    public void b_type_4()
    {
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

        if (b_3_2_intro_animation_check)
        {
            
            ani.SetTrigger("idle");
            e_hp_max = 1000;
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
    public void b_4_movement1()//위아래
    {
        moveupdown();
        speed = Random.Range(-1, 40);
    }
    public void b_4_movement2()//플레이어따라
    {
        if (!shoot_mode)
        {
            speed = 15;
        }
        else
        {
            speed = 0.5f;
        }
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
            else
            {
                if (!player_.GetComponent<shooting_player>().special_power)
                {
                    player_.GetComponent<shooting_player>().power_gauge++;
                }
                col.GetComponent<Bullet>().hit_animation();
                col.GetComponent<Bullet>().speed = 0;
            }

        }
        if (col.CompareTag("ex_bullet"))
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
