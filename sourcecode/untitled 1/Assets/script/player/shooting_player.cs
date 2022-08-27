using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shooting_player : MonoBehaviour//플레이어와 관련된 상태 값&&조작 이외를 처리하는 클레스
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
        //clear= 탄+적 지우개
        clear = GameObject.FindWithTag("clear");
        //파워 게이지 1칸의 최대값=파워 게이지 전체 값/파워 최대 갯수
        power_gauge1.maxValue = power_gauge_max / 3;
        power_gauge2.maxValue = power_gauge_max / 3;
        power_gauge3.maxValue = power_gauge_max / 3;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Gamemanager = GameObject.Find("GameManager");
        // level 3의 스테이지 기믹으로 파워 최대 갯수가 3게에서 2개로 줄어든다
        if (Gamemanager.GetComponent<shooting_GameManager>().level == 3)
        {
            if (!level3_check)
            {
                //3번째 파워 게이지 비활성화
                power_gauge3.gameObject.SetActive(false);
                level3_check = true;
            }

        }
        //플레이어의 사망 에니메이션이 끝났다면 플레이어를 파괴하고 Life를 하나 줄인다
        if (player_die_animation_check)
        {
            Destroy(this.gameObject);
            Player_.GetComponent<Player_spawn>().Life--;
        }
        //파워 수치가 최대 파워 수치 이상의 값을 가질려고 하는 경우 최대 파워 수치로 고정시킨다
        if (!level3_check)//일반
        {
            if (power_gauge > power_gauge_max)
            {
                power_gauge = power_gauge_max;
            }
        }
        else//level 3
        {
            if(power_gauge > (power_gauge_max / 3) * 2)
            {
                power_gauge = (power_gauge_max / 3) * 2;
            }
        }
        // 현재 파워가 하나 이상일경우  첫번째 파워 수치 ui의 값을 최대로 한다
        if (power_gauge <= power_gauge_max/3)
        {
            power_gauge1.value = power_gauge;
        }
        //현재 파워가 하나도 없을 경우 첫번째 파워 수치 ui의 값을 변경 한다.
        else
        {
            power_gauge1.value = power_gauge_max/3;
        }
        //현재 파워가 하나 이상일 경우 두번째 파워 수치 ui의 값을 변경 한다. 
        if (power_gauge >= power_gauge_max/3)
        {
            power_gauge2.value = power_gauge - power_gauge_max/3;

        }
        //현재 파워가 두개 이상일 경우 두번째 파워 수치 ui의 값을  최대로 한다
        else if (power_gauge > (power_gauge_max/3)*2)
        {
            power_gauge2.value = power_gauge_max/3;
        }
        //현재 파워가 하나도 없을 경우 두번째 파워 수치 ui의 값을 0으로 한다.
        else
        {
            power_gauge2.value = 0;
        }
        //현재 파워가 두게 이상일 경우 세번째 파워 수치 ui의 값을 변경 한다.
        if (power_gauge >= (power_gauge_max/3)*2)
        {
            power_gauge3.value = power_gauge - (power_gauge_max / 3) * 2;
        }
        //현재 파워가 3개일 경우 세번째 파워 수치 ui의 값을 최대로 한다.
        else if (power_gauge >= power_gauge_max)
        {
            power_gauge3.value = power_gauge_max/3;
        }
        //현재 파워가 3개가 아닐 경우 세번째 파워 수치 ui의 값을 0으로 한다.
        else
        {
            power_gauge3.value = 0;
        }
        //파워 수치에 따라 현재 파워의 갯수를 text로 표시한다
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
        //무적 상태인지를 bool값으로 체크
            if (untouchable_state == true)
        {
            //플레이어가 죽고 재생성되면서 무적 상태가 된 경우
            if (!special_power)
            {

                ani.SetTrigger("untouchable");
                // 절반의 무적시간 동안 화면의 탄을 모두 없앤다(리스폰 에니메이션 처리)
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
                //무적시간이 다 됐을 때 무적을 해재한다.
                if (untouchable_time_check >= untouchable_time)
                {
                    
                    ani.ResetTrigger("untouchable");
                    un_untouchable();
                    untouchable_time_check = 0;
                }
            }
            //플레이어가 파워를 소모해 강화 상태가 된 경우
            else
            {
                ani.SetTrigger("s_power");
                //power_gauge가 special_power_time에 맞춰 값이 줄어들면서 파워 수치의 ui의 값이 점점 줄어드는 연출
                power_gauge -= power_gauge_max / special_power_time* Time.deltaTime;
                // special_power_time이 만큼의 시간이 지난 후 무적 해재
                if (power_gauge <= 0)
                    {
                    ani.ResetTrigger("s_power");
                    power_gauge = 0;
                    //무적이 해재되는 순간 화면의 탄을 모두 없앤다
                    clear.GetComponent<Clear_bullet>().run2();
                        un_untouchable();
                        special_power = false;
                    }


            }
        }
    }
    public void un_untouchable()
    {
        //무적을 해재(bool값 사용)
        untouchable_state = false;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //무적이 아닐때 적에게 닿을 경우 사망 처리
        if (col.CompareTag("enemy"))
        {
            if (!untouchable_state)
            {
                if (!col.GetComponent<Enemy_basic>().damage_check)
                {
                    ani.SetTrigger("die");
                    player_hitted = true;
                    //닿은 적은 파괴
                    Destroy(col.gameObject);
                }
            }
        }
        //무적이 아닐때 보스에게 닿을 경우 사망 처리
        if (col.CompareTag("boss"))
        {
            if (!untouchable_state)
            {
                //버티기 특수 패턴+텔레포트 에니메이션 실행 중+보스 사망 에니메이션 처리 중에는 비활서화
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
        //무적이 아닐때 탄에 닿을 경우 사망 처리
        if (col.CompareTag("e_bullet"))
        {
            if (!untouchable_state)
            {
                ani.SetTrigger("die");
                player_hitted = true;
                Destroy(col.gameObject);   //닿은 탄은 파괴
            }
        }
        //무적이 아닐때 레이저에 닿을 경우 사망 처리
        if (col.CompareTag("lazer"))
        {
            if (!untouchable_state)
            {

                ani.SetTrigger("die");
                player_hitted = true;
            }
        }
        //무적이 아닐때 ground태그의 특수 탄에 닿을 경우 사망 처리
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
