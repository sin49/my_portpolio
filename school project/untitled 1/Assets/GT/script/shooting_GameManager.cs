using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class shooting_GameManager : MonoBehaviour
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
        Debug.Log("a");
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = field_music;
        audioSource.Play();
        audioSource.loop = true;
        boss_slider.gameObject.SetActive(false);
        time_slider.maxValue = phase_time_max;
        boss_text.gameObject.SetActive(false);
        t = 1;
    }

    // Update is called once per frame
    void Update()
    {
        levelcheck = level;
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
        if (audio_check)
        {
            if (t >= 0)
            {
                t -= Time.deltaTime * 1 / 2;
            }
        }
        else
        {
            if (t <= 1)
            {
                t += Time.deltaTime * 1 / 2;
            }
        }
        player = GameObject.FindWithTag("Player");
        if (Input.GetKeyDown(KeyCode.Escape) && !pause_check)
        {
            Pause();

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pause_check)
        {
            resume();

        }
        phase_time += Time.deltaTime;
        time_slider.value = phase_time;
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
        if (boss_phase)
        {




            if (GameObject.FindGameObjectWithTag("enemy") == null)
            {
                if (GameObject.FindGameObjectWithTag("boss") == null)
                {

                    if (!boss_t_check)
                    {
                        boss_text.gameObject.SetActive(true);
                        boss_t_check = true;
                    }
                    boss_slider.gameObject.SetActive(false);
                }
                else
                {
                    boss_slider.gameObject.SetActive(true);
                    boss = GameObject.FindGameObjectWithTag("boss");
                    boss_slider.maxValue = boss.GetComponent<boss_basic>().e_hp_max;
                    boss_slider.value = boss.GetComponent<boss_basic>().e_hp;
                }
            }

        }
        if (clear_phase)
        {
            time += Time.deltaTime;
            if (time >= 3)
            {
                player.GetComponent<shooting_player>().clear_check = true;
                if (!clear_t_check)
                {
                    clear_text.gameObject.SetActive(true);
                    clear_t_check = true;
                }
                if (player.transform.position.x >= 8 && time >= 8)
                {
                    switch (level)
                    {
                        case 1:
                       
                            SceneManager.LoadScene("normal");
                            break;
                        case 2:
                           
                            SceneManager.LoadScene("hard");
                            break;
                        case 3:
                            SceneManager.LoadScene("Ending_Scene");
                            break;
                    }
                }
            }
        }
    }
    void Pause()
    {
        Time.timeScale = 0;
        pause_check = true;
        pause_text.gameObject.SetActive(true);
    }
    public void resume()
    {
        Time.timeScale = 1;
        pause_check = false;
        pause_text.gameObject.SetActive(false);
    }
    public void restart()
    {
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
        SceneManager.LoadScene("Game Start");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
