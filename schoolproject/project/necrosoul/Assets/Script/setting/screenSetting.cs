using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screenSetting : MonoBehaviour//�ػ� ����
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
       
        if (t == null)//Ǯ��ũ�� ����� ���� ������ ȭ�鿡 �°� �ʱⰪ�� �����Ѵ�(Ǯ��ũ���Ͻ� ü��,�ƴϸ� üũ�ȵ�)
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
    //Ǯ��ũ�� ����� ���۽� full_scren_change �Լ��� ������� ��üȭ������ �ٲ��� �� �ٲ��� �����Ѵ�
    public void set_fullscreen_setting_button()
    {
        //Ű ������ ���� ison�� ����
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
    //�Ű����������� ��üȭ���� �����Ѵ�
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
            //��üȭ�� ����
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
        //�ػ� ����
        screen_resolution_size.text = screen_resol_x[select].ToString() + "��" + screen_resol_Y[select].ToString();
        if (!screen_resolution_size.GetComponent<Button>().interactable)//�ػ� ������ �������� ��
        {
            //�ߺ� üũ ó�� ����
            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                if (!check_ui_select)
                {
                    //�¿� Ű�� �����Ϸ��� �ػ󵵸�  ����
                    if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))
                    {
                        select_minus();
                    }
                    if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))
                    {
                        select_plus();
                    }
                    //���� Ű�� ���õ� �ػ󵵷� �����Ѵ�
                    if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]))
                    {
                        set_screen_resol_size();
                    }
                    //���Ű�� �����ų� �ٸ� ������ ���� �ൿ���� �ػ� ������ ������
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
    public void select_plus()//����Ʈ���� �ػ󵵸� �����ϴ� �ε���(select) ���� ���Ѵ�
    {
        select++;
        if (select > screen_resol_x.Count - 1)
        {
            select = 0;
        }
    }
    public void select_minus()//����Ʈ���� �ػ󵵸� �����ϴ� �ε���(select) ���� ����
    {
        select--;
        if (select < 0)
        {
            select = screen_resol_x.Count - 1;
        }
    }
    public void return_select()//�ػ� ������ �������� �� �ε����� ���� �������� �ػ� �ε����� �ʱ�ȭ��Ų��
    {
        if(select!= setting_manager.s_manger.S.screen_resol_index)
        select = setting_manager.s_manger.S.screen_resol_index;
    }
    void set_screen_resol_size()//�ػ󵵸� �����Ѵ�
    {
        
        int a = screen_resolution_size.text.IndexOf("��");
        screen_resolution_size_X = int.Parse(screen_resolution_size.text.Substring(0, a));
        if (screen_resolution_size.text.Length > 8)//���õ� �ػ� ������ �ؽ�Ʈ�� �а� �ػ��� x���� y���� ���Ѵ�(��������X���������� �̴´ٸ� X�� �������� �� ���� ���ڸ� �д´�)
        {
            screen_resolution_size_Y = int.Parse(screen_resolution_size.text.Substring(screen_resolution_size.text.Length - a));
            setting_manager.s_manger.screen_resol(screen_resolution_size_X, screen_resolution_size_Y, full_scren);
        }
        else
        {
            screen_resolution_size_Y = int.Parse(screen_resolution_size.text.Substring(screen_resolution_size.text.Length - a+1));
            setting_manager.s_manger.screen_resol(screen_resolution_size_X, screen_resolution_size_Y, full_scren);
        }
        //���� x���� y������  �ػ󵵸� �����Ѵ�
        setting_manager.s_manger.S.screen_resolution_size_X = this.screen_resolution_size_X;
        setting_manager.s_manger.S.screen_resolution_size_Y = this.screen_resolution_size_Y;
        setting_manager.s_manger.S.full_scren = this.full_scren;
        setting_manager.s_manger.S.screen_resol_index = this.select;
        setting_manager.s_manger.S.save_setting();
        
    }
    public void set_screen_resol_size_2()//�ػ� ���� ui�� �������� �� select ����ƴٸ� ����� select�� �ڵ����� �ػ� ����(�������� ������������ �������� �ʾƼ� �� ��� �� ������)
    {
        //���õ� �ػ󵵷� ����
        int a = screen_resolution_size.text.IndexOf("��");
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
        //����� ���� ���� �ٸ��ٸ� ����� ���� �����Ѵ�
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

    //�ػ󵵸� �������� Ȯ���ϴ� ui(������ ��� �ȵ�)
    //�ػ󵵸� �������� Ȯ���ϰ� �����Ѵٰ� �Ѵٸ� ���� ������ Ȯ�� �ƴҽ� ���� �ػ󵵷� ���ƿ´�
    public void checl_UI_OK()//�ػ󵵸� �����ϰ� ������ ���� ������ �����Ѵ�
    {
        setting_manager.s_manger.S.screen_resolution_size_X = this.screen_resolution_size_X;
        setting_manager.s_manger.S.screen_resolution_size_Y = this.screen_resolution_size_Y;
        setting_manager.s_manger.S.full_scren = this.full_scren;
        setting_manager.s_manger.S.screen_resol_index = this.select;
      setting_manager.s_manger.S.save_setting();

        check_UI.SetActive(false);
        check_ui_select = false;
    }
    public void checl_UI_FALSE()//�ػ� ������  ����Ѵ�
    {
        setting_manager.s_manger.screen_resol(setting_manager.s_manger.S.screen_resolution_size_X , setting_manager.s_manger.S.screen_resolution_size_Y, setting_manager.s_manger.S.full_scren);
        check_UI.SetActive(false);
        check_ui_select = false;
        
    }

    //�ػ� ui�� ���õ����� �˸���
    public void select_true()
    {
        selected = true;
    }
    }
