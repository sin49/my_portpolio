using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{

    public GameObject door; //��
    public GameObject enemy; //��

    public int spawnCount = 0; //���� Ƚ��

    // Start is called before the first frame update
    void Start()
    {
        spawnCount = 1; // ����� 1ȸ
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCount > 0 && door.gameObject.activeSelf == true) //���� ī��Ʈ�� �ְ�, ���� Ȱ��ȭ �Ǿ��ִٸ�
        {
            GameObject second = (GameObject)Instantiate(enemy); //���� ����
            second.transform.position = this.transform.position;
            spawnCount--; // ��ȯ�� ������ ���� ī��Ʈ ����
        }


        if (spawnCount == 0) //���� ��� ��ȯ �Ǿ��� ��
        {
            if (!GameObject.Find("enemy(Clone)")) //���� �������� ������
            {
                gameObject.SetActive(false); // �ڽ�(������)�� ��Ȱ��ȭ
            }
        }

    }
}
