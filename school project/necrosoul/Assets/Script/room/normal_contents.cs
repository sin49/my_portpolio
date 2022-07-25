using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normal_contents : MonoBehaviour//적이 나오는 방 클래스
{
    //노말 콘텐츠<-사이클<-그룹<-적
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
        //스테이지에 따라 작동할 사이클 변경(index=난이도와 같은 개념)
        if (Gamemanager.GM.stage == 2)
        {
            index = 2;
        }
        else
        {
            index = 1;
        }
        //cycle 탐색
        set_cycle_index(index);
        for (int i = 0; i < cycle.Count; i++)
        {
            cycle[i].gameObject.SetActive(false);
        }
       

    }
    //index에 맞는 cycle을 방에 존재하는 cycle리스트에서 모두 탐색한 다음 그중에서 임의로 선택된 cycle을 방에서 생성할 적 패턴으로 정한다
    public void set_cycle_index(int i)///index에 맞는 cycle 탐색
    {

        int n = i;
        Debug.Log(n);
        for (int a = 0; a < cycle.Count; a++)
        {
            Debug.Log("Level:"+cycle[a].Level);
            if (cycle[a].Level == n)
            {
                index_cycle.Add(cycle[a]);//index값의 cycle 리스트
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
            //index_cycle 중에서 임의의 cycle을 적 스폰 패턴으로 정한다
            choose_cycle = index_cycle[rand];
 
                cycle_chk = true;
                index_chk = false;
            
            
        }
        
    }
    
    public void acitve_enemy()//cycle을 활성화시키고 적을 생성한다
    {
        if (choose_cycle.choose_group == null && choose_cycle.enemy_group.Count != 0)//사이클에서 현재 선택된 그룹이 없다면
        {
            choose_cycle.gameObject.SetActive(true);//사이클 활성화
            choose_cycle.acitve_enemy();//적 생성(그룹 활성화)
        }
        



    }
   
}
