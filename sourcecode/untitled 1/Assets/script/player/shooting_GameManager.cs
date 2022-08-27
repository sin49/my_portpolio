using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class shooting_GameManager : MonoBehaviour//게임 메니저
{
    public float phase_time;
    public float phase_time_max = 60;
    public bool boss_phase;
    public Slider time_slider;
    public Slider boss_slider;
    public bool boss_slider_check;
    public GameObject boss;
    public bool pause_check;
    public GameObject pause_text;
    public bool clear_t_check;
    public bool boss_t_check;
    public Text boss_text;
    public Text clear_text;
    public GameObject player;
    public bool clear_phase;
    public float time;
    public AudioClip field_music;
    public AudioClip boss_music;
    public AudioClip boss_music2;
    public AudioSource audioSource;
    public float audio_time;
    public bool audio_check;
    public float t = 1;
    public bool volume_check;
    public int level;//1:easy 2:normal 3:hard
    public static int levelcheck;
    // Start is called before the first frame update
    void Start()
    {
      //필드 음악 재생
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = field_music;
        audioSource.Play();
        audioSource.loop = true;
        //ui 설정
        boss_slider.gameObject.SetActive(false);
        time_slider.maxValue = phase_time_max;
        boss_text.gameObject.SetActive(false);
        t = 1;
    }

    // Update is called once per frame
    void Update()
    {
        levelcheck = level;
        //음악의 재생이 끝나가기 직전인지를 bool값으로 체크 t값으로 볼륨 조절
        if (audioSource.clip == field_music)
        {
            if (audioSource.time >= 47)
            {
                audio_check = true;
                audioSource.volume = t;
            }
            else
            {
                audioSource.volume = t;
                audio_check = false;
            }
        }
        if (audioSource.clip == boss_music)
        {
            if (audioSource.time >= 26)
            {
                audio_check = true;
                audioSource.volume = t;

            }
            else
            {
                audioSource.volume = t;
                audio_check = false; ;
            }
        }
        //음악이 끝나가기 직전이라면 t값으로 음악의 볼륨을 페이드 아웃
        if (audio_check)
        {
            if (t >= 0)
            {
                t -= Time.deltaTime * 1 / 2;
            }
        }
        //끝난 음악이 반복 재생될 때 페이드 아웃된 음악의 볼륨을 다시 페이드 인
        else
        {
            if (t <= 1)
            {
                t += Time.deltaTime * 1 / 2;
            }
        }
        player = GameObject.FindWithTag("Player");
        //esc를 누를 시 정지된 상태인지를 bool값으로 체크해서 정지,재개
        if (Input.GetKeyDown(KeyCode.Escape) && !pause_check)
        {
            Pause();

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pause_check)
        {
            resume();

        }
        //스테이지의 진행도를 슬라이드 바로 표시 
        phase_time += Time.deltaTime;
        time_slider.value = phase_time;
        //스테이지가 진해이 끝나면 음악을 멈추고 보스 페이즈로 들어감
        if (phase_time >= phase_time_max)
        {
            time_slider.gameObject.SetActive(false);
            if (!audio_check && !boss_phase)
            {
                audioSource.Stop();
                audio_check = true;
            }
            boss_phase = true;
        }
        if (boss_phase)//보스 페이즈
        {



            //현재 스테이지에 적이 존재하지 않는지를 체크 
            if (GameObject.FindGameObjectWithTag("enemy") == null)
            {
                //보스가 아직 생성되지 않았다면
                if (GameObject.FindGameObjectWithTag("boss") == null)
                {
                    //보스전에 돌입함을 플레이어에게 알리는 텍스트 표시
                    if (!boss_t_check)
                    {
                        boss_text.gameObject.SetActive(true);
                        boss_t_check = true;
                    }
                    boss_slider.gameObject.SetActive(false);
                }
                //보스가 존재한다면
                else
                {
                    //보스의 채력ui를 활성화 및 보스의 체력 값을 ui에 연결
                    boss_slider.gameObject.SetActive(true);
                    boss = GameObject.FindGameObjectWithTag("boss");
                    boss_slider.maxValue = boss.GetComponent<boss_basic>().e_hp_max;
                    boss_slider.value = boss.GetComponent<boss_basic>().e_hp;
                }
            }

        }
        if (clear_phase)//보스 처치 후
        {
            time += Time.deltaTime;
            //3초 후 스테이지 클리어 처리
            if (time >= 3)
            {
                //bool값으로 플레이어 조작을 제한&&플레이어 캐릭터 클리어 연출
                player.GetComponent<shooting_player>().clear_check = true;
                //클리어 됐음을 알리는 text를 활성화
                if (!clear_t_check)
                {
                    clear_text.gameObject.SetActive(true);
                    clear_t_check = true;
                }
                //클리어 연출이 마무리&&8초 이상 경과
                if (player.transform.position.x >= 8 && time >= 8)
                {
                    switch (level)//level의 값에 따라 다음 scene으로 이동
                    {
                        case 1:
                       
                            SceneManager.LoadScene("normal");
                            break;
                        case 2:
                           
                            SceneManager.LoadScene("hard");
                            break;
                        case 3:
                            SceneManager.LoadScene("Ending_Scene");//Ending_Scene 없음
                            break;
                    }
                }
            }
        }
    }

    void Pause()
    {
        //timeScale = 0으로 게임을 일시정지&&ui표시
        Time.timeScale = 0;
        pause_check = true;
        pause_text.gameObject.SetActive(true);
    }
 
    public void resume()
    {
        //timeScale = 1으로 게임을 재개
        Time.timeScale = 1;
        pause_check = false;
        pause_text.gameObject.SetActive(false);
    }
    //게임을 일시 정지 시켰을 때 재시작,메인매뉴,나가기를 선택 가능
    
    public void restart()
    {
        //레벨의 값에 따라 .LoadScene으로 그 스테이지를 다시 시작
        switch (level)
        {
            case 1:

                SceneManager.LoadScene("easy");
                Time.timeScale = 1;
                break;
            case 2:
                SceneManager.LoadScene("normal");
                Time.timeScale = 1;
                break;
            case 3:
                SceneManager.LoadScene("hard");
                Time.timeScale = 1;
                break;
        }
    }


public void Mainmenu()
    {
        //Game Start 씬으로 돌아온다 (Game Start씬 없음)
        SceneManager.LoadScene("Game Start");
    }
    public void QuitGame()
    {
        //게임을 종료한다
        Application.Quit();
    }
}
