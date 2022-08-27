using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type3 : MonoBehaviour//보스2 특수 탄환 플레이어의 탄을 막으며 이동경로에 잠시 후 제자리에서 폭팔힌는 폭팔 탄을 생성한다
{
    public float RotateSpeed = 4f;
    public float Radius = 0.1f;
    public float RadiusAdd = 0.08f;
    public float Zpos = 10;
    private Vector3 _centre;
    private float _angle;
    public float spawn_bullet_time;
    public GameObject explosion_bullet;

    private void Start()
    {
        //생성기준의 탄 위치를 중심점
        _centre = transform.position;
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
    }

    private void Update()
    {
        spawn_bullet_time += Time.deltaTime;
        //화면 왼쪽 끝에서 탄이 사라진다.
        if (transform.position.x < -14)
        {
            Destroy(this.gameObject);
        }

        //탄이 생성된 위치를 기준으로 소용돌이 모양으로 회전하며 움직인다.

        //RotateSpeed만큼 각도를 더해 회전 시킨다
        _angle += RotateSpeed * Time.deltaTime;
        // _angle각도에서  Radius의 반지름일때의 벡터 값을 구한다
        var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        //중심점에  offset만큼의 값을 더한 포인트로 탄의 위치를 움직인다.
        transform.position = _centre + offset;
        //반지름 값을 점점 넒힌다
        Radius+=RadiusAdd;

        //일정 시간마다 현재 탄의 위치에 폭팔하는 특수 탄을 생성한다.
        if (spawn_bullet_time >= 0.1)
        {
            GameObject bullet = Instantiate(explosion_bullet, transform.position, transform.rotation);
            spawn_bullet_time = 0;
        }
    }
}
