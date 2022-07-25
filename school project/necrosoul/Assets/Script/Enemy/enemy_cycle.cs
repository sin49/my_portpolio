using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_cycle : MonoBehaviour//적 사이클->그룹->딘일 개채 로 구성
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
        //선택된 그룹이 파괴됬다면 다음 그룹 소환까지의 여유 딜레이를 준다
        if (choose_group != null)
        {
            if (!choose_group.activeSelf)
                choose_group.SetActive(true);
        }
        else
        {
            this.transform.parent.parent.GetComponent<room>().tim = 1.2f;
        }
        if (enemy_group.Count == 0)//모든 그룹이 파괴됐다면 자괴한다
        {
            this.gameObject.transform.parent.GetComponent<normal_contents>().room_cleared = true;
            Destroy(this.gameObject);
        }
    }
    public void acitve_enemy()////그룹을 선택하고 선택된 그룹 활성화
    {

       
            enemy_group[0].gameObject.SetActive(true);
        choose_group= enemy_group[0].gameObject;
        



    }
    public void delete_enemy(GameObject a)//a와 같은 그룹 지우기
    {
        enemy_group.Remove(a);
    }
}
