using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Setting//���� �ý��ۿ� ����� ���� ��
{
    //���� ������ ����, ����, �ҷ����� ���� ������ �����
    public bool full_scren;
    public int screen_resolution_size_X;
    public int screen_resolution_size_Y;
    public float full_volume;
    public float bgm_volume;
    public int screen_resol_index;
    public float sfx_volume;
    //���� ���
    //string Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3" = Application.persistentDataApplication.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3" + "/save/" + "setting.es3";
    public List<KeyCode> Keys = new List<KeyCode>();
    public Setting()
    {
       // load_setting();
    }
    // Update is called once per frame
   
    public void save_setting()//������ ���� ��η� ����
    {
        if(ES3.FileExists(Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3"))
        ES3.Save("setting", this, Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3");

        
    }
    
     public void load_setting()//���� ������ �ҷ��´�
    {
        if (ES3.FileExists(Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3"))//���� ��ο� ������ �����Ѵٸ�
        {
            //�ҷ�����
            Setting s3 = ES3.Load<Setting>("setting", Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3");
            //�ҷ��� �������� �� Ŭ������ ������ �����ϳ�
                full_scren = s3.full_scren;//ȭ�鼳��
    
                screen_resolution_size_X = s3.screen_resolution_size_X;

                screen_resolution_size_Y = s3.screen_resolution_size_Y;
 
                screen_resol_index = s3.screen_resol_index;
    
                full_volume = s3.full_volume;//��������
 
                bgm_volume = s3.bgm_volume;
      
                sfx_volume = s3.sfx_volume;
       
             
                for (int i = 0; i < s3.Keys.Count; i++)//Ű����
                {
                    Keys.Add(s3.Keys[i]);
                }
                if (s3.Keys.Count < 9)//Ű������ ������ �֟� ��� �⺻�� ����
                {
                    int a = 9 - s3.Keys.Count;
                    for(int i = 9-a; i <9; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                Keys.Add(KeyCode.UpArrow);
                                break;
                            case 1:
                                Keys.Add(KeyCode.DownArrow);
                                break;
                            case 2:
                                Keys.Add(KeyCode.LeftArrow);
                                break;
                            case 3:
                                Keys.Add(KeyCode.RightArrow);
                                break;
                            case 4:
                                Keys.Add(KeyCode.Z);
                                break;
                            case 5:
                                Keys.Add(KeyCode.X);
                                break;
                            case 6:
                                Keys.Add(KeyCode.LeftShift);
                                break;
                            case 7:
                                Keys.Add(KeyCode.Tab);
                                break;
                            case 8:
                                Keys.Add(KeyCode.Escape);
                                break;
                        }
                    }
                }

                Debug.Log("�ҷ�����");
            
        }
        else//���ϰ�ο� ����� ������ ���ٸ� ���� ����
        {
            initialize_setting();
            ES3.Save("setting", this, Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3");
            Debug.Log("���θ����");
        }
    }
    public void initialize_setting()//������ ���� ����
    {
        set_original_key_setting();
        set_original_screen_resolution();
        set_original_volume();
    }
 
    public void set_original_key_setting()//Ű���� �ʱ� ��
    {
        Keys.Add(KeyCode.UpArrow);
        Keys.Add(KeyCode.DownArrow);
        Keys.Add(KeyCode.LeftArrow);
        Keys.Add(KeyCode.RightArrow);
        Keys.Add(KeyCode.Z);
        Keys.Add(KeyCode.X);
        Keys.Add(KeyCode.LeftShift);
        Keys.Add(KeyCode.Tab);
        Keys.Add(KeyCode.Escape);
      
    }
    public void set_original_screen_resolution()//�ػ� �ʱ� ��
    {
        full_scren = true;
        screen_resolution_size_X = 1920;
        screen_resolution_size_Y = 1080;
        screen_resol_index = 0;
    }
    public void set_original_volume()//���� �ʱ� ��
    {
        full_volume = 1;
        sfx_volume = 1;
        bgm_volume = 1;
    }
}
