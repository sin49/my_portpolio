using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_spawner : MonoBehaviour
{
    public float spawn_time;
    public int hp;
    public int hp_min;
    public int hp_max;
    public int spawn_number;
    public float speed;
    public float speed_min;
    public float speed_max;
    public float spawn_time_max=3;
    public int spawn_MAX_number;
    public int spawn_MAX_number_2;
    public float respawn_time;
    public bool spawn_check=true;
    public int give_e_bullet_type;
    public Transform spawn_location;
    public GameObject enemy;
    public Transform Player_location3;
    Vector3 vector;
    public GameObject gamemanager;
    public int give_e_type;
    void Start()
    {
        
    }
    void Update()
    {
        hp = Random.Range(hp_min, hp_max);
        vector = new Vector3(Random.Range(8, 9.5f), Random.Range(-3.4f, 3.4f), 10);
        speed = Random.Range(speed_min, speed_max);
        if(gamemanager.GetComponent<shooting_GameManager>().phase_time>= gamemanager.GetComponent<shooting_GameManager>().phase_time_max / 2)
        {
            spawn_MAX_number = spawn_MAX_number_2;
        }
        if (!gamemanager.GetComponent<shooting_GameManager>().boss_phase)
        {
            if (spawn_check)
            {
                spawn_time += Time.deltaTime;
            }
            else
            {
                respawn_time += Time.deltaTime;
            }
            if (spawn_time >= 0.5)
            {
                GameObject enemy1 = Instantiate(enemy, vector, spawn_location.rotation);
                enemy1.GetComponent<Enemy_basic>().e_type = give_e_type;
                enemy1.GetComponent<e_bulletManager>().e_bullet_mode = give_e_bullet_type;
                enemy1.GetComponent<Enemy_basic>().e_hp = hp;
                enemy1.GetComponent<Enemy_basic>().speed = speed;
                enemy1.GetComponent<Enemy_basic>().gamemanager = gamemanager;
                spawn_number++;
                spawn_time = 0;
            }
            if (spawn_number >= spawn_MAX_number)
            {
                spawn_number = 0;
                spawn_check = false;
            }
            if (respawn_time >= spawn_time_max)
            {
                spawn_check = true;
                respawn_time = 0;
            }
        }
    }
}
