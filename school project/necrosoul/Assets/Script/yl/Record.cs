using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour
{
    public ActionRecord ActionRecord=new ActionRecord();
    KeepActionRecord KeepRecord = new KeepActionRecord();

    int hour;
    int min;
    int sec;
    public Text StageName;
    public Text StageNum;
    public Text Timer;
    public Text Kill;
    public Text Money;
    public GameObject ActionText_prefab;
    public GameObject ActionPanel;

    public bool Game_End;

    private void Start()
    {
        ActionRecord.Reset();
        Game_End = false;
    }
    private void FixedUpdate()
    {
        if (!Game_End&&Player_status.p_status.spawn_check)
        {
            if (Player_status.p_status.get_hp() > 0)
            {
                ActionRecord.Time += Time.deltaTime;
            }
            else
            {
                Debug.Log("������ ����� ���� ����");
                finish();
            }
        }
        
    }
    public void finish()
    {
        TimerTrans();
        TextTrans();
        SaveRecord();
        AchievementsManage.achievementsManage.AchClearCheck();
        AchievementsManage.achievementsManage.Clear_Save();
        Debug.Log("�����");
        Game_End = true;
    }

    public void SaveRecord()      //��ü ��Ͽ� �����
    {
        KeepRecord.KeepSave(ActionRecord);
    }

    public void TimerTrans()
    {
        hour = (int)ActionRecord.Time / 3600;
        min = ((int)ActionRecord.Time%3600) / 60;
        sec = ((int)ActionRecord.Time % 3600) % 60;
    }

    public void TextTrans()     //����â �ؽ�Ʈ �߰�
    {
        ActionRecord.Money = Player_status.p_status.Money;
        StageName.text = "�������� :";
        StageNum.text = "�ƹ��� �峪���� �ʴ� ��";
        Kill.text = ActionRecord.EnemyKill.ToString();
        //ActionText_Plus_All();
        //ActionText.text = "-���� ������ " + ActionRecord.Damge + "\n" + "-���� ������ " + ActionRecord.Hit+"\n"+ "-�� " + ActionRecord.Money + " ȹ��\n";
        Timer.text = hour + "�� " + min + "�� " + sec + "�� ";
        Money.text = ActionRecord.Money.ToString()+"GOLD";



    }

    public void ActionText_plus(string text)       //��� �α� �ؽ�Ʈ �߰�
    {
        GameObject Text=Instantiate(ActionText_prefab, ActionPanel.transform);
        Text.GetComponent<Text>().text = text;
    }

    public void ActionText_Plus_All()       //�⺻���� �α� �ؽ�Ʈ����
    {
        ActionText_plus("-���� ������ " + ActionRecord.Damge);
        ActionText_plus("-���� ������ " + ActionRecord.Hit);
        ActionText_plus("-�� " + ActionRecord.Money);
    }
}
