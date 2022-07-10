using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss1_bulletmaster : MonoBehaviour
{
    public bool shoot_mode;
    public GameObject bullet;
    public GameObject bullet2;
    public Transform bullet_location_1;
    public Transform bullet_location_2;
    public Transform bullet_location_3;
    public GameObject player_location;
    //
    public float pattern1_time_check=-2;
    public float pattern1_loop;
    public float enemy_time;
    public bool pattern1_break;
    public int pattern1_check;
    public float Radius = 0.1f;
    private float _angle;
    //
    public int pattern2_bullet_check;
    public float pattern2_time_check;
    public float bullet2_angle = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<boss_basic>().die_check)
        {
            player_location = GameObject.FindWithTag("Player");
            if (!gameObject.GetComponent<boss_basic>().Hcheck)
            {
                pattern1_time_check += Time.deltaTime;
                pattern2_time_check += Time.deltaTime;
                if (!pattern1_break)
                {
                    if (pattern1_time_check >= 2)
                    {
                        enemy_time += Time.deltaTime;
                        if (pattern1_check <= 5)
                        {
                            pattern1();
                            GetComponent<boss_basic>().ani.SetTrigger("attack");
                        }
                    }
                    else
                    {
                        if (GetComponent<boss_basic>().e_hp < GetComponent<boss_basic>().e_hp_max / 2)
                        {
                            if (enemy_time <= 1)
                            {
                                enemy_time = 0;

                            }
                        }
                        else
                        {
                            if (enemy_time <= 4)
                            {
                                enemy_time = 0;

                            }
                        }
                    }
                    if (pattern1_check == 5)
                    {
                        pattern1_loop++;
                        pattern1_check = 0;
                        pattern1_time_check = 0;
                    }

                    if (pattern1_loop == 3)
                    {
                        pattern1_loop = 0;
                        pattern1_break = true;
                    }
                }
                else
                {
                    if (GetComponent<boss_basic>().e_hp < GetComponent<boss_basic>().e_hp_max / 2)
                    {
                        pattern1_break = false;
                    }
                    else
                    {
                        if (pattern1_time_check >= 2)
                        {
                            pattern1_break = false;
                            pattern1_time_check = 0;
                        }

                    }

                }
                if (GetComponent<boss_basic>().e_hp < GetComponent<boss_basic>().e_hp_max / 2)
                {
                    if (pattern2_time_check >= 0.25)
                    {
                        pattern2();
                        pattern2_time_check = 0;
                    }
                }
                else
                {
                    if (pattern2_time_check >= 0.5)
                    {
                        pattern2();
                        pattern2_time_check = 0;
                    }
                }
            }
        }
    }
    public void pattern1()//적의 윗쪽과 아래 쪽에 속도가 다른 조준탄 5개가 나온다
    {
        if (enemy_time >= 0.2)
        {
                GameObject e_bullet1 = Instantiate(bullet, bullet_location_2.position, bullet_location_2.rotation);
                e_bullet1.GetComponent<e_bullet_type2>().player_location=player_location.transform.position;
            
            e_bullet1.GetComponent<e_bullet_type2>().speed = 0.8f;

                GameObject e_bullet2 = Instantiate(bullet, bullet_location_3.position, bullet_location_3.rotation);
                e_bullet2.GetComponent<e_bullet_type2>().player_location=player_location.transform.position;
                e_bullet2.GetComponent<e_bullet_type2>().speed = 0.8f;
            enemy_time = 0;
            pattern1_check++;
        }
    }
    public void pattern2() {
        if (pattern2_bullet_check==0)
        {
            for (int i = 1; i <= 11; i++)
            {
                Radius = 9f;
                _angle = (180f+bullet2_angle)+(0.55f*(i-1));
                
                var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
                GameObject e_bullet1 = Instantiate(bullet2, bullet_location_2.position, bullet_location_2.rotation);
                e_bullet1.GetComponent<e_bullet_type4>().dir = offset;
                e_bullet1.GetComponent<e_bullet_type4>().speed = 0.5f;
                e_bullet1.transform.localScale = new Vector3(6f, 6f, 6f);
            }
            bullet2_angle += 0.25f;
        }
        
    }//원형
}
