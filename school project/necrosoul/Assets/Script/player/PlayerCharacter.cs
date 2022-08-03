using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : GameCharacter//�÷��̾� ����+���� Ŭ����(���� ���� ����ü�� �⺻ ��ȣ������ ���Ե�GameCharacter�� ���) 
{
    public Rigidbody2D rgd;
    public int jump_count = 1;
    public bool onground;//���� ��EҴ���E

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
    //�����+���+��Ʈ ���õ� ���� �ڽ��� �۾��Ѱ��� �ƴ�
    void Start()
    {
        //�ʱⰪ ����
        can_attack = true;//���� ���� ����
        m_audio = AudioManage_Main.instance;//�����
        a = this.GetComponent<Attack>();
        dash_ghost_effect.gameObject.SetActive(false);
        record = GameObject.Find("GameSystem").GetComponent<Record>().ActionRecord;//���
        p_anim = gameObject.GetComponentInChildren<Player_animator>();

        DNP = GameObject.Find("DemoManager");//��Ʈ
        Player_status.p_status.set_layout();//�ʱ�ɷ�ġ ����
        rgd = gameObject.GetComponent<Rigidbody2D>();
        jump_count = Player_status.p_status.get_jump_count();
        spawn_check = true;
        direction = 1;
        can_move = true;
        for (int i = 0; i < 3; i++)//Ǯ����ų �뽬�ܻ��� ����
        {
            var a = Instantiate(dash_ghost_effect.gameObject, this.transform.position, Quaternion.identity);
            dash_ghost_effect_instansi.Add(a);
            a.transform.SetParent(this.transform);
            a.SetActive(false);
        }
        for (int i = 0; i < 3; i++)//Ǯ����ų ȸ�� ����Ʈ ��ƼŬ ����
        {
            var a = Instantiate(Gamemanager.GM.heal_cross_particle.gameObject, this.transform.position, Quaternion.identity);
            heal_cross_instansi.Add(a);
            a.transform.SetParent(this.transform);
            a.SetActive(false);
        }
        if (player_pos == null)//�÷��̾��� ��ġ�� �̴ϸʿ� ����
        {
            GameObject ins = Instantiate(Gamemanager.GM.player_minimap_pos);//�̴ϸʿ� �÷��̾��� ��ġ�� �˸��� ������Ʈ�� �÷��̾��� ��ġ�� �����Ѵ�
            ins.transform.position = this.transform.position;
            ins.transform.SetParent(this.transform);
        }
    }
    GameObject player_heal_cross_pulling()//ȸ������Ʈ Ǯ��
    {
        //����Ʈ���� ��Ȱ��ȭ �� ������Ʈ��ȯ
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
    public void Player_heal_cross_particle(int n)//ȸ�� ����Ʈ�� Ȱ��ȭ ��Ų��
    {
        var obj = player_heal_cross_pulling();
        heal_cross_particle a = obj.GetComponent<heal_cross_particle>();
        a.n = n;
        obj.transform.position = this.transform.position;
        obj.SetActive(true);
    }
    //�÷��̾� �뽬 �ܻ� Ǯ��
    GameObject player_ghost_pulling()
    {
        //����Ʈ���� ��Ȱ��ȭ �� ������Ʈ ��ȯ
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
    public void set_col_boost(bool a)//�ν��� set()(������)
    {
        col_on_room_boost = a;
    }
    public bool get_col_boost()//�ν��� get()(������)
    {
        return col_on_room_boost;
    }
    //�뽬 ���� ���� ����
    public void set_can_dash(bool a)
    {
        can_dash = a;
    }
    //�̵� ���� ���� ����
    public void set_can_move(bool a)
    {
        can_move = a;
    }
    //�÷��̾ ī�޶� �ۿ��� ����� �ʵ��� �Ѵ�
    void player_camera_fitting()
    {
        viewport_pos = Camera.main.WorldToViewportPoint(transform.position);//����Ʈ�� �÷��̾�����ġ ��ȯ
        //ī�޶� ������� �� �� ��ġ�� ����
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
        //�÷��̾��� �Ʒ� �������� �÷����� Ȯ���ϴ� ray�� ���(�÷����� �������� 2��)
        down_ray_1 = Physics2D.Raycast(this.transform.position + (Vector3.down * (Player_Y - 0.1f)), Vector2.down, 0.1f, LayerMask.GetMask("platform_can't_pass"));
        down_ray_2 = Physics2D.Raycast(this.transform.position + (Vector3.down * (Player_Y - 0.1f)), Vector2.down, 0.1f, LayerMask.GetMask("platform_can_pass"));
        //ä��,�����,�̵��ӵ��� Player_statusŬ������ ���� ���󰣴�
        max_hp = Player_status.p_status.get_max_hp();
        Health_point = Player_status.p_status.get_hp();
        Defense_point = Player_status.p_status.get_defense_point();
        move_speed = Player_status.p_status.get_speed();



        //ü��ui (�ڽ��� �۾���������)
        if (HpBar != null)
        {
            HpBar.SetMaxTextValue(Health_point, max_hp);
            HpBar.SetValue(Health_point, max_hp);
        }

        //���߿� �ִ��� ���� Ȯ��
        p_anim.set_ground(onground);

        if (Gamemanager.GM.can_handle || !!death_anim_check)//���� ������ �����̸� ��� ���ϸ��̼� �������� �ƴ� ��
        {

            character_move();//�̵�
            //ī�޶� ���������
            if (!Gamemanager.GM.fade_Outit.activeSelf && Player_status.p_status.get_hp() > 0)//���̵� �ƿ����� �ƴҶ�+ü���� 0���ϰ� �ƴ� ��
                player_camera_fitting();

            //�÷���üũ ��Eray�� �浹 ���̶�� ���� ������ Ȯ��
            if (down_ray_1.collider || down_ray_2.collider)
            {
                landing_chk = true;
            }
            else
            {
                landing_chk = false; ;
            }

            //untouchable_state�� true�� �� ��������
            if (untouchable_state)
                untouchable();

            //ü���� 0�����̸��÷��̾ ������ ������ ���� ���¶��
            if (Health_point <= 0 && spawn_check)
            {

                if (!death_anim_check)
                {
                    //��� ���ϸ��̼��� �����ϰ� ������ ��Ȱ��ȭ
                    death_anim_check = true;
                    p_anim.death_state = true; //��� ó��
                    Gamemanager.GM.can_handle = false;
                }



            }
            //�뽬�� �Ұ��� �ϴٸ�(�뽬 �� ������)
            if (!can_dash)
            {
                //after_dash_timer���� �ٽ� �뽬 ����
                after_dash_timer += Time.deltaTime;
                if (after_dash_timer >= after_dash_timer_check)
                {

                    can_dash = true;
                    after_dash_timer = 0;
                }
            }
            //velocity.y���� 0�� �ƴ϶�� ���߿� �ִ�
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
            //���߿� �ִٸ�
            if (!onground)
            {
                //hangTimer �۵�
                if (hangTimer > 0)
                    hangTimer -= Time.deltaTime;
            }
            else//�ƴ϶��
            {
                //hangTimer ����
                hangTimer = hangtime;
                direct_vector = new Vector2(direct_vector.x, 0);

            }
            //hangTimer�� ����� ���¶�� ������ �۵�
            //�÷������� �������� �Ѿ���� ª�� �ð����� ������ �Ҽ��ִ� ���� �ð��� ����(���� ���� ��ȭ)

            //�ǰ� ���� ���¶��
            if (on_hitted)
            {
                //hitted_timer �� �ǰ� ���� ����
                hitted_timer -= Time.deltaTime;
                if (hitted_timer <= 0)
                {
                    hitted_end();
                }
            }
            //�ǰ� ���� ������ �� �÷��̾� ĳ���Ͱ� �ڷ� �з����� ������ �Ұ��� �ϴ�


            if (rgd.velocity.y <= 0.1 && !onground)//ĳ���Ͱ� �������� ���� ��
            {

                //�÷���üũ��Eray�� �������
                //ray�� �÷��̾� ��������Ʈ �߽������� �Ʒ��� ����ϸ� ���̴� �÷��̾��� ��������Ʈ�� y���� ���ݺ��� �ణ���(������ȭ)
                if (down_ray_1.collider)//�Ϲ� �÷���������� �÷���
                {
                    groundcollision(down_ray_1.transform.gameObject);//���� ȸ��


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
    void jump()//����
    {
        //�뽬 ���°� �ƴ� �� ���� Ű�� ���ȴٸ餤
        if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.JUMP]) && !p_anim.sword_delay && !on_dash && !on_teleport)
        {
            jumpbuffertimer = jumpbuffertime;//���� ���۸� �۵�
        }
        else
        {
            //���� ������ ���°� �ƴ϶�� ������ �ð��� ��������
            if (jumpbuffertimer > 0)
            {
                jumpbuffertimer -= Time.deltaTime;
            }
        }
        //���������� ����(���� Ƚ���� 0�� �ƴ�+�������� �ƴ�+�뽬 ���� �ƴ�+hangTimer�� 0���ϰ� �ƴ�)���� ���۰� �۵����ִٸ�
        if (jumpbuffertimer > 0 && jump_count == Player_status.p_status.get_jump_count() && hangTimer > 0 && !p_anim.sword_delay && !on_dash && !on_teleport)//����Ƚ���� ����E���� �� ��
        {
            //���۸� �ʱ�ȭ�ϰ� ���� ����
            jumpbuffertimer = 0;
            minimum_jump();
            p_anim.jump_anim();
            //jump();
        }
        else if (jumpbuffertimer > 0 && jump_count != 0 && !p_anim.sword_delay && !on_dash && !on_teleport)//���� Ƚ���� ���� �����϶�
        {
            jumpbuffertimer = 0;
            minimum_jump();
            p_anim.jump_anim();
            //jump();
        }
        if (minimum_jump_timer > 0)//�ּ� ���� ����(����Ű�� �ƹ��� ª�� ������ �⺻ ���̱��� �ö��)
        {
            minimum_jump_timer -= Time.deltaTime;
        }
        //����Ű�� ��� ������ ���� ���̰� �������� ª�� ������ ���� ���̰� ��������
        //���� Ű�� ������ ���� �ƴ϶�� velocity.y���� ������ ����
        if (!Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.JUMP]) && rgd.velocity.y > 0 && !on_dash && minimum_jump_timer <= 0 && !on_teleport)
        {

            jumpbuffertimer = 0;
            rgd.velocity = new Vector2(rgd.velocity.x, rgd.velocity.y * 0.5f);
        }
    }
    void dash_recover()//�뽬 Ƚ���� ȸ�� �Ѵ�
    {
        //�뽬 Ƚ���� �Ҹ�Ǿ��� ��
        if (dash_count != Player_status.p_status.get_dash_count())
        {
            dash_recover_timer += Time.deltaTime;
            //dash_recover_timer �� �� ȸ��
            if (dash_recover_timer >= Player_status.p_status.get_dash_recover_time())
            {
                dash_count = Player_status.p_status.get_dash_count();
                dash_recover_timer = 0;
            }


        }
    }
    //�ڷ���Ʈ(Ư�������� ŉ��� �뽬 ��� ���
    public void teleport()
    {
        //�뽬Ƚ���� �������鼭 �뽬�Ҽ��ִ� �����϶� �뽬Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.LeftShift) && dash_count != 0 && can_dash && can_move)
        {
            Debug.Log("�ڷ���Ʈ");
            //�÷��̾� ���ӵ� �ʱ�ȭ
            rgd.velocity = Vector2.zero;
            m_audio.teleport();


            untouchable_state = true;
            on_teleport = true;
            //�ڷ���Ʈ �� �Ͻ� ����
            teleport_untouable_timer = teleport_untouable_time;
            //�ڷ���Ʈ �� ��ġ�� �ޱ����� ray
            RaycastHit2D teleport_ray = Physics2D.Raycast(this.transform.position, Vector2.right, teleport_length, LayerMask.GetMask("platform_can't_pass")); ;
            switch (teleport_direction)//teleport_direction�� ����ray�� ������ �޶�����(teleport_direction�� �������ִ� �̵� ����Ű ����Ʈ ��:0)
            {
                //��Ȯ�� ������ ���� ray�� ��ġ�� ���⿡ ���������Ѵ�
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
            if (teleport_ray.collider != null)//ray�� �浹 ���� ��->��ֹ��� ������ ��
            {
                switch (teleport_direction)
                {
                    //�浹 ��ġ �ٷ� �տ� �÷��̾ �ڷ���Ʈ �� �� �ֵ��� ��ġ�� �����Ѵ�
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
            {//ray�� �浹�����ʾҴ�->�ڷ���Ʈ �ϴ� �������� ��ֹ��� ����->�� ��ġ���� �޴´�
                switch (teleport_direction)
                {
                    //�÷��̾� ��ġ���� teleport_length�� ���⿡ �°� ���� ���͸� ��ġ�� ��´�
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
            //������ �ڷ���Ʈ�� ���� ������Eteleport_pos�� ���⿡�°� �ι����� ray�� ���
            //�� ray�� ����� �÷��̾ �ڷ���Ʈ �� ���ִ� ������ Ȯ���� �Ǿ��ִ����� üũ�Ѵ�
            RaycastHit2D chk_ray1 = Physics2D.Raycast(teleport_pos, Vector2.up, Player_Y * 0.5f, LayerMask.GetMask("platform_can't_pass"));
            RaycastHit2D chk_ray2 = Physics2D.Raycast(teleport_pos, Vector2.down, Player_Y * 0.5f, LayerMask.GetMask("platform_can't_pass"));
            switch (teleport_direction)
            {
                //���� �ڷ���Ʈ->�� �Ʒ��� ray�� ���
                case 0:
                case 1:
                    chk_ray1 = Physics2D.Raycast(teleport_pos, Vector2.up, Player_Y * 0.5f, LayerMask.GetMask("platform_can't_pass"));
                    chk_ray2 = Physics2D.Raycast(teleport_pos, Vector2.down, Player_Y * 0.5f, LayerMask.GetMask("platform_can't_pass"));
                    break;
                //���� �ڷ���Ʈ->�¿�� ray�� ���
                case 2:
                case 3:
                    chk_ray1 = Physics2D.Raycast(teleport_pos, Vector2.right, Player_X * 0.5f, LayerMask.GetMask("platform_can't_pass"));
                    chk_ray2 = Physics2D.Raycast(teleport_pos, Vector2.left, Player_X * 0.5f, LayerMask.GetMask("platform_can't_pass"));
                    break;
            }


            //�� ray�� ��� �浹 �Ǹ� ������ ���� �����
            if (!chk_ray1.collider && !chk_ray2.collider)
            {
                //�� ��ġ �״���ڷ���Ʈ
                transform.position = (Vector3)teleport_pos + (Vector3.forward * this.transform.position.z);
            }
            //�� ray �� �ϳ��� �浹�ɰ�� �� ray�� �������� �÷��̾��� ��ġ ���� ������ ���ϸ� �÷��̾��� ��ġ�� Ȯ���Ҽ��ִ�
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
            else//�� ray��� �浹�� ��� �÷��̾ �ڷ���Ʈ�� ������ ����
            {
                //�÷��̾��� �����ڸ��� ���� ray�� �߻� ��� �κ��� �����ڸ��� ������ teleport_direction�� �������
                //�����ڸ��� �浹�� ��ġ�� �޾Ƽ� �ڷ���Ʈ ��ǥ�� ���
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
                //�ڷ���Ʈ
                transform.position = (Vector3)teleport_pos + (Vector3.forward * this.transform.position.z);
            }
            //�ڷ���Ʈ �׼� �߿��� ���� �Ұ� 
            AE_Teleport();
            can_attack = false;
            dash_count--;
            //ª�� ������ ���� ���ݺҰ�
            can_dash = false;
            dash_recover_check = false;
        }
    }
    //�ڷ���Ʈ �׼��� ������ ��
    void teleport_end()
    {
        //���� ����,
        can_attack = true;
        untouchable_state = false;
        on_teleport = false;
    }
    //�뽬->Ư�� Ű�� ������ ���������� �ٶ󺸰� �ִ� �������� ���� ������ �޴´� �뽬 �߿��� ����
    void dash()
    {
        //�뽬Ƚ���� �������� �뽬�Ҽ��ִ� �����϶� �뽬Ű������ ��
        if (Input.GetKeyDown(KeyCode.LeftShift) && dash_count != 0 && can_dash)
        {
            //�����̺�Ʈ Ŭ������ �뽬Ű�� ������ �˸�
            Gamemanager.GM.game_ev.when_dash_key_input(transform);

            m_audio.Dash();
            rgd.velocity = Vector2.zero;
            //�뽬 �߿��� ����X+ ��������
            can_attack = false;
            untouchable_state = true;
            p_anim.dash();//�뽬���ϸ��̼� ����

            //�뽬�߿��� �뽬�� �ܻ�(��ƼŬ)�� �����
            make_dash_ghost();
            on_dash = true;
            //dash_timer ����
            dash_timer = dash_time;


            //dash_direction->�÷��̾ �ٶ󺸰� �ִ� ����
            dash_direction = Vector2.right * direction * dash_force;
            //ForceMode2D.Impulse�� �̿��� ���������� ������ �޴´�
            rgd.AddForce(dash_direction * Time.deltaTime, ForceMode2D.Impulse);
            //�뽬Ƚ������ �����ð����� �뽬 ����
            dash_count--;
            can_dash = false;
            dash_recover_check = false;
        }

    }
    //�뽬 �ܻ����
    void make_dash_ghost()
    {
        //Ǯ�� ����Ʈ��  ���� ��Ȱ��ȭ �� �뽬�ܻ��� ������ Ȱ��ȭ ��Ų��
        var a = player_ghost_pulling();
        choose_ghost_effect = a;
        a.SetActive(true);

    }

    void dash_end()//�뽬 ���� ����
    {
        //�����̺�Ʈ Ŭ���¿� �뽬�� �������� �˸�
        Gamemanager.GM.game_ev.Dash_End_effect();
        //���� ����
        rgd.velocity = Vector2.zero;
        //���� ���� ����+�̵� ����
        untouchable_state = false;
        p_anim.dash_end();
        can_attack = true;
        can_move = true;
        //�ٽ� �߷��� �޴´�
        rgd.gravityScale = 1;
        on_dash = false;
    }
    //������ �ִ� ����Ű�� ���� teleport_direction�� ���� ���Ѵ�
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
    //ĳ���� �̵�,����,�뽬,���� ����
    new void character_move()
    {

        if (can_move && hitted_timer <= 0)//������ �� �ְ� �ǰݴ��� ���°� �ƴ϶��
        {

            //���������� ������ �ִ� �¿����Ű(lastest_key_input)�� �޴´�
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

            //����Ű �¿� �� �� �ϳ��� ������ ���¿��� �ٸ� ������ ������ �� ������ ���������� ���� ����Ű(lastest_key_input)�� ���Ѵ�
            if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.RIGHT]) && !Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.LEFT])) {
                lastest_key_input = Key_manager.Keys[Key_manager.KeyAction.RIGHT];
            } else if (!Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.RIGHT]) && Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.LEFT]))
            {
                lastest_key_input = Key_manager.Keys[Key_manager.KeyAction.LEFT];
            }
            //�¿� ����Ű�� ������ �ִٸ�
            if ((Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.RIGHT]) || Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.LEFT]) ||
                    Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.RIGHT]) || Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.LEFT])) && !p_anim.sword_delay && !on_dash && !on_teleport && !Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.ATTACK]))
            {
                //���������� ���� ����Ű(lastest_key_input)�� ���� �¿��� ��� �������� �������� ���Ѵ�
                if (lastest_key_input == Key_manager.Keys[Key_manager.KeyAction.RIGHT])//������
                {
                    p_anim.move_state = true;
                    if (direction == -1)//�ٶ󺸰� �ִ� ������ �ݴ��� ���� ����
                    {
                        direction_change();
                    }

                    if (move_weight != 0)//�̵��߷��� 0�� �ȴ϶��
                        rgd.velocity = new Vector2(direction * move_speed * move_weight, rgd.velocity.y);//������ �������� addforce
                    //��üũ�� ray
                    RaycastHit2D wall_ray = Physics2D.Raycast(this.transform.position + Vector3.up * Player_Y * 0.5f, Vector3.right, Player_X, LayerMask.GetMask("platform_can't_pass"));
                    RaycastHit2D wall_ray2 = Physics2D.Raycast(this.transform.position - Vector3.up * Player_Y * 0.85f, Vector3.right, Player_X, LayerMask.GetMask("platform_can't_pass"));
                    Debug.DrawRay(this.transform.position - Vector3.up * Player_Y * 0.85f, Vector3.right * Player_X, Color.red);
                    Debug.DrawRay(this.transform.position + Vector3.up * Player_Y * 0.5f, Vector3.right * Player_X, Color.red);
                    //���� ray�� ������(���� ������) �̵� �߷��� ���� ��Ų��
                    if (wall_ray.collider || wall_ray2.collider)
                    {
                        move_weight *= 0.05f;
                    }
                    else
                    {
                        move_weight = 1;
                    }



                }
                else if (lastest_key_input == Key_manager.Keys[Key_manager.KeyAction.LEFT])//����
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
                //�����ð� ���� ������ �ʰ� �̵� ���̸� rush_timer�� ���� on_rush�� Ȱ��ȭ �ȴ�
                if (rush_timer < rush_time && !on_rush)
                {
                    rush_timer += Time.deltaTime;
                }
                else
                {
                    on_rush = true;
                    rush_timer = 0;
                }
                //on_rush�� Ȱ��ȭ ���� �� ���ݽ� �÷��̾ ��¦ �����ϸ鼭 ����
            }
            else
            {
                if (!on_dash)//�뽬 ���� ��  x���ӵ� ������ ����
                    rgd.velocity = new Vector2(rgd.velocity.x * 0.65f, rgd.velocity.y);
                if (rgd.velocity.x < 1 && rgd.velocity.x > -1 && on_rush)//����Ű�� ���� ����ٸ� on_rush�ʱ�ȭ
                {
                    on_rush = false;
                }
                direct_vector = new Vector2(0, direct_vector.y);
                //�̵����ϸ��̼� ���߱�
                p_anim.move_state = false;
            }
            //����
            jump();

            //����� �÷��������� ����,�뽬 ���� �ƴҶ� �Ʒ� ����Ű�� ������ ���۸� �޴´�
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]) && on_platform && Downbuffertimer <= 0 && !p_anim.sword_delay && !on_dash && !on_teleport)
            {
                Downbuffertimer = Downbuffertime;
            }
            if (Downbuffertimer > 0)
            {
                Downbuffertimer -= Time.deltaTime;
                //���۰� ������ ���¿��� �ѹ��� �Ʒ� ����Ű�� ������ ����� �÷��� �Ʒ��� �������� �ִ� �ڷ�ƾ�� �۵��Ѵ�
                if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.DOWN]) && on_platform && Downbuffertimer >= 0 && !p_anim.sword_delay && !on_dash && !on_teleport)
                {

                    if (!on_corutine_1)
                        StartCoroutine(downplatform());
                }
            }

        }
        if (!Sp_ItemEffect.sp_itemeffect.Sp_Ef[7])// �ڷ���Ʈ�� Ư�������׹� ŉ��� ���°� �ƴ϶��
        {
            if (!on_dash)//�뽬
            {
                dash();
            }
            else
            {
                //�뽬 ���� ����
                rgd.gravityScale = 0;//�߷��� ��������
                if (choose_ghost_effect != null)
                    choose_ghost_effect.transform.position = this.transform.position;//��ƼŬ ��ġ=�÷��̾� ��ġ�� �ܻ�ó�� ���̰�
                untouchable_state = true;//��������
                dash_timer -= Time.deltaTime;
                //ª�� �ð� ��� �� �뽬 ����
                if (dash_timer <= 0)
                {
                    dash_end();
                }
            }
            //�뽬 Ƚ���� ȸ��
            dash_recover();
        }
        else
        {
            //�뽬 ��� �ڷ���Ʈ
            set_teleport_direction();//�ڷ���Ʈ ������ ����
            if (!on_teleport)
            {
                teleport();//�ڷ���Ʈ
            }
            else
            {
                //�ڷ���Ʈ �߿���
                untouchable_state = true;//����
                teleport_untouable_timer -= Time.deltaTime;
                if (teleport_untouable_timer <= 0)//�����ð��� ������ �ڷ���Ʈ ������
                {
                    teleport_end();
                }
            }
            //�뽬 Ƚ���� ȸ��(�ڷ���Ʈ�� �뽬 Ƚ���� ���)
            dash_recover();
        }

    }

    void minimum_jump()//��������
    {
        jump_count--;//���� Ƚ�� ����
        //rgd.velocity = new Vector2(rgd.velocity.x, minimum_jump_vec3.y);
        //�ּ� ���� �ð��� ���Ѵ�
        minimum_jump_timer = minimum_jump_time;
        rgd.velocity = new Vector2(rgd.velocity.x, 0);
        //���� �������� addforce
        rgd.AddForce(new Vector2(rgd.velocity.x, Player_status.p_status.get_jump_force()), ForceMode2D.Impulse);
        raycheck = false;
    }
    public void death()//�÷��̾��� ������ ó��
    {
        if (Gamemanager.GM.game_ev.is_player_death())//�÷��̾ �״��� ��Ȱ�ϴ����� �����̺�Ʈ�� ����� Ȯ��
        {
            Debug.Log("����!");
            death_check = true;


            record.Died++;//���� Ƚ����Ͽ� ���ϱ�
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            Debug.Log("��Ȱ");
            p_anim.resurrection();//��Ȱ ���ϸ��̼��� ����

        }
    }
    public void ressurection()//��Ȱ
    {

        //��Ȱ�� �� �����̺�Ʈ�� � ������ ��Ȱ�ϴ��� Ȯ���ϰ� �ٽ� ü�� ȸ��)
        Player_status.p_status.set_hp(Gamemanager.GM.game_ev.resurrection_hp);
        //���� 2��
        untouchable_timer = 2f;
        //����� ��� ���ϸ��̼��� ��ȿȭ�Ѵ�
        death_anim_check = false;
        ////�ٽ� �÷��̾ ���� ������ ���·� �����
        Gamemanager.GM.can_handle = true;
    }
    //�÷��̾��� �ǰ��� ó��
    public void player_hitted(int dmg)
    {
        if (!untouchable_state && Player_status.p_status.get_hp() > 0)//������ �ƴϸ� ü���� �������� ��
        {
            if (Player_status.p_status.get_hp() - dmg <= 0)//�� �������� �÷��̾ �״´ٸ�
            {
                //action_cam�� Ȱ��ȭ���Ѽ� ���ο� +���� ����
                cameraManager.cm.active_action_cam();
                cameraManager.cm.action_cam.a = action_camera.action.p_death_cam;
            }
            int d = dmg;

            m_audio.attacked();
            //������ ó��
            Player_status.p_status.damage_hp(d, DNP, this.gameObject.transform);
            //���� ���ط� ���
            record.Hit += d;
            //�������¸� Ȱ��ȭ
            if (!untouchable_state)
                untouchable_state = true;
        }
    }
    void untouchable()//���� ��
    {
        //untouchable_timer�� �帣�� ������ ����ȴ�
        if (untouchable_timer >= Player_status.p_status.get_untouchable_time())
        {
            Debug.Log("��������!");
            untouchable_state = false;
            untouchable_timer = 0;
        }
        untouchable_timer += Time.deltaTime;
    }
    //�÷��̾ �ǰݵ� ������ �ݴ������� �з���
    void player_hiited_force(Vector3 col_pos)//col_pos �ǰݽ�Ű�� ���� ������Ʈ ��ġ
    {
        if (!untouchable_state && Player_status.p_status.get_hp() > 0)
        {
            //�÷��̾ �ǰݵ����� �����̺�Ʈ�� �˸�
            Gamemanager.GM.game_ev.when_player_hitted(transform);
            //�浹���� ����
            Vector3 col_direct = transform.position - col_pos;
            int hit_direct;
            //�浹 ������ �ݴ� ������ Ȯ���Ѵ�
            if (col_direct.x > 0)
            {
                hit_direct = -1;
            }
            else
            {
                hit_direct = 1;
            }
            //���ӵ� �ʱ�ȭ+�̵� ���� ����
            rgd.velocity = Vector2.zero;
            can_move = false;
            can_attack = false;
            //�ݴ� �������� �з�����
            rgd.AddForce(new Vector2(hitted_force.x * hit_direct, hitted_force.y), ForceMode2D.Impulse);
            on_hitted = true;
            p_anim.Hit_state = true;
            hitted_timer = hitted_time;
        }
    }
    //player_hiited_force(Vector3 col_pos)�� �����ϳ� Vector2 hitted�� �̿��� ��� �������� �з����� ���� ����
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

    void hitted_end()//�ǰ� ���¸� ������
    {
        //�ڷιз����� ���� ���߰� �ٽ� �̵�+���� ����
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
        //�뽬,���� ���� �ƴҶ�
        if (!on_dash)
        {
            if (!on_teleport)
            {
                if (collision.gameObject.CompareTag("Enemy"))//������ ������
                {
                    if (!untouchable_state)//���� ���°� �ƴ� ��
                    {
                        //Gamemanager�� ������� �޺��� �����
                        Gamemanager.GM.stop_combo();
                        //�÷��̾ �ڷ� �з����� �ϰ� �������� ó���Ѵ�
                        col_pos = collision.transform.position;
                        player_hiited_force(col_pos);
                        player_hitted(collision.gameObject.GetComponentInParent<GameCharacter>().Attack_point);

                        Debug.Log("�ǰݵ�");

                    }
                }
            }
        }
        else
        {
            //�뽬 �߿� ���� ������� �����̺�Ʈ�� �˸���(Ư�� ������ ó��)
            Gamemanager.GM.game_ev.On_Dash_col_effect(collision);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        //����� �÷��� ���� �ִ����� Ȯ���Ѵ�
        if (other.gameObject.CompareTag("Platform"))
        {
            if (other.gameObject.layer == 11)
            {
                on_platform = true;
            }

        }

        
    }
    //���� ����� �� ó��
    public void groundcollision(GameObject a)
    {
        if (a.layer != 11)//�Ϲ� �÷���
        {

              //���� ȸ��+�뽬 ȸ�� Ÿ�̸� �۵�+���� ���� Ƚ�� ȸ��
                jump_count = Player_status.p_status.get_jump_count();
                dash_recover_check = true;
                onground = true;//���� ó��
            Player_status.p_status.air_attack_num = Player_status.p_status.air_attack_num_orignal + Player_status.p_status.air_attack_num_bonus;
                raycheck = true;

        }
        else//����� �÷���
        {
            //����� �÷����� �÷��̾ �÷����� ����ϱ� ������ ���� ó��
            if (rgd.velocity.y<1&&rgd.velocity.y>-1)//���ν�Ƽ�� ���� ���� ���� ��=����� �÷����� ���� �����ߴٸ�
            {

                //���� ȸ��+�뽬 ȸ�� Ÿ�̸� �۵�+���� ���� Ƚ�� ȸ��
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
    //����� �÷��� �Ʒ��� �������� �ڷ�ƾ
    IEnumerator downplatform()
    { 
        var wait = new WaitForSeconds(layer_change_time);
        //����� �÷����� ����� �� �ִ� ���̾�� ����
        on_corutine_1 = true;
        this.gameObject.layer = 10;
        yield return wait;
       //ª�� �ð� ��� �� �ٽ� ���� ���̾�� ���ƿ���
        on_corutine_1 = false;
        this.gameObject.layer = 6;
        Debug.Log("�ڷ�ƾ����");
    }

    //�ڽ��� �۾����� ����
    void AE_Teleport()
    {




        Instantiate(m_teleport_effect, this.transform.position, Quaternion.identity);


    }
    //�ڽ��� �۾����� ����
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



