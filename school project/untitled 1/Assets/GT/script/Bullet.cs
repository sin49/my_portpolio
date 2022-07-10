using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        hit_check = true;
        ani.SetTrigger("hit");
    }
    void Update()
    {
        boss = GameObject.FindWithTag("boss");
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("bullet_hitted"))
        {
            Destroy(this.gameObject);
        }
            player_ = GameObject.FindWithTag("Player");
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        if (transform.position.x > 7.2)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("ground"))
        {
            if (boss != null)
            {
                if (!boss.GetComponent<boss_basic>().endure)
                {
                    if (!player_.GetComponent<shooting_player>().special_power)
                        player_.GetComponent<shooting_player>().power_gauge++;
                    speed = 0;
                    hit_animation();
                }
            }
            
        }
    }
    }
