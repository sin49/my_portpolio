using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_specialobject : MonoBehaviour//3스테이지 보스 주변을 회전하며 플레이어의 공격을 막으며 또한 버티기 패턴을 실행한다
{
    public float spin_;
    public float spin_add;
    public Transform tr;
    public GameObject p_location;
    public float time;
    public float time_2;
    public float time_max=60;
    public Transform[] b_tr;
    public GameObject[] bullet;
    public float radius;
    public bool position_check;
    public bool clear_check;
    public bool clear_check_2;
    public GameObject clear;
    public GameObject boss;
    public GameObject game_manager;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        game_manager = GameObject.Find("GameManager");
        clear = GameObject.FindWithTag("clear");
        if (!GetComponentInParent<boss_basic>().endure)//버티기 패턴에 돌입하지 않았다면 보스 주변을 회전하며 플레이어의 공격을 막는다
        {
            if (GetComponentInParent<boss3_bullet_manager>().rage)//패턴 강화시 더욱 빠른 속도로 회전한다.
            {
                spin_ += spin_add * 6f;
            }
            else
            {
                spin_ += spin_add;
            }

            transform.position = tr.position;
            transform.rotation = Quaternion.Euler(0, 0, spin_);
        }
        else//버티기 패턴 돌입시
        {
            if (!clear_check)
            {
                clear.GetComponent<Clear_bullet>().run();
                clear_check = true;
            }
            p_location = GameObject.FindWithTag("Player");
            //플레이어의 위치를 중심으로 이동하여 회전한다
            tr = p_location.transform;
            transform.position = tr.position;
            transform.rotation = Quaternion.Euler(0, 0, spin_);
            time += Time.deltaTime;
            time_2 += Time.deltaTime;
            //55초 동안 플레이어를 중심으로 회전하며 생성 후 일정시간(time_max)이 지나면 생성 당시의 플레이어의 방향으로 이동하는 탄을 자신의 위치에 생성한다
            //15초 간격으로 패턴이 강화되어 점점 높은 밀도로 탄을 생성한다
            //45초~55초 부터는 연출상의 탄막이며 실질적인 버티기 시간은 45초까지
            if (time >= 15)
            {
                if (time >= 30)
                {
                    if (time >= 45)
                    {
                        if (time >= 50)
                        {
                            if (time >= 55)//55초 이상에는 버티기 패턴을 멈추고 2페이즈로 넘어가며 이 오브젝트는 파괴된다.
                            {
                                game_manager.GetComponent<shooting_GameManager>().audioSource.Stop();
                                clear.GetComponent<Clear_bullet>().run();
                                boss.GetComponent<boss_basic>().ani.SetTrigger("die");
                                Destroy(this.gameObject);
                            }
                            else//50~55초 사이
                            {

                                if (time_2 >= 0.1f)
                                {
                                    for (int i = 0; i <= 3; i++)
                                    {
                                        GameObject e_bullet1 = Instantiate(bullet[i], b_tr[i].position, Quaternion.Euler(0, 0, 0));
                                        e_bullet1.GetComponent<boss3_special_bullet_3>().player_location = tr.position;
                                        e_bullet1.GetComponent<boss3_special_bullet_3>().speed = 0.1f;
                                        e_bullet1.GetComponent<boss3_special_bullet_3>().time_max = 0f;
                                        time_2 = 0;
                                    }
                                }
                                spin_ += spin_add * 30;
                            }
                        }
                        else{//45초~50초 사이
                            if (!clear_check_2)
                            {
                                clear.GetComponent<Clear_bullet>().run();
                                clear_check_2 = true;
                            }
                            if (time_2 >= 0.1f)
                            {
                                for (int i = 0; i <= 3; i++)
                                {
                                    GameObject e_bullet1 = Instantiate(bullet[i], b_tr[i].position, Quaternion.Euler(0, 0, 0));
                                    e_bullet1.GetComponent<boss3_special_bullet_3>().player_location = tr.position;
                                    e_bullet1.GetComponent<boss3_special_bullet_3>().speed = 3f;
                                    e_bullet1.GetComponent<boss3_special_bullet_3>().time_max = 80f;
                                    time_2 = 0;
                                }
                            }
                            spin_ += spin_add * 20;
                        }
                    }
                    else//30~45초 상이
                    {
                        spin_ += spin_add*15;
                        if (time_2 >= 0.8f)
                        {
                            for (int i = 0; i <= 3; i++)
                            {
                                GameObject e_bullet1 = Instantiate(bullet[i], b_tr[i].position, Quaternion.Euler(0, 0, 0));
                                e_bullet1.GetComponent<boss3_special_bullet_3>().player_location = tr.position;
                                e_bullet1.GetComponent<boss3_special_bullet_3>().speed = 2f;
                                e_bullet1.GetComponent<boss3_special_bullet_3>().time_max = 3f;
                                time_2 = 0;
                            }
                        }
                    }
                }
                else//15~30 사이
                {
                    if (time_2 >= 1.4f)
                    {
                        for (int i = 0; i <= 3; i++)
                        {
                            GameObject e_bullet1 = Instantiate(bullet[i], b_tr[i].position, Quaternion.Euler(0, 0, 0));
                            e_bullet1.GetComponent<boss3_special_bullet_3>().player_location = tr.position;
                            e_bullet1.GetComponent<boss3_special_bullet_3>().speed = 2f;
                            e_bullet1.GetComponent<boss3_special_bullet_3>().time_max = 2f;
                            time_2 = 0;
                        }
                    }
                    spin_ += spin_add*10;
                }
            }
            else//0~15 사이
            {
                if (time_2 >= 2)
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        GameObject e_bullet1 = Instantiate(bullet[i], b_tr[i].position, Quaternion.Euler(0, 0, 0));
                        e_bullet1.GetComponent<boss3_special_bullet_3>().player_location = tr.position;
                        e_bullet1.GetComponent<boss3_special_bullet_3>().speed = 2f;
                        e_bullet1.GetComponent<boss3_special_bullet_3>().time_max = 1.5f;
                        time_2 = 0;
                    }
                }
                spin_ += spin_add*5;
                
            }
        }
        if (spin_>=360)
        {
            spin_ = 0;
        }
    }
}
