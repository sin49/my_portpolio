using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour//카메라가 플레이어를 추격
{
    public Transform target;
    public float speed;
   public room r;
    public Vector2 center;
    public Vector2 size;
    public Vector2 this_pos;
    float height;
    float width;
    GameObject[] room_;
    float l;
    private void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;

    }

    private void LateUpdate()
    {
       
        if (GameObject.FindGameObjectWithTag("Player"))//플레이어를 탐색
        {
         
            target = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3);
        }
        if (target != null)//플레이어를 확인했다면
        {
            room_ = GameObject.FindGameObjectsWithTag("room");
            for (int i = 0; i < room_.Length; i++)//현재 플레이어가 있는 방을 확인한다
            {
                if (room_[i].GetComponent<room>().on_player)
                {
                    r = room_[i].GetComponent<room>();
                    //방에서 지정된  카메라 관련 정보를 이 클레스로 불려온다
                    center = r.camera_point.transform.position;//방의 중심점
                    size = r.size;//최대 카메라 이동 범위
                }
            }
            this_pos = this.transform.position;
            //카메라가 플레이어의 위치로 서서히 따라오도록 한다
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
            

            float clampX=center.x;
            if (r != null)
            {
               //카메라가 일정한 이동 범위 밖으로 이동하지 않도록 한다
                if (!r.x_pin)//카메라의 x이동 고정
                {
                    float lx = size.x * 0.5f - width;
                    clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);
                }
                float clampY = center.y;
                if (!r.y_pin)//카메라의 y이동 고정
                {
                    float ly = size.y * 0.5f - height;
                    clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);
                }
                transform.position = new Vector3(clampX, clampY, -10f);
            }
            l = transform.position.x - this_pos.x;
            //카메라의 움직임을 게임이벤트로 캐치한다
            Gamemanager.GM.game_ev.when_camera_move(l) ;
            
        }
        
    }
    //카메라의 이돋 범위를 시각화
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center,size);
    }
}
