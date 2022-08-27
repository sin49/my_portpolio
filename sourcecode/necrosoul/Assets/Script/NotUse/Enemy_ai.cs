using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ai : MonoBehaviour
{
    public List<GameObject> create_object = new List<GameObject>();
    public List<Transform> create_position = new List<Transform>();
    public Transform up;
    public Transform down;
    Vector2 dir;
    private Quaternion rotation;
    RaycastHit2D player_ray_wall_check;
    Vector3 up_direction;
    Vector3 down_direction;
    Rigidbody2D rgd;
    public float attack_time;
    public bool attack_status;
    bool weight_dir;
    GameObject Player;
    float weight;
    public bool jump_check;
    bool onground;
    bool down_corutine;
    float dowwn_time = 0.5f;
    float upbuffer = 0.4f;
    float upbufferTime;
    bool upcheck;
    float downbuffer = 0.4f;
    float downbufferTime;
    bool downcheck;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        attack_status = true;
        rgd = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        weight = Random.Range(-1.0f, 5.0f);
        dir =  Player.transform.position- this.transform.position;
        up_direction= Player.transform.position - up.position;
        down_direction = Player.transform.position - down.position;
        ray_to_player();//���� 18 �ʺ� 28

       
        if (player_ray_wall_check.collider != null)
        {
            //Debug.Log("���� ����");
            move_ai(2);
        }
        else
        {
            move_ai(2);
            if (attack_status)
            {
                StartCoroutine(attack());
            }
        }
    }
    void ray_to_player()
    {
        float length = Mathf.Log(Mathf.Pow(18, 2) + Mathf.Pow(28, 2))*2;
        Debug.DrawLine(transform.position, Player.transform.position.normalized*length, Color.red);
        player_ray_wall_check = Physics2D.Raycast(transform.position, dir, length, LayerMask.GetMask("platform_can't_passs"));
        Debug.DrawLine(down.position, Player.transform.position.normalized * length, Color.green);
     
        
        Debug.DrawLine(up.position, Player.transform.position.normalized * length, Color.green);
    }
    public void move_ai(int i)//�̵� ���
    {
        switch (i)
        {
            case 0://������ ����
                break;
            case 1://�÷��̾� �߰�(�������� ����)
                
                this.transform.Translate(dir * 2 * Time.deltaTime);
                break;
            case 2://�÷��̾� �߰�(�������� ����o)
            
                
                if (this.transform.position.x > Player.transform.position.x + 4)
                {
                    weight_dir = true;
                }
                else if (this.transform.position.x < Player.transform.position.x - 4)
                {
                    weight_dir = false;
                }

                if (weight_dir)
                {
                    transform.Translate(2 * Vector3.left * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.right * 2 * Time.deltaTime);
                }
                if (up_direction.y>0&&upcheck==false) {
                    upbufferTime = upbuffer;
                    upcheck = true;
                }//����ĳ��Ʈ ���� ���� ������� �÷��������� ����

                if (upbufferTime<0&&upcheck==true) { 
                    Debug.DrawLine(transform.position, transform.position+Vector3.up*20, Color.blue);
                    RaycastHit2D up_ray = Physics2D.Raycast(this.transform.position, Vector2.up, 20.0f, LayerMask.GetMask("platform_can_pass"));
                    //RaycastHit2D up_ray = Physics2D.Raycast(this.transform.position, Vector2.up, 20.0f);
                    if (up_ray.collider)
                    {
                        if (!jump_check)
                        {
                            jump();
                        }
                    }
                    /*if (up_ray.collider)
                    {
                        Debug.Log(up_ray.transform.gameObject.layer);
                    }*/
                    /*if (up_ray.collider)
                    {
                        Debug.Log("�����غ�");
                        if (!jump_check)
                        {
                            
                            jump();
                        }
                    }*/
                    upcheck = false;
                }else if (upcheck == true)
                {
                    upbufferTime -= Time.deltaTime;
                }

                if (down_direction.y<0 && downcheck == false&&!jump_check)
                {
                    downbufferTime = downbuffer;
                    downcheck = true;
                }
                if(downbufferTime < 0 && downcheck == true)
                {
                   
                    if (!down_corutine)
                    {
                        StartCoroutine(down_platform());
                    }
                    downcheck = false;
                }
                else if (downcheck == true)
                {
                    downbufferTime -= Time.deltaTime;
                }
                break;
            default:
                break;
        }
    }
    void jump()
    {
        Debug.Log("����");
        //rgd.velocity = new Vector2(rgd.velocity.x, minimum_jump_vec3.y);
        rgd.AddForce(new Vector2(rgd.velocity.x,20.0f), ForceMode2D.Impulse);
        jump_check = true;
    }
    /*public bool move_condition(int i)//�̵��� ������ �ִ°�?
    {
        switch (i)
        {
            case 0://����
                return true;
            case 1:
                return false;
            default:
                return false;
        }

    }*/
    public void attack_ai(int i)//���� ���
    {
        switch (i)
        {
            case 0://�������� ����
                break;
            case 1://0�� ������Ʈ�� 0�� ��ġ���� �÷��̾�� �߻�
              
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                GameObject obj = Instantiate(create_object[0], create_position[0]. position,rotation );
                attack_effect1 a = obj.GetComponent<attack_effect1>();
                a.dir=this.dir;
                break;
            case 2://���� ������Ʈ ����
                
                float dir_= Mathf.Log(Mathf.Pow(dir.x, 2) + Mathf.Pow(dir.y, 2));
                if (dir_ < 2)
                {
                   rotation = Quaternion.LookRotation(dir.normalized);
                    GameObject atk=Instantiate(create_object[0], create_position[0].position,rotation);
                }
                break;
            case 3://�ڱ� �ڽſ��Լ� ������Ʈ ����
                GameObject created = Instantiate(create_object[0], create_position[0].position, Quaternion.identity);
                break;
            default:

                break;
        }
    }
    IEnumerator attack()
    { // ó���� FireState�� false�� �����
        var wait = new WaitForSeconds(attack_time+weight);

        attack_ai(1);
        attack_status = false;
        yield return wait;
        attack_status = true;

    }
    IEnumerator down_platform()
    {
        var wait = new WaitForSeconds(dowwn_time);
        this.gameObject.layer = 13;
        down_corutine = true;
        yield return wait;
        down_corutine = false;
        this.gameObject.layer = 7;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (rgd.velocity.y == 0)
            {
                if (jump_check)
                {
                    jump_check = false;
                }
            }
        }
    }
    /* public bool attack_condition(int i)//���ݿ� ������ �ִ°�?
     {
         switch (i)
         {
             case 0://����
                 return true;
             case 1:
                 return false;
             default:
                 return false;
         }

     }*/
}
