using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadyBookButton : MonoBehaviour
{
    public float vr;
    public float hr;
    public int P_select;
    public int T_select;

    [Header("Ÿ��Ʋ")]
    [SerializeField] List<GameObject> BookTitle = new List<GameObject>();
    
    public Ready_Book RB;
    [Header("���� �־�� �ϴ°�")]
    public GameObject BookTitlePlace;
    public GameObject Shop_SelectPage;
    public GameObject Charic_SelectPage;

    AudioManage_Main m_Audio;
    // Start is called before the first frame update
    void Start()
    {
        m_Audio = AudioManage_Main.instance;
        RB = this.gameObject.GetComponent<Ready_Book>();
        for(int i=0;i<BookTitlePlace.transform.childCount;i++)
        {
            BookTitle.Add(BookTitlePlace.transform.GetChild(i).gameObject);
        }

    }

    private void OnEnable()
    {
        ButtonActive();
        RB.RCB.Un_Click();
    }

    public void ButtonActive()
    {
        if (T_select == 0)      //��ȥ��
        {
            Shop_SelectPage.SetActive(false);
            Charic_SelectPage.SetActive(true);
        }
        else if(T_select==1)   //ĳ����
        {
            Shop_SelectPage.SetActive(true);
            Charic_SelectPage.SetActive(false);
        }
        else
        {
            Shop_SelectPage.SetActive(true);
            Charic_SelectPage.SetActive(false);
        }
    }

    public void ReadybookCheck()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (BookButtonManger.bookButtonManger.GetIndex() == 1)
        {
            ButtonMange();
        }
        else if (BookButtonManger.bookButtonManger.GetIndex() == 2 && T_select==0)  //��ȥ����
        {
            ButtonMange1();
        }
        else if (BookButtonManger.bookButtonManger.GetIndex() == 2 && T_select == 1)  //ĳ���� ���� �� ����
        {
            ButtonMange2();
        }

    }
    public void ButtonMange()
    {
        BookTitle[T_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))    //������
        {
            T_select = 1;
            ButtonActive();
            m_Audio.UI_Chose();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))    //����
        {
            T_select = 0;
            ButtonActive();
            m_Audio.UI_Chose();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]))      //��
        {
           
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]))    //�Ʒ�
        {
            
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]) && BookButtonManger.bookButtonManger.buttoncheck == false)
        {
            Debug.Log("����ħ!");
            BookButtonManger.bookButtonManger.ButtonTimerON();
            BookButtonManger.bookButtonManger.BookTitle_Select = BookTitle[T_select];
            ReadybookCheck();
            if (T_select == 1)
            {
                RB.RCB.In_Click();
            }
            BookButtonManger.bookButtonManger.SetIndex(2);
            m_Audio.UI_Open();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]))
        {
            Debug.Log("Xħ!");
            BookButtonManger.bookButtonManger.BookTitle_Select = null;
            BookButtonManger.bookButtonManger.SetIndex(0);
            m_Audio.UI_Cancle();
        }
    }
    public void ButtonMange1()  //��ȥ��
    {
        RB.Shop[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))    //������
        {

        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))    //����
        {

        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]))      //��
        {
            Debug.Log("��");
            if (P_select <= 0)
            {
                P_select = 3;
            }
            else
            {
                P_select = P_select - 1;
            }
            m_Audio.UI_Chose();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]))    //�Ʒ�
        {
            Debug.Log("�Ʒ�");
            if (P_select >= 3)
            {
                P_select = 0;
            }
            else
            {
                P_select = P_select + 1;
            }
            m_Audio.UI_Chose();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]) && BookButtonManger.bookButtonManger.buttoncheck == false)
        {
            Debug.Log("����ħ!");
            BookButtonManger.bookButtonManger.ButtonTimerON();
            //BookButtonManger.bookButtonManger.SetIndex(3);
            m_Audio.UI_Open();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]))
        {
            Debug.Log("Xħ!");
            RB.Shop[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(false);
            BookButtonManger.bookButtonManger.SetIndex(1);
            m_Audio.UI_Cancle();
        }
    }

    public void ButtonMange2()
    {
       
        if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))    //������
        {

        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))    //����
        {

        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]))      //��
        {
            
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]))    //�Ʒ�
        {
           
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]) && BookButtonManger.bookButtonManger.buttoncheck == false)
        {
            Debug.Log("����ħ!");
            BookButtonManger.bookButtonManger.ButtonTimerON();
            BookButtonManger.bookButtonManger.SetIndex(3);
            AudioManage_BGM.instance.Stage1();
            LoadingSceneManager.l_scenemanager.LoadStage("stage1");
            m_Audio.UI_Open();
            m_Audio.Game_Enter();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]))
        {
            Debug.Log("Xħ!");
            BookButtonManger.bookButtonManger.SetIndex(1);
            RB.RCB.Un_Click();
            m_Audio.UI_Cancle();
        }
    }
}
