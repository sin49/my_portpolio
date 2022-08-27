using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class action_camera : MonoBehaviour//연출용 카메라
{
   public enum action { p_death_cam};//enum으로 실행할 연출을 선택한다
    public action a=new action();
    public float timer;
    float t_chk;
    float t_total;
    Camera cm;
    // Start is called before the first frame update
    void Start()
    {
        cm = this.GetComponent<Camera>();
        t_chk = timer * Time.deltaTime;
    }
    //카메라를 활성화,비활성화 하는 것으로 작동한다
    private void OnDisable()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        t_total = 0;
    }
    private void OnEnable()
    {
      
    }
    // Update is called once per frame
    void Update()
    {
        if (a == action.p_death_cam)//플레이어가 죽었을 때
        {
            if(Gamemanager.GM.Player_obj!=null)//플레이어가 있다면 플레이어의 위치로
                this.transform.position = new Vector3(Gamemanager.GM.Player_obj.transform.position.x, Gamemanager.GM.Player_obj.transform.position.y, this.transform.position.z);
            cm.orthographicSize = 2.5f;//카메라를 플레이어 위치로 확대
            if (t_total < 1)//시간을 느리게 연출한다
            {
                t_total += t_chk;
                if (t_total > 1)
                    t_total = 1;
                Time.timeScale = t_total;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
       
    }
}
