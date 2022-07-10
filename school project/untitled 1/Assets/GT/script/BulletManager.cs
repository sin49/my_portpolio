using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
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
        if (!GetComponent<shooting_player>().clear_check) {
            if (!GetComponent<shooting_playermove>().respawn_check)
            {
                if (!GetComponent<shooting_player>().player_hitted)
                {
                    if (FireState)
                    {
                        if (Input.GetKey(KeyCode.Z))
                        {
                            StartCoroutine(FireCycleControl());
                            Instantiate(bullet, bulletlocation.position, bulletlocation.rotation);
                            if (GetComponent<shooting_player>().special_power)
                            {
                                Instantiate(bullet, bulletlocation2.position, bulletlocation2.rotation);
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            if (!GetComponent<shooting_player>().special_power)
                            {
                                if (GetComponent<shooting_player>().power_gauge >= GetComponent<shooting_player>().power_gauge_max / 3)
                                {
                                    Instantiate(ex_bullet, bulletlocation.position, bulletlocation.rotation);
                                    GetComponent<shooting_player>().power_gauge -= GetComponent<shooting_player>().power_gauge_max / 3;
                                }
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.C))
                        {
                            if (GetComponent<shooting_player>().power_gauge >= GetComponent<shooting_player>().power_gauge_max)
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
    public IEnumerator FireCycleControl()
    {
        FireState = false;
        yield return new WaitForSeconds(FireDelay);
        FireState = true;
    }
}
