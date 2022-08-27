using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type2 : MonoBehaviour// start 함수 실행 기준의 플레이어를 조준하는 탄 클레스 생성 될 때 색깔이 다르게 나옴
{
    public float speed = 1;
    public Transform enemy_location;
    public Vector3 player_location;
    public Vector3 dir;
    public int num;
    public GameObject[] bullet;
    // Start is called before the first frame update
    void Start()
    {
        //생성될때 랜덤한 num의 배열에 존재하는 색깔의 탄으로 생성된다
        num = Random.Range(0, 3);
         bullet[num].gameObject.SetActive(true);
        //start 실행 기준의 플레이어 위치
        dir = player_location - enemy_location.position;
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
    }

    // Update is called once per frame
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
