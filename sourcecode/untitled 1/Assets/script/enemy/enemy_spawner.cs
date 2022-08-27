using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_spawner : MonoBehaviour//적을 생성하는 스포너 클레스
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
        //hp과 적이 생성되는 위치 그리고 적의 속도를 범위안의 임의의 값으로 조정
        hp = Random.Range(hp_min, hp_max);
        vector = new Vector3(Random.Range(8, 9.5f), Random.Range(-3.4f, 3.4f), 10);
        speed = Random.Range(speed_min, speed_max);
        //스테이지의 진행도가 절반을 넘겼을 경우 spawn_MAX_number를 spawn_MAX_number_2의 값으로 바꿔 생성하는 적의 양을 높여 난이도를 올린다
        if (gamemanager.GetComponent<shooting_GameManager>().phase_time>= gamemanager.GetComponent<shooting_GameManager>().phase_time_max / 2)
        {
            spawn_MAX_number = spawn_MAX_number_2;
        }
        //보스 페이즈 일 경우 적의 생성을 멈춘다
        if (!gamemanager.GetComponent<shooting_GameManager>().boss_phase)
        {
            //일정 시간동안 적을 생성-> respawn_time타임동안 생성없음-> 다시 생성을 반복
            if (spawn_check)
            {
                spawn_time += Time.deltaTime;
            }
            else
            {
                respawn_time += Time.deltaTime;
            }
            // spawn_time은 적이 한번에 생성되는 간격
            if (spawn_time >= 0.5)
            {
                //inspector로 설정한 값을 따라 정해진 적의 정보에 맞춰 적을 생성
                GameObject enemy1 = Instantiate(enemy, vector, spawn_location.rotation);
                enemy1.GetComponent<Enemy_basic>().e_type = give_e_type;
                enemy1.GetComponent<e_bulletManager>().e_bullet_mode = give_e_bullet_type;
                enemy1.GetComponent<Enemy_basic>().e_hp = hp;
                enemy1.GetComponent<Enemy_basic>().speed = speed;
                enemy1.GetComponent<Enemy_basic>().gamemanager = gamemanager;
                spawn_number++;
                spawn_time = 0;
            }
            //spawn_MAX_number만큼 생성 했을시 생성을 멈추고 respawn_time만큼 휴식
            if (spawn_number >= spawn_MAX_number)
            {
                spawn_number = 0;
                spawn_check = false;
            }
            //respawn_time이 지나면 다시 적을 생성
            if (respawn_time >= spawn_time_max)
            {
                spawn_check = true;
                respawn_time = 0;
            }
        }
    }
}
