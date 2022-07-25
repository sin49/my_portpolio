using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_camera : MonoBehaviour//보스전 용 카메라
{
    //메인 카메라(camerafollow.cs)와 스크립트가 같으나 보스전 용 스크립트를(boss_stage)를 사용한다
    public Transform target;
    public float speed;
    boss_stage r;
    public Vector2 center;
    public Vector2 size;
    public Vector2 this_pos;
    float height;
    float width;

    GameObject[] room_;
    private void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    private void LateUpdate()
    {
        
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3);
        }
        if (target != null)
        {
            room_ = GameObject.FindGameObjectsWithTag("room");
            for (int i = 0; i < room_.Length; i++)
            {
               
                    r = room_[i].GetComponent<boss_stage>();
                    center = r.camera_point.transform.position;
                    size = r.size;
               
            }
            this_pos = this.transform.position;
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

            //transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
            float clampX = center.x;
            if (r != null)
            {
                
                    float lx = size.x * 0.5f - width;
                    clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

                
                float clampY = center.y;
               
                    float ly = size.y * 0.5f - height;
                    clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);
                
                transform.position = new Vector3(clampX, clampY, -10f);
            }
            Gamemanager.GM.game_ev.when_camera_move(transform.position.x - this_pos.x);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
