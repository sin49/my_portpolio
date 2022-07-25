using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_group : MonoBehaviour//그룹->적 개채
{
    public List<GameObject> enemy = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //이 그룹안의 적이 모두 죽었다면 부모 사이클에게서 이 그룹을 지우고 자괴
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
