using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements_part : MonoBehaviour
{
    public Achievements ach;
    public Image Rock;

    [Header("�ؽ�Ʈ")]
    public Text title;
    public Text contents;
    public Text state_text;

    // Start is called before the first frame update
    void Start()
    {
        title.text = ach.title;
        Main_Record.M_Reord.Keep.indexUpdate();
        state_text.text = ach.state_text;
        AchClearCheck();
    }

    public void AchClearCheck()
    {
        Debug.Log("������ �Ϸᰡ �Ǿ����� üũ��");
        if (AchievementsManage.achievementsManage.Ach_Clear[ach.foreignkey])        //�����Ϸ��϶� ó��
        {
            Debug.Log("���� ����" + ach.stats_plus_num);
            contents.text = ach.contents + "\n" + "( ���� �Ϸ� )";
            Rock.gameObject.SetActive(true);
            //AchievementsManage.achievementsManage.Ach_Clear_place(this.gameObject);
        }
        else    //������ �Ϸᰡ �ȵǾ��ٸ�
        {
            if (ach.ach_index.ToString() == "2")    //�ð� ���� �����̶�� �ٸ��� ǥ��
            {
                TimerTrans();
                contents.text = ach.contents + "\n" + "( " + hour + " �� " + min + " �� " + sec + " �� " + " / " + ach.requirements_num + "�ð�" + " )";
            }
            else
            {
                contents.text = ach.contents + "\n" + "( "  + (int)Main_Record.M_Reord.Keep.Index[int.Parse(ach.ach_index.ToString())] + " / " + ach.requirements_num + " )";
            }
            Rock.gameObject.SetActive(false);
        }
    }
    int hour;
    int min;
    int sec;
    public void TimerTrans()
    {
        hour = (int)Main_Record.M_Reord.Keep.Index[int.Parse(ach.ach_index.ToString())] / 3600;
        min = ((int)Main_Record.M_Reord.Keep.Index[int.Parse(ach.ach_index.ToString())] % 3600) / 60;
        sec = ((int)Main_Record.M_Reord.Keep.Index[int.Parse(ach.ach_index.ToString())] % 3600) % 60;
    }

    
}
