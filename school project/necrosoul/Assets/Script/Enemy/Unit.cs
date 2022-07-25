using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GameCharacter//���� �⺻ �ൿ�� ó���ϴ� Ŭ����(GameCharacter�� �����)
{
   public float size_x;
   public float size_y;
   // List<GameObject> Particle_pulling;
  public Color unit_hitted_color;
    public float hitted_material_time;
    public float hitted_material_timer;
    Material this_material;
    public int enemy_rank;//0=common 1=elite 2=boss
    public bool e_active;
    public float active_timer;
    public float active_time = 1.5f;
    public GameObject Player;
    public GameObject Destroy_effect;
    public Animator e_ani;
    Color color;
    protected int damaged;
    bool on_hiited_force;
    public bool sentinal;
    public GameObject DNP;
    public bool onGround=true;
    public ProgressBarPro progressBar;
    public ActionRecord record;
    Rigidbody2D rgd;
    public bool can_forced;
    public List<Vector2> hitted_force_list = new List<Vector2>();
    public Vector2 hitted_force;
    public float manner_time;
    float manner_timer;
    GameObject enemy_pos;
    private void Awake()
    {
        
       
        DNP = GameObject.Find("DemoManager");
    }
  
    void Start()
    {
        //������ ����Ʈ�� ����
        if (Gamemanager.GM.Enemy_destroy_effect != null)
        {
            Destroy_effect = Gamemanager.GM.Enemy_destroy_effect;//�ı� ����Ʈ �غ�
        }
        this_material = this.transform.GetChild(1).GetComponent<SpriteRenderer>().material;
        can_forced = true;//�ǰݽ� �з���
        if(hitted_force_list.Count>0)
            hitted_force = hitted_force_list[0];
        //���,��Ʈ(�ڽ��� �۾����� ����)
        record = GameObject.Find("GameSystem").GetComponent<Record>().ActionRecord;
        DNP = GameObject.Find("DemoManager");

        Player = Gamemanager.GM.Player_obj;
        can_move = true;
        can_attack = false;
        rgd = GetComponent<Rigidbody2D>();
        prefab = Resources.Load("Ef") as GameObject;
        color = this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color;
        unit_hitted_color = new Color(1, 0.3f, 0.3f);
        hitted_material_time = 0.5f;
        if (enemy_pos == null)
        {
            GameObject ins = Instantiate(Gamemanager.GM.enemy_minimap_pos);
            ins.transform.position = this.transform.position;
            ins.transform.SetParent(this.transform);
        }
    }
    void FixedUpdate()
    {
        
        if (manner_timer > 0)
        {
            manner_timer -= Time.deltaTime;
            can_attack = false;
        }
        if (hitted_material_timer > 0)
        {
            this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = unit_hitted_color;
            hitted_material_timer -= Time.deltaTime;
        }
        else
        {
            this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
        }
        if (GameObject.FindGameObjectWithTag("Player_illusion")==null)
        {

            Player = Gamemanager.GM.Player_obj;
        }
        else
        {
            Player = GameObject.FindGameObjectWithTag("Player_illusion");
        }
       
        if (on_hiited_force&&onGround)//�÷��̾��� �������� �÷������� �������� �ʴ´�
        {//bottom_ray�� �з����� ���������� �ƴ��� üũ�ϰ� ������ ��� �з����� �ʵ��� �Ѵ�
            var bottom_ray = Physics2D.Raycast(transform.position + Vector3.right * (size_x / 2) * direction, Vector3.down, size_y / 2 + 0.4f, LayerMask.GetMask("platform_can't_pass"));
            var bottom_ray_2 = Physics2D.Raycast(transform.position + Vector3.right * (size_x / 2) * direction, Vector3.down, size_y / 2 + 0.4f, LayerMask.GetMask("platform_can_pass"));
           
            if (bottom_ray.collider == null && bottom_ray_2.collider == null)
            {
                rgd.velocity = Vector3.zero;
                on_hiited_force = false;
            }
            if (rgd.velocity.x == 0)
            {
                on_hiited_force = false;
            }
        }
       
        }
    private void OnEnable()
    {
        manner_timer = manner_time;
    }
   
    public GameObject prefab;
    GameObject Ins;

    public bool can_hitted;

    public bool can_hitted_ani;
    private bool death_chk;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //���� �ǰ��� ó��
        if (enemy_rank != 3)
        {
            if (e_active && Health_point > 0)
            {

               if (other.tag == "melee")
                {
                    if (can_hitted_ani)
                    {
                        hitted_animation();
                    }
                    hitted_melee(other.gameObject,other);
                }
            }
        }
    }
    //���� �������ݿ� �¾��� �� �������� ���� Ŭ������ melee_force�� ����ŭ �з����� �Լ�
    void hitted_forced_melee(melee_attack a,Vector3 d)
    {
        Debug.Log("������ ���� �з���:" + a.melee_force);
        rgd.velocity = Vector3.zero;
        on_hiited_force = true;
        if (d.x > 0) {
            rgd.AddForce(new Vector2(-1*a.melee_force, 0), ForceMode2D.Impulse);
        }
        else
        {
            rgd.AddForce(new Vector2(a.melee_force, 0), ForceMode2D.Impulse);
        }
    }
    //Ư���� ��� ���ظ� �޾��� �� a��ŭ �ڷ� �з����� �Լ�
    void hitted_forced_sp(float a)
    {
        on_hiited_force = true;
        rgd.velocity = Vector3.zero;
        rgd.AddForce(new Vector2(a, 0), ForceMode2D.Impulse);
    }
    //�ǰݰ� ���õ� ���ϸ��̼��� ���ϸ����ͷκ��� �۵���Ų��
    void hitted_animation()
    {
        if (this.transform.GetChild(1).GetComponent<Animator>() != null)
        {
            e_ani = this.transform.GetChild(1).GetComponent<Animator>();
            e_ani.SetTrigger("hitted");
            e_ani.SetTrigger("death");
        }
    }
   //�������ݿ����� �ǰݴ����� �� ����� ó��+�˹��� ó���Ѵ�
   public void hitted_melee(GameObject a,Collider2D col)
    {
        //���� ���� a�κ��� ���ظ� �������� ���¶��
        if (!a.GetComponent<melee_attack>().E.Contains(this)&&!a.GetComponent<melee_attack>().disable_hit)
        {
            hitted_material_timer = hitted_material_time;
            var me = a.GetComponent<melee_attack>();

        me.E.Add(this);
            Gamemanager.GM.get_combo();
            var le = Player.transform.position - this.transform.position;
            //�ݴ� �������� �ǰ� ���ߴٸ� �ڸ� ����
            if (direction == 1)
            {
                if (le.x > 0)
                {
                    direction_change_spr();
                }
               
            }
            else
            {
                if (le.x < 0)
                {
                    direction_change_spr();
                }
               
            }

         
          
        //�ڷ� �з���
            if (can_forced)
            {
                hitted_forced_melee(me,Player.transform.position-this.transform.position);
            }
            Debug.Log("������ ����"+ me);
            this.gameObject.GetComponent<Enemy_UI>().Hit();
            //�������ݿ� ������ ������ ��ŭ ���ظ� �ް� ���������� ���� ����Ʈ�� Ȱ��ȭ ��Ų��
            //ġ��Ÿ �� ������*ġ��Ÿ ��ŭ ���ظ� �޴´�
            if (Player_status.p_status.critical()/*||me.on_crit*/)
            {
             
                Font_manager.DN.SpawnText(2, "Critical", a.gameObject.transform);
                damaged = character_lose_health(Mathf.RoundToInt((me.Damage * Player_status.p_status.get_critical_damage())), DNP, a.gameObject.transform);
                var par = Instantiate(Gamemanager.GM.enemy_hitted_critical_particle);
                par.transform.position = this.transform.position;
                if (me.sword_effect.gameObject.activeSelf)
                {
                    me.sword_effect.gameObject.SetActive(false);
                }
                me.sword_effect.GetComponent<p_sword_hitted_particle>().crit = true;
                me.sword_effect.transform.position = this.transform.position;
                me.sword_effect.gameObject.SetActive(true);

                if (direction == 1)
                {
                    par.transform.Rotate(0, -180, 0);
                }
                else
                {

                }
            }
            else//�ƴҽ� �Ϲ����� ������ ó��
            {
                damaged = character_lose_health(me.Damage, DNP, a.gameObject.transform);
                var par = Instantiate(Gamemanager.GM.enemy_hitted_particle);
                par.transform.position = this.transform.position;
                if (direction == 1)
                {
                    par.transform.Rotate(0, -180, 0);
                }
                else
                {

                }
                if (me.sword_effect.gameObject.activeSelf)
                {
                    me.sword_effect.gameObject.SetActive(false);
                }
       
                me.sword_effect.transform.position = this.transform.position;
                me.sword_effect.gameObject.SetActive(true);

            }
            //���� ü���� ��ġ�� ���� ���� ����(ü�¹�Ȱ��ȭ,���)
            if (Health_point <= 0)
            {
                me.E.Remove(this);
            }
            Gamemanager.GM.game_ev.when_Enemy_hitted(damaged, this);

           
            if (progressBar == null)
            {
                progressBar = GetComponent<Enemy_UI>().GetBar();
                progressBar.SetBarColor(Color.red);
            }
            progressBar.SetValue(Health_point, max_hp, true);
            
            record.Damge += me.Damage;
        }
    }
    //Ư���� �������� ���� �ǰ�
    public void hitted_SP(int a)
    {
        hitted_material_timer = hitted_material_time;
        Gamemanager.GM.get_combo();
        hitted_forced_sp(1f);
      
 
        this.gameObject.GetComponent<Enemy_UI>().Hit();
       
        if (Player_status.p_status.critical() )
        {

            damaged = character_lose_health(Mathf.RoundToInt((a * Player_status.p_status.get_critical_damage())), DNP, this.transform);
        }
        else
        {
            damaged = character_lose_health(a, DNP, this.transform);
        }
        Gamemanager.GM.game_ev.when_Enemy_hitted(damaged, this);
        sentinal = true;
        progressBar.SetValue(Health_point, max_hp, true);

        record.Damge += a;
    }
    //��� �ڽ��� �θ�ü���Լ� �׷� Ŭ������ �O�� �� ���� �����ϰ� �÷��׾�� �� ��ġ�� ������Ų �� �ı� ����Ʈ�� �����ϰ� �ı�
    public void death()
    {
        if (!death_chk)
        {
            e_active = false;
            death_chk = true;
            Enemy_status e = GetComponent<Enemy_status>();
            int Total = e.get_money();
            Gamemanager.GM.game_ev.when_get_money(Total);
            Font_manager.DN.SpawnNumber(13, Total, this.gameObject.transform);
            record.EnemyKill++;
            this.gameObject.GetComponent<Enemy_UI>().DestoryUI();
            var par = Gamemanager.GM.destroy_particle_pulling();
            par.transform.position = this.transform.position;
            par.SetActive(true);
            Instantiate(Destroy_effect, this.transform.position, Quaternion.identity);

            if (this.transform.parent.parent.GetComponent<Enemy_group>() != null)
            {
                Enemy_group a = this.transform.parent.parent.GetComponent<Enemy_group>();
                a.enemy.Remove(this.transform.parent.gameObject);
                Debug.Log("aaaaa");
            }
            
            Gamemanager.GM.game_ev.Enemy_death(this.transform);
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
    //ü�� Ȯ��
    public void HpCheack()
    {
        if (Health_point <= 0&& !death_chk)
        {
            death();

        }
    }

}

