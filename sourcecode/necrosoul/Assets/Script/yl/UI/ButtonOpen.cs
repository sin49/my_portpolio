using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpen : MonoBehaviour
{
    public GameObject AchPanel;
    public GameObject AchNewText;
    public void Open()
    {
        Debug.Log("���� ��ư ���� ��ư �������ϴ�.");
        AchPanel.SetActive(true);
        AchNewText.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
