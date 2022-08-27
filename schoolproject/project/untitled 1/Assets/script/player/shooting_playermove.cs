using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting_playermove : MonoBehaviour//플레이어의 이동을 처리하는 클레스
{
    public float speed = 8;
    public float s_power_speed;
    public float orgiginal_speed;
    public float posX;
    public float posY;
    public bool L_check;
    public bool R_check;
    public bool U_check;
    public bool D_check;
    public bool respawn_check;
    public GameObject game_manager;
    void Start()
    {
        orgiginal_speed = speed;//기본 이동 속도
        s_power_speed = speed * 2;//강화 상태 이동 속도
    }
    void Update()
    {
    }
    void FixedUpdate()
    {
        //클리어 에니메이션을 실행해야 하는지를 체크
        if (!GetComponent<shooting_player>().clear_check)
        {
            //리스폰 에니메이션을 실행해야 하는지를 체크
            if (!respawn_check)
            {
                //강화 상태인지를 체크하고 이동속도를 조정
                if (GetComponent<shooting_player>().special_power)
                {
                    speed = s_power_speed;
                }
                else
                {
                    speed = orgiginal_speed;
                }
                posX = transform.position.x;
                posY = transform.position.y;
                // float axisX = Input.GetAxis("Horizontal");
                //float axisY = Input.GetAxis("Vertical");

                //사망 한게 아니라면 최대 이동 구역안에서 arrow키로 조작 가능 bool값으로 최대 이동 구역을 검사
                if (!GetComponent<shooting_player>().player_hitted)
                {
                    if (Input.GetKey(KeyCode.LeftArrow) && L_check == true)
                    {
                        transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
                    }
                    if (Input.GetKey(KeyCode.RightArrow) && R_check == true)
                    {
                        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
                    }
                    if (Input.GetKey(KeyCode.UpArrow) && U_check == true)
                    {
                        transform.Translate(new Vector2(0, speed * Time.deltaTime));
                    }
                    if (Input.GetKey(KeyCode.DownArrow) && D_check == true)
                    {
                        transform.Translate(new Vector2(0, -1 * speed * Time.deltaTime));
                    }
                }
                //현재 플레이어의 위치값이 최대 이동 구역을 벗어날려고 할 경우 bool값으로 이동을 제한 시킴
                if (posX < -6)
                {
                    L_check = false;
                }
                else
                {
                    L_check = true;
                }
                if (posX > 6)
                {
                    R_check = false;
                }
                else
                {
                    R_check = true;
                }
                if (posY < -4.1)
                {
                    D_check = false;
                }
                else
                {
                    D_check = true;
                }
                if (posY > 4.1)
                {
                    U_check = false;
                }
                else
                {
                    U_check = true;
                }
            }
            //리스폰 에니메이션:화면 왼쪽 바깥으로부터 특정 x값에 도달할때까지 오른쪽으로 이동한다
            else
            {
                float c_speed = 5;
                transform.Translate((new Vector2(c_speed * Time.deltaTime, 0)));
                if (transform.position.x >= -6)
                {
                    respawn_check = false;
                }
            }
        }
        //클리어 에니메이션:화면 오른쪽 바깥을 향해 계속 이동한다
        else
        {
            float c_speed = 5;
            transform.Translate((new Vector2(c_speed * Time.deltaTime, 0)));
            
        }
        }
    }

