using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear_bullet : MonoBehaviour//게임 내 존재하는 적과 탄을 제거
{
    public GameObject[] bullet;
    public GameObject[] enemy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //적과 탄을 탐색
        bullet = GameObject.FindGameObjectsWithTag("e_bullet");
        enemy = GameObject.FindGameObjectsWithTag("enemy");
        
        
    }
    public void run()//탐색한 적과 탄을 모두 제거한다
    {
        for (int i = 0; i < bullet.Length; i++)
        {
            Destroy(bullet[i]);

        }
        for (int i = 0; i < enemy.Length; i++)
        {
            Destroy(enemy[i]);

        }
    }
    public void run2()//탐색한 탄을 모두 제거한다
    {
        for (int i = 0; i < bullet.Length; i++)
        {
            Destroy(bullet[i]);

        }
        
    }

}
