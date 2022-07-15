using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class e_deathrattle : MonoBehaviour//적이 죽을 때 작동하는 클레스
{
    public Transform[] position;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //적이 죽을때 플레이어를 추격하는 4마리의 적을 생성한다.
        //enemy_basic의 사망 조건과 겹치지 않게 하기위해 트리거가 되는 int값을 1로 잡음
        if (gameObject.GetComponent<Enemy_basic>().e_hp <= 1)
        {
            for(int i = 0; i <= 3; i++)
            {
                GameObject enemy1 = Instantiate(enemy, position[i].position, position[i].rotation);
                enemy1.GetComponent<e_bulletManager>().e_bullet_mode = 2;
                enemy1.GetComponent<Enemy_basic>().e_hp = 7;
                enemy1.GetComponent<Enemy_basic>().e_type = 2;
                enemy1.GetComponent<Enemy_basic>().speed = 0.5f;
            }
            Destroy(this.gameObject);
        }
    }
}
