using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingSceneManager : MonoBehaviour//로딩 씬 메니저
{
    public string stageScene;
    public string bossScene;
    public string MainScene;
    string nextScene;
    public static LoadingSceneManager l_scenemanager;
    public float t = 0;
    private void Awake()
    {
        l_scenemanager = this;
    }
    void Start()
    {
        
       
       // DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadStage()//로딩 후 스테이지로
    {
        nextScene = stageScene;
        
        SceneManager.LoadScene("Loading");
        StartCoroutine(LoadScene());
    }
    public void LoadStage(string s)//로딩 후 스테이지로(string으로 스테이지 지정 가능)
    {
        nextScene = s;

        SceneManager.LoadScene("Loading");
        StartCoroutine(LoadScene());
    }
    public void Loadboss()//보스로 로딩
    {
        nextScene = bossScene;
    
        SceneManager.LoadScene("Loading");
        StartCoroutine(LoadScene());
    }
    public void LoadMain()//메인화면으로 로딩
    {
        nextScene = MainScene;
        
        SceneManager.LoadScene("Loading");
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()//다른 씬을 불려오는 동안 또 하나의 씬(로딩씬)을 불려와서 불려오기가 끝나기 전까지 플레이어에게 로딩씬을 보인다 
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        
        op.allowSceneActivation = false;//씬의 로딩이 끝나면 바로 넘어가지 않도록

      //로딩이 끝나자마자 다음씬으로 넘어가는 것이 너무 빠르면 눈이 아프다는 내부 의견으로 기본 로딩 시간을 넣기로 함

        while (!op.isDone)//다음 씬으로 넘어간 것이 아니라면(로딩씬 동안)
        {
            yield return null;
            t += Time.deltaTime;
           
            if (nextScene == MainScene)//스테이지 초기화
            {
                Gamemanager.GM.stage = 0;
            }
            if (t >= 0.8)//0.8초 이후
                {
                    t = 0;
                    op.allowSceneActivation = true;//로딩이 끝나면 다음 씬으로
                if (nextScene == "stage1")
                {
                    Gamemanager.GM.stage += 1;
                }
                if (Gamemanager.GM != null)
                {
                    if (Gamemanager.GM.initializing)
                    {
                        Gamemanager.GM.initializing = false;
                    }
                }
                    yield break;
                }
       
        }
 
         
        //float timer = 0.0f;
        /* while (!op.isDone)
         {
             yield return null;
             timer += Time.deltaTime;
             if(op.progress<0.9f)
         }*/
    }
}
