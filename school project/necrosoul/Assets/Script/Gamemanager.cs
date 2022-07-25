using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gamemanager : MonoBehaviour//게임 메니저
{
    
    public GameObject Enemy_destroy_effect;
    public GameObject player_minimap_pos;
    public GameObject enemy_minimap_pos;
    public GameObject portal_minimap_pos;
    public GameObject shop_minimap_pos;
    public GameObject event_minimap_pos;
    public GameObject drop_consumable_item;
    public Vector3 lastest_enemy_point;
    public int lastest_get_money;
    public int lastest_lose_money;
    public bool get_money_chk;
    public Item lastest_get_item;
    public bool get_item_chk;
    public bool lose_money_chk;
    public List<Item> loot_box = new List<Item>();
    public List<GameObject> bad_status_effect = new List<GameObject>();
    public List<GameObject> destroy_particle_instansi = new List<GameObject>();
    public ParticleSystem destroy_particle;
    public ParticleSystem heal_cross_particle;
    public ParticleSystem p_sword_effect;
    public ParticleSystem money_used_particle;
    public ParticleSystem enemy_hitted_particle;
    public ParticleSystem enemy_hitted_critical_particle;
    public GameObject Player;
    public GameObject Player_create;
    public GameObject Player_obj;
    public boss_map_system b_room_controller;
    public room_controller room_controller;
    public GameObject Player_spawn;
    public bool spawn_check;
    public static Gamemanager GM;
    public bool Onbattle;
    public GameObject sp_item_frame;
    public GameObject inventory_ui;
    public bool map_mode;
    public int stage=0;//0 게임 시작 안함 1~....게임중
    public bool boss;
    public GameObject fade_init;
    public GameObject fade_Outit;
    public bool fade_in_complete;
    public bool fade_out_complete;
    public bool can_handle;
    public GameObject End_panel;
    public int stage_end_line;
    public bool boss_clear;
    public bool room_end;
    public int room_num;
    public List<GameObject> Game_ui=new List<GameObject>();
    public List<GameObject> Main_UI = new List<GameObject>();
    public GameObject Game_sys;
    [Header("loop_upper")]
    public float atk_upr;
    public float spd_upr;
    public float hp_upr;
    public float b_spd_upr;
    public Game_Event game_ev=new Game_Event();
    public GameObject sp_item_UI;
    public bool sp_item_check;
    float t = 0;
    bool back_to_main;
    public bool initializing;

    public GameObject Totalbook;
    public int combo;
    public float combo_duration_max;
    float combo_duration;
    public ItemDatabase itemDB;
    public int last_enemy;

    public bool get_sp_item()//특수 아이템을 생성
    {
        if (!sp_item_frame.activeSelf)
        {
            BookButtonManger.bookButtonManger.ButtonTimerON2();
            sp_item_frame.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void get_item()  //아이템 생성
    {
        if (itemDB == null)
        {
            itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        }
        var item = itemDB.get_item_by_rarity(itemDB.item_list); //아이템 생성
        itemDB.Make_Get_item(item);   //중복찾기
        Debug.Log(item.Name + "아이템 들어감");
        game_ev.when_player_get_item(item); //아이템 효과 적용


        lastest_get_item = item.CreateItem();
        get_item_chk = true;
    }

    public void get_item(Item i)        //상점
    {
        if (itemDB == null)
        {
            itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
            ;
        }
        Debug.Log(i.Name + "아이템 들어감2");
        itemDB.Make_Get_item(i);

        game_ev.when_player_get_item(i);

        lastest_get_item = i.CreateItem();
        get_item_chk = true;
    }

    public void get_item_SP(Item i)// 아이템 흭득시 특수 효과 처리용
    {
        if (itemDB == null)
        {
            itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
            ;
        }

        itemDB.Make_Get_item(i);//itemDB에서 지정된 아이템을 흭득

    }
    private void Awake()
    {
        room_num = 5;
        GM = this;
        
    }

    void Start()
    {
        can_handle = true;
        
        DontDestroyOnLoad(this.gameObject);
        DonDestoryManage.DDM.DDM_List.Add(this.gameObject);//자신이 추가하지 않음(DontDestroyOnLoad 설정을 특정 타이밍에 파괴하는 리스트)
        for (int i = 0; i < 5; i++)//파괴 파티클을 풀링용으로 생성
        {
            var a = Instantiate(destroy_particle.gameObject, this.transform);
            destroy_particle_instansi.Add(a);
            a.SetActive(false);
        }
    }
    public GameObject destroy_particle_pulling()//파괴 효과 풀링(리스트에서 비활성화된 오브젝트 탐색)
    {
        int index = 0;
        for(int i = 0; i < destroy_particle_instansi.Count;i++)
        {
            if (!destroy_particle_instansi[i].activeSelf)
            {
                return destroy_particle_instansi[i];
            }
        }
        return destroy_particle_instansi[index];
    }
    //콤보 시스템(사용 안됨)
    void combo_system()
    {
        if (combo > 0)//콤보가 존재할 때 일정시간똥안 콤보를 얻지못하면 초기화
        {

            combo_duration -= Time.deltaTime;
            if (combo_duration <= 0)
            {
                stop_combo();
            }
        }
    }
    public void get_combo()//콤보를 쌓는다
    {
        combo++;
        combo_duration = combo_duration_max;
    }
    public void stop_combo()///콤보를 초기화 한다
    {
        combo = 0;
        combo_duration = 0;
    }

    //메인 화면으로 돌아간다(로딩씬을 거침)
    void loadMain2()
    {
        if (fade_out_complete)
        {
            LoadingSceneManager.l_scenemanager.LoadMain();
      
        }
    }
    // Update is called once per frame
    void Update()
    {
        //펭이드 인,아웃관련 변수 처리
        if (!fade_init.activeSelf)
            fade_in_complete = false;
        if (!fade_Outit.activeSelf)
            fade_out_complete = false;
        if (stage == 0) {//게임 중이 아닐 때 ui,관련 오브젝트 비활성화
            for(int i = 0; i < Game_ui.Count; i++)
            {
                Game_ui[i].SetActive(false);
            }
            Game_sys.SetActive(false);
        }
        else {//게임중일때
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.PAUSE]))//정지 키를 누르면 일싲정지 창이 나타난다
                {
                if (Totalbook.activeSelf)
                {
                    Totalbook.SetActive(false);
                }
                else
                {
                    Totalbook.SetActive(true);
                }

            }
            //게임 관련 ui, 오브젝트 활성화
            for (int i = 0; i < Game_ui.Count; i++)
            {
                Game_ui[i].SetActive(true); 
            }
            Game_sys.SetActive(true);

            //메인으로 돌아가기
            if (back_to_main)
                {
                    loadMain2();
                }
           
            //보스스테이지가 아닐 때
            if (!boss)
            {
                on_game();
              
                
            }
            else//보스 스테이지 일 때
            {
                on_game_b();
            }
        }
        //}
    }
    void on_game_b()//보스 스테이지일 때  스테이지를 시작할 준비를 하는 함수
    {
        if (b_room_controller == null)
        {
            b_room_controller = GameObject.Find("boss_map_system").GetComponent<boss_map_system>();
        }
        else
        {
            if (b_room_controller.map_making_complete && !initializing)//맵 생성이 끝났을 때
            {
                
                if (Player_spawn == null && !initializing)//플레이어를 스폰 찌점으로 생성
                {
                   
                    Player_spawn = GameObject.FindGameObjectWithTag("Spawner");
                }
                if (Player_create == null&&!spawn_check && !initializing)
                {
                   //생성 후 페이드 인+쪼작 가능
                    fade_in();

                    fade_Outit.SetActive(false);
                    fade_out_complete = false;
                    Player_create = Instantiate(Player);
                    Player_obj = Player_create.transform.GetChild(0).gameObject;
                    Player_obj.transform.position = Player_spawn.transform.position;
                    can_handle = true;
                }

                if (!spawn_check && Player_create != null && Player_spawn != null&& !initializing)
                {
                    
               //생성이 마무리 됨
                   
                    Player_obj.SetActive(true);
                    spawn_check = true;
                }
            }
            //스테이지 종료 조건을달성하고 페이드 아웃이 끝났을 때
            if (room_end&&fade_out_complete)
            {
                room_end = false;
                if(!initializing)
                    GM_initialize();//관련 변수 초기화+다음 스테이지로
            }
            if (stage == stage_end_line&&boss_clear)//마지막 스테이지의 보스를 처치할 경우
            {
                //게임이 끝내고 결과창을 표시
                End_panel.SetActive(true);
            }
        }
    }
    public void GM_initialize()//변수 초기화 +다음 스테이지or보스로 이동
    {
        initializing = true;
            Destroy(Player_spawn);//생성한 플레이어 파괴
        
            if (!Gamemanager.GM.boss)//보스 스테이지가 아니라면
            {
                int a = room_controller.room.Count;
                for (int i = a - 1; i == 0; i--)//방 설정 초기화
                {
                    Destroy(room_controller.room[i]);
                    room_controller.room.RemoveAt(i);
                }
            room_controller.map_making_complete = false;

            LoadingSceneManager.l_scenemanager.Loadboss();//로딩 씬을 통해 보스 씬 이동
            boss = true;//보스 스테이지임을 알림
                
            }
            else//보스스테이지라면
            {
            //보스방 설정 초기화
            int b = b_room_controller.boss_room_prefab.Length;
            for(int i = b - 1; i == 0; i--)
            {
                Destroy(b_room_controller.boss_room_prefab[i]);
            }
            //다음 스테이지로 이동
            LoadingSceneManager.l_scenemanager.LoadStage();//로딩 씬
                boss = false;
               
            }
            //게임메니저의 플레이어 생성,스테이지 달성도 관련 변수 초기화
        room_num = 5;
            room_controller = null;
            b_room_controller = null;
            Player_spawn = null;
            Player_create = null;
            spawn_check = false;
            boss_clear = false;
        sp_item_check = false;

        Player_status.p_status.spawn_check = false;
        t = 0;
    }
   
    
   
    void on_game()//일반 스테이지
    {

        if (room_controller == null&& GameObject.Find("map_system")!=null)
            room_controller = GameObject.Find("map_system").GetComponent<room_controller>();
        else
        {
            if (room_controller.map_making_complete && !initializing)//방생성이 끝났다면
            {
                //플레이어 생성(구조는 on_game_b와 동일)
                if (Player_spawn == null)
                {
                   
                    Player_spawn = GameObject.FindGameObjectWithTag("Spawner");
                    
                }
                if (Player_create == null && !spawn_check && !initializing)
                {
                    Player_create = Instantiate(Player);
                    Player_create.transform.position = Vector3.zero;
                    fade_in();
                    fade_Outit.SetActive(false);
               
                    Player_obj = Player_create.transform.GetChild(0).gameObject;
                    Player_obj.transform.position = Player_spawn.transform.position;
                    can_handle = true;
                }

                if (!spawn_check && Player_create != null && Player_spawn != null && !initializing)
                {
             
                   
                   
                    Player_obj.SetActive(true);
                    spawn_check = true;
                }

                if (room_num == 0)//모든 층을 돌파시
                {
       //페이드 아웃+스테이지 완료 처리
                    fade_out();
                    room_end = true;
                }
                if (room_end && fade_out_complete)//설정을 초기화하고 보스 스테이지로
                {
                    room_end = false;
                    if (!initializing)
                        GM_initialize();
                    
                }

            }
        }
    }
    //카메라 페이드 인 이펙트를 활성화
    public void fade_in()
    {
        if(Player_obj!=null)
            Player_obj.GetComponent<PlayerCharacter>().Player_canvas.SetActive(true);
        fade_init.SetActive(true);
    }
    //카메라 페이드 아웃 이펙트를 활성화
    public void fade_out()
    {

        can_handle = false;
        if (Player_obj != null)
            Player_obj.GetComponent<PlayerCharacter>().Player_canvas.SetActive(false);
        fade_Outit.SetActive(true);
    }
   

}
