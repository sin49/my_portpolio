using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_basic : MonoBehaviour
{
    public int e_hp;
    public float speed;
    public bool Vcheck=true;
    public bool Hcheck=true;
    public bool damage_check;
    public int e_type;//적의 종류
    //e_type2
    public Transform enemy_location;
    public GameObject player_location;
    public Vector3 dir;
    public bool onchasing=false;
    public float chasingtime=1;
    public GameObject gamemanager;
    //
    public float time;
    public bool teleport_check;
    public bool teleport_check2;
    public GameObject teleport_sprite;
    public bool die_Check;
    public bool die_ani_check;
    public Animator ani;
    
    void Start()
    {
        ani=GetComponent<Animator>();
    }
    void Update()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("enemy_teleport 0"))
        {
            GetComponent<e_bulletManager>().shoot_mode2 = true;
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("enemy_teleport"))
        {
            damage_check = false;
            GetComponent<e_bulletManager>().shoot_mode2 = false;
            ani.ResetTrigger("teleport2");
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("enemy_die"))
        {
            Destroy(this.gameObject);
        }
        player_location = GameObject.FindWithTag("Player");
        
        if (transform.position.y >= 4)
        {
            Vcheck = true;
        }
        else if (transform.position.y <= -4)
        {
            Vcheck = false;
        }
        
        if (transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }
        if (e_hp <= 0)
        {
            ani.SetTrigger("die");
            die_Check = true;
        }
        if (!die_Check)
        {
            if (!onchasing)
            {

                switch (e_type)
                {
                    case 1:
                        E_type_1();
                        break;
                    case 2:
                        E_type_2();
                        break;
                    case 3:
                        E_type_3();
                        break;
                    case 4:
                        E_type_4();
                        break;
                    case 5:
                        E_type_5();
                        break;
                }
            }
            else
            {
                Chasing();
            }
        }
    }
    public void E_type_1()//왼쪽이동/위아래로 반복하며 이동
    {
        transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
        if (Vcheck == true)
        {
            transform.Translate(new Vector2(0, -1 * speed * Time.deltaTime));
        }
        else
        {
            transform.Translate(new Vector2(0, 1 * speed * Time.deltaTime));
        }
        //
    }
    public void E_type_2()//가로일직선이동
    {
        transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
    }
    public void E_type_3()//순간이동
    {
        time += Time.deltaTime;
        if (!teleport_check)
        {
            if (time >= 1)
            {
                ani.SetTrigger("teleport");
                GetComponent<e_bulletManager>().shoot_mode2 = true;
                teleport_sprite.gameObject.SetActive(true);
                teleport_sprite.transform.position = new Vector3(Random.Range(0.5f, 5.5f), Random.Range(-3.5f, 3.5f), 10);
                teleport_check = true;
                time = 0;
            }
        }
        else
        {
            if (time >= 2)
            {
                if (!teleport_check2)
                {
                    ani.ResetTrigger("teleport");
                    ani.SetTrigger("teleport2");
                    transform.position = teleport_sprite.transform.position;
                    Destroy(teleport_sprite);
                    teleport_check2 = true;
                }
            }
        }
    }
    public void E_type_4()//가로일직선이동특정좌표까지
    {
        if (transform.position.x >= 5.8)
        {
            transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
            gameObject.GetComponent<e_bulletManager>().e_shoot_time = 0;
        }
    }
    public void E_type_5()//계속순간이동
    {
        time += Time.deltaTime;
        if (!teleport_check)
        {
            if (time >= 2)
            {
                ani.SetTrigger("teleport");
                teleport_sprite.gameObject.SetActive(true);
                teleport_sprite.transform.position = new Vector3(Random.Range(0.5f, 5.5f), Random.Range(-4f, 4f), 10);
                teleport_check = true;
                teleport_check2 = false;
                time = 0;
            }
        }
        else
        {
            if (time >= 2)
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
                }
            }
        }
    }
    public void Chasing()
    {
        if (GameObject.FindWithTag("Player"))
        {
            chasingtime += Time.deltaTime;
            dir = player_location.transform.position - enemy_location.position;
            transform.Translate(new Vector2(dir.x * speed * Time.deltaTime / 2, dir.y * speed * Time.deltaTime / 2));
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bullet"))
        {
            if (!die_Check)
            {
                if (transform.position.x < 9)
                {
                    if (!damage_check)
                    {
                        if (!col.GetComponent<Bullet>().hit_check)
                        {
                            e_hp--;

                            if (!player_location.GetComponent<shooting_player>().special_power)
                            {
                                player_location.GetComponent<shooting_player>().power_gauge++;
                            }
                            col.GetComponent<Bullet>().hit_animation();
                            col.GetComponent<Bullet>().speed = 0;
                        }
                    }
                }
            }
        }
        if (col.CompareTag("ex_bullet"))
        {
            if (!die_Check)
            {

                if (!damage_check)
                {
                    e_hp -= 50;
                    
                }
            }

        }
    }
}
