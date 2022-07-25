using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screenSetting : MonoBehaviour//해상도 설정
{
    public bool selected;
    public bool selected_2;
    public bool full_scren;
    public Toggle t;
    public int screen_resolution_size_X;
    public int screen_resolution_size_Y;
    public List<int> screen_resol_x = new List<int>();
    public List<int> screen_resol_Y = new List<int>();
    public GameObject check_UI;
    public Text screen_resolution_size;
    public bool check_ui_select;
    float timer;
    public setting_window_V2 w;
    bool chk;
    int select;
    void Start()
    {
       
        if (t == null)//풀스크린 토글을 현재 설정된 화면에 맞게 초기값을 설정한다(풀스크린일시 체킄,아니면 체크안됨)
        {
            t = transform.GetChild(2).GetComponent<Toggle>();
            if (Screen.fullScreen)
            {
                full_scren = true;
                t.SetIsOnWithoutNotify(true);
                Debug.Log("a");
            }
            else
            {
                full_scren = false;
                t.SetIsOnWithoutNotify(false);
                Debug.Log("b");
            }
        }
        else
        {
            if (Screen.fullScreen)
            {
                full_scren = true;
                t.SetIsOnWithoutNotify(true);
                Debug.Log("a");
            }
            else
            {
                full_scren = false;
                t.SetIsOnWithoutNotify(false);
                Debug.Log("b");
            }
        }
       
        timer = 0.2f;
        
    }
    //풀스크린 토글을 조작시 full_scren_change 함수를 실행시켜 전체화면으로 바꿀지 안 바꿀지 결정한다
    public void set_fullscreen_setting_button()
    {
        //키 조작을 위해 ison을 관리
        Toggle a = t;
        if (a.isOn)
        {
            a.isOn = false;
            full_scren_change(false);
        }
        else
        {
            a.isOn = true;
            full_scren_change(true);
        }
    }
    //매개변수에따라 전체화면을 설정한다
    public void full_scren_change(bool a)
    {
       
        if (!a)
            full_scren = false;
        else
            full_scren = true;

        set_screen_resol_size();
    }
    // Update is called once per frame
    void Update()
    {
        if (!chk)
        {
            select = setting_manager.s_manger.S.screen_resol_index;
            //전체화면 설정
            full_scren = setting_manager.s_manger.S.full_scren;
            if (setting_manager.s_manger.S.full_scren)
            {
                full_scren = true;
                t.SetIsOnWithoutNotify(true);
                Debug.Log("a");
            }
            else
            {
                full_scren = false;
                t.SetIsOnWithoutNotify(false);
                Debug.Log("b");
            }
            chk = true;
        }
        if (t == null)
        {
            t = transform.GetChild(2).GetComponent<Toggle>();
            if (Screen.fullScreen)
            {
                full_scren = true;
                t.SetIsOnWithoutNotify(true);
                Debug.Log("aa");
            }
            else
            {
                full_scren = false;
                t.SetIsOnWithoutNotify(false);
                Debug.Log("bb");
            }
        }
        //해상도 설정
        screen_resolution_size.text = screen_resol_x[select].ToString() + "×" + screen_resol_Y[select].ToString();
        if (!screen_resolution_size.GetComponent<Button>().interactable)//해상도 설정을 시작했을 때
        {
            //중복 체크 처리 방지
            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                if (!check_ui_select)
                {
                    //좌우 키로 변경하려는 해상도를  고른다
                    if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))
                    {
                        select_minus();
                    }
                    if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))
                    {
                        select_plus();
                    }
                    //공격 키로 선택된 해상도로 변경한다
                    if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]))
                    {
                        set_screen_resol_size();
                    }
                    //취소키를 누르거나 다른 설정을 고르는 행동으로 해상도 변경을 끝낸다
                    if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.PAUSE]) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]) || Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]) 
               )
                    {
                        return_select();
                    }
                    if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]) || Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        return_select();
                    }
                }
            }
        }
        else
        {

            return_select();
        }
    }
    public void select_plus()//리스트에서 해상도를 선택하는 인덱스(select) 값을 더한다
    {
        select++;
        if (select > screen_resol_x.Count - 1)
        {
            select = 0;
        }
    }
    public void select_minus()//리스트에서 해상도를 선택하는 인덱스(select) 값을 뺀다
    {
        select--;
        if (select < 0)
        {
            select = screen_resol_x.Count - 1;
        }
    }
    public void return_select()//해상도 설정을 빠져나올 때 인덱스를 현재 적용중인 해상도 인덱스로 초기화시킨다
    {
        if(select!= setting_manager.s_manger.S.screen_resol_index)
        select = setting_manager.s_manger.S.screen_resol_index;
    }
    void set_screen_resol_size()//해상도를 변경한다
    {
        
        int a = screen_resolution_size.text.IndexOf("×");
        screen_resolution_size_X = int.Parse(screen_resolution_size.text.Substring(0, a));
        if (screen_resolution_size.text.Length > 8)//선택된 해상도 설정의 텍스트를 읽고 해상도의 x값과 y값을 구한다(ㅁㅁㅁㅁXㅁㅁㅁㅁ를 앍는다면 X를 기준으로 앞 뒤의 숫자를 읽는다)
        {
            screen_resolution_size_Y = int.Parse(screen_resolution_size.text.Substring(screen_resolution_size.text.Length - a));
            setting_manager.s_manger.screen_resol(screen_resolution_size_X, screen_resolution_size_Y, full_scren);
        }
        else
        {
            screen_resolution_size_Y = int.Parse(screen_resolution_size.text.Substring(screen_resolution_size.text.Length - a+1));
            setting_manager.s_manger.screen_resol(screen_resolution_size_X, screen_resolution_size_Y, full_scren);
        }
        //읽은 x값과 y값으로  해상도를 변경한다
        setting_manager.s_manger.S.screen_resolution_size_X = this.screen_resolution_size_X;
        setting_manager.s_manger.S.screen_resolution_size_Y = this.screen_resolution_size_Y;
        setting_manager.s_manger.S.full_scren = this.full_scren;
        setting_manager.s_manger.S.screen_resol_index = this.select;
        setting_manager.s_manger.S.save_setting();
        
    }
    public void set_screen_resol_size_2()//해상도 설정 ui를 빠져나올 때 select 변경됐다면 변경된 select로 자동으로 해상도 변경(변경할지 변경하지말지 정해지지 않아서 두 방법 다 공존함)
    {
        //선택된 해상도로 변경
        int a = screen_resolution_size.text.IndexOf("×");
        screen_resolution_size_X = int.Parse(screen_resolution_size.text.Substring(0, a ));
        if (screen_resolution_size.text.Length > 8)
        {
            screen_resolution_size_Y = int.Parse(screen_resolution_size.text.Substring(screen_resolution_size.text.Length - a));
            setting_manager.s_manger.screen_resol(screen_resolution_size_X, screen_resolution_size_Y, full_scren);
        }
        else
        {
            screen_resolution_size_Y = int.Parse(screen_resolution_size.text.Substring(screen_resolution_size.text.Length - a + 1));
            setting_manager.s_manger.screen_resol(screen_resolution_size_X, screen_resolution_size_Y, full_scren);
        }
        //저장된 설정 값과 다르다면 저장된 값을 변경한다
        if (screen_resolution_size_X!= setting_manager.s_manger.S.screen_resolution_size_X || screen_resolution_size_Y != setting_manager.s_manger.S.screen_resolution_size_Y)
        {
            setting_manager.s_manger.screen_resol(screen_resolution_size_X, screen_resolution_size_Y, full_scren);
            setting_manager.s_manger.S.screen_resolution_size_X = this.screen_resolution_size_X;
            setting_manager.s_manger.S.screen_resolution_size_Y = this.screen_resolution_size_Y;
            setting_manager.s_manger.S.full_scren = this.full_scren;
            setting_manager.s_manger.S.screen_resol_index = this.select;
            setting_manager.s_manger.S.save_setting();
           
        }
        else
        {
            w.but_unselected();
            timer = 0.2f;
        }
    }

    //해상도를 적용할지 확인하는 ui(지금은 사용 안됨)
    //해상도를 적용할지 확인하고 적용한다고 한다면 설정 값으로 확정 아닐시 원래 해상도로 돌아온다
    public void checl_UI_OK()//해상도를 적용하고 지정된 설정 값으로 저장한다
    {
        setting_manager.s_manger.S.screen_resolution_size_X = this.screen_resolution_size_X;
        setting_manager.s_manger.S.screen_resolution_size_Y = this.screen_resolution_size_Y;
        setting_manager.s_manger.S.full_scren = this.full_scren;
        setting_manager.s_manger.S.screen_resol_index = this.select;
      setting_manager.s_manger.S.save_setting();

        check_UI.SetActive(false);
        check_ui_select = false;
    }
    public void checl_UI_FALSE()//해상도 적용을  취소한다
    {
        setting_manager.s_manger.screen_resol(setting_manager.s_manger.S.screen_resolution_size_X , setting_manager.s_manger.S.screen_resolution_size_Y, setting_manager.s_manger.S.full_scren);
        check_UI.SetActive(false);
        check_ui_select = false;
        
    }

    //해상도 ui가 선택됐음을 알린다
    public void select_true()
    {
        selected = true;
    }
    }
