using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour//방의 정보 상호작용 등
{
    public GameObject end_portal;
    public int enemy_count;
    public Transform consumable_drop;
   // public List<GameObject> consumable_created=new List<GameObject>();
    public GameObject Inven;
    public float room_width;
    public float room_height;
    public door[] door;
    public bool event_clear;
    public int room_element;
    public bool room_cleared;//방을 가봤는지
    public GameObject Itemsprite;
    [Header("normal_contents")]
    public GameObject[] enemy;
    public bool delay_check;
    public float delaytime=1.0f;
    public float delaytimer;
    int count;
    int Itemtype;
    public bool on_player;
    private bool open_check;
    bool item_given_check;
    public Item item = new Item();  //아이템
    public ItemDatabase ItemDatabase;
    public int door_num;
    public Vector2 XY;
    public List<room> r_connect;
    public List<room> r_connected;
    public int r_length;
    public int connect_num;
   // public GameObject end_door;
    public Transform[] spawn_door_pos;
    public GameObject exit_portal;
    public bool move_chk;
    public bool move_chk2;
    public float active_timer;
    public GameObject item_get_ui;
    public Vector2 size;
    public GameObject camera_point;
    public GameObject n_contents;
    public bool x_pin;
    public bool y_pin;
    public float tim = 1.2f;
    AudioManage_Main m_audio;
    float clear_slow_timer = 1.5f;
    float clear_slow_time;
    //생성자로 방의 종류 문의 갯수 를 정한다
    public room()
    {
        room_element = 1;//방의 종류 
        //1 시작방 2 일반 방 3:상점 4:이벤트 방
    }
    public room(int a)
    {
        room_element = a;
    }
    public room(int a,int b,int c)
    {
        room_element = a;
        r_length = b;
        connect_num = Random.Range(1, c+1);//문의 갯수
        if (connect_num == c + 1)
            connect_num = c;
       
    }
    //방이 클리어 되면 문의 갯수만큼 포탈을 활성화 일정한 위치로 시킨다
    public void make_clear_room()
    {
        if (r_connect.Count != 0)
        {
            for (int i = 0; i < r_connect.Count; i++)
            {

                // GameObject a = Instantiate(end_door);
                spawn_door_pos[i].gameObject.SetActive(true);
                spawn_door_pos[i].GetComponent<portallV2>().connenct_room = r_connect[i].gameObject;
                spawn_door_pos[i].GetComponent<portallV2>().this_room = this.gameObject;


            }
        }
        else
        {
            Instantiate(end_portal, spawn_door_pos[0].position, Quaternion.identity);
        }
    }
   
  //문의 갯수를 정한다
    public void set_connect_num(int a)
    {
        connect_num = a;
    }
   //방의 종류와 문의 갯수 ,층을 설정한다
    public void set_door(int a, int b, int c)
    {
        connect_num = a;
        room_element = b;
        r_length = c;
    }
    

    void Start()
    {
        tim = 1.2f;
        clear_slow_timer = 0.5f;
        clear_slow_time = Time.deltaTime;
        if (room_element == 1)//시작방이라면 초기부터 플레이어가 있다
        {
            on_player = true;
            move_chk = true;
        }

        delaytimer = delaytime;
        open_check = true;
      
    }
    private void Awake()
    {
        m_audio = AudioManage_Main.instance;

    }
    
  /*  public void active_room_element()//사용 안함
    {
        room_contents_controller r = this.gameObject.GetComponent<room_contents_controller>();
        r.content.SetActive(true);
    }*/
    void Update()
    {
        //make_wall();//사용 안함
   
        if (on_player)//플레이어가 방에 있을 때
        {
            if (room_element == 2)//일반 전투 방
            {
                Gamemanager.GM.last_enemy = enemy_count;//방에 남은 적의 수를 저장

                //화면이 완전히 페이드 아웃 될 때까지 대기
                if (move_chk &&! Gamemanager.GM.fade_init.activeSelf)
                {
                    move_chk2 = true;
                  
                }
                if (move_chk2&&active_timer>0)
                {
                    active_timer-=Time.deltaTime;
                }

                //페이드 아웃 이후 약간의 시간 이후 전투를 시작하고 적을 활성화
                if (active_timer<=0)
                {
                    active_timer = 0;
                    if (!room_cleared)//전투 시작
                    {

                        Gamemanager.GM.Onbattle = true;
                        battle_mode();
                    }
                    else//전투 종료
                    {
                        if (Gamemanager.GM.Onbattle)//전투 종료 후 포탈 생성
                        {
                            Gamemanager.GM.Onbattle = false;
                            make_clear_room();
                        }

                        if (!item_given_check)//플레이어에게 아이템을 보상으로
                        {
                            int rand = Random.Range(0, 100);
                            if (rand < 40)//40%확률로 체력을 회복시켜주는 소모 아이템을 생성한다
                            {
                                //생성 위치는 마지막으로 처치된 적의 위치
                                var a = Instantiate(Gamemanager.GM.drop_consumable_item, Gamemanager.GM.lastest_enemy_point, Quaternion.identity);
                                a.transform.parent = this.transform;
                            }
                            //랜덤 아이템을 방보상으로 얻는다
                            Gamemanager.GM.get_item();
                        }

                        item_given_check = true;
                        active_timer = 0;
                        move_chk2 = false;
                    }
                }
                
            }
            else if (room_element==4)//이벤트 방이라면
            {
                if (event_clear)//이벤트를 클리어할 때 방을 클리어
                {
                    make_clear_room();
                    event_clear = false;
                }
            }
            else//아닐시 입장 직후 바로 클리어
            {
                if (!room_cleared)
                {
                    make_clear_room();
                    room_cleared = true;
                }
                

            }
          
        }

        
    }

   void battle_mode()//전투 상태
    {
        
        
        normal_contents a = n_contents.GetComponent<normal_contents>();
     
        if (a.room_cleared)//방의 전투가 끝났을 때 시간이 살짝 느려지는 연출
        {

            if (clear_slow_timer > 0)
            {
                clear_slow_timer -= clear_slow_time;
                Time.timeScale = 0.5f;
            }
            else
            {
                if (Time.timeScale != 1)
                    Time.timeScale = 1;
                room_cleared = true;
            }
        }
        else
        {
            if (tim > 0)
            {
                tim -= Time.deltaTime;
            }
            else
            {
                if (a.choose_cycle != null && a.choose_cycle.choose_group != null)//사이클과 그룹이 선택 되어 있다면 그룹의 적이 enemy_count는 그룹의 적의 수
                {
                    enemy_count = a.choose_cycle.choose_group.GetComponent<Enemy_group>().enemy.Count;
                   
                }
                if (a.cycle_chk && a.start_chk&&tim<=0)//사이클이 선택 되있고 방에 입장한 후 시간이 좀 흘렸다면 적을 활성화
                {
                    a.acitve_enemy();
             
                }
            }
           
        }
      
        
    }
    public void move_player()//플레이어가 이 방으로 이동했을 때 
    { 
        GameObject p = Gamemanager.GM.Player_obj;
        p.transform.position = this.exit_portal.transform.position;// 플레이어의 위치를 조정
        Gamemanager.GM.fade_in();//페이드 인
        m_audio.portalExit();
        if (Gamemanager.GM.fade_Outit.activeSelf)//페이드 아웃 종료
            Gamemanager.GM.fade_Outit.SetActive(false);
        Gamemanager.GM.game_ev.when_room_enter();//플레이어가 방에 들어갔을 때의 이벤트 작동
        Gamemanager.GM.can_handle = true;//플레이어가 조작을 할수있게
        move_chk = true;

    }
    private void OnDrawGizmos()//방 안의 카메라의 이동범위를 시각화
    {
        Gizmos.color = Color.yellow;
        if (camera_point != null)
        {
            Gizmos.DrawWireCube(camera_point.transform.position, size);
        }
        else
        {
            Gizmos.DrawWireCube(this.gameObject.transform.position, size);
        }

    }
    /* 지금 사용하지 않음
    bool collision_door_to_player_check()
    {
        int count = 0;
        for (int i = 0; i < door.Length; i++)
        {
            if (door[i].link != null&&door[i].col_Player)
            {
                count++;
            }
        }
        if (count == 0)
        {
            return true;
        }
        else
            return false;
    }
    public void open_door()
    {
        for (int i = 0; i < door.Length; i++)
        {
            if (door[i].link != null)
            {
                door[i].open_door();
            }
            
        }
        open_check = true;
    }
    public void close_door()
    {
        for (int i = 0; i < door.Length; i++)
        {
            if (door[i].link != null)
            {
                door[i].close_door();
            }
        }
        open_check = false;
    }
    public void random_door()
    {
        while (count < 2)// 연결되지 않은 문이 하나라도 생길 때 까지
        {
            for (int i = 0; i < door.Length; i++)
            {
                if (door[i].link == null)
                    count += door[i].make_door_random();
            }
        }
    }
    public void make_wall()
    {
      
      // var room_controller = this.transform.parent.GetComponent<room_controller>();
        for(int i = 0; i < door.Length; i++)
        {
            if (door[i].link == null)
            {
                door[i].active_wall();
            }
            else
            {
                door[i].active_door();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy.Add(other.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //on_player = true;
            //CameraFollow.center = new Vector2(this.transform.position.x, this.transform.position.y);
        }
        if (collision.CompareTag("Enemy"))
        {
            //enemy.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //on_player = false;
            
        }
        if (collision.CompareTag("Enemy"))
        {
           // enemy.Remove(collision.gameObject);
        }
    }*/

}
