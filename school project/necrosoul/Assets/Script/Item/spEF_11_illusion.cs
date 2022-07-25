using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spEF_11_illusion : MonoBehaviour//Ư�������� 11��:�̳� Ŭ����
{
    public int HP;//�ǰ� ���� Ƚ��
    public float untouchable_time;
    public float untouchable_timer;
    Animator ani;
    public float life_timer;
    public float life_time;//���ӽð�
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
        HP = 10;//�ǰ� Ƚ��
        life_time = 10f;//���� �ð�
        life_timer = life_time;
        
    }
    void Update()
    {
        //���� �� ��ŭ �ǰݴ��ϸ� ��Ȱ��ȭ
        if (HP < 0)
        {
            this.gameObject.SetActive(false);
        }
        //�ǰ� ���� �� ª�� �����ð� ����
        if (untouchable_timer > 0)
        {
            untouchable_timer -= Time.deltaTime;
            ani.SetBool("hitted", true);
        }
        else
        {
            ani.SetBool("hitted", true);
        }
        // ���ӽð��� ������ ��Ȱ��ȭ
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
        //���� ���ݿ� ����
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
