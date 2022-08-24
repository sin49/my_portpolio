using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingSceneManager : MonoBehaviour//�ε� �� �޴���
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
    public void LoadStage()//�ε� �� ����������
    {
        nextScene = stageScene;
        
        SceneManager.LoadScene("Loading");
        StartCoroutine(LoadScene());
    }
    public void LoadStage(string s)//�ε� �� ����������(string���� �������� ���� ����)
    {
        nextScene = s;

        SceneManager.LoadScene("Loading");
        StartCoroutine(LoadScene());
    }
    public void Loadboss()//������ �ε�
    {
        nextScene = bossScene;
    
        SceneManager.LoadScene("Loading");
        StartCoroutine(LoadScene());
    }
    public void LoadMain()//����ȭ������ �ε�
    {
        nextScene = MainScene;
        
        SceneManager.LoadScene("Loading");
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()//�ٸ� ���� �ҷ����� ���� �� �ϳ��� ��(�ε���)�� �ҷ��ͼ� �ҷ����Ⱑ ������ ������ �÷��̾�� �ε����� ���δ� 
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        
        op.allowSceneActivation = false;//���� �ε��� ������ �ٷ� �Ѿ�� �ʵ���

      //�ε��� �����ڸ��� ���������� �Ѿ�� ���� �ʹ� ������ ���� �����ٴ� ���� �ǰ����� �⺻ �ε� �ð��� �ֱ�� ��

        while (!op.isDone)//���� ������ �Ѿ ���� �ƴ϶��(�ε��� ����)
        {
            yield return null;
            t += Time.deltaTime;
           
            if (nextScene == MainScene)//�������� �ʱ�ȭ
            {
                Gamemanager.GM.stage = 0;
            }
            if (t >= 0.8)//0.8�� ����
                {
                    t = 0;
                    op.allowSceneActivation = true;//�ε��� ������ ���� ������
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
