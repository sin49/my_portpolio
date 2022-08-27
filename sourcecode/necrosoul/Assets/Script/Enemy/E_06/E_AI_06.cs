using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AI_06 : MonoBehaviour//�÷��̾� ��ġ�� �����ϰ� ���߽� ���� �����̻��� �ο��ϴ� ��
{
    //�Ĺ�
    Enemy_status E_Status;
    Vector2 dir;

    public List<Transform> create_position = new List<Transform>();
    public float attack_time;
    bool attack_status;

    public float bullet_size;
    float attack_weight;
    Unit unit;
    public GameObject Player;

    public List<GameObject> create_object = new List<GameObject>();
    public GameObject created_object ;

    public float move_distance_max;
    public float enemy_size_x;
    public float enemy_size_y;
    public float moving_buffer;
    float moving_weight;
    public float range_distance;
    public bool on_attack;
    public float idle_time;
    public E_AI_06_range attack_range;
    Animator e_ani;
    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new Vector2(enemy_size_x, enemy_size_y));
    }
    void Start()
    {
        
            Player = this.transform.GetComponent<Unit>().Player;
        
        unit = this.GetComponent<Unit>();
        attack_status = true;
        e_ani = this.transform.GetChild(1).GetComponent<Animator>();
        E_Status = this.gameObject.GetComponent<Enemy_status>();
        E_Status.set_layout(3);
        unit.can_hitted_ani = true;
        unit.max_hp = E_Status.get_max_hp();
        unit.Health_point = E_Status.get_hp();
        unit.Defense_point = E_Status.get_defense_point();
        unit.move_speed = E_Status.get_speed();
        unit.Attack_point = E_Status.get_atk();
        unit.size_x = enemy_size_x;
        unit.size_y = enemy_size_y;
    }
    
    void FixedUpdate()
    {
        brain();
    }
    void brain()//ai ����
    {
        if (unit.Health_point > 0)
        {
            Player = this.transform.GetComponent<Unit>().Player;
            dir = Player.transform.position - this.transform.position;
            if (Player.GetComponent<PlayerCharacter>()!=null)//�÷��̾ ������ ��
            {
                if (attack_range.on_player && Player.GetComponent<PlayerCharacter>().onground)//�÷��̾ ���� ���� ��+�÷��̾ ���� ���� ��
                {
                    unit.can_attack = true;//����
                }
                else
                {
                    unit.can_attack = false;
                }
            }
            else
            {
                if (attack_range.on_player)
                {
                    unit.can_attack = true;
                }
                else
                {
                    unit.can_attack = false;
                }
            }
            //�ϰ� ���� �� ���� ����
            if (created_object != null && created_object.activeSelf)
            {
                on_attack = true;
                
            }else if(created_object != null && !created_object.activeSelf)
            {
                on_attack = false;
            }
            e_ani.SetBool("on_attack", on_attack);
            if (unit.can_attack)
            {
                if (attack_status && !on_attack)//�÷��̾ ���� �� �� �ִ� ���ǿ���
                {
                    if (dir.x > 0 && unit.direction == 1)
                    {
                        unit.direction_change_spr();
                    }
                    else if (dir.x < 0 && unit.direction == -1)
                    {
                        unit.direction_change_spr();
                    }
                    // attack_�÷��̾� ��ǥ �޾Ƽ� ������Ʈ ����
                    StartCoroutine("attack");//����+�������� ����
                }
              
            }
            else
            {
                StartCoroutine("idle");//���  ����
                                  
            }
        }
        else
        {
            die();
        }
    }

    void die()
    {
        unit.HpCheack();
        
       
    }
    //�÷��̾��� ��ġ�� ���� ������Ʈ�� ����
    public void create_bullet()
    {
        //������ �� ����
        if (created_object == null)
        {
            RaycastHit2D bot_ray = Physics2D.Raycast(Player.transform.position, Vector2.down, 99f, LayerMask.GetMask("platform_can't_pass"));
            GameObject obj = Instantiate(create_object[0], bot_ray.point+(Vector2.up*Player.GetComponent<PlayerCharacter>().Player_Y*0.7f), Quaternion.identity);
            created_object = obj;
            created_object.transform.SetParent(this.transform.parent);
            attack_effect_06 a = obj.transform.GetChild(0).GetComponent<attack_effect_06>();
            a.Attack = unit.Attack_point * 2;
            // Enemy_status e = this.GetComponent<Enemy_status>();
        }
        else// �� �Ŀ��� Ȱ��ȭ
        {
            RaycastHit2D bot_ray = Physics2D.Raycast(Player.transform.position, Vector2.down, 99f, LayerMask.GetMask("platform_can't_pass"));
            created_object.transform.position = bot_ray.point + (Vector2.up * Player.GetComponent<PlayerCharacter>().Player_Y * 0.7f);
            attack_effect_06 a = created_object.transform.GetChild(0).GetComponent<attack_effect_06>();
            a.Attack = unit.Attack_point*2;
            created_object.SetActive(true);
        }
    }






    






   
    //���� �ڷ�ƾ
    IEnumerator attack()
    { //���÷� attack_status�� ����
        var wait = new WaitForSeconds(attack_time - (float)(0.2 * Gamemanager.GM.stage - 1) + attack_weight);
        attack_status = false;
        e_ani.SetBool("move", false);
        e_ani.SetTrigger("attack");
        yield return wait;
        attack_status = true;
        attack_weight = Random.Range(-1, 1);

    }
    
   
    IEnumerator idle()
    {
        var wait = new WaitForSeconds(idle_time);
        e_ani.SetBool("move", false);

        yield return wait;

    }

}
