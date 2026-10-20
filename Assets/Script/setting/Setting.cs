using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Setting//게임 시스템에 저장된 설정 값
{
    //설정 파일을 저장, 수정, 불려오기 등은 에셋을 사용함
    public bool full_scren;
    public int screen_resolution_size_X;
    public int screen_resolution_size_Y;
    public float full_volume;
    public float bgm_volume;
    public int screen_resol_index;
    public float sfx_volume;
    //저장 경로
    //string Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3" = Application.persistentDataApplication.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3" + "/save/" + "setting.es3";
    public List<KeyCode> Keys = new List<KeyCode>();
    public Setting()
    {
       // load_setting();
    }
    // Update is called once per frame
   
    public void save_setting()//정해진 파일 경로로 저장
    {
        if(ES3.FileExists(Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3"))
        ES3.Save("setting", this, Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3");

        
    }
    
     public void load_setting()//설정 파일을 불려온다
    {
        if (ES3.FileExists(Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3"))//파일 경로에 파일이 존재한다면
        {
            //불려오기
            Setting s3 = ES3.Load<Setting>("setting", Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3");
            //불려온 설정값을 이 클레스의 값으로 지정하나
                full_scren = s3.full_scren;//화면설정
    
                screen_resolution_size_X = s3.screen_resolution_size_X;

                screen_resolution_size_Y = s3.screen_resolution_size_Y;
 
                screen_resol_index = s3.screen_resol_index;
    
                full_volume = s3.full_volume;//볼륨설정
 
                bgm_volume = s3.bgm_volume;
      
                sfx_volume = s3.sfx_volume;
       
             
                for (int i = 0; i < s3.Keys.Count; i++)//키설정
                {
                    Keys.Add(s3.Keys[i]);
                }
                if (s3.Keys.Count < 9)//키설정에 오류가 있읅 경우 기본값 저장
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

                Debug.Log("불려오기");
            
        }
        else//파일경로에 저장된 설정이 없다면 새로 생성
        {
            initialize_setting();
            ES3.Save("setting", this, Application.persistentDataPath + "/" + SavePath.path + "/" + "SSSSS.es3");
            Debug.Log("새로만들기");
        }
    }
    public void initialize_setting()//설정을 새로 생성
    {
        set_original_key_setting();
        set_original_screen_resolution();
        set_original_volume();
    }
 
    public void set_original_key_setting()//키설정 초기 값
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
    public void set_original_screen_resolution()//해상도 초기 값
    {
        full_scren = true;
        screen_resolution_size_X = 1920;
        screen_resolution_size_Y = 1080;
        screen_resol_index = 0;
    }
    public void set_original_volume()//볼륨 초기 값
    {
        full_volume = 1;
        sfx_volume = 1;
        bgm_volume = 1;
    }
}
