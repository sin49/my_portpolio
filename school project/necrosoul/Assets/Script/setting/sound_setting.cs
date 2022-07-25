using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class sound_setting : MonoBehaviour//사운드 설정
{
    public float main_voulume;
    public float bgm_volume;
    public float sfx_volume;
    public Button Main_button;
    public Button bgm_button;
    public Button sfx_button;
    public Slider main_slider;
    public Slider bgm_slider;
    public Slider sfx_slider;
    public setting_window_V2 w;
    public int selected;
    public AudioMixer MasterMixer;
    public AudioMixer SFXMixer;
    public AudioMixer BGMMIxer;
    bool chk;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!chk)//설정된 값으로 설정 ui 변경
        {
           main_voulume  = setting_manager.s_manger.S.full_volume;
            bgm_volume = setting_manager.s_manger.S.bgm_volume;
            sfx_volume = setting_manager.s_manger.S.sfx_volume;
           
            chk = true;
        }
        main_slider.value = main_voulume;
        bgm_slider.value = bgm_volume;
        sfx_slider.value = sfx_volume;
        //볼륨 설정
        set_sound();
        //설정 선택시 설정과 관련된 볼륨 조절
        if (!Main_button.interactable)//마스터볼륨
            set_main();
        if (!bgm_button.interactable)//배경볼륨
            set_bgm();
        if (!sfx_button.interactable)//효과음볼륨
            set_sfx();

      //최대 볼륨 수치 조절(ui 오류 방지)
        if (main_voulume < -40)
            main_voulume = -40;
        if (main_voulume > 0)
            main_voulume = 0;
        if (bgm_volume < -40)
            bgm_volume = -40;
        if (bgm_volume > 0)
            bgm_volume = 0;
        if (sfx_volume < -40)
            sfx_volume = -40;
        if (sfx_volume > 0)
            sfx_volume = 0;
    }
    public void set_main()//마스터 볼륨 선택
    {
        selected = 1;
    }
    public void set_bgm()//배경 선택
    {
        selected = 2;
    }
    public void set_sfx()//효과음 선택
    {
        selected = 3;
    }
    public void un_set()
    {
        selected = 0;
    }
    void set_sound()
    {
        if (selected != 0)
        {
            if(Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.RIGHT] )|| Input.GetKeyDown(KeyCode.RightArrow))
            {
                switch (selected)
                {
                    case 1:
                        main_voulume += 0.1f;
                        break;
                    case 2:
                        bgm_volume += 0.1f;
                        break;
                    case 3:
                        sfx_volume += 0.1f;
                        break;
                }
            }
            if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.LEFT]) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                switch (selected)
                {
                    case 1:
                        main_voulume -= 0.1f;
                        break;
                    case 2:
                        bgm_volume -= 0.1f;
                        break;
                    case 3:
                        sfx_volume -= 0.1f;
                        break;
                }
            }
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.PAUSE]) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP])|| Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK])||
                Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP])|| Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN])||Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.DownArrow))
            {
                un_set();
                w.but_unselected();
            }
                setting_manager.s_manger.sound_manager();
            AudioContorl_Total();
            AudioContorl_Bgm();
            AudioContorl_SFX();
        }
    }
    public void AudioContorl_Total()    //마스터 볼륨 설정
    {
        float sound = main_slider.value;
        if (sound == -40f) MasterMixer.SetFloat("Master", -80);
        else MasterMixer.SetFloat("Master", sound);
    }

    public void AudioContorl_Bgm()  //bgm 볼륨 설정
    {
        float sound = bgm_slider.value;
        if (sound == -40f) MasterMixer.SetFloat("BGM", -80);
        else MasterMixer.SetFloat("BGM", sound);
    }

    public void AudioContorl_SFX()      //효과음 볼륨 설정
    {
        float sound = sfx_slider.value;
        if (sound == -40f) MasterMixer.SetFloat("SFX", -80);
        else MasterMixer.SetFloat("SFX", sound);
    }
}
