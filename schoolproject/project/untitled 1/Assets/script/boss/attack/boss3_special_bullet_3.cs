using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_special_bullet_3 : MonoBehaviour//스테이지3보스 버티기 패턴용 탄 일정 시간 후 Start()시점의 플레이어의 위치를 향해 날아간다
{
    public float speed = 1;
    public Transform enemy_location;
    public Vector3 player_location;
    public Vector3 dir;
    public float time;
    public float time_max;
    Vector3 pos;
    // Start is called before the first frame update
    
    void Start()
    {
        //dir로 날아갈 방향을 정한다
        dir = player_location - enemy_location.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        //일정 시간이 되기전까지 탄은 제자리에서 정지한다
        time += Time.deltaTime;

        //화면밖으로 나갈시 사라진다
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
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;

        //time_max 초 이후 정해진 dir로 이동한다.
        if (time >= time_max)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }
}
