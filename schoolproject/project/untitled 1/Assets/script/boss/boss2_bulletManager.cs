using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2_bulletManager : MonoBehaviour//스테이지 2 보스 공격 클레스
{
    public bool shoot_mode;
    public GameObject boss2_S_bullet;
    public Transform bullet_location;
    public bool pattern_check;
    public float time_check;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<boss_basic>().die_check)
        {
            if (shoot_mode)//boss_basic.cs로 부터 shoot_mode의 값을 제어하여 공격을 시작한다 
            {
                if (!pattern_check)//패턴1을 한번 실행하며 체력이 절반이하일때 패턴2도 한번 실행한다
                {

                    pattern1();
                    if (GetComponent<boss_basic>().e_hp < GetComponent<boss_basic>().e_hp_max / 2)
                        pattern2();
                    pattern_check = true;
                }
                else//실행 후 10초의 딜레이를 가진다
                {
                    time_check += Time.deltaTime;
                    if (time_check >= 10)
                    {
                        shoot_mode = false;
                        pattern_check = false;
                        time_check = 0;
                    }
                }
            }
        }
    }
    public void pattern1()//회전하는 특수탄환 생성
    {
        GameObject special_bullet = Instantiate(boss2_S_bullet, bullet_location.position, bullet_location.rotation);
        
    }
    public void pattern2()//패턴1과 반대방향으로 다르게 회전하며 움직이는 특수탄환 생성
    {
        GameObject special_bullet2 = Instantiate(boss2_S_bullet, bullet_location.position, bullet_location.rotation);
        special_bullet2.GetComponent<e_bullet_type3>().RotateSpeed = -3;
        special_bullet2.GetComponent<e_bullet_type3>().RadiusAdd=0.05f;
    }
}
