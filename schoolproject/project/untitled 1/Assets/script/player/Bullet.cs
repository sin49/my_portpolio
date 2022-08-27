using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour//플레이어의 탄
{
    public float speed = 15;
    public Animator ani;
    public GameObject player_;
    public bool hit_check;
    public GameObject boss;
    // Update is called once per frame
    void Start()
    {
        ani = GetComponent<Animator>();
    }
    public void hit_animation()
    {
        //적중 에니메이션 실행 hit_check로 적이 중복으로 맞지않도록 조절한다.
        hit_check = true;
        ani.SetTrigger("hit");
    }
    void Update()
    {
        //보스 탐색(특수패턴용)
        boss = GameObject.FindWithTag("boss");
        //적중 에니메이션이 끝나면 자괴
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("bullet_hitted"))
        {
            Destroy(this.gameObject);
        }
        //파워 게이지를 채우기 위한 플레이어 탐색
        player_ = GameObject.FindWithTag("Player");
        //오른쪽으로 움직임
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        //화면 밖이면 자괴
        if (transform.position.x > 7.2)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //스테이지 2 보스 특수 탄환 패턴(플레이어의 탄을 막음)
        if (col.CompareTag("ground"))
        {
            if (boss != null)
            {
                if (!boss.GetComponent<boss_basic>().endure)
                {
                    //보스가 아직 살아있는 상태라면 특수 탄환이 플레이어의 탄을 막는다.
                    if (!player_.GetComponent<shooting_player>().special_power)
                        player_.GetComponent<shooting_player>().power_gauge++;//특수 탄환에 접촉해도 파워게이지를 채운다
                    speed = 0;
                    hit_animation();
                }
            }
            
        }
    }
    }
