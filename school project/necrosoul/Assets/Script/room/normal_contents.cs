using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normal_contents : MonoBehaviour//���� ������ �� Ŭ����
{
    //�븻 ������<-����Ŭ<-�׷�<-��
    public List<enemy_cycle> cycle = new List<enemy_cycle>();
    public List<enemy_cycle> index_cycle = new List<enemy_cycle>();
    
    public bool index_chk;
    public int index;
    int rand;
    public bool start_chk;
    public bool cycle_chk;
    public enemy_cycle choose_cycle;
    public bool room_cleared;
    private void Awake()
    {
       
    }
    void Start()
    {
        //���������� ���� �۵��� ����Ŭ ����(index=���̵��� ���� ����)
        if (Gamemanager.GM.stage == 2)
        {
            index = 2;
        }
        else
        {
            index = 1;
        }
        //cycle Ž��
        set_cycle_index(index);
        for (int i = 0; i < cycle.Count; i++)
        {
            cycle[i].gameObject.SetActive(false);
        }
       

    }
    //index�� �´� cycle�� �濡 �����ϴ� cycle����Ʈ���� ��� Ž���� ���� ���߿��� ���Ƿ� ���õ� cycle�� �濡�� ������ �� �������� ���Ѵ�
    public void set_cycle_index(int i)///index�� �´� cycle Ž��
    {

        int n = i;
        Debug.Log(n);
        for (int a = 0; a < cycle.Count; a++)
        {
            Debug.Log("Level:"+cycle[a].Level);
            if (cycle[a].Level == n)
            {
                index_cycle.Add(cycle[a]);//index���� cycle ����Ʈ
            }
        }
        index_chk = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Gamemanager.GM.fade_in_complete)
        {
            start_chk = true;
        }
        if (index_chk)
        {
           
                rand = Random.Range(0, index_cycle.Count);
            //index_cycle �߿��� ������ cycle�� �� ���� �������� ���Ѵ�
            choose_cycle = index_cycle[rand];
 
                cycle_chk = true;
                index_chk = false;
            
            
        }
        
    }
    
    public void acitve_enemy()//cycle�� Ȱ��ȭ��Ű�� ���� �����Ѵ�
    {
        if (choose_cycle.choose_group == null && choose_cycle.enemy_group.Count != 0)//����Ŭ���� ���� ���õ� �׷��� ���ٸ�
        {
            choose_cycle.gameObject.SetActive(true);//����Ŭ Ȱ��ȭ
            choose_cycle.acitve_enemy();//�� ����(�׷� Ȱ��ȭ)
        }
        



    }
   
}
