using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_group : MonoBehaviour//�׷�->�� ��ä
{
    public List<GameObject> enemy = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�� �׷���� ���� ��� �׾��ٸ� �θ� ����Ŭ���Լ� �� �׷��� ����� �ڱ�
        if (enemy.Count == 0&& this.gameObject.transform.parent.GetComponent<enemy_cycle>()!=null)
        {
            this.gameObject.transform.parent.GetComponent<enemy_cycle>().enemy_group.Remove(this.gameObject);
            if (this.gameObject.transform.parent.GetComponent<enemy_cycle>().choose_group == this.gameObject)
            {
                this.gameObject.transform.parent.GetComponent<enemy_cycle>().choose_group = null;
            }
            Destroy(this.gameObject);
        }
    }
}
