using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portallV2 : MonoBehaviour//포탈(방과 방 사이 연결
{
    public GameObject connenct_room;
    public GameObject this_room;
    public bool move_check;
    public bool anim_check;
    GameObject element_frame;
    public Sprite[] room_element_image;
    // Start is called before the first frame update
    void Start()
    {
        element_frame = this.transform.GetChild(1).GetChild(0).gameObject;
        //연결된 방의 종류에 따라 안내판 그림이 바뀐다
        if (connenct_room.GetComponent<room>().room_element == 2)
        {
            set_frame_image(0);
        }
        else if (connenct_room.GetComponent<room>().room_element == 3)
        {
            set_frame_image(2);

        }
        else if (connenct_room.GetComponent<room>().room_element == 4)
        {
            set_frame_image(1);
        }

        //포탈의 위치를 미니맵에  표시한다
        GameObject a = Instantiate(Gamemanager.GM.portal_minimap_pos, this.transform);
        a.transform.position = this.transform.position;
    }
            public void animation_end_check()
    {
        anim_check = true;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (move_check && Gamemanager.GM.fade_out_complete)//플레이어가 이 포탈을 통해 다음 방으로 이동시
        {
            if (connenct_room == null)
            {
                Destroy(this.gameObject);
            }
            connenct_room.SetActive(true);//다음 방을 활성화
            connenct_room.GetComponent<room>().on_player = true;
            connenct_room.GetComponent<room>().exit_portal.SetActive(true);
           this_room.SetActive(false);//이 방을 비활성화
            this_room.GetComponent<room>().on_player = false;
            GameObject p = Gamemanager.GM.Player_obj;
            Debug.Log(connenct_room.GetComponent<room>().exit_portal.transform.position);
            p.transform.position = connenct_room.GetComponent<room>().exit_portal.transform.position;//플레이어를 다음방으로 이동
           
           

            move_check = false;
            connenct_room.GetComponent<room>().move_player();
        }
    }
    void set_frame_image(int i)//방 안내판 그림
    {
        element_frame.GetComponent<SpriteRenderer>().sprite = room_element_image[i];
    }
    public void move_player()//포탈을 상효작용 하면 페이드 아웃+조작을 잠시 무효화
    {

        Gamemanager.GM.fade_out();
        Gamemanager.GM.can_handle = false;
        move_check = true;
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
       /* if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                connenct_room.SetActive(true);
                connenct_room.GetComponent<room>().on_player = true;
                this_room.SetActive(false);
                this_room.GetComponent<room>().on_player = false;
                GameObject p = Gamemanager.GM.Player_obj;
                Debug.Log(connenct_room.GetComponent<room>().exit_portal.transform.position);
                p.transform.position = connenct_room.GetComponent<room>().exit_portal.transform.position;
         

            }
        }*/
    }
}
