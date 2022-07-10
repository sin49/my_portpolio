using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_specialobject : MonoBehaviour
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
        if (!GetComponentInParent<boss_basic>().endure)
        {
            if (GetComponentInParent<boss3_bullet_manager>().rage)
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
        else
        {
            if (!clear_check)
            {
                clear.GetComponent<Clear_bullet>().run();
                clear_check = true;
            }
            p_location = GameObject.FindWithTag("Player");
            tr = p_location.transform;
            transform.position = tr.position;
            transform.rotation = Quaternion.Euler(0, 0, spin_);
            time += Time.deltaTime;
            time_2 += Time.deltaTime;
            if (time >= 15)
            {
                if (time >= 30)
                {
                    if (time >= 45)//시간이 50이상
                    {
                        if (time >= 50)
                        {
                            if (time >= 55)
                            {
                                game_manager.GetComponent<shooting_GameManager>().audioSource.Stop();
                                clear.GetComponent<Clear_bullet>().run();
                                boss.GetComponent<boss_basic>().ani.SetTrigger("die");
                                Destroy(this.gameObject);
                            }
                            else
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
                        else{
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
                    else//시간이 30이상 40미만
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
                else//시간이 15이상 30미만
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
            else//시간이 15이하
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
