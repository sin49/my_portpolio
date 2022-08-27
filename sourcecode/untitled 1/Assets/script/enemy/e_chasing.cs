using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_chasing : MonoBehaviour//플레이어가 일정 범위 안에 있으면 적이 플레이어를 추격하게 하는 클레스
{
    Enemy_basic e_basic;
    public GameObject enemy;
    public float C_time=1;
    // Start is called before the first frame update
    void Start()
    {
        e_basic = GetComponent<Enemy_basic>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    //이 충돌 범위에 플레이어가 닿았다면 플레이어를 추격하나
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Collide");
            enemy.GetComponent<Enemy_basic>().onchasing = true;
            StartCoroutine(ChaisngControl());
        }
    }
    //플레이어를 추격하는 ai로 변경한는 코루틴
    public IEnumerator ChaisngControl()
    {
        while (true)
        {
            enemy.GetComponent<Enemy_basic>().onchasing = true;
            yield return new WaitForSeconds(C_time);
        }
    }
}
