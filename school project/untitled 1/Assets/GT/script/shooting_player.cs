using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shooting_player : MonoBehaviour
{
    public float untouchable_time = 4;
    public GameObject Player_;
    public float untouchable_time_check = 0;
    public bool untouchable_state = false;
    public float power_gauge;
    public float power_gauge_max;
    public Slider power_gauge1;
    public Slider power_gauge2;
    public Slider power_gauge3;
    public Text power_text;
    public float time;
    public bool die_check;
    public bool special_power;
    public float special_power_time;
    public bool player_die_animation_check;
    public bool player_hitted;
    public GameObject clear;
    public bool clear_check;
    public GameObject Gamemanager;
    public bool level3_check;
    Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        clear = GameObject.FindWithTag("clear");
        power_gauge1.maxValue = power_gauge_max / 3;
        power_gauge2.maxValue = power_gauge_max / 3;
        power_gauge3.maxValue = power_gauge_max / 3;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Gamemanager = GameObject.Find("GameManager");
        if (Gamemanager.GetComponent<shooting_GameManager>().level == 3)
        {
            if (!level3_check)
            {
                power_gauge3.gameObject.SetActive(false);
                level3_check = true;
            }

        }
        if (player_die_animation_check)
        {
            Destroy(this.gameObject);
            Player_.GetComponent<Player_spawn>().Life--;
        }
        if (!level3_check)
        {
            if (power_gauge > power_gauge_max)
            {
                power_gauge = power_gauge_max;
            }
        }
        else
        {
            if(power_gauge > (power_gauge_max / 3) * 2)
            {
                power_gauge = (power_gauge_max / 3) * 2;
            }
        }
        if (power_gauge <= power_gauge_max/3)
        {
            power_gauge1.value = power_gauge;
        }
        else
        {
            power_gauge1.value = power_gauge_max/3;
        }
        if (power_gauge >= power_gauge_max/3)
        {
            power_gauge2.value = power_gauge - power_gauge_max/3;
        }else if (power_gauge > (power_gauge_max/3)*2)
        {
            power_gauge2.value = power_gauge_max/3;
        }
        else
        {
            power_gauge2.value = 0;
        }
        if (power_gauge >= (power_gauge_max/3)*2)
        {
            power_gauge3.value = power_gauge - (power_gauge_max / 3) * 2;
        }else if (power_gauge >= power_gauge_max)
        {
            power_gauge3.value = power_gauge_max/3;
        }
        else
        {
            power_gauge3.value = 0;
        }
        if (power_gauge >= power_gauge_max/3)
        {
            if (power_gauge >= (power_gauge_max / 3) * 2)
            {
                if (power_gauge >= power_gauge_max)
                {
                    power_text.text = "3/3";
                }
                else
                {
                    power_text.text = "2/3";
                }
            }
            else
            {
                power_text.text = "1/3";
            }
        }
        else
        {
            power_text.text = "0/3";
        }
            if (untouchable_state == true)
        {
            if (!special_power)
            {

                ani.SetTrigger("untouchable");
                if (die_check == true)
                {
                    if (untouchable_time_check <= untouchable_time / 2)
                    {
                        clear.GetComponent<Clear_bullet>().run2();
                    }
                    else
                    {
                        die_check = false;
                    }
                }
                untouchable_time_check += Time.deltaTime;
                if (untouchable_time_check >= untouchable_time)
                {
                    
                    ani.ResetTrigger("untouchable");
                    un_untouchable();
                    untouchable_time_check = 0;
                }
            }
            else
            {
                ani.SetTrigger("s_power");

                power_gauge -= power_gauge_max / special_power_time* Time.deltaTime;
                    if (power_gauge <= 0)
                    {
                    ani.ResetTrigger("s_power");
                    power_gauge = 0;
                        clear.GetComponent<Clear_bullet>().run2();
                        un_untouchable();
                        special_power = false;
                    }


            }
        }
    }
    public void un_untouchable()
    {
        untouchable_state = false;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("enemy"))
        {
            if (!untouchable_state)
            {
                if (!col.GetComponent<Enemy_basic>().damage_check)
                {
                    ani.SetTrigger("die");
                    player_hitted = true;
                    Destroy(col.gameObject);
                }
            }
        }
        if (col.CompareTag("boss"))
        {
            if (!untouchable_state)
            {
                if (!col.GetComponent<boss_basic>().endure)
                {
                    if (!col.GetComponent<boss_basic>().die_check)
                    {
                        if (!col.GetComponent<boss_basic>().teleport_ani_check)
                        {
                            ani.SetTrigger("die");
                            player_hitted = true;
                        }
                    }
                }
            }
        }
        if (col.CompareTag("e_bullet"))
        {
            if (!untouchable_state)
            {
                ani.SetTrigger("die");
                player_hitted = true;
                Destroy(col.gameObject);
            }
        }
        if (col.CompareTag("lazer"))
        {
            if (!untouchable_state)
            {

                ani.SetTrigger("die");
                player_hitted = true;
            }
        }
        if (col.CompareTag("ground"))
        {
            if (!untouchable_state)
            {

                ani.SetTrigger("die");
                player_hitted = true;
            }
        }
    }
    
}
