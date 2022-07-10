using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting_playermove : MonoBehaviour
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
        orgiginal_speed = speed;
        s_power_speed = speed * 2;
    }
    void Update()
    {
    }
    void FixedUpdate()
    {
        if (!GetComponent<shooting_player>().clear_check)
        {
            if (!respawn_check)
            {

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
                float axisX = Input.GetAxis("Horizontal");
                float axisY = Input.GetAxis("Vertical");
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
        else
        {
            float c_speed = 5;
            transform.Translate((new Vector2(c_speed * Time.deltaTime, 0)));
            
        }
        }
    }

