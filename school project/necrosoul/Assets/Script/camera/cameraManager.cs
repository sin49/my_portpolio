using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour//카메라들을 관리하는 클레스
{
    public static cameraManager cm;
    public action_camera action_cam;
    //메인 카메라
    public GameObject main_cm;
    public GameObject main_cm_minimap;
  //보스 카메라
    public GameObject boss_cm;
    public GameObject boss_cm_minimap;
    private void Awake()
    {
        cm = this;
        action_cam = this.transform.GetChild(1).GetComponent<action_camera>();
        main_cm= this.transform.GetChild(0).gameObject;
        main_cm_minimap = this.transform.GetChild(0).GetChild(0).gameObject;
        boss_cm = this.transform.GetChild(2).gameObject;
        boss_cm_minimap= this.transform.GetChild(2).GetChild(0).gameObject;
    }
    //미니맵은 카메라로 구성되어있으며 특정 오브젝트를 표시하고 그외에는 지형외에는 보이지 않게하여 작동시킨다
    //카메라 화면은 화면 구석에 위치시킨다
    public void active_minimap()//미니맵 카메라를 활성화 한다
    {
        if (Gamemanager.GM.boss)
        {
            boss_cm_minimap.SetActive(true);
        }
        else
        {
            main_cm_minimap.SetActive(true);
        }
    }
    public void disable_minimap()//미니맵 비활성화
    {
        if (Gamemanager.GM.boss)
        {
            boss_cm_minimap.SetActive(false);
        }
        else
        {
            main_cm_minimap.SetActive(false);
        }
    }
    public void active_action_cam()//연출 카메라 활성화
    {
        action_cam.gameObject.SetActive(true);
        if (Gamemanager.GM.boss)
        {
            boss_cm.SetActive(false);
        }
        else
        {
            main_cm.SetActive(false);
        }
    }
    public void active_main_cam()//메인 카메라 활성화
    {
        action_cam.gameObject.SetActive(false);
        if (Gamemanager.GM.boss)
        {
            boss_cm.SetActive(true);
        }
        else
        {
            main_cm.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Gamemanager.GM.boss)
        {
            main_cm.SetActive(false);
        }
        else
        {
            boss_cm.SetActive(false);
        }
    }
}
