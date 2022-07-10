using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    KeepActionRecord Kep;
    List<bool> Ach_Clear=new List<bool>();

    [Header("���̺� ���� ��� �ؽ�Ʈ")]
    public Text Stage_T;
    public Text Clear_T;
    public Text Ach_T;
    public Text Time_T;

    public Text NullText;
    [Header("��¿���")]
    public GameObject Image;
    public GameObject aliveSave;
    public GameObject NullSave;

    bool Savecheck;
    string path;
    int AchClearCount;
    int ClearIndex;
    string RecentStage;

    int hour;
    int min;
    int sec;

    private void Start()
    {
        path = this.gameObject.name;
        KeepLoad();
    }

    public void KeepLoad()
    {
        if (ES3.FileExists(Application.persistentDataPath + "/"+path+"/" + "AllRecord.es3"))
        {
            Debug.Log("���̺� ���� ����");
            Savecheck = true;
            Kep = ES3.Load<KeepActionRecord>("AllRecord", Application.persistentDataPath + "/" + path + "/" + "AllRecord.es3");
            Ach_Clear = ES3.Load("Ach_Clear", Application.persistentDataPath + "/" + path + "/" + "Ach_Clear.es3", Ach_Clear);
            CheckAchClear();
            CheckStageClear();
            TimerTrans();
            OnSave();
            
        }
        else
        {
            OffSave();
            Savecheck = false;
        }
    }

    public void CheckAchClear()
    {
        for(int i=0;i<Ach_Clear.Count;i++)
        {
            if(Ach_Clear[i])
            {
                AchClearCount++;
                ClearIndex = i;
            }
        }
       
    }

    public void CheckStageClear()
    {
        List<Dictionary<string, object>> Data = CSVReader.Read("Stage");
        RecentStage = Data[ClearIndex]["Name"].ToString();      //�ӽ÷� �־����� ���߿� �������� Ŭ��� ��������

    }

    public void ButtonOn()      //Ŭ������ ��
    {
        SavePath.path = this.path;
        SceneManager.LoadScene("StageSelect");
    }
  
    public void TimerTrans()
    {
        hour = (int)Kep.T_Time / 3600;
        min = ((int)Kep.T_Time % 3600) / 60;
        sec = ((int)Kep.T_Time % 3600) % 60;
    }

    public void OffSave()
    {
        Image.SetActive(false);
        aliveSave.SetActive(false);
        NullSave.SetActive(true);
        NullText.text = "�����";
    }
    public void OnSave()
    {
        Image.SetActive(true);
        aliveSave.SetActive(true);
        NullSave.SetActive(false);
        Stage_T.text = RecentStage;
        Clear_T.text = "������Ʈ ����";
        Ach_T.text = AchClearCount + " / " + Ach_Clear.Count;
        Time_T.text = "\n" + hour + " �ð� " + min + " �� " + sec + " �� ";
    }
}
