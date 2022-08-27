using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room_contents_controller : MonoBehaviour//방의 종류를 정하는 클레스
    //정해진 방의 구성요소를 방을 불려올 때 생성시켜서 불려온다
{//현재는 프리팹 내부에 방의 구성요소를 다 집어넣어서 쓰이지 않음
    public GameObject[] spawn_contents;
    public GameObject[] normal_contents;
    public GameObject[] event_contents;
    public GameObject[] shop_contents;
    public GameObject[] endroom_contents;
    public GameObject[] rare_event_contents;
    public GameObject content;
    public room this_room;
    public bool check_room_contents;


    // Start is called before the first frame update
    void Start()
    {
        this_room = gameObject.GetComponentInParent<room>();
       
    }
  
    // Update is called once per frame
    void Update()
    {
        if (!check_room_contents)
        {
            active_contents();
        }
    }
    void active_contents()
    {
        int random;
        switch (this_room.room_element)
        {
            case 1:
                random = Random.Range(0, spawn_contents.Length);
              
                GameObject a = Instantiate(spawn_contents[random], this.transform.position, Quaternion.identity);
                a.transform.SetParent(this.transform);
                check_room_contents = true;
                content = a;
                break;
            case 2:
                random = Random.Range(0, normal_contents.Length);
                GameObject b = Instantiate(normal_contents[random], this.transform.position, Quaternion.identity);
                b.transform.SetParent(this.transform);
        
                check_room_contents = true;
                content = b;
                
                break;
            case 3:
                random = Random.Range(0, 1);
                if (random < 0.1f)
                {

                }
                else
                {
                    random = Random.Range(0, event_contents.Length);
                    GameObject c = Instantiate(event_contents[random], this.transform.position, Quaternion.identity);
                    c.transform.SetParent(this.transform);
                    content = c;
     
                    check_room_contents = true;
                }
                break;
            case 4:
                random = Random.Range(0, endroom_contents.Length);
                GameObject g = Instantiate(endroom_contents[random], this.transform.position, Quaternion.identity);
                g.transform.SetParent(this.transform);
                content = g;

                check_room_contents = true;
                break;
            case 6:
                {
                    random = Random.Range(0, shop_contents.Length);
                    GameObject c = Instantiate(shop_contents[random], this.transform.position, Quaternion.identity);
                    c.transform.SetParent(this.transform);
                    content = c;
     
                    check_room_contents = true;
                }
                break;
        }
        
    }
}
