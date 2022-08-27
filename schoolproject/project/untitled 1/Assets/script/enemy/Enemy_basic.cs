using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_basic : MonoBehaviour//적의 정보와 기본 행동을 결정하는 ai 클레스
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
        //적이 텔레포트 하는 경우 텔레포트 에니메이션이 끝나기 전까지는 공격하지 않고 공격받지 않는다.
        //enemy_teleport 0=정해진 위치에서 텔레포트하면서 나오는 에니메이션
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("enemy_teleport 0"))
        {
            GetComponent<e_bulletManager>().shoot_mode2 = true;
        }
        //enemy_teleport=현재 위치에서 텔레포트하기위해 사라지는 에니메이션
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("enemy_teleport"))
        {
            damage_check = false;
            GetComponent<e_bulletManager>().shoot_mode2 = false;
            ani.ResetTrigger("teleport2");
        }
        //적이 죽는 에니메이션이 끝날을 때 이 오브젝트를 파괴한다.
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("enemy_die"))
        {
            Destroy(this.gameObject);
        }
        player_location = GameObject.FindWithTag("Player");
        //위아래 양끝으로 반복해서 이동하기 위한 bool자료형의 조건 조정
        if (transform.position.y >= 4)
        {
            Vcheck = true;
        }
        else if (transform.position.y <= -4)
        {
            Vcheck = false;
        }
        //화면 왼쪽끝으로 완전히 사라질시 파괴
        if (transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }
        //hp이 0이되면 사망에니메이션을 실행 죽고있는 상태임을  die_Check로 표시 하여 공격ai+이동 ai+충돌을 비활성화
        if (e_hp <= 0)
        {
            ani.SetTrigger("die");
            die_Check = true;
        }
        if (!die_Check)//죽지 않았을 때
        {
            if (!onchasing)//플레이어를 추격하는 ai가 아닐 경우
            {
                //e_type의 값에 따라 원하는 이동 ai를 실행시킨다.
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
                //플레이어를 추격
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
    public void E_type_3()//힌반민 순간이동
    {
        time += Time.deltaTime;
        //teleport_check의 bool 값으로 적의 순간이동 여부를 확인
        if (!teleport_check)
        {
            //1초 뒤 임의의 위치에 텔레포트를 하는 위치를 예고하는 이펙트를 생성+적이 사라지는 에니메이션 실행
            if (time >= 1)
            {
                ani.SetTrigger("teleport");
                GetComponent<e_bulletManager>().shoot_mode2 = true;
                teleport_sprite.gameObject.SetActive(true);
                //순간이동 위치는 화면 오른 편의 임의의 위치
                teleport_sprite.transform.position = new Vector3(Random.Range(0.5f, 5.5f), Random.Range(-3.5f, 3.5f), 10);
                teleport_check = true;
                time = 0;
            }
        }
        else
        {
            //2초 뒤 텔레포트의 위치를 예고하는 이펙트의 위치로 적을 이동+사라진 적이 나타나는 텔레포트로 나타나는 에니메이션 실행
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
    public void E_type_4()//특정x값까지 가로로 일직선이동
    {
        if (transform.position.x >= 5.8)
        {
            transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
            gameObject.GetComponent<e_bulletManager>().e_shoot_time = 0;
        }
    }
    public void E_type_5()//계속 순간이동
    {
        time += Time.deltaTime;
        //e_type_3과 동일하나 teleport_check를 수시로 true 와 false로 전환시키면서 계속 순간이동 하도록
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
    //플레이어의 위치로 이동하면서 추격하는 ai
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
        //플레이어의 탄에 닿았을 때
        if (col.CompareTag("bullet"))
        {
            if (!die_Check)//죽는 중이 아니라면
            {
                if (transform.position.x < 9)//적이 화면 오른쪽 바깥에 위치한게 아니라면
                {
                    if (!damage_check)//공격 받을 수 있는 상태라면
                    {
                        if (!col.GetComponent<Bullet>().hit_check)//플레이어의 탄이 적에게 닿은 적이 없다면
                        {
                            //적의 hp를 깍고 플레이어의 파워 수치를 증가 +플레이어의 탄에게 적중에니메이션을 실행
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
        if (col.CompareTag("ex_bullet"))//특수 탄에 닿았을 때 
        {
            if (!die_Check)
            {
                //데미지 처리+특수탄은 파괴되지않고 적을 관통
                if (!damage_check)
                {
                    e_hp -= 50;
                    
                }
            }

        }
    }
}
