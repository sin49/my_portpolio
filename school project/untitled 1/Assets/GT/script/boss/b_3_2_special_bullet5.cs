using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_3_2_special_bullet5 : MonoBehaviour//3스테이지 보스 2페이즈 특수 탄 플레이어를 향해 날아가며 점점 빨라진다.
{
    public float speed=0.001f;
    public float speed_plus;
    public Transform enemy_location;
    public Vector3 player_location;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = player_location - enemy_location.position;
        speed_plus = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }
        //speed_plus를 통해 speed의 값을 늘려 시간이 흐를수록 점점 빨라진다
        speed += speed_plus;
        speed_plus += 0.001f;
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
