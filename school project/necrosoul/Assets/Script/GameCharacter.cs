using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    public bool death_check;//��� üũ
    public List<bad_status> B_status = new List<bad_status>();
   public int b_status_count;
    public int Health_point;
    public int max_hp;
    public int Attack_point;
    public int Defense_point;
    public float move_speed = 1.0f;//�̵��ӵ�
    public bool untouchable_state;
    public float untouchable_timer = 0;
    public bool can_attack;
    public bool can_move;
    public int direction = 1;//ĳ������ ���� 1, -1 �� ���

    public bool attack_trigger;
    public bool attacked_trigger;
    /// <bad_status>
    public void bad_status()
    {
        for (int i = 0; i < B_status.Count; i++)
        {
            if (B_status[i].status_timer > 0)
            {
                status_effect(B_status[i]);
            }
            else
            {
                status_uneffect(B_status[i]);
            }
        }
    }
    public bad_status get_bad_status(bad_status b)
    {
        if (this.GetComponent<PlayerCharacter>() != null)
        {
            if (Gamemanager.GM.game_ev.when_player_get_bad_status())
            {
                for (int i = 0; i < B_status.Count; i++)
                {
                    if (b.status_num == B_status[i].status_num)
                    {
                        return null;
                    }
                }
                B_status.Add(b.copy_bad_status());
                return b;
            }
            else
            {
                return null;
            }
        }
        else
        {

            for (int i = 0; i < B_status.Count; i++)
            {
                if (b.status_num == B_status[i].status_num)
                {
                    return null;
                }
            }
            B_status.Add(b.copy_bad_status());
            return b;
        }
        }
    public void status_effect(bad_status b)
    {

        switch (b.status_num)
        {
            case 0:
                binding_effect(b);
                break;
        }
        if (b.b_status_effect == null)
        {
            var b_ef= Instantiate(Gamemanager.GM.bad_status_effect[b.status_num], this.gameObject.transform);
            b_ef.transform.position = this.transform.position;
            b.b_status_effect = b_ef;
        }
    }
    public void status_uneffect(bad_status b)
    {
        if (b.b_status_effect != null)
        {
            Destroy(b.b_status_effect);
        }
        switch (b.status_num)
        {
            case 0:
                binding_uneffect(b);
                break;
        }

    }
    public void binding_effect(bad_status b)
    {
        Rigidbody2D rgd = this.GetComponent<Rigidbody2D>();
        rgd.velocity = Vector3.zero;
        can_move = false;
        Debug.Log("����ȿ����");
        b.status_timer -= Time.deltaTime;
    }
    public void binding_uneffect(bad_status b)
    {
        can_move = true;
        B_status.Remove(b);
    }
    /// 
    void Start()
    {
        
        //Rigidbody2D rgd = this.gameObject.GetComponent<Rigidbody2D>();

    }
    
    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        b_status_count = B_status.Count;
        if (Defense_point > 100)
            Defense_point = 100;
        if (Health_point >= max_hp)
            Health_point = max_hp;
        if (Health_point > 0)
        {
            bad_status();
          
        }
    }
    public void direction_change()//����ٲٱ� �� ��
    {
        direction *= -1;
        transform.Rotate(0, 180, 0);
        if (transform.rotation.y > 360)
            transform.Rotate(0, -360, 0);
    }
   
    public void direction_change_spr()
    {
        direction *= -1;
        Transform spr = this.gameObject.transform.GetChild(1);
        spr.Rotate(0, 180, 0);
        if (spr.rotation.y > 360)
            spr.Rotate(0, -360, 0);
    }
    public int character_lose_health(int dmg, GameObject DNP, Transform Tr)  //obj=���� ������Ʈ �ǰݽ� ü���� �Ҵ� ����
    {
        if (!untouchable_state)
        {

            attacked_trigger = true;
            float damage_lose = Defense_point / 100;
            int damage = Mathf.RoundToInt(dmg * (1.0f - damage_lose));//������ ����
            if (damage == 0)
            {
                damage = 1;
            }
            if ( Health_point > 0)
                Font_manager.DN.SpawnNumber(3, dmg, Tr);
            Health_point = Health_point - damage;//ü���� ��������ŭ ����
            attacked_trigger = false;
            return damage;
        }
        else
        {
            return 0;
        }
        
    }
    public void character_move()//ĳ���� �̵� ��ӿ�
    {
        /* Vector3 move_force = new Vector3(move_speed * direct, 0, 0);
         rgd.AddForce(move_force);
         RaycastHit hit;
         Debug.DrawRay(chr_eye1.transform.position, new Vector3(0.1f, -0.9f, 0), Color.blue);
         if (Physics.Raycast(chr_eye1.transform.position, new Vector3(0.1f, -0.9f, 0), out hit, 2))
         {
             if (hit.collider.CompareTag("platform"))
                 direct = direct * -1;
         }
         //if(XXXXXX)//�ڽ��� ���� Ȯ���ϰ� �������ٸ�
         /*direct= direct*-1;
          * 
          */
    }//�⺻ �̵��� �÷����� �ȶ������� �¿� �Դٰ��� ������
     //�������� ó���ϸ� ũ�⸶�� �޶����� �����? ->���� ������ ���ϸ� ��������

    void character_death()//��ӿ�
    {
        //�ʿ��� �̺�Ʈ�� �ۼ�

    }
    /*void jump()//���� ���
    {
        /*if (jump_number != 0)
        {


            Vector3 jump_vector = new Vector3(0, jump_force, 0);
            rgd.AddForce(jump_vector);
        }*/
    //}


}
public class bad_status
{
    public int status_num;//�������ͽ� ����
    public float status_timer;//���� �ð�
    public float status_time;//�������ͽ� �ð�
    public GameObject b_status_effect;
                             // public string status_name="";
                             // public Sprite status_image;
    public bad_status(int i, float time)
    {
        status_num = i;
        status_time = time;
        status_timer = status_time;
    }
    public bad_status copy_bad_status()
    {
        bad_status b=new bad_status(this.status_num, this.status_time);
        
        return b;
    }
}
