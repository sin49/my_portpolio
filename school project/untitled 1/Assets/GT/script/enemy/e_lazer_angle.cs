using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_lazer_angle : MonoBehaviour//레이저를 쏘기 전 각도를 결정+플레이어에게 예고하는 레이저 궤도의 각도 조정 클레스
{
    public Transform enemy_location;
    public Transform player_location;
    public Vector3 dir;
    public float angle_;
    // Start is called before the first frame update
    void Start()
    {
        //플레이어를 조준
        Vector3 dir = player_location.position - transform.position;
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;

        // 각도를 타겟 방향으로 향하도록 회전함
        float angle = Mathf.Atan2(dir.y*-1, dir.x*-1) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
