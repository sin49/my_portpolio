using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour//적,플레이어 등의 게임 상의 객체의 부모 클래스
{
    public bool death_check;//사망 체크
    public List<bad_status> B_status = new List<bad_status>();
   public int b_status_count;
    public int Health_point;
    public int max_hp;
    public int Attack_point;
    public int Defense_point;
    public float move_speed = 1.0f;//이동속도
    public bool untouchable_state;
    public float untouchable_timer = 0;
    public bool can_attack;
    public bool can_move;
    public int direction = 1;//캐릭터의 방향 1, -1 만 허용

    public bool attack_trigger;
    public bool attacked_trigger;
    /// <bad_status>
    public void bad_status()//상태이상을 처리한다
    {
        for (int i = 0; i < B_status.Count; i++)
        {
            if (B_status[i].status_timer > 0)//상탱이상의 지속시간 동안 효과를  받는다
            {
                status_effect(B_status[i]);
            }
            else//지속시간이 끝나면  효과를 끝낸다
            {
                status_uneffect(B_status[i]);
            }
        }
    }
    public bad_status get_bad_status(bad_status b)//상태이상을 받기
    {
        if (this.GetComponent<PlayerCharacter>() != null)//플레이어 대상
        {
            if (Gamemanager.GM.game_ev.when_player_get_bad_status())//플레이어가 상태이상을 받는 게임 이벤트
            {
                for (int i = 0; i < B_status.Count; i++)
                {
                    if (b.status_num == B_status[i].status_num)//이미 같은 상태이상을 받은 경우
                    {
                        return null;//무효
                    }
                }
                B_status.Add(b.copy_bad_status());//지정된 상태이상을 받는다
                return b;
            }
            else
            {//특수 요인으로 상태이상이 무효화 처리
                return null;
            }
        }
        else//적 대상
        {
            
            for (int i = 0; i < B_status.Count; i++)
            {
                if (b.status_num == B_status[i].status_num)//이미 같은 상태이상을 받은 경우 무효
                {
                    return null;
                }
            }
            B_status.Add(b.copy_bad_status());
            return b;
        }
        }
    public void status_effect(bad_status b)//상태이상의 효과
    {

        switch (b.status_num)//status_num=상태이상의 종류
        {
            case 0://0번 구속
                binding_effect(b);
                break;
        }
        if (b.b_status_effect == null)//상태이상의 이펙트 처리
        {
            var b_ef= Instantiate(Gamemanager.GM.bad_status_effect[b.status_num], this.gameObject.transform);//상태이상의 종류에 맞는 이펙트를 캐릭터 위치로 생성
            b_ef.transform.position = this.transform.position;
            b.b_status_effect = b_ef;
        }
    }
    public void status_uneffect(bad_status b)//상태이상의 효과를 없애고 해재한다
    {
        if (b.b_status_effect != null)//상태이상 이펙트를 제거한다
        {
            Destroy(b.b_status_effect);
        }
        switch (b.status_num)//상태이상의 효과를 없앤다
        {
            case 0:
                binding_uneffect(b);
                break;
        }

    }
    public void binding_effect(bad_status b)//구속 효과
    {
        //이동 불가,대시 불가, 공격은 가능
        Rigidbody2D rgd = this.GetComponent<Rigidbody2D>();
        rgd.velocity = Vector3.zero;
        can_move = false;
        Debug.Log("구속효과중");
        b.status_timer -= Time.deltaTime;
    }
    public void binding_uneffect(bad_status b)//구속 효과 해재
    {
        //이동 가능
        can_move = true;
        B_status.Remove(b);//상태이상 리스트에서 제거
    }
    /// 
    void Start()
    {
        
        
    }
    
    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //변수 값 설정
        b_status_count = B_status.Count;
        if (Defense_point > 100)
            Defense_point = 100;
        if (Health_point >= max_hp)
            Health_point = max_hp;
        if (Health_point > 0)
        {
            bad_status();//상태이상 처리
          
        }
    }
    public void direction_change()//방향바꾸기 좌 우
    {
        direction *= -1;
        transform.Rotate(0, 180, 0);
        if (transform.rotation.y > 360)
            transform.Rotate(0, -360, 0);
    }
   
    public void direction_change_spr()//방향바꾸기 (스프라이트만)
    {
        direction *= -1;
        Transform spr = this.gameObject.transform.GetChild(1);
        spr.Rotate(0, 180, 0);
        if (spr.rotation.y > 360)
            spr.Rotate(0, -360, 0);
    }
    public int character_lose_health(int dmg, GameObject DNP, Transform Tr)  //피격시 체력을 잃는 계산식
    {
        if (!untouchable_state)//뭑이 아닐 때
        {

            attacked_trigger = true;
            float damage_lose = Defense_point / 100;
            int damage = Mathf.RoundToInt(dmg * (1.0f - damage_lose));//데미지 계산식
            if (damage == 0)
            {
                damage = 1;
            }
            if ( Health_point > 0)
                Font_manager.DN.SpawnNumber(3, dmg, Tr);
            Health_point = Health_point - damage;//체력을 데미지만큼 감소
            attacked_trigger = false;
            return damage;
        }
        else
        {
            return 0;
        }
        
    }
   

    
   

}
public class bad_status//상태이상 클레스
{
    public int status_num;//스테이터스 종류
    public float status_timer;//남은 시간
    public float status_time;//스테이터스 시간
    public GameObject b_status_effect;
                             // public string status_name=""; //상태이상 이름
                             // public Sprite status_image;//상태이상 이미지
    public bad_status(int i, float time)//상태이상의 종류와 시간을 정하는 생성자
    {
        status_num = i;
        status_time = time;
        status_timer = status_time;
    }
    public bad_status copy_bad_status()//상태이상을 복사한다= 캐릭터가 상태이상에 걸리게한다
    {
        bad_status b=new bad_status(this.status_num, this.status_time);
        
        return b;
    }
}
