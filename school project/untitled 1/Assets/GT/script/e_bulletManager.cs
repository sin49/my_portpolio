using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bulletManager : MonoBehaviour
{
    public bool shoot_mode;
    public GameObject e_bullet;
    public GameObject enemy;
    public float e_shoot_time;
    public float e_shoot_time_MAX = 2;
    public GameObject player_location;
    public float posx;
    public float posy;
    public int e_bullet_mode;
    public Transform bulletlocation;
    e_bullet_type2 e_bullet_T2;
    Enemy_basic e_basic;
    //
    public float Radius = 0.1f;
    private float _angle;
    public bool e_shoot_check;
    public bool shoot_mode2;
    //
    public float time;
    public int pattern_check;
    //
    public GameObject spawn_enemy;
    public int spawn_max;
    public Transform spawn_position;
    
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        e_basic = GetComponent<Enemy_basic>();
        e_bullet_T2 = GetComponent<e_bullet_type2>();
    }

    // Update is called once per frame
    void Update()
    {
        player_location = GameObject.FindWithTag("Player");
        e_shoot_time += Time.deltaTime;
        posx = transform.position.x;
        posy = transform.position.y;
        if (posx < -6.5)
        {
            shoot_mode = false;
            enemy.GetComponent<Enemy_basic>().onchasing = false;
        }
        else
        {
            shoot_mode = true;
        }
        if (posx > 6.5)
        {
            shoot_mode = false;
            enemy.GetComponent<Enemy_basic>().onchasing = false;
        }
        else
        {
            shoot_mode = true;
        }
        if (posy < -3.5)
        {
            shoot_mode = false;
            enemy.GetComponent<Enemy_basic>().onchasing = false;
        }
        else
        {
            shoot_mode = true;
          
        }
        if (posy > 3.5)
        {
            shoot_mode = false;
            enemy.GetComponent<Enemy_basic>().onchasing = false;
        }
        else
        {
            shoot_mode = true;
        }
        if (shoot_mode)
        {
            if (!shoot_mode2) {
                if (!GetComponent<Enemy_basic>().die_Check)
                {
                    if (GameObject.FindWithTag("Player"))
                    {
                        if (e_shoot_time >= e_shoot_time_MAX)
                        {
                            switch (e_bullet_mode)
                            {
                                case 1:
                                    e_shoot_mode_1();
                                    e_shoot_time = 0;
                                    break;
                                case 2:
                                    e_shoot_mode_2();
                                    e_shoot_time = 0;
                                    break;
                                case 3:
                                    e_shoot_mode_3();
                                    e_shoot_time = 0;
                                    break;
                                case 4:
                                    e_shoot_mode_4();
                                    break;
                                case 5:
                                    e_shoot_mode_5();
                                    break;
                                case 6:
                                    e_shoot_mode_6();
                                    e_shoot_time = 0;
                                    break;
                                case 7:
                                    e_shoot_mode_7();
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
    public void e_shoot_mode_1()//직선으로 쏜다
    {
        GameObject e_bullet1 = Instantiate(e_bullet, bulletlocation.position, bulletlocation.rotation);
        e_bullet1.GetComponent<e_bullet_type2>().player_location = bulletlocation.position+new Vector3(-5,0,0);
        e_bullet1.transform.localScale = new Vector3(15f, 15f, 15f);
    }
    public void e_shoot_mode_2()//조준탄
    {
        GameObject e_bullet1 = Instantiate(e_bullet, bulletlocation.position, bulletlocation.rotation);
        e_bullet1.GetComponent<e_bullet_type2>().player_location = player_location.transform.position;
    }
    public void e_shoot_mode_3()//3way탄
    {
        GameObject e_bullet1 = Instantiate(e_bullet, bulletlocation.position, bulletlocation.rotation);
        e_bullet1.GetComponent<e_bullet_type2>().player_location = player_location.transform.position;
        e_bullet1.GetComponent<e_bullet_type2>().speed = 0.9f;
        GameObject e_bullet2 = Instantiate(e_bullet, bulletlocation.position, bulletlocation.rotation);
        e_bullet2.GetComponent<e_bullet_type2>().player_location = player_location.transform.position+new Vector3(0,3.3f,0);
        e_bullet2.GetComponent<e_bullet_type2>().speed = 0.8f;
        GameObject e_bullet3 = Instantiate(e_bullet, bulletlocation.position, bulletlocation.rotation);
        e_bullet3.GetComponent<e_bullet_type2>().player_location = player_location.transform.position + new Vector3(0, -3.3f, 0);
        e_bullet3.GetComponent<e_bullet_type2>().speed = 0.8f;
    }
    public void e_shoot_mode_4()//확산탄쓴 후 돌진명령
    {
        if (!e_shoot_check)
        {
            for (int i = 1; i <= 6; i++)
            {
                Radius = 9f;
                _angle = 180f + (0.9f * (i - 1));

                var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
                GameObject e_bullet1 = Instantiate(e_bullet, bulletlocation.position, bulletlocation.rotation);
                e_bullet1.GetComponent<e_bullet_type4>().dir = offset;
                e_bullet1.GetComponent<e_bullet_type4>().speed = 0.5f;
                e_bullet1.transform.localScale = new Vector3(9f, 9f, 9f);
            }
            gameObject.GetComponent<Enemy_basic>().onchasing = true;
            gameObject.GetComponent<Enemy_basic>().speed = 4;
            gameObject.GetComponent<Enemy_basic>().e_type = 2;
            e_shoot_check = true;
        }
    }
    public void e_shoot_mode_5()//조준 줄줄이
    {
        time += Time.deltaTime;
        if (pattern_check != 4)
        {
            if (time >= 0.4)
            {
                GameObject e_bullet1 = Instantiate(e_bullet, bulletlocation.position, bulletlocation.rotation);
                e_bullet1.GetComponent<e_bullet_type2>().player_location = player_location.transform.position;
                e_bullet1.GetComponent<e_bullet_type2>().speed = 0.5f;
                time = 0;
                pattern_check++;
            }
        }
        else
        {
            e_shoot_time_MAX = 3f;
            e_shoot_time = 0;
            pattern_check = 0;
        }
    }
    public void e_shoot_mode_6()
    {
        e_shoot_time_MAX = 1f;
        GameObject e_bullet1 = Instantiate(e_bullet, bulletlocation.position, bulletlocation.rotation);
        e_bullet1.GetComponent<e_bullet_type2>().player_location = new Vector3(Random.Range(-9,9),Random.Range(-4.5f,4.5f), 10);
        e_bullet1.GetComponent<e_bullet_type2>().speed = 0.6f;
        e_bullet1.transform.localScale = new Vector3(3f, 3f, 3f);
    }
    public void e_shoot_mode_7()
    {
        time += Time.deltaTime;
        if (spawn_max != 10)
        {
            if (time >= 0.3)
            {
                GameObject s_enemy = Instantiate(spawn_enemy, spawn_position.position, spawn_position.rotation);
                s_enemy.GetComponent<Enemy_basic>().onchasing = true;
                s_enemy.GetComponent<Enemy_basic>().e_type = 2;
                s_enemy.GetComponent<Enemy_basic>().speed = 4f;
                s_enemy.GetComponent<Enemy_basic>().e_hp = 1;
                GameObject s_enemy2 = Instantiate(spawn_enemy, bulletlocation.position, bulletlocation.rotation);
                s_enemy2.GetComponent<Enemy_basic>().onchasing = true;
                s_enemy2.GetComponent<Enemy_basic>().e_type = 2;
                s_enemy2.GetComponent<Enemy_basic>().speed = 4f;
                s_enemy2.GetComponent<Enemy_basic>().e_hp = 1;
                spawn_max++;
                time = 0;
            }
        }
        else
        {
            e_shoot_time = 0;
            spawn_max = 0;
        }
    }
}
