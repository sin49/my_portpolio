using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_cycle : MonoBehaviour//�� ����Ŭ->�׷�->���� ��ä �� ����
{
    public int Level;
    public List<GameObject> enemy_group = new List<GameObject>();
    public GameObject choose_group;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //���õ� �׷��� �ı���ٸ� ���� �׷� ��ȯ������ ���� �����̸� �ش�
        if (choose_group != null)
        {
            if (!choose_group.activeSelf)
                choose_group.SetActive(true);
        }
        else
        {
            this.transform.parent.parent.GetComponent<room>().tim = 1.2f;
        }
        if (enemy_group.Count == 0)//��� �׷��� �ı��ƴٸ� �ڱ��Ѵ�
        {
            this.gameObject.transform.parent.GetComponent<normal_contents>().room_cleared = true;
            Destroy(this.gameObject);
        }
    }
    public void acitve_enemy()////�׷��� �����ϰ� ���õ� �׷� Ȱ��ȭ
    {

       
            enemy_group[0].gameObject.SetActive(true);
        choose_group= enemy_group[0].gameObject;
        



    }
    public void delete_enemy(GameObject a)//a�� ���� �׷� �����
    {
        enemy_group.Remove(a);
    }
}
