using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour//플레이어의 탄을 생성하는 클래스
{
    public GameObject bullet;
    public GameObject ex_bullet;
    public Transform bulletlocation;
    public Transform bulletlocation2;
    public float FireDelay=1f;
    private bool FireState=true;
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        //플레이어가 살아있고 조작가능한 상태인지를 확인
        if (!GetComponent<shooting_player>().clear_check) {
            if (!GetComponent<shooting_playermove>().respawn_check)
            {
                if (!GetComponent<shooting_player>().player_hitted)
                {
                    if (FireState)//firestate로 연사속도를 조절한다.
                    {
                        //z키를 누르는 중이라면 플레이어가  bulletlocation의 위치로 탄을 쏜다.
                        if (Input.GetKey(KeyCode.Z))
                        {
                            StartCoroutine(FireCycleControl());
                            Instantiate(bullet, bulletlocation.position, bulletlocation.rotation);
                            if (GetComponent<shooting_player>().special_power)//무적 상태라면  bulletlocation2의 위치로 일반적인 상태보다 더 많은 탄을 쏜다.
                            {
                                Instantiate(bullet, bulletlocation2.position, bulletlocation2.rotation);
                            }

                        }
                        //x키를 누를시 파워게이지를 1칸 소모해 bulletlocation의 위치로 강화탄을 쏜다
                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            if (!GetComponent<shooting_player>().special_power)//무적 상태가 아니라면
                            {
                                if (GetComponent<shooting_player>().power_gauge >= GetComponent<shooting_player>().power_gauge_max / 3)//파워게이지가 있는지 체크
                                {
                                    Instantiate(ex_bullet, bulletlocation.position, bulletlocation.rotation);
                                    GetComponent<shooting_player>().power_gauge -= GetComponent<shooting_player>().power_gauge_max / 3;
                                }
                            }
                        }
                        //c키를 누를시 파워게이지를 3칸 소모해 강화 상태가 된다.
                        if (Input.GetKeyDown(KeyCode.C))
                        {
                            if (GetComponent<shooting_player>().power_gauge >= GetComponent<shooting_player>().power_gauge_max)//파워게이지가 있는지 체크
                            {
                                GetComponent<shooting_player>().untouchable_state = true;
                                GetComponent<shooting_player>().special_power = true;

                            }
                        }
                    }
                }
            }
        }
    }
    //FireDelay의 값으로 연사 속도를 조절하는 코루틴
    public IEnumerator FireCycleControl()
    {
        FireState = false;
        yield return new WaitForSeconds(FireDelay);
        FireState = true;
    }
}
