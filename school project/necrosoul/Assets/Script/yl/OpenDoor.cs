using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject spawner; // ������

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == true) //�ڽ�(��)�� Ȱ��ȭ �Ǿ� ������
        {
            
            if (spawner.gameObject.activeSelf == false) // �����ʰ� ��� �ִ��� üũ
            {
                
                gameObject.SetActive(false); // �����ʰ� �������� �ڽŵ� ���� 
            }
        }
    }
}
