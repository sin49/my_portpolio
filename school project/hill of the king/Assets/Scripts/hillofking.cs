using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hillofking : MonoBehaviour//거점
{
    public int hillstate;//0=중립 1=빨강팀 2=파랑팀
    public float hill_time;//타이머
    public float red_hill_time;
    public float blue_hill_time;
    public float conquer_time;//점령 시간
    public float red_conquer_time;
    public float blue_conquer_time;
    public bool red_conquer_state;
    public bool blue_conquer_state;
    public Material normal_material;
    public Material blue_material;
    public Material red_material;
    public GameManager gameManager;
    public hillofkingui hok_ui;
    public gameendui game_set_ui;
    public GameObject floor;
    public Material floor_normal;
    public Material floor_red;
    public Material floor_blue;
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        red_hill_time = hill_time;
        blue_hill_time = hill_time;
        game_set_ui.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null)
        {
            if (gameManager.game_set)
            {
                hok_ui.gameObject.SetActive(false);
                game_set_ui.gameObject.SetActive(true);
                return;
            }
            else
            {
                hok_ui.gameObject.SetActive(true);
            }
        }
        if (blue_conquer_time <= 0)//최대값 정하기
        {
            blue_conquer_time = 0;
        }
        if (red_conquer_time <= 0)
        {
            red_conquer_time = 0;
        }
        if (red_conquer_time >= conquer_time)
        {
            red_conquer_time = conquer_time;
        }
        if (blue_conquer_time >= conquer_time)
        {
            blue_conquer_time = conquer_time;
        }
        if (hillstate == 1)//빨강팀이 점령 중일 때
        {
            GetComponent<MeshRenderer>().material = red_material;
            floor.GetComponent<MeshRenderer>().material = floor_red;
            red_hill_time -= Time.deltaTime;
            if (red_hill_time <= 0)
            {
                red_hill_time = 0;
                gameManager.red_win();
                //빨강팀 우승!
            }
        }else if (hillstate == 2)//파랑팀이 점령 중일 때
        {
            GetComponent<MeshRenderer>().material = blue_material;
            floor.GetComponent<MeshRenderer>().material = floor_blue;
            blue_hill_time -= Time.deltaTime;
            if (blue_hill_time <= 0)
            {
                blue_hill_time = 0;
                gameManager.blue_win();
                //파랑팀 우승!
            }

        }
        else
        {
            GetComponent<MeshRenderer>().material = normal_material;//중립일때
            floor.GetComponent<MeshRenderer>().material = floor_normal;
        }
        /////////////////////////
        if (red_conquer_state && blue_conquer_state)//빨강팀과 파랑팀 둘다 밟고 있을 때
        {
            if (red_hill_time <= 1&&hillstate==1)
            {
                red_hill_time = 1;//추가시간
            }
            if (blue_hill_time <= 1 && hillstate == 2)
            {
                blue_hill_time = 1;//추가시간
            }
                
                return;
        }else if (red_conquer_state && !blue_conquer_state)//빨강팀만 밟고 있을 때
        {
            if (hillstate == 0)//중립이라면
            {
                if (blue_conquer_time <= 0)
                {
                    red_conquer_time += Time.deltaTime;
                    if (red_conquer_time >= conquer_time)
                    {
                        hillstate = 1;
                    }
                }
                else
                {
                    blue_conquer_time -= Time.deltaTime*1.5f;
                    
                }
            }else if (hillstate == 1)//빨강팀이 점령 중이면
            {
                return;
            }else if (hillstate == 2)//파랑팀이 점령 중이면
            {
                if (blue_hill_time <= 1)
                {
                    blue_hill_time = 1;//추가 시간
                }
                blue_conquer_time -= Time.deltaTime * 1.5f;
                if (blue_conquer_time <= 0)
                {
                    hillstate = 0;
                }
            }
        }else if(!red_conquer_state && blue_conquer_state)//파랑팀만 밟고있을 때
        {
            if (hillstate == 0)//중립일 때
            {
                if (red_conquer_time <= 0)
                {
                    
                    blue_conquer_time += Time.deltaTime;
                    if (blue_conquer_time >= conquer_time)
                    {
                        hillstate = 2;
                    }
                }
                else
                {
                    red_conquer_time -= Time.deltaTime * 1.5f;

                }
            }
            else if (hillstate == 1)//빨강팀이 점령 중일 때
            {
                if (red_hill_time <= 1)
                {
                    red_hill_time = 1;//추가시간
                }
                red_conquer_time -= Time.deltaTime * 1.5f;
                if (red_conquer_time <= 0)
                {
                    hillstate = 0;
                }
            }
            else if (hillstate == 2)//파랑팀이 점령중일때
            {
                return;
            }
        }
        else//아무도 밟고있지않을때
        {
            if (hillstate == 0)//증립이라면
            {
                if (red_conquer_time > 0)
                {
                    red_conquer_time -= Time.deltaTime;
                }
                else
                {
                    red_conquer_time = 0;
                }
                if (blue_conquer_time > 0)
                {
                    blue_conquer_time -= Time.deltaTime;
                }
                else
                {
                    blue_conquer_time = 0;
                }
            }
        }
    }
    void OnTriggerStay(Collider col)//밟고있는 중인지 체크
    {
        if (col.CompareTag("Player"))
        {
            var a = col.GetComponent<playercontroler>();
            if (a.team == 0)
            {
                red_conquer_state = true;
            }
            if (a.team == 1)
            {
                blue_conquer_state = true;
            }
        }
    }
    void OnTriggerExit(Collider col)//벗어났는지 체크
    {
        if (col.CompareTag("Player"))
        {
            var a = col.GetComponent<playercontroler>();
            if (a.team == 0)
            {
                red_conquer_state = false;
            }
            if (a.team == 1)
            {
                blue_conquer_state = false;
            }
        }
    }
    [PunRPC]
    public void red_conquer_state_false()//플레이어가 점령지점 안에 죽었을 때 점령 상태를 벗어난 것으로 바꾼다
    {
        red_conquer_state = false;
    }
    [PunRPC]
    public void blue_conquer_state_false()//플레이어가 점령지점 안에 죽었을 때 점령 상태를 벗어난 것으로 바꾼다
    {
        blue_conquer_state = false;
    }
}
