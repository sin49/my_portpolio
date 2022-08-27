using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_3_2_hit : MonoBehaviour//3스테이지 보스 2페이즈 피격 기믹 클레스
{//이 오브젝트를 공격하는 것으로만 보스가 피해를 받는다
    public float color_time;
    public GameObject player_;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        player_ = GameObject.FindWithTag("Player");
        if(GetComponentInParent<boss_basic>().e_hp == 0)
        {
            Destroy(this.gameObject);
        }
    }
    //색깔을 빨갛게 만들어 보스의 패턴을 예고한다
    IEnumerator makecolor()
    {
        for (float i = 1f; i >= 0; i -= color_time)
        {
            Color color = new Vector4(1, i, i, 1);
            transform.GetComponent<SpriteRenderer>().color = color;
            
            yield return 0;

        }
    }
    //색깔을 초기화 시킨다
    public void color_change2()
    {
            Color color = new Vector4(1, 1, 1, 1);
            transform.GetComponent<SpriteRenderer>().color = color;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        //이 스크립트를 가진 오브젝트가 피격당하는 것ㅇ으로 보스가 피해를 받는다
        if (col.CompareTag("bullet"))
        {
            if (!player_.GetComponent<shooting_player>().special_power)
            {
                player_.GetComponent<shooting_player>().power_gauge++;
            }
            col.GetComponent<Bullet>().hit_animation();
            col.GetComponent<Bullet>().speed = 0;
            GetComponentInParent<boss_basic>().e_hp--;
        }
    }
}
