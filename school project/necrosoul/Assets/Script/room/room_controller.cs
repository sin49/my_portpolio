using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/*����� ���
 * V1 ���->�� �ϳ� ����� ���� ������ ���ϰ� �׷��� ���� �浹üũ�� �˾Ƽ� ����
 * ������: ���� ����� �����̶� ���� ������ �ȵǴ� ��찡 ����,���ڷ� �̾����� ������ �ʹ� ����
 *
 * V2 ���-> �� ����� �� ���� ��� ���� ���� ����� �� ���� �� ���ο� ���� �����....
 *�� ������ �浹üũ�� �ƴ϶� ������ �� ����
 * ���� ����Ǿ��ִٸ� �������� ���� �ʰ� ������ ���� ��
 * V3 ���->����Ʈ�� ���� ������ ������ �� �����Ѵ�
 * �� ����Ʈ�� ����� ������ �����Ǿ� �ִ�
 * ������ �Ϸ� �Ǿ��ٸ� ����Ʈ�� �а� �� ������ ���� ���� ������ ���� ����
 * �� ���� ���� ������ ���� ���� ���� ���Ƿ� �������� ����->�� ���� ���� �̷������� �ٿ�����״ٸ� ���������� ���� �ݺ�....
 * ��� ���� ����  �Ǿ��ٸ� ������� ������(Ʈ�������� ����?)
 * 
 * ...V1,V2�� �Ϲ����� �α� ��Ÿ���� �ʰ� ����� ����� ���� ��� ������� �����,V3�� �� ������ ����� ������ �ƴ� �ܹ��� ����
 * 
 * ����� V3�� ���� ������ V1 ����� ���� V2����� �ּ����� ���簡 ��������
 */


public class room_controller : MonoBehaviour//���������� �� ���� �����ϴ� ��ũ��Ʈ
{
    public int room_create_number;//���������

    int a;
    public GameObject[] start_room;
    public GameObject[] room_database;//��
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
    public List<room> r_List_n = new List<room>();//�����ų ��(���� �ȵ�)
    public List<room> r_List_n2 = new List<room>();//�����ų ��(�����);
    public Item item = new Item();  //������
    public ItemDatabase ItemDatabase;
    public List<int> r_length_count = new List<int>();
    //List<Dictionary<string, object>> m_Data = CSVReader.Read("map_making");
    // Start is called before the first frame update
    public void make_room_V3()//����� V3
    {

        int make_room_num = room_create_number + 1;//���� ��+������� ��
 

        for (int i = 0; i < make_room_num; i++)
        {
            Debug.Log(i);
            if (i == 0)//���� ��(���� ���� + Ʃ�丮��)
            {
                r_length_count.Add(1);
                room r = new room();
                int r_door_num = 1;//���� �ϳ��� �����
                r.set_connect_num(r_door_num);
                room_database_V3.Add(r);
                List<room> r_list = new List<room>();
                r_list.Add(r);
                room_database_V4.Add(r_list);
                r_length_count.Add(r_door_num);
                //room_database_V4: �� ������ �� ���߸���Ʈ
                //room_database_V3:�� ���� ���� ������ ����Ʈ

                //r_length_count:�� ���� ���� ������ ������ ����Ʈ
                //room_database_V4 �����ε� ������ �����ϳ� ���ü�+����� �뵵�� ����
            }
            else if (i == 1)//�ι�° ��:���� ����+�� �ϳ���
            {
                List<room> r_list = new List<room>();
                for (int o = 0; o < r_length_count[1]; o++)//�ϳ��� ����
                {
                    room r = new room(2, i, r_length_count[1]);

                    room_database_V3.Add(r);
                    r_list.Add(r);
                }
                room_database_V4.Add(r_list);

            }
            else if (i == make_room_num - 1)//������ ���� �� �ϳ���+�������� ����
            {
                List<room> r_list = new List<room>();
                room r = new room(3, i, 0);

                r_list.Add(r);
                room_database_V4.Add(r_list);
                r_length_count.Add(1);
            }
            else if (i == (make_room_num - 1) * 0.5f)//�� ���������� ���� ������ �������� ����
            {
                int r_door_num;

                r_door_num = Random.Range(3, 6);
                List<room> r_list = new List<room>();
                room r = new room(3, i, r_door_num);
                r_list.Add(r);
                room_database_V4.Add(r_list);
                r_length_count.Add(1);
            }

            else//�� �̿��� ���� 3~5 �� ������ ����
            {
                int r_door_num;

                r_door_num = Random.Range(3, 6);//3~5
                int e_num = 0;
                List<room> r_list = new List<room>();

                for (int o = 0; o < r_door_num; o++)
                {
                    //�̺�Ʈ �� ����
                    int rand = 100;
                    if (event_count < event_num && e_num < floor_event_num)//�̺�Ʈ ���� ���������� �ִ� ���� ������ �������� �ʾ��� ��
                    {
                        rand = Random.Range(0, 100);//Ȯ�� ����
                    }
                    room r;

                    if (rand < event_room_percent && event_count < event_num && e_num < floor_event_num)//�̺�Ʈ ���� Ȯ���� ���� �޼���
                    {
                        r = new room(4, i, r_door_num);//�̺�Ʈ ���� �����Ѵ�
                        event_count++;//event_count�� event_num �� ���� ������ �� �̺�Ʈ �� ������ �����
                        e_num++;
                    }
                    else//�Ϲ� ��
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
        //room_database_V4�� ������� �� ������Ʈ�� ������ �����Ѵ�
        make_room_V3_2(room_database_V4);
        connect_room_V3(room_database_V4);
        set_room_pos(room_database_V4);
        map_making_complete = true;
    }
    //scene�� ���� �� room�� ������ �ҷ����� ����� �Ҽ��� �־�����
    //scene->scene���� �̵��� �� �ε����� ��� ������� �ϴ� ������ �� scene�� ��� ���� �����ϴ� �������
    //����->����->���ġ
    public void connect_room_V3(List<List<room>> r)//��� ���� ����
    {
        for (int i = 0; i < r.Count - 1; i++)//������
        {
            r_List_n = r[i + 1];//�����ų ��(���� ��𿡵� ���� �ȵ�)
            r_List_n2 = new List<room>();//r_List_n�� ����Ʈ�� ��ȯ��Ű�� ���� �뵵
                                         //�ϳ��� ������ �Ǿ��������� ���� ����� ���� �Ͼ�� �ʵ��� �ϰ�(r_List_n)
                                         //r_List_n2�� �̿��� ������ Ƚ���� ������ ������� ����ȵ� ��Ż�� ������
            for (int n = 0; n < r[i].Count; n++)//�����
            {


                for (int a = 0; a < r[i][n].connect_num; a++)
                {
                    if (r_List_n.Count != 0)
                    {

                        r[i][n].r_connect.Add(r_List_n[0]);//���� ���� ��� �������� ���� ���� ��Ų��
                        r_List_n[0].r_connected.Add(r[i][n]);
                        r_List_n2.Add(r_List_n[0]);//r_List_n�� 0�� ������Ҹ� r_List_n2�� �ְ� ����Ʈ���� �����Ѵ�
                        r_List_n.RemoveAt(0);
                    }

                    
                }
         
                for (int ab = r_List_n2.Count; ab > 0; ab--)//r_List_n2�� ������Ҹ� �ٽ� r_List_n�� ���Ƿ� �ִ´�
                {
                    int r_num = Random.Range(0, ab - 1);
                    r_List_n.Add(r_List_n2[r_num]);
                    r_List_n2.RemoveAt(r_num);
                }
                //r_list�� r_list_2�� ���� ��ȯ�Ǹ鼭 r_list�� ������Ұ� ���Ƿ� �����ǰ� �װ� ������� �� ������ ����
                //(ó�� ������ ����Ʈ�� �߰��� ������ �����Ͽ� �濬���� �ȵǴ� ���� ����� ��츦 ����

            }
            


        }


    }
    public void set_room_pos(List<List<room>> r)//���� ��ġ(position)�� ���Ѵ�
    {
        int ac = 0;
        //���� ��ġ�� ��� ����+�ך�Ʈ �� �� ���� �������� �������� Ȯ���ϱ� ���� ���� ��ġ�� ���� �°� �̵��Ѵ�
        for (int i = 0; i < r.Count; i++)
        {
            for (int n = 0; n < r[i].Count; n++)
            {
                //���� ���̿� ���̸� Ȯ���Ѵ�
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
                //���� ���̿� ���̿� �°� �� ������ �̵���Ų��
                a.transform.position = new Vector3(r_x, r_y * (ac + 1), this.transform.position.z);
                if (i != 0)
                {
                    a.gameObject.SetActive(false);
                }


            }
        }

    }
    public void make_room_V3_2(List<List<room>> r)//room_database �� ������ �°� ���� �����Ѵ�
    {
  
        int r_num = r.Count;
        for (int i = 0; i < r_num; i++)//��
        {
            int r_r_num = r[i].Count;
            for (int n = 0; n < r_r_num; n++)//��
            {
                int random_number;
                GameObject a = null;
                if (r[i][n].room_element == 1)//���� ��
                {
                    if (Gamemanager.GM.stage == 1)
                    {
                        a = Instantiate(start_room[0]);//�������� 1(Ʃ�丮��)
                    }
                    else
                    {
                        a = Instantiate(start_room[1]);//�������� 2����(Ʃ�丮�� ����)
                    }
                }
                else if (r[i][n].room_element == 2)//�Ϲݹ�
                {
                    random_number = Random.Range(0, room_database.Length);//������ ���� �ҷ��´�
                    a = Instantiate(room_database[random_number]);
                }
               
                else if (r[i][n].room_element == 3)//����
                {
                    a = Instantiate(shop_room[0]);
                }
                else if (r[i][n].room_element == 4)//�̺�Ʈ
                {
                    a = Instantiate(event_room[0]);
                }


                //���� ������ ����Ʈ�� ������ ��ġ��Ų��
                a.GetComponent<room>().set_door(r[i][n].connect_num, r[i][n].room_element, r[i][n].r_length);


                
                //��ü ���� ������ ����ִ� ����Ʈ ������Ҹ� �Ű��ϰ� ��ü�� ������ ���� ������ �ٽ� ����Ʈ�� �߰��Ѵ�
                room_database_V3.RemoveAt(0);
                r[i].Add(a.GetComponent<room>());
                room_database_V3.Add(a.GetComponent<room>());


                
                a.transform.SetParent(this.transform);
              
            }
            //�����ϴµ� ���� ����Ʈ (room_database_V4)�� ���� �����Ͽ� �ʱ�ȭ ��Ų��
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
        if (!map_making_complete && !Gamemanager.GM.boss)//���� ���������� �ƴϰ� ���� �������� �ʾҴٸ� �� ����
        {
            make_room_V3();
        }

    }
}








////////////////////////////////////////////////////////////������ ����///////////////
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
  
    public GameObject make_room_V2()//�ƹ� ���̳� �����
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
    
    public GameObject make_room_V2(GameObject r)//�ƹ� ���� ������ �̾���δ�
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
        
        //���� ���� üũ�ؼ� ���̸鼭 ������� ���� ���� ����Ʈ�� ��´�
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
                                Debug.Log("���� ���ļ� �ı���" + (r_XY[0] -3) + r_XY[1]);
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
                            room_pos.Add(new Vector2(r_XY[0] - 3, r_XY[1]));//���� ��Ű�� ���� ��ǥ ��������
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
                                Debug.Log("���� ���ļ� �ı���" + r_XY[0] + (r_XY[1] + 3));
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
                               
                                Debug.Log("���� ���ļ� �ı���"+ r_XY[0]+ (r_XY[1] - 3));
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
        //����Ʈ�� ���� ��ŭ ���� ����� ���� �����Ų��

        //������ ���� �� ���� üũ�Ҽ� �ְ� ���ͷ� ������
        //start�� awake���� �����ٴ� ���� �̿��ؼ� ������ �ٵ� �Ŀ� �ٽ� ���� ����� ������� ����Ѵ� ����?
        //start awake���� ��� ����� list�� ��� ���� ���������� �ҷ����� ������� �ٲٸ� �����ϴ�! �̰� ����
        return null;

    }
    public GameObject create_room_V2()//�� �ϳ� ���� �� �̾���̴� ���
    {
        //room[room_count] = make_room_V2();//�� �����
        GameObject b = make_room_V2();//�� �����
        a++;
        if (room.Count< room_create_number)
        {
            return create_room_V2(room);//�ƴϸ� �ƾ���̱�
            
        }
        else
        {
            return null;//��ȯ���� �ʿ��ϸ� ä���

        }


    }
    public GameObject create_room_V2(List<GameObject> room)//������ ��� ����
    {
        if (room.Count < room_create_number)//��ΰ� �շ�������
            {
            if (r_count >= room.Count)
            {
                
                r_count = room.Count-1;
                room[r_count].GetComponent<room>().random_door();
            }
            GameObject r2 = make_room_V2(room[r_count]);//�� ������ �̾���̱�
            r_count++;

            //room[room_count] = make_room_V2(r);
            
                if (room.Count> room_create_number)//������ ����
                    return null;
                else
                    create_room_V2(room);//�׸��� �׹��� ��θ� �˻���
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