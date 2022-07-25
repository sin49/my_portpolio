using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GameCharacter//적의 기본 행동을 처리하는 클레스(GameCharacter를 상속함)
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
        //변수와 이펙트를 설정
        if (Gamemanager.GM.Enemy_destroy_effect != null)
        {
            Destroy_effect = Gamemanager.GM.Enemy_destroy_effect;//파괴 이펙트 준비
        }
        this_material = this.transform.GetChild(1).GetComponent<SpriteRenderer>().material;
        can_forced = true;//피격시 밀려남
        if(hitted_force_list.Count>0)
            hitted_force = hitted_force_list[0];
        //기록,폰트(자신이 작업하지 않음)
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
       
        if (on_hiited_force&&onGround)//플레이어의 공격으로 플랫폼에서 떨어지지 않는다
        {//bottom_ray로 밀려날시 떨어지는지 아닌지 체크하고 떨어질 경우 밀려나지 않도록 한다
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
        //적의 피격을 처리
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
    //적이 근접공격에 맞았을 때 근접공격 내부 클레스의 melee_force의 값만큼 밀려나는 함수
    void hitted_forced_melee(melee_attack a,Vector3 d)
    {
        Debug.Log("근접에 의해 밀려남:" + a.melee_force);
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
    //특수한 방법 피해를 받았을 때 a만큼 뒤로 밀려나는 함수
    void hitted_forced_sp(float a)
    {
        on_hiited_force = true;
        rgd.velocity = Vector3.zero;
        rgd.AddForce(new Vector2(a, 0), ForceMode2D.Impulse);
    }
    //피격과 관련된 에니메이션을 에니메이터로부터 작동시킨다
    void hitted_animation()
    {
        if (this.transform.GetChild(1).GetComponent<Animator>() != null)
        {
            e_ani = this.transform.GetChild(1).GetComponent<Animator>();
            e_ani.SetTrigger("hitted");
            e_ani.SetTrigger("death");
        }
    }
   //근접공격에의해 피격당했을 때 대미지 처리+넉백을 처리한다
   public void hitted_melee(GameObject a,Collider2D col)
    {
        //근접 공격 a로부터 피해를 받지않은 상태라면
        if (!a.GetComponent<melee_attack>().E.Contains(this)&&!a.GetComponent<melee_attack>().disable_hit)
        {
            hitted_material_timer = hitted_material_time;
            var me = a.GetComponent<melee_attack>();

        me.E.Add(this);
            Gamemanager.GM.get_combo();
            var le = Player.transform.position - this.transform.position;
            //반대 방향으로 피격 당했다면 뒤를 본다
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

         
          
        //뒤로 밀려남
            if (can_forced)
            {
                hitted_forced_melee(me,Player.transform.position-this.transform.position);
            }
            Debug.Log("데미지 입힘"+ me);
            this.gameObject.GetComponent<Enemy_UI>().Hit();
            //근접공격에 설정된 데미지 만큼 피해를 받고 근접공격의 명중 이펙트를 활성화 시킨다
            //치명타 시 데미지*치명타 만큼 피해를 받는다
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
            else//아닐시 일반적인 데미지 처리
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
            //적의 체력의 수치에 따라 상태 갱신(체력바활성화,사망)
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
    //특수한 수단으로 인한 피격
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
    //사망 자신의 부모객체에게서 그룹 클레스를 찿아 이 적을 제거하고 플레잉어에게 돈 수치를 증가시킨 후 파괴 이펙트를 생성하고 파괴
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
    //체력 확인
    public void HpCheack()
    {
        if (Health_point <= 0&& !death_chk)
        {
            death();

        }
    }

}

