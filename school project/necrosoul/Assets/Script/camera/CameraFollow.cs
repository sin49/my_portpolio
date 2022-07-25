using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour//ī�޶� �÷��̾ �߰�
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
       
        if (GameObject.FindGameObjectWithTag("Player"))//�÷��̾ Ž��
        {
         
            target = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3);
        }
        if (target != null)//�÷��̾ Ȯ���ߴٸ�
        {
            room_ = GameObject.FindGameObjectsWithTag("room");
            for (int i = 0; i < room_.Length; i++)//���� �÷��̾ �ִ� ���� Ȯ���Ѵ�
            {
                if (room_[i].GetComponent<room>().on_player)
                {
                    r = room_[i].GetComponent<room>();
                    //�濡�� ������  ī�޶� ���� ������ �� Ŭ������ �ҷ��´�
                    center = r.camera_point.transform.position;//���� �߽���
                    size = r.size;//�ִ� ī�޶� �̵� ����
                }
            }
            this_pos = this.transform.position;
            //ī�޶� �÷��̾��� ��ġ�� ������ ��������� �Ѵ�
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
            

            float clampX=center.x;
            if (r != null)
            {
               //ī�޶� ������ �̵� ���� ������ �̵����� �ʵ��� �Ѵ�
                if (!r.x_pin)//ī�޶��� x�̵� ����
                {
                    float lx = size.x * 0.5f - width;
                    clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);
                }
                float clampY = center.y;
                if (!r.y_pin)//ī�޶��� y�̵� ����
                {
                    float ly = size.y * 0.5f - height;
                    clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);
                }
                transform.position = new Vector3(clampX, clampY, -10f);
            }
            l = transform.position.x - this_pos.x;
            //ī�޶��� �������� �����̺�Ʈ�� ĳġ�Ѵ�
            Gamemanager.GM.game_ev.when_camera_move(l) ;
            
        }
        
    }
    //ī�޶��� �̵� ������ �ð�ȭ
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center,size);
    }
}
