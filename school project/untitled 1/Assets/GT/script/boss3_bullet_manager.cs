using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_bullet_manager : MonoBehaviour
{
    public GameObject lazer;
    public GameObject lazer2;
    public GameObject lazer3;
    public Transform lazer_transform;
    public float time;
    public GameObject Player_location;
    public int pattern_check;
    //
    public int teleport_check;//텔레포트를 몇번했는가
    public bool teleport_check2;//무슨종류의 텔레포트
    public float Radius = 0.1f;
    private float _angle;
    public GameObject bullet2;
    public Transform bullet_location;
    public GameObject magic_circle;
    public GameObject magic_circle_2;
    public bool m_spawn_check;
    public int loop_check;
    public GameObject special_bullet;
    public int pattern5_max;
    public bool rage;
    public bool attack_ani_check;
    // Start is called before the first frame update
    void Start()
    {
        pattern5_max = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<boss_basic>().endure)
        {
            if (GetComponent<boss_basic>().e_hp < GetComponent<boss_basic>().e_hp_max / 2)
            {
                rage = true;
            }
            if (rage)
            {
                pattern5_max = 4;
            }
            time += Time.deltaTime;
            Player_location = GameObject.FindWithTag("Player");
            if (GetComponent<boss_basic>().b_3_check != 0)
            {
                if (time >= 1.5f)
                {
                    switch (pattern_check)
                    {
                        case 0:
                            pattern1();
                            pattern_check++;
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
                        case 4:
                            pattern5();
                            break;
                    }
                }
            }
        }
    }
    public void pattern1()//조준레이저
    {
            GameObject lazer1 = Instantiate(lazer, lazer_transform.position, lazer_transform.rotation);
        lazer1.GetComponent<e_lazer_angle>().player_location = Player_location.transform;
        GetComponent<boss_basic>().ani.SetTrigger("attack2");
            time = 0;
    }
    public void pattern2()//텔포
    {
        if (teleport_check != 3)
        {
            if (!teleport_check2)
            {
                gameObject.GetComponent<boss_basic>().teleport1();
                
            }
            else
            {
                gameObject.GetComponent<boss_basic>().teleport2();
                
                pattern2_2();
            }
        }
        else
        {
            gameObject.GetComponent<boss_basic>().b_3_check=0;
            pattern_check++;
            teleport_check = 0;
            time = 0;
        }
    }
    public void pattern2_2()//텔포탄
    {
        for (int i = 1; i <= 11; i++)
        {
            Radius = 9f;
            _angle = (180f + (0.55f * (i - 1)));

            var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            GameObject e_bullet1 = Instantiate(bullet2, bullet_location.position, bullet_location.rotation);
            e_bullet1.GetComponent<e_bullet_type4>().dir = offset;
            e_bullet1.GetComponent<e_bullet_type4>().speed = 0.5f;
            e_bullet1.transform.localScale = new Vector3(5f, 5f, 5f);
        }
        
    }
    public void pattern3()//마법진
    {
        if (!attack_ani_check)
        {
            GetComponent<boss_basic>().ani.SetTrigger("attack");
            attack_ani_check = true;
        }
        if (!m_spawn_check)
        {
            
            GameObject m_circle = Instantiate(magic_circle, bullet_location.position, bullet_location.rotation);
            if (rage)
            {
                    GameObject m_circle_2 = Instantiate(magic_circle_2, bullet_location.position, bullet_location.rotation);
                    m_spawn_check = true;
                    time = 0;
            }
            else
            {
                m_spawn_check = true;
                time = 0;
            }
        }
        else
        {
            if (time >= 3.5f)
            {
                
                pattern_check++;
                m_spawn_check = false;
                time = 0;
            }
        }
    }
    public void pattern4()
    {
        if (loop_check != 3)
        {
            if (time >= 1)
            {
                if (!rage)
                {
                    pattern1();
                    loop_check++;
                    time = 0;
                }
                else
                {
                    if (loop_check == 2)
                    {
                        GetComponent<boss_basic>().ani.SetTrigger("attack2");
                        GameObject lazer_2 = Instantiate(lazer2, lazer_transform.position, lazer_transform.rotation);
                        loop_check++;
                        time = 0;
                    }
                    else
                    {
                        pattern1();
                        loop_check++;
                    }
                }
            }
        }
        else
        {
            loop_check = 0;
            pattern_check++;
            time = 0;
        }
    }
    public void pattern5()
    {
        if (loop_check != pattern5_max)
        {
            gameObject.GetComponent<boss_basic>().b_3_check=2;
            if (time >= 2.8f)
            {
                GameObject s_bullet = Instantiate(special_bullet, bullet_location.position, bullet_location.rotation);
                loop_check++;
                time = 0;
            }
        }
        else
        {
            if (time >= 4f)
            {
                gameObject.GetComponent<boss_basic>().b_3_check = 1;
                loop_check = 0;
                pattern_check = 1;
                time = 0;
            }
        }
    }
}
