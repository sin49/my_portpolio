using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AI_01 : MonoBehaviour
{
    //�ذ�
    Enemy_status E_Status;
    Vector2 dir;
    public float bullet_speed;
    private Quaternion rotation;
    public List<Transform> create_position = new List<Transform>();
    public float attack_time;
    bool attack_status;
    public List<GameObject> enemy_pulling;
   public Transform enemy_pulling_transform;
    public float bullet_size;
    float attack_weight;
    Unit unit;
    public GameObject Player;
    public List<GameObject> create_object = new List<GameObject>();
    public List<GameObject> created_object = new List<GameObject>();
    float move_distance;
    public float move_distance_max;
    public float enemy_size_x;
    public float enemy_size_y;
    public float moving_buffer;
    float moving_weight;
    public float range_distance;
    bool move_corutine_check;
    bool idle_corutine_check;
    bool moving_status;
    public float idle_time;
    public E_AI_01_range E_range;

    Animator e_ani;
    // Start is called before the first frame update
    void Start()
    {
       
        unit = this.GetComponent<Unit>();
        unit.size_x = enemy_size_x;
        unit.size_y = enemy_size_y;
        attack_status = true;
        e_ani = this.transform.GetChild(1).GetComponent<Animator>();
        E_Status = this.gameObject.GetComponent<Enemy_status>();
        E_Status.set_layout(0);
        unit.can_hitted_ani = true;
        unit.max_hp = E_Status.get_max_hp();
        unit.Health_point = E_Status.get_hp();
        unit.Defense_point = E_Status.get_defense_point();
        unit.move_speed = E_Status.get_speed();
        unit.Attack_point = E_Status.get_atk();
        for(int i = 0; i < 3; i++)
        {
            var a = Instantiate(create_object[0]);
            enemy_pulling.Add(a);
            a.transform.position = new Vector2(-999, -999);
            a.transform.parent = enemy_pulling_transform;
            a.SetActive(false);
        }
    }
    GameObject pulling_bullet()
    {
        for(int i = 0; i < enemy_pulling.Count; i++)
        {
            if (!enemy_pulling[i].activeSelf)
            {
                return enemy_pulling[i];
            }
        }
        return null;
    }
    void ray_to_player()
    {
  
            Player= this.transform.GetComponent<Unit>().Player;
        
        float length = Mathf.Log(Mathf.Pow(18, 2) + Mathf.Pow(28, 2)) * 2;
        //Debug.DrawLine(transform.position+ Vector3.up * (bullet_size / 2), Player.transform.position.normalized * range_distance, Color.red);
        if (Player != null)
            Debug.DrawLine(transform.position, Player.transform.position, Color.blue);
       // Debug.DrawLine(transform.position - Vector3.up * (bullet_size / 2), Player.transform.position.normalized * range_distance, Color.red);
       // var ray1= Physics2D.Raycast(transform.position+Vector3.up*(bullet_size/2), dir, length, LayerMask.GetMask("platform_can't_pass"));
       // var ray2 = Physics2D.Raycast(transform.position - Vector3.up * (bullet_size / 2), dir, length, LayerMask.GetMask("platform_can't_pass"));
        var ray= Physics2D.Raycast(transform.position, dir.normalized, dir.magnitude, LayerMask.GetMask("platform_can't_pass"));
       // var ray2 = Physics2D.Raycast(transform.position, dir.normalized*-1, length, LayerMask.GetMask("platform_can't_pass"));
        if (ray.collider == null&&E_range.on_player)
        {
            unit.can_attack = true;
            //Debug.Log("???");
       
        }
        else
        {

            unit. can_attack = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        brain();
    }
    void brain()//ai ����
    {
        if (unit.Health_point > 0)
        {
           
            ray_to_player();
            if (Player != null)
                dir = Player.transform.position - this.transform.position;

            
            if (unit.can_attack)//���� �ȸ���+�����Ÿ���
            {
                if (dir.x > 0&&unit.direction==1)
                {
                    unit.direction_change_spr();
                }else if (dir.x < 0 && unit.direction == -1)
                {
                    unit.direction_change_spr();
                }
                    if (attack_status)
                StartCoroutine("attack");
            }
            else
            {
                StartCoroutine("idle");//���  ����
                //if (move_corutine_check)
                /*{
                    if (!moving_status)
                        StartCoroutine("move");//�̵�����
                }
                else*/
                //{
                   // if (!idle_corutine_check)
                      //  StartCoroutine("idle");//���  ����
                //}
                //if (moving_status)
                //{
                    //move_ai_0();
                //}
            }
        }
        else
        {
            die();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new Vector2(enemy_size_x, enemy_size_y));
    }
    void die()
    {
        unit.HpCheack();
        e_ani.SetTrigger("death");
        for (int i = 0; i < created_object.Count; i++)
        {
            Destroy(created_object[i]);
        }
    }

    public void create_bullet()
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        var a = pulling_bullet();
        a.transform.position = create_position[0].position;
        a.transform.rotation = rotation;

        dir = Player.transform.position - this.transform.position;
        attack_effect1 b = a.GetComponent<attack_effect1>();
        b.Attack = unit.Attack_point;
        b.bullet_spped = bullet_speed;
        b.dir = this.dir;
        a.SetActive(true);
    }


    










    void move_ai_0()
    {
        transform.Translate(Vector3.left*unit.direction *unit.move_speed * Time.deltaTime);//�������� ������
  
        move_distance +=  unit.move_speed* Time.deltaTime;
        //���鿡 ����ĳ��Ʈ�� ������
        Debug.DrawLine(transform.position,transform.position- (new Vector3(0.2f,0,0)+ new Vector3(enemy_size_x/2,0,0)) * unit.direction, Color.green);
        var wall_ray = Physics2D.Raycast(transform.position, Vector3.left * unit.direction, enemy_size_x / 2 + 0.2f, LayerMask.GetMask("platform_can't_pass"));
        if (wall_ray.collider != null)
        {
            unit.direction_change_spr();
            move_distance = 0;
        }
        //���� �Ÿ� �̻��̸� ���� �ٲ�
        if (move_distance >= move_distance_max)
        {
            unit.direction_change_spr();
            move_distance = 0;
        }
        //�տ� �÷����� ������ ���� �ٲ�
        var bottom_ray = Physics2D.Raycast(transform.position + Vector3.left * (enemy_size_x / 2) * unit.direction, Vector3.down, enemy_size_y / 2 + 0.4f, LayerMask.GetMask("platform_can't_pass"));
        var bottom_ray_2 = Physics2D.Raycast(transform.position + Vector3.left * (enemy_size_x / 2) * unit.direction, Vector3.down, enemy_size_y / 2 + 0.4f, LayerMask.GetMask("platform_can_pass"));
        Debug.DrawLine(transform.position+Vector3.left * (enemy_size_x / 2) * unit.direction, transform.position + Vector3.left * (enemy_size_x / 2) * unit.direction+(Vector3.down*(enemy_size_y/2)+new Vector3(0,0.4f)), Color.blue);
        if (bottom_ray.collider == null&&bottom_ray_2.collider==null)
        {
            unit.direction_change_spr();
            move_distance = 0;
        }
        
    }
   
    IEnumerator attack()
    { // ó���� FireState�� false�� �����
        var wait = new WaitForSeconds(attack_time-(float)(0.2*Gamemanager.GM.stage-1) + attack_weight);
        if (pulling_bullet()!=null)
        {
            attack_status = false;
            e_ani.SetBool("move", false);
            e_ani.SetTrigger("attack");
        }
        yield return wait;
        attack_status = true;
        attack_weight = Random.Range(-1, 1);

    }
    IEnumerator move()
    { 
        var wait = new WaitForSeconds(moving_buffer+moving_weight);

        moving_status = true;
        e_ani.SetBool("move", true);
        yield return wait;
        move_corutine_check = false;
        moving_status = false;
        e_ani.SetBool("move", false);
        moving_weight = Random.Range(-1, 1);
    }
    IEnumerator idle()
    {
        var wait = new WaitForSeconds(idle_time);
        e_ani.SetBool("move", false);
        idle_corutine_check = true;
        yield return wait;
        move_corutine_check = true;
        idle_corutine_check = false;
    }

}
