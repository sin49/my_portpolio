using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManage : MonoBehaviour
{
    static public AchievementsManage achievementsManage;

    [Header("Achievement")]
    [SerializeField]
    public List<Achievements> Achievement_All;  //�������� ����
    public List<bool> Ach_Clear;
    [SerializeField] public Dictionary<string, List<Achievements>> AchievementsList = new Dictionary<string, List<Achievements>>(); //���� ���̱�� ����Ʈ

    [Header("����Ŭ ��� â")]
    public Record record;

    [Header("����Ŭ���� ����")]
    public List<Achievements> AchClear_panel;
    public AchClear_panel AchClear_panel_place;
    public GameObject AchNewText;
    Achievements AchPrefab;
    GameObject AchPrefab_instance;

    Achievements ach = new Achievements();

    [Header("���� ����")]
    public int Ach_MaxHp;
    public int Ach_Damge;
    public int Ach_Defense;
    public int Ach_Jump;
    public int Ach_Speed;
    public int Ach_AtkSPD;

    [Header("���� �־���ϴ� ��")]
    public Ach_Book achbook;

    private void Awake()
    {
        achievementsManage = this;
        CreateItem();
        Refrsh_Ach_apply();
        achbook.FirstPreparation();
    }

    public void CreateItem()
    {
        List<Dictionary<string, object>> Data = CSVReader.Read("Achievements");

        for (int i = 0; i < Data.Count; i++)
        {
            ach.foreignkey = int.Parse(Data[i]["foreignkey"].ToString());
            ach.type= Data[i]["type"].ToString();
            ach.title= Data[i]["title"].ToString();
            ach.contents= Data[i]["contents"].ToString();
            ach.requirements_num = Data[i]["requirements_num"].ToString();
            ach.state_text = Data[i]["stats_text"].ToString();
            ach.stats_name= Data[i]["stats_name"].ToString();
            ach.stats_plus_num= Data[i]["stats_plus_num"].ToString();
            ach.ach_index = Data[i]["ach_index"].ToString();
            Ach_Clear.Add(false);
            Achievement_All.Add(ach.Create());
            //Ach_Classify(ach);
        }
        Clear_Load();
    }

    public void CreateAchList(string Conect)        //�������� �з���Ű�� ��ųʸ��� �����.
    {
        List<Achievements> Ach = new List<Achievements>();
        for(int i=0;i<Achievement_All.Count;i++)
        {
            if (Achievement_All[i].stats_name == Conect)
            {
                if (!Ach_Clear[i])      //Ŭ���� ���Ѱ͸� �ֱ�
                {
                    Ach.Add(Achievement_All[i]);
                }
            }
            else if (Conect == "clear") //Ŭ����
            {
                if (Ach_Clear[i])
                {
                    Ach.Add(Achievement_All[i]);
                }
            }
            
        }
        AchievementsList.Add(Conect, Ach);
    }

    public void AchClearMove()      //���� Ŭ���� ó��
    {
        for(int i=0; i<Ach_Clear.Count;i++)
        {
            if(Ach_Clear[i])
            {
                Debug.Log("���� ���� Ŭ����1212" + Achievement_All[i].stats_name);
                Debug.Log("���� ���� Ŭ����" + AchievementsList[Achievement_All[i].stats_name][i].title);
                AchievementsList[Achievement_All[i].stats_name].Add(Achievement_All[i]);
                AchievementsList[Achievement_All[i].stats_name].RemoveAt(i);
            }
        }
    }
    
    public void AchClearUpdate(int AchClear_Index)        //����Ŭ �г� ����
    {
            AchClear_panel_place.Title.text = AchClear_panel[AchClear_Index - 1].title;
            AchClear_panel_place.Content.text = AchClear_panel[AchClear_Index - 1].contents;
            AchClear_panel_place.Count.text = AchClear_Index.ToString() + "/" + AchClear_panel.Count;
    }

    public void AchClearCheck()     //���� ���� Ŭ üũ
    {
        Debug.Log("���� ���ʷ� Ŭ���� �ߴ��� üũ ��");
        for (int i=0; i<Achievement_All.Count;i++)
        {
            if(!Ach_Clear[i])   //Ŭ���� �ȵȰ͵� �߿���
            {
                if(Main_Record.M_Reord.Check_Ach(Achievement_All[i].type, Achievement_All[i].requirements_num))     //�� ��
                {
                    AchClear_panel.Add(Achievement_All[i]);
                    Ach_Clear[i] = true;
                   // record.ActionText_plus("- "+Achievement_All[i].title + " Ŭ����");
                }
            }
        }

        if (AchClear_panel.Count > 0)   //���� Ŭ���� �г� ����
        {
            AchClear_panel_place.gameObject.SetActive(true);
            AchClear_panel_place.AchPanelEnd();
            //AchNewText.SetActive(true);
            AchClearMove();
        }
        Refrsh_Ach_apply();
    }

    public void Clear_Load()
    {
        if (ES3.FileExists(Application.persistentDataPath + "/" + SavePath.path + "/" + "Ach_Clear.es3"))
        {
            List<bool> Past = new List<bool>();
            Past = ES3.Load("Ach_Clear", Application.persistentDataPath + "/" + SavePath.path + "/" + "Ach_Clear.es3",Ach_Clear);
            for (int i = 0; i < Past.Count; i++)      //������Ʈ
            {
                Ach_Clear[i] = Past[i];
            }
        }
        else
        {
            Debug.Log("���̺� ������ �������� �ʾ� ���� �����մϴ�.");
            Clear_Save();
            Clear_Load();
        }
    }
    public void Clear_Save()
    {
        ES3.Save("Ach_Clear",Ach_Clear, Application.persistentDataPath + "/" + SavePath.path + "/" + "Ach_Clear.es3");
    }

    public void Refrsh_Ach_apply()      //Ŭ���� üũ �� ���� ���� �۾�
    {
        for (int i = 0; i < Ach_Clear.Count; i++)
        {
            if (Ach_Clear[i])   //���� Ŭ������
            {
                if (!Achievement_All[i].Statapply)
                {
                    Debug.Log("�����Ϸ� ������ �����ŵ�ϴ�.");
                    PlusState(Achievement_All[i]);    //������ �����Ų��
                }
            }
        }
    }
    public void PlusState(Achievements ach)      //���� ����
    {
        Debug.Log("�����Ϸ� ������ ������ ��" + ach.type);
        Debug.Log("�����Ϸ� ������ ������ ��" + ach.stats_plus_num);
        ach.Statapply = true;
        switch (ach.stats_name)
        {
            case "hp":
                Ach_MaxHp += int.Parse(ach.stats_plus_num.ToString());
                break;
            case "power":
                Ach_Damge += int.Parse(ach.stats_plus_num.ToString());
                break;
            case "armor":
                Ach_Defense += int.Parse(ach.stats_plus_num.ToString());
                break;
            case "jump":
                Ach_Jump += int.Parse(ach.stats_plus_num.ToString());
                break;
            case "speed":
                Ach_Speed += int.Parse(ach.stats_plus_num.ToString());
                break;
        }
    }
}
