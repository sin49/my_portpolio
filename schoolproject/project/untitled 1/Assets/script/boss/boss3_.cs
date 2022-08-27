using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_ : MonoBehaviour//3스테이지 보스의 2페이즈의 오브젝트를 생성하는 클레스
{
    public bool endure_check;
    public float time;
    public GameObject boss3_2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endure_check)//1페이즈로 부터 2페이즈 오브젝트 생성 명령을 boss_basic.cs를 통해 endure_check로 받는다
        {
            time += Time.deltaTime;
            if (time > 8)
            {
                GameObject boss = Instantiate(boss3_2, new Vector3(5f, 0, 10), transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }
}
