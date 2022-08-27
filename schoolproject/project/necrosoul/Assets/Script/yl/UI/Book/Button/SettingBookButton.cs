using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingBookButton : MonoBehaviour
{
    public float vr;
    public float hr;
    public int P_select;

    [Header("Ÿ��Ʋ")]
    [SerializeField] List<GameObject> BookTitle = new List<GameObject>();
    public Setting_Book SB;

    bool Act;
    // Start is called before the first frame update
    void Start()
    {
        SB = this.gameObject.GetComponent<Setting_Book>();
      
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
        SB.Title[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        vr = Input.GetAxis("Vertical");
        hr = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Horizontal"))
        {
            if (hr > 0)     //������
            {
                Debug.Log("������");
                SB.R_Button();
                P_select = 0;
            }
            else        //����
            {
                Debug.Log("����");
                SB.L_Button();
                P_select = 0;
            }
            //SB.Title[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        }
        else if (Input.GetButtonDown("Vertical"))
        {
            if (vr > 0)     //��
            {
                Debug.Log("��");
                if (P_select == 0)
                {
                    P_select = SB.ActiveButton-1;
                }
                else
                {
                    P_select--;
                }
            }
            else        //�Ʒ�
            {
                Debug.Log("�Ʒ�");
                if (P_select == SB.ActiveButton-1)
                {
                    P_select = 0;
                }
                else
                {
                    P_select++;
                }

            }
            //SB.Title[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        }
        //if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]))
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("����ħ!");
            
            BookButtonManger.bookButtonManger.BookTitle_Select = SB.Title[P_select];
            BookButtonManger.bookButtonManger.SetIndex(2);
            SB.Title[P_select].GetComponent<StageContent>().StageButtonON();
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            Debug.Log("Xħ!");
            BookButtonManger.bookButtonManger.BookMark_Select = null;
            BookButtonManger.bookButtonManger.SetIndex(0);
            SB.Title[P_select].GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        }
    }



}
