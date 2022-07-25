using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour//플레이어의 공격 클레스
{
    PlayerCharacter p_chr;
    public GameObject prefab;
    public GameObject Gun;
    public GameObject Shoot;
    Rigidbody2D rgd;
    Quaternion mouse_rotation;
    public Vector2 direction;
    float angle;
    public Player_animator p_anim;
    bool rotate_chk;
    public int direct;//-1or 1
    private bool FireState; // 미사일 발사 속도를 제어할 변수
    public float attack_buffer_time;
    public float attack_buffer_timer;
    public float hang_time;
    public float hang_timer;
    public Player_animator p_ani;
    
    private void Start()
    {
        rgd = this.GetComponent<Rigidbody2D>();
        p_chr = this.gameObject.GetComponent<PlayerCharacter>();
        p_chr.can_attack = true;
        if (p_ani == null)
         p_ani.transform.GetChild(1).GetComponent<Player_animator>();

       
        FireState = true;
        p_anim = GetComponentInChildren<Player_animator>();
    }
    private void FixedUpdate()
    {
        //인벤토리가 안 열려있고 공격 가능한 상태라면
        if (OpenDownInventory.check==false || p_chr.can_attack) 
        {
           //S mouse_point();
            if (Gamemanager.GM.can_handle)
            {
                // MouseAttack();
                // X_attack();
                melee_attack();
            }
        }
    }
    // 플레이어의 마우스 좌표 받기 (사용 안함)
    void mouse_point()
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        mouse_rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        MouseAttack();
    }

    //마우스 좌클릭을 사용해 마우스의 좌표 방향으로 공격 (사용 안함)
    void MouseAttack()  
    {
        Vector3 rotation_num=new Vector3(0,180,0);
        Quaternion rotation__=Quaternion.AngleAxis(180, Vector3.up);
        if (Player_status.p_status.get_volly())//단발식(클릭 한번=공격 한번)
        {
            if (FireState&&!Gamemanager.GM.map_mode)
            {
                if (Input.GetMouseButton(0))
                {
                    //StartCoroutine(FireCycleControl());
                    ShootBullet(Gun, Shoot,Player_status.p_status.get_atk(), Player_status.p_status.get_bullet_speed(),direction.normalized);
                    if (direction.x >= 0)
                    {
                        direct = 1;
                    }
                    else
                    {
                        direct = -1;
                    }
                    //플레이어가 바라보는 방향과 공격방향이 반대일 경우 플레이어의 스프라이트를 회전시킨다
                    if (direct != p_chr.direction)
                    {
                        if (p_chr.onground)
                            p_anim.attack_anim_mirror();
                        else
                            p_anim.air_attack_anim_mirror();
                    }
                    else
                    {
                        if (p_chr.onground)
                            p_anim.attack_anim();
                        else
                            p_anim.air_attack_anim();
                    }
                }
                
            }
        }
        else//연발식(클릭하는동안 공격)
        {
            if (FireState)
            {
                if (Input.GetMouseButtonDown(0))
                {
                   // StartCoroutine(FireCycleControl());
                    ShootBullet(Gun, Shoot, Player_status.p_status.get_atk(), Player_status.p_status.get_bullet_speed(),direction.normalized);
                    if (direction.x >= 0)
                    {
                        direct = 1;
                    }
                    else
                    {
                        direct = -1;
                    }
                    if (direct != p_chr.direction)
                    {
                        if (p_chr.onground)
                            p_anim.attack_anim_mirror();
                        else
                            p_anim.air_attack_anim_mirror();
                    }
                    else
                    {
                        if (p_chr.onground)
                            p_anim.attack_anim();
                        else
                            p_anim.air_attack_anim();
                    }
                }
               
            }
        }
    }
    void ShootBullet(GameObject Gun,GameObject Shoot,int damage,float speed,Vector2 direction)    //총 발사 (사용 안함)
    {
        Gun.transform.rotation = Gun.transform.rotation;
        var myInstance = ObjectPool.GetObject(Gun.transform,Shoot.transform,damage,speed,direction);
    }
    void ShootBullet( GameObject Shoot, int damage, float speed)    //총 발사 (사용 안함)
    {
        Gun.transform.rotation = Gun.transform.rotation;
        Vector2 v = new Vector2(1, 0);
        Vector2 dir = (Vector2)Shoot.transform.position;
        if (p_chr.direction == 1)
        {
            Vector2 dir_0 = (Vector2)Shoot.transform.position + v;
            dir = dir_0 - dir;
        }
        else
        {
            Vector2 dir_0 = (Vector2)Shoot.transform.position - v;
            dir = dir_0 - dir;
        }
        var myInstance = ObjectPool.GetObject( Shoot.transform, damage, speed,dir.normalized);
    }

  /*  IEnumerator FireCycleControl()
    { // 처음에 FireState를 false로 만들고
        var wait = new WaitForSeconds(Player_status.p_status.get_firedelay());

        FireState = false; // FireDelay초 후에
        yield return wait; //FireState를 true로 만든다.
        FireState = true;
    }*/
    void melee_attack()//근접 공격 키를 눌려서 에니메이션과 함깨 근접공격 판정을 가진 오브젝트 활성화
    {
        if (Input.GetKey(Key_manager.Keys[Key_manager.KeyAction.ATTACK]))
        {
            
            attack_buffer_timer = attack_buffer_time;
        }
        if (attack_buffer_timer > 0)
        {
            attack_buffer_timer -= Time.deltaTime;
        }
       
        if (attack_buffer_timer > 0 && !p_ani.sword_delay&&p_chr.can_attack)
        {

            if (p_chr.onground)
            {
                
                p_anim.sword_attack_anim();
            }
            else if (!p_chr.onground && Player_status.p_status.air_attack_num > 0)
            {
                p_anim.air_attack_anim();
                Player_status.p_status.air_attack_num--;
            }
            attack_buffer_timer = 0;
        }
    }
  
}
