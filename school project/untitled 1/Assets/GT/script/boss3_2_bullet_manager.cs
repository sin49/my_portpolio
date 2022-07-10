using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_2_bullet_manager : MonoBehaviour
{
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;
    public GameObject bullet5;
    public int pattern_check;
    public float time;
    public float time3;
    public float time2;
    public int bullet_check;
    public GameObject player;
    public GameObject b_3_hit;
    // Start is called before the first frame update
    void Start()
    {
        b_3_hit.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<boss_basic>().die_check)
        {
            if (GetComponent<boss_basic>().b_3_2_intro_animation_check)
            {
                player = GameObject.FindWithTag("Player");
                time += Time.deltaTime;
                time2 += Time.deltaTime;
                time3 += Time.deltaTime;
                if (time2 >= 0.3f)
                {
                    if (pattern_check != 3)
                    {
                        GameObject e_bullet1 = Instantiate(bullet1, transform.position, transform.rotation);
                        e_bullet1.GetComponent<e_bullet_type2>().player_location = new Vector3(Random.Range(-9, 7f), Random.Range(-4.5f, 4.5f), 10);
                        e_bullet1.GetComponent<e_bullet_type2>().speed = Random.Range(0.5f, 0.7f);
                        float i = (Random.Range(5f, 8f));
                        e_bullet1.transform.localScale = new Vector3(i, i, i);

                    }
                    time2 = 0;
                }
                if (time >= 4f)
                {
                    switch (pattern_check)
                    {
                        case 0:
                            pattern1();
                            break;
                        case 1:
                            pattern2();
                            break;
                        case 2:
                            pattern3();
                            break;
                        case 3:
                            pattern4();
                            break;

                    }
                }
            }
        }
    }
    public void pattern1()//튕김
    {
        if (bullet_check != 8) {
            if (time3 >= 1.5f)
            {
                GameObject e_bullet = Instantiate(bullet2, transform.position, transform.rotation);
                e_bullet.GetComponent<boss3_2_special_bullet>().speed = Random.Range(5, 10);
                e_bullet.GetComponent<boss3_2_special_bullet>().Vcheck = Random.Range(0, 2);
                time3 = 0;
                bullet_check++;
            }
        }
        else
        {
            bullet_check = 0;
            pattern_check++;
            time = 0;
        }
    }
    public void pattern2()//탱탱볼
    {
            if (bullet_check != 8)
            {
                if (time3 >= 2f)
                {
                    GameObject e_bullet = Instantiate(bullet3, transform.position, transform.rotation);
                    e_bullet.GetComponent<e_bullet_type7>().speed = Random.Range(4, 10);
                    time3 = 0;
                    bullet_check++;
                }
            }
            else
            {
            bullet_check = 0;
            pattern_check++;
                time = 0;
            }
    }
    public void pattern3()//바퀴
    {
        if (bullet_check != 6)
        {
            if (time3 >= 4f)
            {
                if (bullet_check <= 2)
                {
                    GameObject e_bullet = Instantiate(bullet4, transform.position+new Vector3(5,0,0), transform.rotation);
                    e_bullet.GetComponent<b_3_2_specialbullet4>().speed = Random.Range(2f, 5f);
                    float a = Random.Range(0, 2);
                    float b = 1;
                    if (a < 1)
                    {
                        b = -1;
                    }
                    e_bullet.GetComponentInChildren<spinner_spin>().spin_add = Random.Range(1, 2)*b;
                    time3 = 0;
                    bullet_check++;
                }
                else
                {
                    GameObject e_bullet = Instantiate(bullet5, transform.position, transform.rotation);
                    e_bullet.GetComponent<b_3_2_special_bullet5>().speed = 0.0001f;
                    e_bullet.GetComponent<b_3_2_special_bullet5>().player_location = player.transform.position;
                    //플레이어를 향해
                    time3 = 0;
                    bullet_check++;
                }
            }
        }
        else
        {
            bullet_check = 0;
            pattern_check++;
            time = 0;
        }
    }
    public void pattern4()
    {
        if (time <= 8)
        {
            GetComponent<boss_basic>().ani.SetTrigger("attack");
            GetComponent<boss_basic>().b_4_movement = true;
            b_3_hit.gameObject.SetActive(true);
            b_3_hit.GetComponent<b_3_2_hit>().StartCoroutine("makecolor");
        }
        else
        {
            if (time >= 12)
            {
                if (time > 14)
                {
                    GetComponent<boss_basic>().b_4_movement = false;
                    GetComponent<boss_basic>().shoot_mode = false;
                    GetComponent<boss_basic>().ani.ResetTrigger("attack");
                    pattern_check =0;
                    time = 0;
                    b_3_hit.GetComponent<b_3_2_hit>().StopCoroutine("makecolor");
                    b_3_hit.GetComponent<b_3_2_hit>().color_change2();
                    b_3_hit.gameObject.SetActive(false);
                }
            }
            else {
                if (time > 9)//9미만5초과
                {
                    if (time3 >= 0.15f)
                    {
                        GameObject e_bullet = Instantiate(bullet1, b_3_hit.transform.position, b_3_hit.transform.rotation);
                        e_bullet.GetComponent<e_bullet_type2>().player_location = transform.position + new Vector3(-5, 0, 0);
                        e_bullet.GetComponent<e_bullet_type2>().speed = 6;
                        e_bullet.transform.localScale = new Vector3(20f, 20f, 20f);
                        time3 = 0;
                    }
                    else
                    {
                        GetComponent<boss_basic>().shoot_mode = true;
                    }
                }
                
            }
        }
    }
}
