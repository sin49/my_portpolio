using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BookMarkButton : MonoBehaviour
{
    
    public int P_select;

    [Header("�ϸ�ũ")]
    [SerializeField] List<GameObject> BookMark = new List<GameObject>();
    [SerializeField] public GameObject BookMark_Select;
    public BookMange BM;
    public AudioManage_Main M_Audio;

    // Start is called before the first frame update
    void Start()
    {
        M_Audio = AudioManage_Main.instance;
        BM = GameObject.Find("TotalBook").GetComponent<BookMange>();
        Fristsetting();
    }

    // Update is called once per frame
    void Update()
    {
        if (BookButtonManger.bookButtonManger.GetIndex() == 0)
        {
            ButtonMange();
        }
    }

    public void Fristsetting()
    {   
        //�����ϸ鼭 �ʱ�ȭ
        BookMark.Clear();
        for (int i = 0; i < BM.BookMark.Count; i++)
        {
            BM.BookMark[i].GetComponent<Toggle>().isOn = false;
        }
        for (int i = 0; i < BM.Book.Count; i++)
        {
            BM.Book[i].SetActive(false);
        }

        Debug.Log("�ϸ�ũ ����" + BM.BookMark.Count);
        for (int i = 0; i < BM.BookMark.Count; i++)
        {
            Debug.Log("Ȯ��");
            if (BM.BookMark[i].activeSelf)
            {
                Debug.Log("������");
                BookMark.Add(BM.BookMark[i]);
            }
        }
    }
    public void ButtonMange()
    {
        if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))    //������
        {
            Debug.Log("������");
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))    //����
        {
            Debug.Log("����");
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]))      //��
        {
            Debug.Log("��");
            if (P_select == 0)
            {
                P_select = BookMark.Count - 1;
            }
            else
            {
                P_select--;
            }
            BookMark[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
            M_Audio.UI_Page();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]))    //�Ʒ�
        {
            Debug.Log("�Ʒ�");
            if (P_select == BookMark.Count - 1)
            {
                P_select = 0;
            }
            else
            {
                P_select++;
            }
            BookMark[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
            M_Audio.UI_Page();
        }
        else if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK])&& BookButtonManger.bookButtonManger.buttoncheck == false)
        {
            BookButtonManger.bookButtonManger.ButtonTimerON();
            Debug.Log("����ħ!");
            M_Audio.UI_Click();
            BookButtonManger.bookButtonManger.BookMark_Select = BookMark[P_select];
            BookButtonManger.bookButtonManger.SetIndex(1);
        }
    }

    void OnEnable()
    {
        // ��������Ʈ ü�� �߰�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("�ٲ�鼭 ���� �ΰ�?");
        Fristsetting();
    }

    void OnDisable()
    {
        // ��������Ʈ ü�� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
