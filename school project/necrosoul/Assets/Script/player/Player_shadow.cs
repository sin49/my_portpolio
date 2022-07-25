using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_shadow : MonoBehaviour//�÷��̾��� ȯ��,�׸��� Ŭ����(Ʃ�丮��,�̴� ���� ����Ʈ ������ ����Ѵ�)
{
    public PlayerCharacter p;
    public bool p_created;
    public int attack_num;
    public Vector2 dash_direction;
    public float dash_time = 1f;
    public float dash_timer = 0;
    public Player_shadow_animator p_anim;
    public float dash_force;
    public int animation_level;
    public bool shadow_type;
    public float shadow_time;//���� �ð�
    private bool on_dash;
    private Vector3 move_vector;
    private Vector2 direct_vector;
    private float move_speed;
    private float move_weight;
    public int direction;
    
    private int jump_count;
    private bool dash_recover_check;
    private Rigidbody2D rgd;
    private bool raycheck;
    private bool onground;
    private int air_attack_num;
    private float jumpbuffertimer;
    private float hangTimer;
    private float jumpbuffertime;
    private bool on_corutine_1;
    private bool on_platform;
    Color c;
    float c_alpha;
    public float shadow_original_timer;
    public float Downbuffertime;
    public float Downbuffertimer;
    public bool once_chk;
    public bool anim_chk;
    SpriteRenderer s_r;
    public float Player_Y;
    internal bool can_move;
    float once_timer;
    public void Start()
    {
        //ȯ���� rgba ���� �����Ѵ�
        s_r = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        c = s_r.color;
        c_alpha = c.a;
        p_anim = this.transform.GetChild(0).GetComponent<Player_shadow_animator>();
        rgd = this.GetComponent<Rigidbody2D>();
        // make_stat();
    }
    public void make_stat()//ȯ���� �ɷ�ġ�� �����Ѵ�
    {
        
        if(p==null)
            p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        Player_Y = p.Player_Y;
        dash_time = p.dash_time;
        dash_force = p.dash_force;
        move_speed = p.move_speed;
        direction = 1;
        /*if (p.direction == -1)
            direction_change();*/
        jump_count = 1;

    }
    private void FixedUpdate()
    {
        
        if (p == null)//�÷��̾ �������ִٸ� �÷��̾��� �ɷ�ġ�� ���󰣴�
        {
            make_stat();
        }
        if (p_created)//�÷��̾ ���� ������ٸ� �÷��̾� ��ġ�� �ִ´�
        {
            this.transform.position = p.transform.position;
        }
        if (on_dash)//�뽬�߿��� �߷��� ���� �ʴ´�
        {
            dash_timer -= Time.deltaTime;
            rgd.gravityScale = 0;
            if (dash_timer < 0)
            {
                dash_end();
               
            }
        }

        //shadow_type�� ���� ���� �ΰ��� �������� ������
        //true�� ��� ���ӽð��� ������ �ڵ����� �ı��ȴ�
        //false�� ��� ���ӽð��� �������� �ʴ´�
        if (shadow_type)
        {
            if (shadow_time >= 0)//shadow_time ���� ����
            {
                c.a =c_alpha* shadow_time / shadow_original_timer;//���� �帴������
                s_r.color = c;
                shadow_time -= Time.deltaTime;
                animation_work();
            }
            else
            {
                this.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            animation_work();
        }

        if (rgd.velocity.y != 0)//�÷��� ���� �ִ��� üũ�Ѵ�(���� ���ϸ��̼�,���� ���� ���ϸ��̼�)
        {
            if (onground)
            {

                onground = false;
            }
        }
        else
        {
            if (!onground)
            {

                onground = true;
            }
        }
        p_anim.set_ground(onground);//onground ���� ���� ���ϸ����Ϳ��� �÷��� ���� �ִ��� ������ ����


        if (rgd.velocity.y <= 0 && !raycheck)//���߿� ���� �� ���� ������ ���� Ƚ�� ȸ��
        {
            //���߿� ���� �� �ڽ��� �Ʒ��ʿ�  ray(��������Ʈ�� ������ ���ݺ��� ��¦ ���)�� ���(�÷��� ��������ŭ ray�� ���)
            Debug.DrawRay(this.transform.position, Vector2.down * Player_Y, Color.red);
            //�Ϲ� �÷���
            RaycastHit2D down_ray_1 = Physics2D.Raycast(this.transform.position - Vector3.up, Vector2.down, Player_Y, LayerMask.GetMask("platform_can't_pass"));
            //����� �÷���
            RaycastHit2D down_ray_2 = Physics2D.Raycast(this.transform.position - Vector3.up, Vector2.down, Player_Y, LayerMask.GetMask("platform_can_pass"));
            //ray�� ��Ҵٸ� ���� ������� �˸��� ������ ȸ��
            if (down_ray_1.collider)
            {
                groundcollision(down_ray_1.transform.gameObject);

                Debug.Log("�÷���");
            }
            if (down_ray_2.collider)
            {
                groundcollision(down_ray_2.transform.gameObject);

                Debug.Log("�ܹ���");
            }
            else
            {

            }
        }
    }
    public void animation_work()//ȯ���� �׼��� �ٷ��
    {
        if (once_chk)//�׼��� �ѹ��� �������� ��� ���������� ���θ� üũ�Ѵ�
        {
            if (!anim_chk)//anim_chk�� �׼��� �����ߴ��� üũ�Ѵ�
            {
                switch (animation_level)//animation_level�� ���� ���� ������ �׼��� �����Ѵ�
                {
                    case 1:
                        attack(1);
                        break;
                    case 2:
                        attack(2);
                        break;
                    case 3:
                        attack(3);
                        break;
                    case 4:
                        if (onground)
                        {
                            jump();
                        }


                        break;
                    case 5:
                        move_left();
                        break;
                    case 6:
                        Move_right();
                        break;
                    case 7:
                        StartCoroutine(downplatform());
                        break;
                    case 8:
                        if (!on_dash)
                        {
                            dash();
                        }

                        break;
                    case 9:
                        attack_combo();
                        break;
                    case 10:
                        if (!on_dash)
                        {
                            dash_mirror();
                        }
                        break;
                    case 11:
                        air_attack();
                        break;
                }
               
            }
        }
        else//���ӽð����� ��� ����
        {
            switch (animation_level)
            {
                case 1:
                    attack(1);
                    break;
                case 2:
                    attack(2);
                    break;
                case 3:
                    attack(3);
                    break;
                case 4:
                    if (onground)
                    {
                        jump();
                    }


                    break;
                case 5:
                    move_left();
                    break;
                case 6:
                    Move_right();
                    break;
                case 7:
                    StartCoroutine(downplatform());
                    break;
                case 8:
                    if (!on_dash)
                    {
                        dash();
                    }
                    break;
                case 9:
                    attack_combo();
                    break;
                case 10:
                    if (!on_dash)
                    {
                        dash_mirror();
                    }
                    break;
                case 11:
                    air_attack();
                    break;
            }
        }
    }
    //���� ���� �׼� ����
    private void air_attack()
    {
        anim_chk = true;
        //���߰��� ���ϸ��̼� ����
        p_anim.air_attack_anim();
    }
    //�뽬 �׼� ����(��������)
    void dash()
    {
        //�÷��̾��� �뽬������ ����
        
            rgd.velocity = Vector2.zero;


            p_anim.dash();
        // ���������� rgd�� ���� ���� ���Ѵ�(ForceMode2D.Impulse)
        //�� �� ª�� �ð�(dash_timer)���� ��� ���󰣴�
        //dash_timer�� ������ velocity�� ���� 0���� ����� �뽬�� ������
        on_dash = true;
            dash_timer = dash_time;

            //dash_direction =  Vector2.right*direction * Player_status.p_status.get_dash_force();
            dash_direction = Vector2.right * direction * dash_force;
            Debug.Log(dash_direction);
            rgd.AddForce(dash_direction * Time.deltaTime, ForceMode2D.Impulse);
  

            dash_recover_check = false;
        

    }
    //�뽬 �׼� ����(�޹���)
    void dash_mirror()
    {


        rgd.velocity = Vector2.zero;


        p_anim.dash();

        on_dash = true;
        dash_timer = dash_time;
        if (this.transform.GetChild(0).transform.rotation.y != 180)
        {
            
            this.transform.GetChild(0).transform.Rotate(0, 180, 0);
        }
        direction = -1;
        //dash_direction =  Vector2.right*direction * Player_status.p_status.get_dash_force();
        dash_direction = Vector2.right * direction * dash_force;
        Debug.Log(dash_direction);
        rgd.AddForce(dash_direction * Time.deltaTime, ForceMode2D.Impulse);


        dash_recover_check = false;


    }
    //dash_timer�� �ٵƴٸ� velocity�� ���� 0���� ����� ������Ű�� �߷��� ������ �ްԸ��巯 �뽬�� ������
    void dash_end()
    {
        rgd.velocity = Vector2.zero;

        p_anim.dash_end();

        anim_chk = true;
        rgd.gravityScale = 1;
        on_dash = false;
    }
    // ����Ű �Ʒ��� �ι� ���޾� �������ν� ����� �÷��� �Ʒ��� �������� �ִ�
    public void down_platform()
    {

        //�Ʒ� Ű�� ���۷� �����鼭 Ű�� �ѹ� ���� ���¸� Ȯ���Ѵ�
        if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.DOWN]) && on_platform && Downbuffertimer <= 0 && !p_anim.sword_delay && !on_dash)
        {
            Downbuffertimer = Downbuffertime;
        }
        //���۰� �����Ǿ��ִ� ���� �Ʒ� Ű�� �ѹ� �� ������ ������ ���̾ �����Ű�� �ڷ�ƾ�� �����Ͽ� ����� �÷����� ����ϰ� �Ѵ�
        if (Downbuffertimer > 0)
        {
            Downbuffertimer -= Time.deltaTime;
            if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.DOWN]) && on_platform && Downbuffertimer >= 0 && !p_anim.sword_delay && !on_dash)
            {
                //layer_change_timer = layer_change_time;
                if (!on_corutine_1)
                    StartCoroutine(downplatform());
            }
        }
    }
    //����� �÷����� ����ϴ� �ڷ�ƾ
    IEnumerator downplatform()
    { 
        var wait = new WaitForSeconds(Downbuffertime);
        //����� �÷����� ����Ҽ��ִ� ���̾�� �����Ѵ�
        on_corutine_1 = true;
        this.gameObject.layer = 10;
        //ª�� �ð��� ������ �� ������ ���̾�� �����Ѵ�
        yield return wait;
        on_corutine_1 = false;
        this.gameObject.layer = 6;
     
    }
    //�̵� �׼�(������)
    public void Move_right()
    {
        p_anim.move_state = true;
        //��������Ʈ�� �ݴ� �����̶�� ��������Ʈ�� �¿츦 ������ direction�� �����Ѵ�
        if (direction == -1)
        {
            direction_change();
        }
        //�̵� ���͸� ����
        move_vector = new Vector3(direction * move_speed , 0, 0);

        //�ڽ��� �̵���ο� ���� �����ϴ����� ray�� Ȯ���Ѵ�
        RaycastHit2D wall_ray = Physics2D.Raycast(this.transform.position, Vector3.right, p.Player_X, LayerMask.GetMask("platform_can't_pass"));

        Debug.DrawRay(this.transform.position, Vector3.right * p.Player_X, Color.red);
        //ray�� �������ν� ���� �����ߴٸ� �̵� ������ ������ �̵��ӵ��� �������ν� ���� ����ϴ� ������ �����Ѵ�
        if (wall_ray.collider)
        {

            move_weight *= 0.93f;
        }
        else
        {
            move_weight = 1;
        }

        //ĳ���Ͱ� �̵�
        transform.Translate(move_vector * move_weight * Time.deltaTime);
    }
    //�÷��̾��� 3Ÿ �޺� �׼��� ����
    public void attack_combo()
    {
        if (onground)//���� �������� ����
        {
            p_anim.attack_combo();
           
        }
    }
    //�̵� �׼�(����)
    public void move_left()
    {
        p_anim.move_state = true;
        if (direction == 1)
        {
            direction_change();
        }

        move_vector = new Vector3(direction * move_speed * Time.deltaTime * -1, 0, 0);
        direct_vector = new Vector2(-1, direct_vector.y);


        RaycastHit2D wall_ray = Physics2D.Raycast(this.transform.position, Vector3.left, p.Player_X, LayerMask.GetMask("platform_can't_pass"));
        Debug.DrawRay(this.transform.position, Vector3.left * p.Player_X, Color.red);
        if (wall_ray.collider)
        {

            move_weight *= 0.93f;
        }
        else
        {
            move_weight = 1;
        }


        transform.Translate(move_vector * move_weight);

    }
 //���� �׼� ����
    public void jump()
    {
        //���� Ƚ���� �������� ��
        if (jump_count > 0)
        {
            //���� ���ͷ� ���� ���������� ���ϰ� �ִϸ��̼� ����
            jump_count--;
            p_anim.jump_anim();
            rgd.AddForce(new Vector2(rgd.velocity.x, Player_status.p_status.get_jump_force()), ForceMode2D.Impulse);
            raycheck = false;
            anim_chk = true;
        }
    }
//���� �׼� �ѹ��� ����
    void attack()
    {
       

        

                if (onground)
                    p_anim.sword_attack_anim();
                else if (!onground && air_attack_num > 0)
                {
                    p_anim.air_attack_anim();
                   air_attack_num--;
                }
                
            

        }
    //������ ���� �޺� �׼� �Ѻκ��� ����
    void attack(int i)
    {




        if (onground)
            p_anim.sword_attack_anim(i-1);
       



    }
    //���� ��Ҵٸ� ���� Ƚ���� ȸ��
    public void groundcollision(GameObject a)
    {
        if (a.layer != 11)//�Ϲ� �÷����̶��
        {
            //������ ��� ��� ���� ȸ��
            
            jump_count = Player_status.p_status.get_jump_count();
            dash_recover_check = true;
            onground = true;
          air_attack_num = Player_status.p_status.air_attack_num_orignal + Player_status.p_status.air_attack_num_bonus;
            raycheck = true;

        }
        else//����� �÷����Ͻ�
        {

            if (onground)//�÷��� ���� ���� ���� ���� ȸ��
            {
                //����� �÷����� �÷��̾ ����ϹǷ� �÷��� ��� �߿� �÷��̾ ���� ȸ���� ȸ���ϴ� ��Ȳ�� ���´�
                
                jump_count = Player_status.p_status.get_jump_count();
                dash_recover_check = true;
                onground = true;
               air_attack_num = Player_status.p_status.air_attack_num_orignal + Player_status.p_status.air_attack_num_bonus;
                raycheck = true;

            }



        }

    }
    //�̵� �׼��� �����
    public void stop_move()
    {
        direct_vector = new Vector2(0, direct_vector.y);
        p_anim.move_state = false;
    }
    //������ �ٲ۴�(direction 1=������ -1=����)
    void direction_change()
    {
        direction *= -1;
        transform.Rotate(0, 180, 0);
        if (transform.rotation.y > 360)
            transform.Rotate(0, -360, 0);
    }
  
}
