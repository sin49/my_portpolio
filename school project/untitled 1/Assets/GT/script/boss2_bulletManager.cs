using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2_bulletManager : MonoBehaviour
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
            if (shoot_mode)
            {
                if (!pattern_check)
                {

                    pattern1();
                    if (GetComponent<boss_basic>().e_hp < GetComponent<boss_basic>().e_hp_max / 2)
                        pattern2();
                    pattern_check = true;
                }
                else
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
    public void pattern1()
    {
        GameObject special_bullet = Instantiate(boss2_S_bullet, bullet_location.position, bullet_location.rotation);
        
    }
    public void pattern2()
    {
        GameObject special_bullet2 = Instantiate(boss2_S_bullet, bullet_location.position, bullet_location.rotation);
        special_bullet2.GetComponent<e_bullet_type3>().RotateSpeed = -3;
        special_bullet2.GetComponent<e_bullet_type3>().RadiusAdd=0.05f;
    }
}
