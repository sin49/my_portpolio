using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet : MonoBehaviour// start 함수 실행 기준의 플레이어를 조준하는 탄 클레스
{
    public float speed;
    public Transform enemy_location;
    public Vector3 player_location;
    public Vector3 dir;
    void Start()
    {
        //start 실행 기준의 플레이어 위치
        dir = player_location - enemy_location.position;
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
    }

  
    void Update()
    {
        //화면 밖일시 파괴
        if (transform.position.x < -8)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.x > 8)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.y > 7)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
        //dir 벡터로 탄이 이동
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
