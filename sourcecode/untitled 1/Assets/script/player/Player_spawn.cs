using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player_spawn : MonoBehaviour//플레이어의 잔기와 재생성,게이지UI 연결을 관리
{
    public int Life;
    public Text Life_text;
    public GameObject Player;
    public Transform respawn_check;
    public float respawn_time = 1;
    public float respawn_time_check = 0;
    public Slider power_gauge1;
    public Slider power_gauge2;
    public Slider power_gauge3;
    public Text power_text;
    
    public GameObject clear;
    // Start is called before the first frame update
    void Start()
    {
        clear = GameObject.FindWithTag("clear");
        
    }

    // Update is called once per frame
    void Update()
    {
        // Life로 잔기 표시
        Life_text.text = ("life: " + Life);
        //잔기가 남아있고 플레이어가 파괴됬다면 재생성
        if (Life > 0)
        {
            if (GameObject.FindWithTag("Player") == null)//Player라는 태그를 가진 오브젝트를 찿지 못했다=플레이어가 파괴됐다.
            {
                //바로 생성되는 것을 방지하기 위해 살짝 여유 시간을 준다
                respawn_time_check += Time.deltaTime;
                if (respawn_time_check >= respawn_time)
                {
                    clear.GetComponent<Clear_bullet>().run2();//재생성 될 때 화면의 탄을 지운다
                    //플레이어를 respawn_check위치로 재생성하고 초기 값을 설정하고 ui를 연결한다
                    //respawn_check의 위치는 연출을 위해 왼쪽 화면 바깥에 위치한다
                    GameObject Player1 = Instantiate(Player, respawn_check.position, respawn_check.rotation);
                    //플레이어 조작 제한&& 플레이어 캐릭터 리스폰 연출
                    Player1.GetComponent<shooting_playermove>().respawn_check = true;

                    Player1.GetComponent<shooting_player>().Player_ = gameObject;
                    Player1.GetComponent<shooting_player>().untouchable_state = true;//플레이어가 재생성되고 일정 시간동안 무적
                    //ui 연결
                    Player1.GetComponent<shooting_player>().power_gauge1 = power_gauge1;
                    Player1.GetComponent<shooting_player>().power_gauge2 = power_gauge2;
                    Player1.GetComponent<shooting_player>().power_gauge3 = power_gauge3;
                    Player1.GetComponent<shooting_player>().power_text= power_text;

                    Player1.GetComponent<shooting_player>().die_check = true;
                    respawn_time_check = 0;
                }
            }
        }
        else if(Life<=0)
        {
            //잔기가 없다면 게임오버 씬을 불려온다.(게임오버 씬이 존재하지 않음)
            SceneManager.LoadScene("gameover");
        }
    }
}
