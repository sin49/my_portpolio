using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageBookButton : MonoBehaviour
{
    public float vr;
    public float hr;
    public int P_select;

    [Header("Ÿ��Ʋ")]
    [SerializeField] List<GameObject> BookTitle = new List<GameObject>();
    public Stage_Book SB;
    public GameObject SelectPage;

    AudioManage_Main m_Audio;
    // Start is called before the first frame update
    void Start()
    {
        m_Audio = AudioManage_Main.instance;
        SB = this.gameObject.GetComponent<Stage_Book>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
        if (BookButtonManger.bookButtonManger.GetIndex() == 1)
        {
            ButtonMange();
        }

    }
    public void ButtonMange()
    {
        SelectPage.SetActive(false);
        SB.Title[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        
        if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))    //������
        {
            if (P_select == 1 || P_select == SB.ActiveButton - 1)    //������ ��ư�� �����ٸ�
            {
                SB.R_Button();
                P_select = 0;
                m_Audio.UI_Page();
            }
            else
            {
                P_select++;
            }
            m_Audio.UI_Chose();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))    //����
        {
            Debug.Log("����");
            if (P_select == 0 || P_select == 2)
            {
                SB.L_Button();
                P_select = 0;
                m_Audio.UI_Page();
            }
            else
            {
                P_select--;
            }
            m_Audio.UI_Chose();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]))      //��
        {
            Debug.Log("��");
            if (P_select - 2 < 0)
            {
                P_select = P_select + 2;
            }
            else
            {
                P_select = P_select - 2;
            }
            m_Audio.UI_Chose();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]))    //�Ʒ�
        {
            Debug.Log("�Ʒ�");
            if (P_select + 2 >= 4)
            {
                P_select = P_select - 2;
            }
            else
            {
                P_select = P_select + 2;
            }
            m_Audio.UI_Chose();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]) && BookButtonManger.bookButtonManger.buttoncheck == false)
        {
            Debug.Log("����ħ!");
            BookButtonManger.bookButtonManger.ButtonTimerON();
            BookButtonManger.bookButtonManger.BookTitle_Select = SB.Title[P_select];
            BookButtonManger.bookButtonManger.SetIndex(1);
            SB.Title[P_select].GetComponent<StageContent>().StageButtonON();
            m_Audio.UI_Open();
            m_Audio.Game_Enter();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]))
        {
            SelectPage.SetActive(false);
            Debug.Log("Xħ!");
            BookButtonManger.bookButtonManger.SetIndex(0);
            m_Audio.UI_Cancle();
        }
    }


}
