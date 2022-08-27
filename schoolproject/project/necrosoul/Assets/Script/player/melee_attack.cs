using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class melee_attack : MonoBehaviour//근접 공격 판정 클레스
{
    public ParticleSystem sword_effect;
    public bool set_effect_rotate;
   public float effect_rotation=-444;
    public int Damage;
    public int index = 0;
    public float melee_force;
    public bool Double_attack_on;
    public bool on_crit;
    public bool disable_hit;
    public List<Unit> E = new List<Unit>();
    
    private void Awake()
    {
        //이펙트를 생성한 후 파괴하지 않고 비활성화 시킴으로써 게임의 속도를 원할히 만든다
        if (sword_effect == null)//칼이 적에게 적중했을 때의 이펙트를 만든다
        {
            GameObject a = Instantiate(Gamemanager.GM.p_sword_effect.gameObject);
            sword_effect = a.GetComponent<ParticleSystem>();
            a.SetActive(false);//비활성화 시킨다.
            var b = a.GetComponent<p_sword_hitted_particle>();
            if (set_effect_rotate)//이펙트의 회전값을 직접 설정했다면 그 설정값으로 회전한다(안할시 랜덤한 각도로 회전ㄴ)
            {
                b.setting_rotation = effect_rotation;
            }
        }
    }
    private void OnEnable()
    {
       // this.transform.rotation = Quaternion.identity;
       
        switch (index)//플레이어의 공격력 값을 받아 이 근접 공격 유형에 적합한 데미지로 바꾼다 
        {
            case 0://플레이어 3타 콤보의 마지막
                Damage = Mathf.RoundToInt(Damage * 2f);
                break;
            default://그외

                break;

        }
    }
    private void FixedUpdate()
    {
        
    }
    private void OnDisable()
    {
        if (Double_attack_on)//이단 공격이 활성화 되있다면 이단 공격 판정을 실행한다
        {
            double_attack_system();
        }
        else
        {//적 중복 체크용 리스트를 초기화한다
            int n = E.Count;
            for (int i = 0; i < n; i++)
            {
                E.RemoveAt(0);
            }

        }
    }
   
    void double_attack_system()//이단 공격 시스템
    {
        //공격 판정을 제거, 속성을 초기화
        Double_attack_on = false;//이 근접공격의 이단 공격 설정을 초기화한다 
        disable_hit = true;
        //중복체크용 리스트를 받는다(중복 체크용 리스트=이 근접 공격에 명중한 적 리스트)
        int n = E.Count;
        for (int i = 0; i <n; i++)
        {
            //리스트 에 포함된 적에게 데미지 판정을 준다( 근접공격이 적중 하면서 한번 근접공격이 비활성화 될 때 한번으로 이단 공격을 만든다)
            if (Player_status.p_status.critical())
            {
                E[0].character_lose_health(Mathf.RoundToInt((Damage * Player_status.p_status.get_critical_damage())), E[i].DNP,gameObject.transform);
            }
            else
            {
                E[0].character_lose_health(Damage, E[i].DNP, gameObject.transform);
            }
            E.RemoveAt(0);
        }
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Gamemanager.GM.game_ev.P_Attack_col_effect(collision);//게임 이벤트를 관리하는 클레스에 근접공격이 명중했다는 이벤트를 보낸다
        
    }
}
