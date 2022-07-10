using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenTitleButton : MonoBehaviour
{
    public float vr;
    public float hr;
    public int P_select;

    public AudioManage_Main m_audio;

    [Header("Ÿ��Ʋ")]
    [SerializeField] List<GameObject> InvenContent = new List<GameObject>();
    public InvenTitle IT;


    [Header("Title ������ �ֱ�")]
    public CanvasGroup In_Title_P;

    [Header("Select ���� ����ֱ�")]
    public GameObject Select;
    Toggle My_T;
    // Start is called before the first frame update
    void Start()
    {
        Select.SetActive(false);
        My_T = this.gameObject.GetComponent<Toggle>();
        m_audio = AudioManage_Main.instance;
        IT = this.gameObject.GetComponent<InvenTitle>();
        P_select = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(My_T.isOn)
        {
            Select.SetActive(false);
        }
        else
        {
            Select.SetActive(true);
        }

        PlayButton();

    }
    public void PlayButton()
    {
        Debug.Log("����� Ű��?" + IT.key);
        if (BookButtonManger.bookButtonManger.GetIndex() == 2 && this.name == "IB_Title_In" && BookButtonManger.bookButtonManger.BookTitle_Select.name == "IB_Title_In")
        {
            ButtonMange();
            Debug.Log("�κ�1");
        }
        else if (BookButtonManger.bookButtonManger.GetIndex() == 2 && this.name == "IB_Title_Sp" && BookButtonManger.bookButtonManger.BookTitle_Select.name == "IB_Title_Sp")
        {
            ButtonMange2();
            Debug.Log("�κ�2");
        }
        else if (BookButtonManger.bookButtonManger.GetIndex() == 2 && this.name == "IB_Title_Sy" && BookButtonManger.bookButtonManger.BookTitle_Select.name == "IB_Title_Sy")
        {
            ButtonMange3();
            Debug.Log("�κ�3");
        }
    }

    public void ButtonMange()
    {
        if (IT.ActiveButton != 0)
        {
            IT.Inven_Content[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);

            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))    //������
            {
                Debug.Log("������");
                if (P_select == IT.ActiveButton - 1 || P_select == 3)
                {
                    IT.R_Button();
                }
                else
                {
                    if (P_select < IT.ActiveButton)
                    {
                        P_select++;
                    }
                }
                m_audio.UI_Chose();
            }
            else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))    //����
            {
                Debug.Log("����");
                if (P_select == 0 || P_select==4)
                {
                    IT.L_Button();
                }
                else
                {
                    P_select--;
                }
                m_audio.UI_Chose();
            }
            else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]))      //��
            {
                Debug.Log("��");
                if (P_select - 3 > 0 && IT.ActiveButton>4)   //���� �Ʒ��� ����
                {
                    P_select = P_select-4;
                }
                else
                {
                    //P_select = P_select - 3;
                }
                m_audio.UI_Chose();
            }
            else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]))    //�Ʒ�
            {
                Debug.Log("�Ʒ�");
                if (P_select < 4 && IT.ActiveButton > 4)   //���� ���� ����
                {
                    //P_select = P_select + 4 - IT.ActiveButton;
                    if (P_select+4 < IT.ActiveButton)
                    {
                        P_select = P_select + 4;
                    }
                }
                else
                {
                    //P_select = P_select + 4;
                }
                m_audio.UI_Chose();
            }
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]) && BookButtonManger.bookButtonManger.buttoncheck == false)
            {
                BookButtonManger.bookButtonManger.ButtonTimerON();
                Debug.Log("���������Ǵ�");
                m_audio.UI_Select();
                IT.Inven_Content[P_select].GetComponent<Inven_Content>().TakeItem();
            }
            else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]))
            {
                Open_In_title();

                Debug.Log("Xħ!");
                BookButtonManger.bookButtonManger.BookTitle_Select = null;
                BookButtonManger.bookButtonManger.SetIndex(1);
                IT.Inven_Content[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(false);
                m_audio.UI_Cancle();
            }
        }
        else
        {
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]))
            {
                Open_In_title();

                Debug.Log("Xħ!");
                BookButtonManger.bookButtonManger.BookTitle_Select = null;
                BookButtonManger.bookButtonManger.SetIndex(1);
                m_audio.UI_Cancle();
            }
        }
    }

    public void ButtonMange2()
    {
        if (IT.ActiveButton != 0)
        {
            IT.spInven_Content[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);

            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))    //������
            {
                Debug.Log("������");
                if (P_select == IT.ActiveButton - 1 || P_select == 3)
                {
                    IT.R_Button();
                }
                else
                {
                    if (P_select < IT.ActiveButton)
                    {
                        P_select++;
                    }
                }
                m_audio.UI_Chose();
            }
            else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))    //����
            {
                Debug.Log("����");
                if (P_select == 0 || P_select == 4)
                {
                    IT.L_Button();
                }
                else
                {
                    P_select--;
                }
                m_audio.UI_Chose();
            }
            else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP])&& IT.ActiveButton>=4)      //��
            {
                Debug.Log("��");
                if (P_select - 3 > 0 && IT.ActiveButton > 4)   //���� �Ʒ��� ����
                {
                    P_select = P_select - 4;
                }
                else
                {
                    //P_select = P_select - 3;
                }
                m_audio.UI_Chose();
            }
            else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]) && IT.ActiveButton >= 4)    //�Ʒ�
            {
                Debug.Log("�Ʒ�");
                if (P_select < 4 && IT.ActiveButton > 4)   //���� ���� ����
                {
                    //P_select = P_select + 4 - IT.ActiveButton;
                    if (P_select + 4 < IT.ActiveButton)
                    {
                        P_select = P_select + 4;
                    }
                }
                else
                {
                    //P_select = P_select + 4;
                }
                m_audio.UI_Chose();
            }
            else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]))
            {
                Open_In_title();

                Debug.Log("Xħ!");
                BookButtonManger.bookButtonManger.BookTitle_Select = null;
                BookButtonManger.bookButtonManger.SetIndex(1);
                IT.spInven_Content[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(false);
                m_audio.UI_Cancle();
            }
        }
        else
        {
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]))
            {
                Open_In_title();

                Debug.Log("Xħ!");
                BookButtonManger.bookButtonManger.BookTitle_Select = null;
                BookButtonManger.bookButtonManger.SetIndex(1);
                m_audio.UI_Cancle();
            }
        }
    }

    public void ButtonMange3()
    {
        if (IT.ActiveButton != 0)
        {
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))    //������
            {
                IT.R_Button();
                m_audio.UI_Chose();
            }
            else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))    //����
            {

                IT.L_Button();
                m_audio.UI_Chose();
            }
            else
            {
                if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]))
                {
                    Open_In_title();

                    Debug.Log("Xħ!");
                    BookButtonManger.bookButtonManger.BookTitle_Select = null;
                    BookButtonManger.bookButtonManger.SetIndex(1);
                    m_audio.UI_Cancle();
                }
            }
        }
    }

    public void Open_In_title()
    {
        //Ach_Content_P.SetActive(false);
        In_Title_P.alpha = 1;

    }

    public void Open_In_Cotent()
    {
        //Ach_Content_P.SetActive(true);
        In_Title_P.alpha = 0;
    }
}
