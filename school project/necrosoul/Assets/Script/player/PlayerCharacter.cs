using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : GameCharacter//플레이어 조작+상태 클레스(게임 내부 생물체의 기본 상호적용이 포함된GameCharacter를 상속) 
{
    public Rigidbody2D rgd;
    public int jump_count = 1;
    public bool onground;//땅에 큱E年쩝갋

    public GameObject DNP;

    public bool spawn_check;
    Vector3 move_vector;

    [Header("jump")]
    float hangtime = 0.2f;
    float hangTimer;
    float jumpbuffertime = 0.5f;
    float jumpbuffertimer;

    GameObject player_pos;
    float rush_time = 1f;
    float rush_timer;
    public bool on_rush;
    bool raycheck;

    [Header("dash input")]
    int dash_count;
    float after_dash_timer;
    float after_dash_timer_check = 0.05f;
    bool dash_recover_check;
    float dash_recover_timer;
    public float teleport_length = 6.55f;
    public bool on_teleport;
    Vector2 teleport_pos;
    public float teleport_untouable_time = 0.1f;
    public float teleport_untouable_timer;
    public int teleport_direction;//0:right1:left 2:up 3:down
    bool can_dash = true;
    public Vector2 dash_direction;
    public float dash_time = 1f;
    public float dash_timer = 0;

    public AudioManage_Main m_audio;

    [SerializeField]
    public bool on_dash;

    KeyCode lastest_key_input;

    [Header("hitted  input")]
    public Vector2 hitted_force;
    public GameObject m_teleport_effect;
    public float hitted_time = 0.5f;
    public float hitted_timer = 0;
    bool on_hitted;
    Vector2 col_pos;
    bool col_on_room_boost;
    public ActionRecord record;
    public bool death_anim_check;
    [Header("platform")]
    bool on_platform;
    bool on_corutine_1;
    public float Downbuffertime = 0.4f;
    RaycastHit2D down_ray_1;
    RaycastHit2D down_ray_2;
    float Downbuffertimer;
    public GameObject Player_canvas;
    float layer_change_time = 0.25f;
    float layer_change_timer;
    Vector3 viewport_pos;
    public float move_weight;
    Vector2 direct_vector;
    float minimum_jump_time = 0.15f;
    float minimum_jump_timer;
    public ProgressBarPro HpBar;            //Hp
    public ParticleSystem dash_ghost_effect;
    public List<GameObject> dash_ghost_effect_instansi = new List<GameObject>();
    public GameObject choose_ghost_effect;
    public Player_animator p_anim;
    Attack a;
    public float Player_X;
    public float Player_Y;
    public bool landing_chk;
    public List<GameObject> heal_cross_instansi = new List<GameObject>();
    public float dash_force;
    //오디오+기록+폰트 관련된 것은 자신이 작업한것이 아님
    void Start()
    {
        //초기값 설정
        can_attack = true;//공격 가능 여부
        m_audio = AudioManage_Main.instance;//오디오
        a = this.GetComponent<Attack>();
        dash_ghost_effect.gameObject.SetActive(false);
        record = GameObject.Find("GameSystem").GetComponent<Record>().ActionRecord;//기록
        p_anim = gameObject.GetComponentInChildren<Player_animator>();

        DNP = GameObject.Find("DemoManager");//폰트
        Player_status.p_status.set_layout();//초기능력치 설정
        rgd = gameObject.GetComponent<Rigidbody2D>();
        jump_count = Player_status.p_status.get_jump_count();
        spawn_check = true;
        direction = 1;
        can_move = true;
        for (int i = 0; i < 3; i++)//풀링시킬 대쉬잔상을 생성
        {
            var a = Instantiate(dash_ghost_effect.gameObject, this.transform.position, Quaternion.identity);
            dash_ghost_effect_instansi.Add(a);
            a.transform.SetParent(this.transform);
            a.SetActive(false);
        }
        for (int i = 0; i < 3; i++)//풀링시킬 회복 이펙트 파티클 생성
        {
            var a = Instantiate(Gamemanager.GM.heal_cross_particle.gameObject, this.transform.position, Quaternion.identity);
            heal_cross_instansi.Add(a);
            a.transform.SetParent(this.transform);
            a.SetActive(false);
        }
        if (player_pos == null)//플레이어의 위치를 미니맵에 생성
        {
            GameObject ins = Instantiate(Gamemanager.GM.player_minimap_pos);//미니맵에 플레이어의 위치를 알리는 오브젝트를 플레이어의 위치로 생성한다
            ins.transform.position = this.transform.position;
            ins.transform.SetParent(this.transform);
        }
    }
    GameObject player_heal_cross_pulling()//회복이펙트 풀링
    {
        //리스트에서 비활성화 된 오브젝트반환
        int index = 0;
        for (int i = 0; i < heal_cross_instansi.Count; i++)
        {
            if (!heal_cross_instansi[i].activeSelf)
            {
                index = i;
            }
        }
        return heal_cross_instansi[index];
    }
    public void Player_heal_cross_particle(int n)//회복 이펙트를 활성화 시킨다
    {
        var obj = player_heal_cross_pulling();
        heal_cross_particle a = obj.GetComponent<heal_cross_particle>();
        a.n = n;
        obj.transform.position = this.transform.position;
        obj.SetActive(true);
    }
    //플레이어 대쉬 잔상 풀링
    GameObject player_ghost_pulling()
    {
        //리스트에서 비활성화 된 오브젝트 반환
        int index = 0;
        for (int i = 0; i < dash_ghost_effect_instansi.Count; i++)
        {
            if (!dash_ghost_effect_instansi[i].activeSelf)
            {
                index = i;
            }
        }
        return dash_ghost_effect_instansi[index];
    }
    public void set_col_boost(bool a)//부스터 set()(사용안함)
    {
        col_on_room_boost = a;
    }
    public bool get_col_boost()//부스터 get()(사용안함)
    {
        return col_on_room_boost;
    }
    //대쉬 가능 여부 설정
    public void set_can_dash(bool a)
    {
        can_dash = a;
    }
    //이동 가능 여부 설정
    public void set_can_move(bool a)
    {
        can_move = a;
    }
    //플레이어가 카메라 밖에서 벗어나지 않도록 한다
    void player_camera_fitting()
    {
        viewport_pos = Camera.main.WorldToViewportPoint(transform.position);//뷰포트에 플레이어의위치 변환
        //카메라를 벗어나려할 때 그 위치를 고정
        if (viewport_pos.x < 0) viewport_pos.x = 0;
        if (viewport_pos.y < 0) viewport_pos.y = 0;
        if (viewport_pos.x > 1) viewport_pos.x = 1;
        if (viewport_pos.x > 1) viewport_pos.y = 1;
        transform.position = Camera.main.ViewportToWorldPoint(viewport_pos);


        if (transform.rotation.y > 360)
            transform.Rotate(0, -360, 0);
    }
    void FixedUpdate()
    {
        //플레이어의 아랫 방향으로 플랫폼을 확인하는 ray를 쏜다(플랫폼의 종류별로 2개)
        down_ray_1 = Physics2D.Raycast(this.transform.position + (Vector3.down * (Player_Y - 0.1f)), Vector2.down, 0.1f, LayerMask.GetMask("platform_can't_pass"));
        down_ray_2 = Physics2D.Raycast(this.transform.position + (Vector3.down * (Player_Y - 0.1f)), Vector2.down, 0.1f, LayerMask.GetMask("platform_can_pass"));
        //채력,방엉澹,이동속도는 Player_status클레스의 값을 따라간다
        max_hp = Player_status.p_status.get_max_hp();
        Health_point = Player_status.p_status.get_hp();
        Defense_point = Player_status.p_status.get_defense_point();
        move_speed = Player_status.p_status.get_speed();



        //체력ui (자신이 작업하지않음)
        if (HpBar != null)
        {
            HpBar.SetMaxTextValue(Health_point, max_hp);
            HpBar.SetValue(Health_point, max_hp);
        }

        //공중에 있는지 여부 확인
        p_anim.set_ground(onground);

        if (Gamemanager.GM.can_handle || !!death_anim_check)//조작 가능한 상태이며 사망 에니메이션 진행중이 아닐 때
        {

            character_move();//이동
            //카메라에 벗어나지않음
            if (!Gamemanager.GM.fade_Outit.activeSelf && Player_status.p_status.get_hp() > 0)//페이드 아웃중이 아닐때+체력이 0이하가 아닐 때
                player_camera_fitting();

            //플랫폼체크 용Eray가 충돌 중이라면 착지 힌것을 확인
            if (down_ray_1.collider || down_ray_2.collider)
            {
                landing_chk = true;
            }
            else
            {
                landing_chk = false; ;
            }

            //untouchable_state가 true일 때 무적상태
            if (untouchable_state)
                untouchable();

            //체력이 0이하이며플레이어가 생성이 완전히 끝난 상태라면
            if (Health_point <= 0 && spawn_check)
            {

                if (!death_anim_check)
                {
                    //사망 에니메이션을 시작하고 조작을 비활성화
                    death_anim_check = true;
                    p_anim.death_state = true; //사망 처리
                    Gamemanager.GM.can_handle = false;
                }



            }
            //대쉬가 불가능 하다면(대쉬 후 딜레이)
            if (!can_dash)
            {
                //after_dash_timer이후 다시 대쉬 가능
                after_dash_timer += Time.deltaTime;
                if (after_dash_timer >= after_dash_timer_check)
                {

                    can_dash = true;
                    after_dash_timer = 0;
                }
            }
            //velocity.y값이 0이 아니라면 공중에 있다
            if (rgd.velocity.y != 0)
            {
                if (onground)
                {

                    onground = false;
                }
            }
            else
            {

            }
            //공중에 있다면
            if (!onground)
            {
                //hangTimer 작동
                if (hangTimer > 0)
                    hangTimer -= Time.deltaTime;
            }
            else//아니라면
            {
                //hangTimer 정함
                hangTimer = hangtime;
                direct_vector = new Vector2(direct_vector.x, 0);

            }
            //hangTimer가 양수인 상태라면 점프가 작동
            //플랫폼에서 공중으로 넘어가더라도 짧은 시간동안 점프를 할수있는 여유 시간이 존재(점프 판정 완화)

            //피격 중인 상태라면
            if (on_hitted)
            {
                //hitted_timer 후 피격 상태 종료
                hitted_timer -= Time.deltaTime;
                if (hitted_timer <= 0)
                {
                    hitted_end();
                }
            }
            //피격 중인 상태일 땐 플레이어 캐릭터가 뒤로 밀려나며 조작이 불가능 하다


            if (rgd.velocity.y <= 0.1 && !onground)//캐릭터가 떨어지는 중일 때
            {

                //플랫폼체크용Eray에 닿았을때
                //ray는 플레이어 스프라이트 중심점에서 아래로 출발하며 길이는 플레이어의 스프라이트의 y값의 절반보다 약간길다(판정완화)
                if (down_ray_1.collider)//일반 플랫폼과양방향 플랫폼
                {
                    groundcollision(down_ray_1.transform.gameObject);//점프 회복


                }
                if (down_ray_2.collider)
                {
                    groundcollision(down_ray_2.transform.gameObject);


                }
                else
                {

                }
            }
        }
    }
    void jump()//점프
    {
        //대쉬 상태가 아닐 때 점프 키를 눌렸다면ㄴ
        if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]) && !p_anim.sword_delay && !on_dash && !on_teleport)
        {
            jumpbuffertimer = jumpbuffertime;//점프 버퍼를 작동
        }
        else
        {
            //점프 가능한 상태가 아니라면 버퍼의 시간이 지나간다
            if (jumpbuffertimer > 0)
            {
                jumpbuffertimer -= Time.deltaTime;
            }
        }
        //점프가능한 상태(점프 횟수가 0이 아님+공격중이 아님+대쉬 중이 아님+hangTimer가 0이하가 아님)에서 버퍼가 작동돼있다면
        if (jumpbuffertimer > 0 && jump_count == Player_status.p_status.get_jump_count() && hangTimer > 0 && !p_anim.sword_delay && !on_dash && !on_teleport)//점프횟수가 가득혖E상태 일 때
        {
            //버퍼를 초기화하고 점프 실행
            jumpbuffertimer = 0;
            minimum_jump();
            p_anim.jump_anim();
            //jump();
        }
        else if (jumpbuffertimer > 0 && jump_count != 0 && !p_anim.sword_delay && !on_dash && !on_teleport)//점프 횟수가 남은 상태일때
        {
            jumpbuffertimer = 0;
            minimum_jump();
            p_anim.jump_anim();
            //jump();
        }
        if (minimum_jump_timer > 0)//최소 점프 높이(점프키를 아무리 짧게 눌려도 기본 높이까지 올라옴)
        {
            minimum_jump_timer -= Time.deltaTime;
        }
        //점프키를 길게 누르면 점프 높이가 높아지며 짧게 누르면 점프 높이가 낮아진다
        //점프 키를 누르는 중이 아니라면 velocity.y값이 빠르게 감소
        if (!Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.JUMP]) && rgd.velocity.y > 0 && !on_dash && minimum_jump_timer <= 0 && !on_teleport)
        {

            jumpbuffertimer = 0;
            rgd.velocity = new Vector2(rgd.velocity.x, rgd.velocity.y * 0.5f);
        }
    }
    void dash_recover()//대쉬 횟수를 회복 한다
    {
        //대쉬 횟수가 소모되었을 ㅐ
        if (dash_count != Player_status.p_status.get_dash_count())
        {
            dash_recover_timer += Time.deltaTime;
            //dash_recover_timer 초 후 회복
            if (dash_recover_timer >= Player_status.p_status.get_dash_recover_time())
            {
                dash_count = Player_status.p_status.get_dash_count();
                dash_recover_timer = 0;
            }


        }
    }
    //텔레포트(특정아이템 흭득시 대쉬 대신 사용
    public void teleport()
    {
        //대쉬횟수를 가졌으면서 대쉬할수있는 상태일때 대쉬키를 눌렸을 때
        if (Input.GetKeyDown(KeyCode.LeftShift) && dash_count != 0 && can_dash && can_move)
        {
            Debug.Log("텔레포트");
            //플레이어 가속도 초기화
            rgd.velocity = Vector2.zero;
            m_audio.teleport();


            untouchable_state = true;
            on_teleport = true;
            //텔레포트 중 일시 무적
            teleport_untouable_timer = teleport_untouable_time;
            //텔레포트 할 위치를 받기위한 ray
            RaycastHit2D teleport_ray = Physics2D.Raycast(this.transform.position, Vector2.right, teleport_length, LayerMask.GetMask("platform_can't_pass")); ;
            switch (teleport_direction)//teleport_direction에 따라ray의 방향이 달라진다(teleport_direction은 누르고있는 이동 방향키 디폴트 값:0)
            {
                //정확한 판정을 위해 ray의 위치를 방향에 따라조절한다
                case 0:
                    teleport_ray = Physics2D.Raycast(this.transform.position + (Vector3.right * Player_X * 0.5f), Vector2.right, teleport_length, LayerMask.GetMask("platform_can't_pass"));
                    break;
                case 1:
                    teleport_ray = Physics2D.Raycast(this.transform.position + (Vector3.left * Player_X * 0.5f), Vector2.left, teleport_length, LayerMask.GetMask("platform_can't_pass"));
                    break;
                case 2:
                    teleport_ray = Physics2D.Raycast(this.transform.position + (Vector3.up * Player_Y * 0.5f), Vector2.up, teleport_length, LayerMask.GetMask("platform_can't_pass"));
                    break;
                case 3:
                    teleport_ray = Physics2D.Raycast(this.transform.position + (Vector3.down * Player_Y * 0.5f), Vector2.down, teleport_length, LayerMask.GetMask("platform_can't_pass"));
                    break;
            }
            if (teleport_ray.collider != null)//ray가 충돌 됐을 때->장애물이 존재할 때
            {
                switch (teleport_direction)
                {
                    //충돌 위치 바로 앞에 플레이어가 텔레포트 할 수 있도록 위치를 조정한다
                    case 0:
                        teleport_pos = teleport_ray.point - (Vector2.right * Player_X * 0.5f);
                        break;
                    case 1:
                        teleport_pos = teleport_ray.point - (Vector2.left * Player_X * 0.5f);
                        break;
                    case 2:
                        teleport_pos = teleport_ray.point - (Vector2.up * Player_Y * 0.5f);
                        break;
                    case 3:
                        teleport_pos = teleport_ray.point - (Vector2.down * Player_Y * 0.5f);
                        break;
                }
            }
            else
            {//ray가 충돌하지않았다->텔레포트 하는 방향으로 장애물이 없다->그 위치값을 받는다
                switch (teleport_direction)
                {
                    //플레이어 위치에서 teleport_length를 방향에 맞게 더한 벡터를 위치로 잡는다
                    case 0:
                        teleport_pos = (Vector2)this.transform.position + (Vector2.right * teleport_length);
                        break;
                    case 1:
                        teleport_pos = (Vector2)this.transform.position + (Vector2.left * teleport_length);
                        break;
                    case 2:
                        teleport_pos = (Vector2)this.transform.position + (Vector2.up * teleport_length);
                        break;
                    case 3:
                        teleport_pos = (Vector2)this.transform.position + (Vector2.down * teleport_length);
                        break;
                }
            }
            //정밀한 텔레포트를 위해 정해진Eteleport_pos에 방향에맞게 두방향의 ray를 쏜다
            //이 ray를 사용해 플레이어가 텔레포트 할 수있는 공간이 확보가 되어있는지를 체크한다
            RaycastHit2D chk_ray1 = Physics2D.Raycast(teleport_pos, Vector2.up, Player_Y * 0.5f, LayerMask.GetMask("platform_can't_pass"));
            RaycastHit2D chk_ray2 = Physics2D.Raycast(teleport_pos, Vector2.down, Player_Y * 0.5f, LayerMask.GetMask("platform_can't_pass"));
            switch (teleport_direction)
            {
                //가로 텔레포트->위 아래로 ray를 쏜다
                case 0:
                case 1:
                    chk_ray1 = Physics2D.Raycast(teleport_pos, Vector2.up, Player_Y * 0.5f, LayerMask.GetMask("platform_can't_pass"));
                    chk_ray2 = Physics2D.Raycast(teleport_pos, Vector2.down, Player_Y * 0.5f, LayerMask.GetMask("platform_can't_pass"));
                    break;
                //세로 텔레포트->좌우로 ray를 쏜다
                case 2:
                case 3:
                    chk_ray1 = Physics2D.Raycast(teleport_pos, Vector2.right, Player_X * 0.5f, LayerMask.GetMask("platform_can't_pass"));
                    chk_ray2 = Physics2D.Raycast(teleport_pos, Vector2.left, Player_X * 0.5f, LayerMask.GetMask("platform_can't_pass"));
                    break;
            }


            //두 ray가 모두 충돌 되면 제약이 없는 빈공간
            if (!chk_ray1.collider && !chk_ray2.collider)
            {
                //그 위치 그대로텔레포트
                transform.position = (Vector3)teleport_pos + (Vector3.forward * this.transform.position.z);
            }
            //두 ray 중 하나가 충돌될경우 그 ray를 기점으로 플레이어의 위치 값의 절반을 더하면 플레이어의 위치를 확보할수있다
            else if (!chk_ray1.collider && chk_ray2.collider)
            {
                switch (teleport_direction)
                {
                    case 0:
                    case 1:
                        transform.position = (Vector3)(chk_ray2.point + Player_Y * 0.5f * Vector2.up + Vector2.up * 0.2f) + (Vector3.forward * this.transform.position.z);

                        break;
                    case 2:
                    case 3:
                        transform.position = (Vector3)(chk_ray2.point + Player_X * 0.5f * Vector2.right) + (Vector3.forward * this.transform.position.z);

                        break;
                }
            }
            else if (chk_ray1.collider && !chk_ray2.collider)
            {
                switch (teleport_direction)
                {
                    case 0:
                    case 1:
                        transform.position = (Vector3)(chk_ray2.point - Player_Y * 0.5f * Vector2.up - Vector2.up * 0.2f) + (Vector3.forward * this.transform.position.z);

                        break;
                    case 2:
                    case 3:
                        transform.position = (Vector3)(chk_ray2.point - Player_X * 0.5f * Vector2.right) + (Vector3.forward * this.transform.position.z);

                        break;
                }
            }
            else//두 ray모두 충돌할 경우 플레이어가 텔레포트할 공간이 없다
            {
                //플레이어의 가장자리로 부터 ray를 발생 어느 부분을 가장자리로 할지는 teleport_direction에 따라결정
                //가장자리가 충돌한 위치를 받아서 텔레포트 좌표로 사용
                switch (teleport_direction)
                {
                    case 0:

                        chk_ray2 = Physics2D.Raycast((Vector2)transform.position - Player_Y * 0.5f * Vector2.up, Vector2.right, teleport_length, LayerMask.GetMask("platform_can't_pass"));
                        teleport_pos = new Vector2(chk_ray2.point.x, transform.position.y);
                        break;
                    case 1:

                        chk_ray2 = Physics2D.Raycast((Vector2)transform.position - Player_Y * 0.5f * Vector2.up, Vector2.left, teleport_length, LayerMask.GetMask("platform_can't_pass"));
                        teleport_pos = new Vector2(chk_ray2.point.x, transform.position.y);
                        break;
                    case 2:
                        chk_ray2 = Physics2D.Raycast((Vector2)transform.position - Player_X * 0.5f * Vector2.right, Vector2.up, teleport_length, LayerMask.GetMask("platform_can't_pass"));
                        teleport_pos = new Vector2(transform.position.x, chk_ray2.point.y);
                        break;
                    case 3:
                        chk_ray2 = Physics2D.Raycast((Vector2)transform.position - Player_X * 0.5f * Vector2.right, Vector2.down, teleport_length, LayerMask.GetMask("platform_can't_pass"));
                        teleport_pos = new Vector2(transform.position.x, chk_ray2.point.y);
                        break;
                }
                //텔레포트
                transform.position = (Vector3)teleport_pos + (Vector3.forward * this.transform.position.z);
            }
            //텔레포트 액션 중에는 공격 불가 
            AE_Teleport();
            can_attack = false;
            dash_count--;
            //짧은 딜레이 동안 공격불가
            can_dash = false;
            dash_recover_check = false;
        }
    }
    //텔레포트 액션이 끝났을 때
    void teleport_end()
    {
        //공격 가능,
        can_attack = true;
        untouchable_state = false;
        on_teleport = false;
    }
    //대쉬->특정 키를 누르면 순간적으로 바라보고 있는 방향으로 강한 가속을 받는다 대쉬 중에는 무적
    void dash()
    {
        //대쉬횟수를 가졌으며 대쉬할수있는 상태일때 대쉬키눌렸을 때
        if (Input.GetKeyDown(KeyCode.LeftShift) && dash_count != 0 && can_dash)
        {
            //게임이벤트 클레스에 대쉬키를 누름을 알림
            Gamemanager.GM.game_ev.when_dash_key_input(transform);

            m_audio.Dash();
            rgd.velocity = Vector2.zero;
            //대쉬 중에는 공격X+ 무적상태
            can_attack = false;
            untouchable_state = true;
            p_anim.dash();//대쉬에니메이션 실행

            //대쉬중에는 대쉬중 잔상(파티클)을 만든다
            make_dash_ghost();
            on_dash = true;
            //dash_timer 설정
            dash_timer = dash_time;


            //dash_direction->플레이어가 바라보고 있는 방향
            dash_direction = Vector2.right * direction * dash_force;
            //ForceMode2D.Impulse를 이용해 순간적으로 가속을 받는다
            rgd.AddForce(dash_direction * Time.deltaTime, ForceMode2D.Impulse);
            //대쉬횟수감소 일정시간동안 대쉬 감소
            dash_count--;
            can_dash = false;
            dash_recover_check = false;
        }

    }
    //대쉬 잔상생성
    void make_dash_ghost()
    {
        //풀링 리스트로  부터 비활성화 된 대쉬잔상을 지정해 활성화 시킨다
        var a = player_ghost_pulling();
        choose_ghost_effect = a;
        a.SetActive(true);

    }

    void dash_end()//대쉬 상태 종료
    {
        //게임이벤트 클레승에 대쉬가 끝났음을 알림
        Gamemanager.GM.game_ev.Dash_End_effect();
        //가속 종료
        rgd.velocity = Vector2.zero;
        //무적 해재 공격+이동 가능
        untouchable_state = false;
        p_anim.dash_end();
        can_attack = true;
        can_move = true;
        //다시 중력을 받는다
        rgd.gravityScale = 1;
        on_dash = false;
    }
    //누르고 있는 방향키에 따라 teleport_direction의 값을 정한다
    public void set_teleport_direction()
    {
        if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))
        {
            teleport_direction = 0;
        } else if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.LEFT]))
        {
            teleport_direction = 1;
        }
        else if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.UP]))
        {
            teleport_direction = 2;
        }
        else if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.DOWN]))
        {
            teleport_direction = 3;
        }
        else
        {
            if (direction == 1)
            {
                teleport_direction = 0;
            }
            else
            {
                teleport_direction = 1;
            }
        }
    }
    //캐릭터 이동,점프,대쉬,종합 정리
    new void character_move()
    {

        if (can_move && hitted_timer <= 0)//움직일 수 있고 피격당한 상태가 아니라면
        {

            //마지막으로 누르고 있는 좌우방향키(lastest_key_input)를 받는다
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))
            {
                lastest_key_input = Key_manager.Keys[Key_manager.KeyAction.RIGHT];
            }
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT]))
            {
                lastest_key_input = Key_manager.Keys[Key_manager.KeyAction.LEFT];
            }
            if (Input.GetKeyUp(Key_manager.Keys[Key_manager.KeyAction.RIGHT]))
            {
                lastest_key_input = Key_manager.Keys[Key_manager.KeyAction.LEFT];
            }
            if (Input.GetKeyUp(Key_manager.Keys[Key_manager.KeyAction.LEFT]))
            {
                lastest_key_input = Key_manager.Keys[Key_manager.KeyAction.RIGHT];
            }

            //방향키 좌우 둘 중 하나가 눌려진 상태에서 다른 방향을 누르면 그 방향을 마지막으로 누른 방향키(lastest_key_input)로 정한다
            if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.RIGHT]) && !Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.LEFT])) {
                lastest_key_input = Key_manager.Keys[Key_manager.KeyAction.RIGHT];
            } else if (!Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.RIGHT]) && Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.LEFT]))
            {
                lastest_key_input = Key_manager.Keys[Key_manager.KeyAction.LEFT];
            }
            //좌우 방향키를 누르고 있다면
            if ((Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.RIGHT]) || Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.LEFT]) ||
                    Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]) || Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT])) && !p_anim.sword_delay && !on_dash && !on_teleport && !Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.ATTACK]))
            {
                //마지막으로 누른 방향키(lastest_key_input)에 따라 좌우중 어느 방향으로 움직일지 정한다
                if (lastest_key_input == Key_manager.Keys[Key_manager.KeyAction.RIGHT])//오른쪽
                {
                    p_anim.move_state = true;
                    if (direction == -1)//바라보고 있는 방향이 반대라면 방향 변경
                    {
                        direction_change();
                    }

                    if (move_weight != 0)//이동중량이 0이 안니라면
                        rgd.velocity = new Vector2(direction * move_speed * move_weight, rgd.velocity.y);//지정된 방향으로 addforce
                    //벽체크요 ray
                    RaycastHit2D wall_ray = Physics2D.Raycast(this.transform.position + Vector3.up * Player_Y * 0.5f, Vector3.right, Player_X, LayerMask.GetMask("platform_can't_pass"));
                    RaycastHit2D wall_ray2 = Physics2D.Raycast(this.transform.position - Vector3.up * Player_Y * 0.85f, Vector3.right, Player_X, LayerMask.GetMask("platform_can't_pass"));
                    Debug.DrawRay(this.transform.position - Vector3.up * Player_Y * 0.85f, Vector3.right * Player_X, Color.red);
                    Debug.DrawRay(this.transform.position + Vector3.up * Player_Y * 0.5f, Vector3.right * Player_X, Color.red);
                    //벽에 ray가 닿으면(벽에 닿으면) 이동 중량을 감소 시킨다
                    if (wall_ray.collider || wall_ray2.collider)
                    {
                        move_weight *= 0.05f;
                    }
                    else
                    {
                        move_weight = 1;
                    }



                }
                else if (lastest_key_input == Key_manager.Keys[Key_manager.KeyAction.LEFT])//왼쪽
                {
                    p_anim.move_state = true;
                    if (direction == 1)
                    {
                        direction_change();
                    }

                    direct_vector = new Vector2(-1, direct_vector.y);
                    if (move_weight != 0)
                        rgd.velocity = new Vector2(direction * move_speed * move_weight, rgd.velocity.y);

                    RaycastHit2D wall_ray = Physics2D.Raycast(this.transform.position + Vector3.up * Player_Y * 0.5f, Vector3.left, Player_X, LayerMask.GetMask("platform_can't_pass"));
                    RaycastHit2D wall_ray2 = Physics2D.Raycast(this.transform.position - Vector3.up * Player_Y * 0.85f, Vector3.left, Player_X, LayerMask.GetMask("platform_can't_pass"));
                    Debug.DrawRay(this.transform.position - Vector3.up * Player_Y * 0.85f, Vector3.left * Player_X, Color.red);
                    Debug.DrawRay(this.transform.position + Vector3.up * Player_Y * 0.5f, Vector3.left * Player_X, Color.red);
                    if (wall_ray.collider || wall_ray2.collider)
                    {

                        move_weight *= 0.05f;
                    }
                    else
                    {
                        move_weight = 1;
                    }

                    // SpriteRenderer s;

                }
                //일정시간 동안 멈추지 않고 이동 중이면 rush_timer가 차고 on_rush가 활성화 된다
                if (rush_timer < rush_time && !on_rush)
                {
                    rush_timer += Time.deltaTime;
                }
                else
                {
                    on_rush = true;
                    rush_timer = 0;
                }
                //on_rush가 활성화 중일 때 공격시 플레이어가 살짝 돌진하면서 공격
            }
            else
            {
                if (!on_dash)//대쉬 중일 때  x가속도 빠르게 감소
                    rgd.velocity = new Vector2(rgd.velocity.x * 0.65f, rgd.velocity.y);
                if (rgd.velocity.x < 1 && rgd.velocity.x > -1 && on_rush)//방향키를 때고 멈췄다면 on_rush초기화
                {
                    on_rush = false;
                }
                direct_vector = new Vector2(0, direct_vector.y);
                //이동에니메이션 멈추기
                p_anim.move_state = false;
            }
            //점프
            jump();

            //양방향 플랫폼위에서 공격,대쉬 중이 아닐때 아랫 방향키를 누르면 버퍼를 받는다
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]) && on_platform && Downbuffertimer <= 0 && !p_anim.sword_delay && !on_dash && !on_teleport)
            {
                Downbuffertimer = Downbuffertime;
            }
            if (Downbuffertimer > 0)
            {
                Downbuffertimer -= Time.deltaTime;
                //버퍼가 유지된 상태에서 한번더 아랫 방향키를 누르면 양방향 플랫폼 아래로 떨어질수 있는 코루틴이 작동한다
                if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]) && on_platform && Downbuffertimer >= 0 && !p_anim.sword_delay && !on_dash && !on_teleport)
                {

                    if (!on_corutine_1)
                        StartCoroutine(downplatform());
                }
            }

        }
        if (!Sp_ItemEffect.sp_itemeffect.Sp_Ef[7])// 텔레포트용 특정아이테미 흭득된 상태가 아니라면
        {
            if (!on_dash)//대쉬
            {
                dash();
            }
            else
            {
                //대쉬 중일 때는
                rgd.gravityScale = 0;//중력을 받지않음
                if (choose_ghost_effect != null)
                    choose_ghost_effect.transform.position = this.transform.position;//파티클 위치=플레이어 위치로 잔상처럼 보이게
                untouchable_state = true;//무적상태
                dash_timer -= Time.deltaTime;
                //짧은 시간 경과 후 대쉬 종료
                if (dash_timer <= 0)
                {
                    dash_end();
                }
            }
            //대쉬 횟수를 회복
            dash_recover();
        }
        else
        {
            //대쉬 대신 텔레포트
            set_teleport_direction();//텔레포트 방향을 정함
            if (!on_teleport)
            {
                teleport();//텔레포트
            }
            else
            {
                //텔레포트 중에는
                untouchable_state = true;//무적
                teleport_untouable_timer -= Time.deltaTime;
                if (teleport_untouable_timer <= 0)//무적시간이 끝나면 텔레포트 마무리
                {
                    teleport_end();
                }
            }
            //대쉬 횟수를 회복(텔레포트는 대쉬 횟수를 사용)
            dash_recover();
        }

    }

    void minimum_jump()//점프실행
    {
        jump_count--;//점프 횟수 감소
        //rgd.velocity = new Vector2(rgd.velocity.x, minimum_jump_vec3.y);
        //최소 점프 시간을 정한다
        minimum_jump_timer = minimum_jump_time;
        rgd.velocity = new Vector2(rgd.velocity.x, 0);
        //윗쪽 방향으로 addforce
        rgd.AddForce(new Vector2(rgd.velocity.x, Player_status.p_status.get_jump_force()), ForceMode2D.Impulse);
        raycheck = false;
    }
    public void death()//플레이어의 죽음을 처리
    {
        if (Gamemanager.GM.game_ev.is_player_death())//플레이어가 죽는지 부활하는지를 게임이벤트를 사용해 확인
        {
            Debug.Log("죽음!");
            death_check = true;


            record.Died++;//죽은 횟수기록에 더하기
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            Debug.Log("부활");
            p_anim.resurrection();//부활 에니메이션을 실행

        }
    }
    public void ressurection()//부활
    {

        //부활할 때 게임이벤트로 어떤 이유로 부활하는지 확인하고 다시 체력 회복)
        Player_status.p_status.set_hp(Gamemanager.GM.game_ev.resurrection_hp);
        //무적 2초
        untouchable_timer = 2f;
        //실행된 사망 에니메이션을 무효화한다
        death_anim_check = false;
        ////다시 플레이어를 조작 가능한 상태로 만든다
        Gamemanager.GM.can_handle = true;
    }
    //플레이어의 피격을 처리
    public void player_hitted(int dmg)
    {
        if (!untouchable_state && Player_status.p_status.get_hp() > 0)//무적이 아니며 체력이 남아있을 때
        {
            if (Player_status.p_status.get_hp() - dmg <= 0)//이 공격으로 플레이어가 죽는다면
            {
                //action_cam을 활성화시켜서 슬로우 +줌인 연출
                cameraManager.cm.active_action_cam();
                cameraManager.cm.action_cam.a = action_camera.action.p_death_cam;
            }
            int d = dmg;

            m_audio.attacked();
            //데미지 처리
            Player_status.p_status.damage_hp(d, DNP, this.gameObject.transform);
            //받은 피해량 기록
            record.Hit += d;
            //무적상태를 활성화
            if (!untouchable_state)
                untouchable_state = true;
        }
    }
    void untouchable()//무적 중
    {
        //untouchable_timer가 흐르고 무적이 해재된다
        if (untouchable_timer >= Player_status.p_status.get_untouchable_time())
        {
            Debug.Log("무적해제!");
            untouchable_state = false;
            untouchable_timer = 0;
        }
        untouchable_timer += Time.deltaTime;
    }
    //플레이어가 피격된 방향의 반대쪽으로 밀려남
    void player_hiited_force(Vector3 col_pos)//col_pos 피격시키게 만든 오브젝트 위치
    {
        if (!untouchable_state && Player_status.p_status.get_hp() > 0)
        {
            //플레이어가 피격됐음을 게임이벤트로 알림
            Gamemanager.GM.game_ev.when_player_hitted(transform);
            //충돌방향 설정
            Vector3 col_direct = transform.position - col_pos;
            int hit_direct;
            //충돌 방향의 반대 방향을 확인한다
            if (col_direct.x > 0)
            {
                hit_direct = -1;
            }
            else
            {
                hit_direct = 1;
            }
            //가속도 초기화+이동 공격 금지
            rgd.velocity = Vector2.zero;
            can_move = false;
            can_attack = false;
            //반대 방향으로 밀려난다
            rgd.AddForce(new Vector2(hitted_force.x * hit_direct, hitted_force.y), ForceMode2D.Impulse);
            on_hitted = true;
            p_anim.Hit_state = true;
            hitted_timer = hitted_time;
        }
    }
    //player_hiited_force(Vector3 col_pos)와 동일하나 Vector2 hitted를 이용해 어느 방향으로 밀려날지 설정 가능
    public void player_hiited_force(Vector3 col_pos, Vector2 hitted)
    {
        if (!untouchable_state && Player_status.p_status.get_hp() > 0)
        {
            Gamemanager.GM.game_ev.when_player_hitted(transform);
            Vector3 col_direct = transform.position - col_pos;
            int hit_direct;
            if (col_direct.x > 0)
            {
                hit_direct = 1;
            }
            else
            {
                hit_direct = -1;
            }
            rgd.velocity = Vector2.zero;
            can_move = false;
            //rgd.gravityScale = 1;
            rgd.AddForce(new Vector2(hitted.x * hit_direct, hitted.y), ForceMode2D.Impulse);
            on_hitted = true;
            p_anim.Hit_state = true;
            hitted_timer = hitted_time;
        }
    }

    void hitted_end()//피격 상태를 끝낸다
    {
        //뒤로밀려나는 것을 멈추고 다시 이동+공격 가능
        rgd.velocity = Vector2.zero;
        can_move = true;
        can_attack = true;

        on_hitted = false;
    }


    public void hitted_event(Vector2 v)
    {
        player_hiited_force(v);
    }
    public void hitted_event(Vector2 v, Vector2 f)
    {
        player_hiited_force(v, f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //대쉬,무적 중이 아닐때
        if (!on_dash)
        {
            if (!on_teleport)
            {
                if (collision.gameObject.CompareTag("Enemy"))//적에게 닿으면
                {
                    if (!untouchable_state)//무적 상태가 아닐 때
                    {
                        //Gamemanager에 기록중인 콤보를 멈춘다
                        Gamemanager.GM.stop_combo();
                        //플레이어를 뒤로 밀려나게 하고 데미지를 처리한다
                        col_pos = collision.transform.position;
                        player_hiited_force(col_pos);
                        player_hitted(collision.gameObject.GetComponentInParent<GameCharacter>().Attack_point);

                        Debug.Log("피격됨");

                    }
                }
            }
        }
        else
        {
            //대쉬 중에 적에 닿았음을 게임이벤트로 알린다(특정 아이템 처리)
            Gamemanager.GM.game_ev.On_Dash_col_effect(collision);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        //양방향 플랫폼 위에 있는지를 확인한다
        if (other.gameObject.CompareTag("Platform"))
        {
            if (other.gameObject.layer == 11)
            {
                on_platform = true;
            }

        }

        
    }
    //땅에 닿았을 때 처리
    public void groundcollision(GameObject a)
    {
        if (a.layer != 11)//일반 플랫폼
        {

              //점프 회복+대쉬 회복 타이머 작동+공중 공격 횟수 회복
                jump_count = Player_status.p_status.get_jump_count();
                dash_recover_check = true;
                onground = true;//착지 처리
            Player_status.p_status.air_attack_num = Player_status.p_status.air_attack_num_orignal + Player_status.p_status.air_attack_num_bonus;
                raycheck = true;

        }
        else//양방향 플랫폼
        {
            //양방향 플랫폼은 플레이어가 플랫폼을 통과하기 때문에 따로 처리
            if (rgd.velocity.y<1&&rgd.velocity.y>-1)//벨로시티의 절대 값이 작을 때=양방향 플랫폼에 거의 착지했다면
            {

                //점프 회복+대쉬 회복 타이머 작동+공중 공격 횟수 회복
                jump_count = Player_status.p_status.get_jump_count();
                    dash_recover_check = true;
                    onground = true;
                Player_status.p_status.air_attack_num = Player_status.p_status.air_attack_num_orignal + Player_status.p_status.air_attack_num_bonus;
                raycheck = true;
                
            }



        }
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform")&&other.gameObject.layer!=11)
        {
            this.gameObject.layer = 6;
           // this.GetComponent<player_room_boost_mode>().un_boost();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (collision.gameObject.layer == 11)
            {
                on_platform = false;
            }
        }
    }
    //양방향 플랫폼 아래로 내려가는 코루틴
    IEnumerator downplatform()
    { 
        var wait = new WaitForSeconds(layer_change_time);
        //양방향 플랫폼을 통과할 수 있는 레이어로 변경
        on_corutine_1 = true;
        this.gameObject.layer = 10;
        yield return wait;
       //짧은 시간 대기 후 다시 원래 레이어로 돌아오기
        on_corutine_1 = false;
        this.gameObject.layer = 6;
        Debug.Log("코루틴해제");
    }

    //자신이 작업하지 않음
    void AE_Teleport()
    {




        Instantiate(m_teleport_effect, this.transform.position, Quaternion.identity);


    }
    //자신이 작업하지 않음
    public void SpawnDustEffect(GameObject dust, float dustXOffset = 0, float dustYOffset = 0)
    {
        if (dust != null)
        {
            
            // Set dust spawn position
            Vector3 dustSpawnPosition = transform.position-Vector3.up*0.2f + new Vector3(dustXOffset * direction, dustYOffset, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            // Turn dust in correct X direction
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(direction, 1, 1);
        }
    }


}



