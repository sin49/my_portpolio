using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BookMange : MonoBehaviour
{
    public List<GameObject> BookMark = new List<GameObject>();
    public GameObject BookMarkPlace;
    public List<GameObject> Book = new List<GameObject>();
    public GameObject BookPlace;

    public GameObject Text;
    

    private void Awake()
    {
        FirstSetting();

    }

    // Start is called before the first frame update
    void Start()
    {
        SceneAppear_Manager();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SceneAppear_Manager()
    {
        Debug.Log("���� �Ѿ�� ����� �������� ����Ѵ�.");
        this.gameObject.SetActive(false);
        Text.SetActive(false);
        if (SceneManager.GetActiveScene().name == "StageSelect")        //����â�̶��
        {
            this.gameObject.SetActive(true);
            Text.SetActive(true);
            for (int i = 0; i < BookMark.Count; i++)
            {
                BookMark[i].SetActive(true);
            }

            BookMark[1].SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == "Main")
        {

        }
        else        //���� �÷��� â�̶��
        {
            for (int i = 0; i < BookMark.Count; i++)
            {
                BookMark[i].SetActive(true);
            }
            BookMark[0].SetActive(false);
        }
        Debug.Log("����ȯ�ƾƾƾƤ��Ƥ����ƾƾƾƤ��ƾƾӤ��ƾƾƾӾƾƾƾ�");
    }

    public void FirstSetting()
    {
        for (int i = 0; i < BookPlace.transform.childCount; i++)
        {
            Book.Add(BookPlace.transform.GetChild(i).gameObject);
        }
        for (int i=0; i<BookMarkPlace.transform.childCount;i++)
        {
            BookMarkPlace.transform.GetChild(i).GetComponent<BookMarkEvent>().Connect_Book = Book[i];
            BookMark.Add(BookMarkPlace.transform.GetChild(i).gameObject);

        }

    }

    void OnEnable()
    {
        // ��������Ʈ ü�� �߰�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneAppear_Manager();
    }

    void OnDisable()
    {
        // ��������Ʈ ü�� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
