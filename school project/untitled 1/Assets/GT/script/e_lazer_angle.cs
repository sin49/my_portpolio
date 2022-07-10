using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_lazer_angle : MonoBehaviour
{
    public Transform enemy_location;
    public Transform player_location;
    public Vector3 dir;
    public float angle_;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 dir = player_location.position - transform.position;
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;

        // 타겟 방향으로 회전함
        float angle = Mathf.Atan2(dir.y*-1, dir.x*-1) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
