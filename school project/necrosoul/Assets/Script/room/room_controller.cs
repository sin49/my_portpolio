using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/*방생성 방식
 * V1 방식->방 하나 만들고 랜덤 돌려서 정하고 그러다 보면 충돌체크로 알아서 연결
 * 문제점: 방을 만들면 랜덤이라서 서로 연결이 안되는 경우가 생김,일자로 이어지는 느낌이 너무 강함
 *
 * V2 방식-> 방 만들고 그 방의 모든 문에 방을 만들고 그 방이 또 새로운 방을 만들고....
 *방 연결은 충돌체크가 아니라 생성할 때 연결
 * 문은 연결되어있다면 랜덤으로 가지 않고 무조건 열린 문
 * V3 방식->리스트에 방의 정보를 생성한 후 저장한다
 * 이 리스트의 방들은 층으로 구성되어 있다
 * 생성이 완료 되었다면 리스트를 읽고 그 정보를 토대로 층을 단위로 방을 생성
 * 그 층의 방을 지정해 다음 층의 방을 임의로 지정시켜 연결->한 층의 방을 이런식으로 다연결시켰다면 다음층으로 가서 반복....
 * 모든 방이 연결  되었다면 방생성을 마무리(트리구조와 유사?)
 * 
 * ...V1,V2는 일반적인 로그 스타일의 맵과 비슷한 방식을 지향 방과 방사이의 양방향,V3는 방 사이의 양방향 연결이 아닌 단방향 연결
 * 
 * 현재는 V3를 쓰고 있으며 V1 방식은 삭제 V2방식은 주석으로 잔재가 남아있음
 */


public class room_controller : MonoBehaviour//스테이지에 쓸 방을 생성하는 스크립트
{
    public int room_create_number;//방생성갯수

    int a;
    public GameObject[] start_room;
    public GameObject[] room_database;//방
    public GameObject[] event_room;
    public GameObject[] shop_room;
    public int shop_num;
    public int event_num;
    int event_count;
    public int floor_event_num;
    public int shop_room_percent;
    public int event_room_percent;
    public List<GameObject> room = new List<GameObject>();
    public List<Vector2> room_pos = new List<Vector2>();
    public List<room> room_database_V3 = new List<room>();
    public List<List<room>> room_database_V4 = new List<List<room>>();
    private int r_count;
    public bool make_wall_check;
    public bool map_making_complete;
    public bool make_map_elemental;
    public int x = 9;
    public int x_weight;
    int x_Min = 2;
    int x_Max = 7;
    public int y = 9;
    public int y_weight;
    int y_Min = 2;
    int y_Max = 7;
    public int[,] mini_map;
    public int event_room_num;
    int event_room_count;

    public float event_room_P;
    public int shop_room_num;
    int shop_room_int;
    int shop_room_count;
    minimap m_map;
    public List<room> r_List_n = new List<room>();//연결시킬 방(연결 안됨)
    public List<room> r_List_n2 = new List<room>();//연결시킬 방(연결됨);
    public Item item = new Item();  //아이템
    public ItemDatabase ItemDatabase;
    public List<int> r_length_count = new List<int>();
    //List<Dictionary<string, object>> m_Data = CSVReader.Read("map_making");
    // Start is called before the first frame update
    public void make_room_V3()//방생성 V3
    {

        int make_room_num = room_create_number + 1;//시작 층+만들려는 층
 

        for (int i = 0; i < make_room_num; i++)
        {
            Debug.Log(i);
            if (i == 0)//시작 층(전투 없음 + 튜토리얼)
            {
                r_length_count.Add(1);
                room r = new room();
                int r_door_num = 1;//방을 하나만 만든다
                r.set_connect_num(r_door_num);
                room_database_V3.Add(r);
                List<room> r_list = new List<room>();
                r_list.Add(r);
                room_database_V4.Add(r_list);
                r_length_count.Add(r_door_num);
                //room_database_V4: 층 구분이 들어가 이중리스트
                //room_database_V3:층 구분 없이 구성된 리스트

                //r_length_count:각 층의 방의 갯수를 저장한 리스트
                //room_database_V4 만으로도 구성은 가능하나 가시성+디버그 용도로 생성
            }
            else if (i == 1)//두번째 층:전투 있음+방 하나만
            {
                List<room> r_list = new List<room>();
                for (int o = 0; o < r_length_count[1]; o++)//하나만 생성
                {
                    room r = new room(2, i, r_length_count[1]);

                    room_database_V3.Add(r);
                    r_list.Add(r);
                }
                room_database_V4.Add(r_list);

            }
            else if (i == make_room_num - 1)//마지막 층은 방 하나만+상점으로 고정
            {
                List<room> r_list = new List<room>();
                room r = new room(3, i, 0);

                r_list.Add(r);
                room_database_V4.Add(r_list);
                r_length_count.Add(1);
            }
            else if (i == (make_room_num - 1) * 0.5f)//각 스테이지의 층의 절반은 상점으로 고정
            {
                int r_door_num;

                r_door_num = Random.Range(3, 6);
                List<room> r_list = new List<room>();
                room r = new room(3, i, r_door_num);
                r_list.Add(r);
                room_database_V4.Add(r_list);
                r_length_count.Add(1);
            }

            else//그 이외의 층은 3~5 의 방으로 구성
            {
                int r_door_num;

                r_door_num = Random.Range(3, 6);//3~5
                int e_num = 0;
                List<room> r_list = new List<room>();

                for (int o = 0; o < r_door_num; o++)
                {
                    //이벤트 방 생성
                    int rand = 100;
                    if (event_count < event_num && e_num < floor_event_num)//이벤트 방이 스테이지의 최대 생성 갯수에 도달하지 않았을 때
                    {
                        rand = Random.Range(0, 100);//확률 생성
                    }
                    room r;

                    if (rand < event_room_percent && event_count < event_num && e_num < floor_event_num)//이벤트 생성 확률의 조건 달성시
                    {
                        r = new room(4, i, r_door_num);//이벤트 방을 생성한다
                        event_count++;//event_count가 event_num 과 같은 숫자일 때 이벤트 방 생성을 멈춘다
                        e_num++;
                    }
                    else//일반 방
                    {
                        
                        r = new room(2, i, r_door_num);
                    }
                    room_database_V3.Add(r);
                    r_list.Add(r);

                   

                }
                room_database_V4.Add(r_list);

                r_length_count.Add(r_door_num);

        ;

            }

        }
        //room_database_V4를 기반으로 방 오브젝트를 실제로 생성한다
        make_room_V3_2(room_database_V4);
        connect_room_V3(room_database_V4);
        set_room_pos(room_database_V4);
        map_making_complete = true;
    }
    //scene에 들어올 때 room의 정보를 불려오는 방식을 할수도 있었지만
    //scene->scene으로 이동할 때 로딩씬을 계속 거쳐줘야 하는 문제로 한 scene에 모든 방을 생성하는 방법으로
    //생성->연결->재배치
    public void connect_room_V3(List<List<room>> r)//방과 바을 연결
    {
        for (int i = 0; i < r.Count - 1; i++)//층단위
        {
            r_List_n = r[i + 1];//연결시킬 방(방이 어디에도 연결 안됨)
            r_List_n2 = new List<room>();//r_List_n의 리스트를 순환시키기 위한 용도
                                         //하나도 연결이 되어있지않은 방이 생기는 경우는 일어나지 않도록 하고(r_List_n)
                                         //r_List_n2를 이용해 정해진 횟수의 연결을 실행시켜 연결안된 포탈이 없도록
            for (int n = 0; n < r[i].Count; n++)//방단위
            {


                for (int a = 0; a < r[i][n].connect_num; a++)
                {
                    if (r_List_n.Count != 0)
                    {

                        r[i][n].r_connect.Add(r_List_n[0]);//현재 층의 방과 다음층의 방을 연결 시킨다
                        r_List_n[0].r_connected.Add(r[i][n]);
                        r_List_n2.Add(r_List_n[0]);//r_List_n의 0번 구성요소를 r_List_n2에 넣고 리스트에서 제거한다
                        r_List_n.RemoveAt(0);
                    }

                    
                }
         
                for (int ab = r_List_n2.Count; ab > 0; ab--)//r_List_n2의 구성요소를 다시 r_List_n에 임의로 넣는다
                {
                    int r_num = Random.Range(0, ab - 1);
                    r_List_n.Add(r_List_n2[r_num]);
                    r_List_n2.RemoveAt(r_num);
                }
                //r_list와 r_list_2가 서로 순환되면서 r_list의 구성요소가 임의로 구성되고 그걸 기반으로 방 연결을 실행
                //(처음 연결은 리스트에 추가된 순으로 시작하여 방연결이 안되는 방이 생기는 경우를 방지

            }
            


        }


    }
    public void set_room_pos(List<List<room>> r)//방의 위치(position)을 정한다
    {
        int ac = 0;
        //방이 겹치는 경우 방지+테슽트 할 때 층의 구성으로 육안으로 확인하기 위해 방의 위치를 층에 맞게 이동한다
        for (int i = 0; i < r.Count; i++)
        {
            for (int n = 0; n < r[i].Count; n++)
            {
                //방의 길이와 높이를 확인한다
                room a = r[i][n];
                float r_x = a.room_width * a.r_length;
                float r_y = a.room_height / (r_length_count[a.r_length] + 1);
                if (ac == 0)
                {
                    ac = r_length_count[r[i][n].r_length] - 1;
                    //num++;
                }
                else
                {
                    ac--;
                }
                a.transform.SetParent(this.transform);
                //방을 길이와 높이에 맞게 층 단위로 이동시킨다
                a.transform.position = new Vector3(r_x, r_y * (ac + 1), this.transform.position.z);
                if (i != 0)
                {
                    a.gameObject.SetActive(false);
                }


            }
        }

    }
    public void make_room_V3_2(List<List<room>> r)//room_database 의 정보에 맞게 방을 생성한다
    {
  
        int r_num = r.Count;
        for (int i = 0; i < r_num; i++)//층
        {
            int r_r_num = r[i].Count;
            for (int n = 0; n < r_r_num; n++)//방
            {
                int random_number;
                GameObject a = null;
                if (r[i][n].room_element == 1)//시작 방
                {
                    if (Gamemanager.GM.stage == 1)
                    {
                        a = Instantiate(start_room[0]);//스테이지 1(튜토리얼)
                    }
                    else
                    {
                        a = Instantiate(start_room[1]);//스테이지 2부터(튜토리얼 없음)
                    }
                }
                else if (r[i][n].room_element == 2)//일반방
                {
                    random_number = Random.Range(0, room_database.Length);//랜덤한 방을 불려온다
                    a = Instantiate(room_database[random_number]);
                }
               
                else if (r[i][n].room_element == 3)//상점
                {
                    a = Instantiate(shop_room[0]);
                }
                else if (r[i][n].room_element == 4)//이벤트
                {
                    a = Instantiate(event_room[0]);
                }


                //방의 정보를 리스트의 정보와 일치시킨다
                a.GetComponent<room>().set_door(r[i][n].connect_num, r[i][n].room_element, r[i][n].r_length);


                
                //객체 없이 정보만 들어있는 리스트 구성요소를 쩨거하고 객체로 생성된 방의 정보를 다시 리스트에 추가한다
                room_database_V3.RemoveAt(0);
                r[i].Add(a.GetComponent<room>());
                room_database_V3.Add(a.GetComponent<room>());


                
                a.transform.SetParent(this.transform);
              
            }
            //생성하는데 사용된 리스트 (room_database_V4)의 값을 제거하여 초기화 시킨다
            for (int b = r_r_num; b > 0; b--)
            {
                if (r[i].Count != 0)
                    r[i].RemoveAt(0);
            }



        }
    }
    private void Awake()
    {


    }
    void Start()
    {
        



    }

    // Update is called once per frame
    void Update()
    {
        if (!map_making_complete && !Gamemanager.GM.boss)//보스 스테이지가 아니고 방이 생성되지 않았다면 방 생성
        {
            make_room_V3();
        }

    }
}








////////////////////////////////////////////////////////////쓰이지 않음///////////////
    /*public void make_map_element(room a)
    {

          
            if (room.Count == room_create_number)
            {
            a.room_element = 4;
            }
            else
            {
            if (shop_room_num != 0)
            {
                if (room.Count % shop_room_int == 0 && shop_room_count < shop_room_num)
                {
                    a.room_element = 6;
                    shop_room_count++;
                }
                else
                {
                    if (event_room_count < event_room_num)
                    {
                        float ran = Random.Range(0, 1);
                        if (ran <= event_room_P)
                        {
                            a.room_element = 3;
                            event_room_count++;
                        }
                        else
                        {
                            a.room_element = 2;
                        }
                    }
                    else
                    {
                        a.room_element = 2;
                    }
                }
            }
            else
            {
                a.room_element = 2;
            }
            }
        
    }
  
    public GameObject make_room_V2()//아무 방이나 만든다
    {
        mini_map = new int[x, y];
        int random_number = Random.Range(0, room_database.Length);
        GameObject a = Instantiate(room_database[random_number]);

        room.Add(a);
        a.GetComponent<room>().room_element = 1;
        a.GetComponent<room>().on_player = true;
        a.transform.SetParent(this.transform);
        mini_map[x /2, y /2] = 2;
   
      
        return a;
    }

    door find_door(door[] a, int b)
    {

        List<door> room_ = new List<door>();
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i].direct == b)
            {
                room_.Add(a[i]);
            }
        }
        int random = Random.Range(0, room_.Count);
        return room_[random];
    }
    
    public GameObject make_room_V2(GameObject r)//아무 방을 생성해 이어붙인다
    {
        room room_ = r.GetComponent<room>();

        bool destroy=false;
        List<door> open_door = new List<door>();

        for (int i = 0; i < room_.door.Length; i++)
        {
            if (room_.door[i].door_active && room_.door[i].link == null) {
                open_door.Add(room_.door[i]);
                
            }
        }
        
        //방의 문을 체크해서 문이면서 연결되지 않은 문을 리스트에 담는다
        for (int i = 0; i < open_door.Count; i++)
        {
            if (room.Count < room_create_number)
            {
                Vector3 pos1 = r.transform.position;
                int random_number = Random.Range(0, room_database.Length);
                GameObject a = Instantiate(room_database[random_number]);
                a.transform.SetParent(this.transform);
                room a_room = a.GetComponent<room>();
                
                make_map_element(a_room);
                door b;
                switch (open_door[i].direct)
                {
                    case 0:

             
                        b = find_door(a.GetComponent<room>().door, 1);
                        a.gameObject.transform.position = new Vector3(pos1.x - r.GetComponent<room>().room_width, pos1.y, pos1.z);
                        for (int p = 0; p < room_pos.Count; p++)
                        {
                            if (a_room.get_XY() == room[p].GetComponent<room>().get_XY())
                            {
                                Debug.Log("방이 겹쳐서 파괴됨" + (r_XY[0] -3) + r_XY[1]);
                                if (a_room.room_element == 3)
                                {
                                    event_room_count--;
                                }
                                Destroy(a);
                                destroy = true;
                                break;
                            }
                        }
                        if (!destroy)
                        {
                            room_pos.Add(new Vector2(r_XY[0] - 3, r_XY[1]));//생성 시키는 방의 좌표 가져오기
                            room.Add(a);
                            if (x_Min > r_XY[0]-3)
                            {
                                x_Min -= 3;
                                x_weight += 3;
                                int new_x = x + 3;
                                mini_map = new int[new_x, y];

                                for (int x_ = 0; x_ < x; x_++)
                                {
                                    for (int y_ = 0; y_ < y; y_++)
                                    {
                                        mini_map[x_, y_] = mini_map[x_ + 3, y_];

                                    }
                                }
                                mini_map[(int)r_XY[0] - 1 + x_weight, (int)r_XY[1] + y_weight] = 1;
                                mini_map[(int)r_XY[0] - 2 + x_weight, (int)r_XY[1] + y_weight] = 1;
                                mini_map[(int)r_XY[0] - 3 + x_weight, (int)r_XY[1] + y_weight]=2;
                                x = new_x;
                            }
                            else
                            {
                                mini_map[(int)r_XY[0] - 1 + x_weight, (int)r_XY[1] + y_weight] = 1;
                                mini_map[(int)r_XY[0] - 2 + x_weight, (int)r_XY[1] + y_weight] = 1;
                                mini_map[(int)r_XY[0] - 3 + x_weight, (int)r_XY[1] + y_weight] = 2;
                            }
                        }
                        b.link = open_door[i].gameObject;
                        open_door[i].link = b.gameObject;
                        break;

                    case 1:
                        a.GetComponent<room>().set_XY(r_XY[0] + 3, r_XY[1]);
                        b = find_door(a.GetComponent<room>().door, 0);
                        a.gameObject.transform.position = new Vector3(pos1.x + r.GetComponent<room>().room_width, pos1.y, pos1.z);
                        for (int p = 0; p < room_pos.Count; p++)
                        {
                            if (a.GetComponent<room>().get_XY() == room[p].GetComponent<room>().get_XY())
                            {
                                
                                if (a_room.room_element == 3)
                                {
                                    event_room_count--;
                                }
                                Destroy(a);

                                destroy = true;
                                break;
                            }
                        }
                        if (!destroy)
                        {
                            room_pos.Add(new Vector2(r_XY[0] + 3, r_XY[1]));
                            room.Add(a);
                            if (x_Max < r_XY[0]+3)
                            {
                                x_Max += 3;
                                int new_x = x + 3;
                                mini_map = new int[new_x, y];

                                
                                mini_map[(int)r_XY[0] + 1 + x_weight, (int)r_XY[1] + y_weight] = 1;
                                mini_map[(int)r_XY[0] + 2 + x_weight, (int)r_XY[1] + y_weight] = 1;
                                mini_map[(int)r_XY[0] + 3 + x_weight, (int)r_XY[1] + y_weight] = 2;
                                x = new_x;
                            }
                            else
                            {
                                mini_map[(int)r_XY[0] + 1 + x_weight, (int)r_XY[1] + y_weight] = 1;
                                mini_map[(int)r_XY[0] + 2 + x_weight, (int)r_XY[1] + y_weight] = 1;
                                mini_map[(int)r_XY[0] + 3 + x_weight, (int)r_XY[1] + y_weight] = 2;
                            }
                        }

                        b.link = open_door[i].gameObject;
                        open_door[i].link = b.gameObject;
                        break;

                    case 2:
                        a.GetComponent<room>().set_XY(r_XY[0], r_XY[1] + 3);
                        b = find_door(a.GetComponent<room>().door, 3);
                        a.gameObject.transform.position = new Vector3(pos1.x, pos1.y + r.GetComponent<room>().room_height, pos1.z);
                        for (int p = 0; p < room_pos.Count; p++)
                        {
                            if (a.GetComponent<room>().get_XY() == room[p].GetComponent<room>().get_XY())
                            {
                                Debug.Log("방이 겹쳐서 파괴됨" + r_XY[0] + (r_XY[1] + 3));
                                if (a_room.room_element == 3)
                                {
                                    event_room_count--;
                                }
                                Destroy(a);
                                destroy = true;
                                break;
                            }
                        }
                        if (!destroy)
                        {
                            room_pos.Add(new Vector2(r_XY[0], r_XY[1] + 3));
                            room.Add(a);
                            if (y_Max < r_XY[1]+3)
                            {
                                y_Max += 3;
                                int new_y = y + 3;
                                mini_map = new int[x, new_y];


                                mini_map[(int)r_XY[0]  + x_weight, (int)r_XY[1]+1 + y_weight] = 1;
                                mini_map[(int)r_XY[0] + x_weight, (int)r_XY[1]+2 + y_weight] = 1;
                                mini_map[(int)r_XY[0]  + x_weight, (int)r_XY[1]+3 + y_weight] = 2;
                                y = new_y;
                            }
                            else
                            {
                                mini_map[(int)r_XY[0] + x_weight, (int)r_XY[1]+1 + y_weight] = 1;
                                mini_map[(int)r_XY[0]  + x_weight, (int)r_XY[1]+2 + y_weight] = 1;
                                mini_map[(int)r_XY[0]  + x_weight, (int)r_XY[1]+3 + y_weight] = 2;
                            }
                        }
                        b.link = open_door[i].gameObject;
                        open_door[i].link = b.gameObject;
                        break;

                    case 3:
                        a.GetComponent<room>().set_XY(r_XY[0], r_XY[1] - 3);
                        b = find_door(a.GetComponent<room>().door, 2);
                        a.gameObject.transform.position = new Vector3(pos1.x, pos1.y - r.GetComponent<room>().room_height, pos1.z);
                        for (int p = 0; p < room_pos.Count; p++)
                        {
                            if (a.GetComponent<room>().get_XY() == room[p].GetComponent<room>().get_XY())
                            {
                               
                                Debug.Log("방이 겹쳐서 파괴됨"+ r_XY[0]+ (r_XY[1] - 3));
                                if (a_room.room_element == 3)
                                {
                                    event_room_count--;
                                }
                                Destroy(a);
                                destroy = true;
                                break;
                            }
                        }
                        if (!destroy)
                        {
                            room_pos.Add(new Vector2(r_XY[0], r_XY[1] - 3));
                            room.Add(a);
                            if (y_Min > r_XY[1] - 3)
                            {
                                y_Min-= 3;
                                y_weight += 3;
                                int new_y = y + 3;
                                mini_map = new int[x, new_y];
                                for (int x_ = 0; x_ < x; x_++)
                                {
                                    for (int y_ = 0; y_ < y; y_++)
                                    {
                                        mini_map[x_, y_] = mini_map[x_, y_+3];

                                    }
                                }

                                mini_map[(int)r_XY[0] + x_weight, (int)r_XY[1] - 1 + y_weight] = 1;
                                mini_map[(int)r_XY[0] + x_weight, (int)r_XY[1] - 2 + y_weight] = 1;
                                mini_map[(int)r_XY[0] + x_weight, (int)r_XY[1] - 3 + y_weight] = 2;
                                y = new_y;
                            }
                            else
                            {
                                mini_map[(int)r_XY[0]  + x_weight, (int)r_XY[1] - 1 + y_weight] = 1;
                                mini_map[(int)r_XY[0] + x_weight, (int)r_XY[1] - 2 + y_weight] = 1;
                                mini_map[(int)r_XY[0]  + x_weight, (int)r_XY[1] - 3 + y_weight] = 2;
                            }
                        }
                        b.link = open_door[i].gameObject;
                        open_door[i].link = b.gameObject;
                        break;
                    default:
                        Debug.Log("fatal error!");
                        break;
                }
            }
        }
        //리스트의 길이 만큼 방을 만들고 문을 연결시킨다

        //생성된 방은 또 문을 체크할수 있게 제귀로 빠진다
        //start가 awake보다 느리다는 점을 이용해서 생성이 다된 후에 다시 방을 만드는 방식으로 재귀한다 가능?
        //start awake보다 재귀 방식을 list에 담긴 방을 순차적으로 불려오는 방식으로 바꾸면 가능하다! 이건 가능
        return null;

    }
    public GameObject create_room_V2()//방 하나 생성 후 이어붙이는 방식
    {
        //room[room_count] = make_room_V2();//방 만들고
        GameObject b = make_room_V2();//방 만들고
        a++;
        if (room.Count< room_create_number)
        {
            return create_room_V2(room);//아니면 아어붙이기
            
        }
        else
        {
            return null;//반환값이 필요하면 채우기

        }


    }
    public GameObject create_room_V2(List<GameObject> room)//지금은 사용 금지
    {
        if (room.Count < room_create_number)//통로가 뚫려있으면
            {
            if (r_count >= room.Count)
            {
                
                r_count = room.Count-1;
                room[r_count].GetComponent<room>().random_door();
            }
            GameObject r2 = make_room_V2(room[r_count]);//방 생성후 이어붙이기
            r_count++;

            //room[room_count] = make_room_V2(r);
            
                if (room.Count> room_create_number)//다차면 스톱
                    return null;
                else
                    create_room_V2(room);//그리고 그방의 통로를 검사함
            }
        return null;
       
    }
    void close_no_link_door(List<GameObject> d)
    {
        
        for(int i = 0; i < d.Count; i++)
        {
            d[i].GetComponent<room>().make_wall();
        }
    }
    void reset_all()
    {
        while (room.Count != 0)
        {
            room.RemoveAt(room.Count - 1);
        }
        while (room_pos.Count != 0)
        {
            room_pos.RemoveAt(room_pos.Count - 1);
        }
    }

 

}
*/