using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class setting_manager : MonoBehaviour//설정을 불려오고 그 값대로 게임을 설정하는 클레스
{
    public Setting S;
    AudioMixer am;
    public Key_manager k;
    public static setting_manager s_manger;
    public bool key_Make_chk;
    private void Awake()
    {
        //설정을 불려오기(없다면 생성)
        s_manger = this;
        if (S == null)
        {
            S = new Setting();
        }
        S.load_setting();
        set_setting();
        DontDestroyOnLoad(this.gameObject);
   
    }
  
    public void set_setting()//저장된 설정 값에 따라 구성
    {
        screen_resol(S.screen_resolution_size_X, S.screen_resolution_size_Y, S.full_scren);
        Key_setting(k);
        sound_manager();
    }
    public void screen_resol(int a, int b, bool ful)//화면 설정
    {
        Screen.SetResolution(a, b, ful);
    }
    public void new_setting()//새로운 설정 파일 생성
    {
        Setting s_new = new Setting();
        s_new.initialize_setting();
        s_new.save_setting();
        S = s_new;
        set_setting();
    }
    public void Key_setting(Key_manager k)//키세팅 설정
    {
        if (!key_Make_chk)//첫 키 설정 체크
        {
    ///add

            Key_manager.Keys.Add(Key_manager.KeyAction.UP, S.Keys[0]);
            Key_manager.Keys.Add(Key_manager.KeyAction.DOWN, S.Keys[1]);
            Key_manager.Keys.Add(Key_manager.KeyAction.LEFT, S.Keys[2]);
            Key_manager.Keys.Add(Key_manager.KeyAction.RIGHT, S.Keys[3]);
            Key_manager.Keys.Add(Key_manager.KeyAction.ATTACK, S.Keys[4]);
            Key_manager.Keys.Add(Key_manager.KeyAction.JUMP, S.Keys[5]);
            Key_manager.Keys.Add(Key_manager.KeyAction.DASH, S.Keys[6]);
            Key_manager.Keys.Add(Key_manager.KeyAction.INVENTORY, S.Keys[7]);
            Key_manager.Keys.Add(Key_manager.KeyAction.PAUSE, S.Keys[8]);
            Debug.Log("ssss"+Key_manager.Keys[Key_manager.KeyAction.UP]);
            key_Make_chk = true;
        }
        else//두번ㄴ째 부터
        {
            //값 변경

            Key_manager.Keys[Key_manager.KeyAction.UP] = S.Keys[0];
            Key_manager.Keys[Key_manager.KeyAction.DOWN] = S.Keys[1];
            Key_manager.Keys[Key_manager.KeyAction.LEFT] = S.Keys[2];
            Key_manager.Keys[Key_manager.KeyAction.RIGHT] = S.Keys[3];
            Key_manager.Keys[Key_manager.KeyAction.ATTACK] = S.Keys[4];
            Key_manager.Keys[Key_manager.KeyAction.JUMP] = S.Keys[5];
            Key_manager.Keys[Key_manager.KeyAction.DASH] = S.Keys[6];
            Key_manager.Keys[Key_manager.KeyAction.INVENTORY] = S.Keys[7];
            Key_manager.Keys[Key_manager.KeyAction.PAUSE] = S.Keys[8];
        }
    }
    public void sound_manager()//사운드 설정값 적용(사용되지 않음)
    {
       // am.SetFloat("bgm", S.full_volume * S.bgm_volume);
       // am.SetFloat("SFX", S.full_volume * S.sfx_volume);
    }
}
