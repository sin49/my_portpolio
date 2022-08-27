using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spEF_11_illusion : MonoBehaviour//특수아이템 11번:미끼 클레스
{
    public int HP;//피격 가능 횟수
    public float untouchable_time;
    public float untouchable_timer;
    Animator ani;
    public float life_timer;
    public float life_time;//지속시간
    List<Unit> Enemies;
    GameObject[] E;
    private void Awake()
    {
        ani = this.GetComponent<Animator>();
        HP = 10;
        untouchable_time = Player_status.p_status.get_untouchable_time();
    }
  
    private void OnEnable()
    {
        HP = 10;//피격 횟수
        life_time = 10f;//지속 시간
        life_timer = life_time;
        
    }
    void Update()
    {
        //일정 수 만큼 피격당하면 비활성화
        if (HP < 0)
        {
            this.gameObject.SetActive(false);
        }
        //피격 했을 때 짧은 무적시간 존재
        if (untouchable_timer > 0)
        {
            untouchable_timer -= Time.deltaTime;
            ani.SetBool("hitted", true);
        }
        else
        {
            ani.SetBool("hitted", true);
        }
        // 지속시간이 지나면 비활성화
        if (life_timer >0)
        {
            life_timer -= Time.deltaTime;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //적의 공격에 반응
        if (collision.CompareTag("Enemy"))
        {
            if (untouchable_timer <= 0)
            {
                HP--;
                untouchable_timer = untouchable_time;
                
            }
        }else if (collision.CompareTag("Enemy_bullet"))
        {
            if (untouchable_timer <= 0)
            {
                HP--;
                untouchable_timer = untouchable_time;
                Destroy(collision.gameObject);
            }
            
        }
    }

}
