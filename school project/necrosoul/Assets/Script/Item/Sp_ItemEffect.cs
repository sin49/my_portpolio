using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sp_ItemEffect : MonoBehaviour//특수 아이템 효과
{
    public static Sp_ItemEffect sp_itemeffect;

    public List<bool> Sp_Ef = new List<bool>();
    public List<int> Sp_have = new List<int>();

    [Header("특성변수들")]
    public float sp0_timer;
    public int sp0_dmg;
    public bool sp0_chk;
    public List< Unit> sp2_enemies=new List<Unit>();
    public Unit sp2_enemy;
    public GameObject sp2_effect;
    public float sp3_timer;
    public bool sp5_used;
    public int used_revive_item_num;
    public int sp2_dmg;
    public bool sp3_On;
    public int sp8_num;
    public int sp9_killenemy;
    public spEF_11_illusion EF_11_illusion;
    public spEF_11_illusion EF_11_illusion_instansi;
    public bool EF_11_room_chk;
    private void Awake()
    {
        sp_itemeffect = this;
    }
    private void FixedUpdate()
    {
        if (Sp_Ef[0])       //특수아이템 1번 타이머 작동
        {
            if (sp0_timer<10)
            {
                sp0_timer += Time.deltaTime;
            }
        }
        if (Sp_Ef[3] && !sp3_On)       //특수아이템 3번 (상태이상 보호)를 다시 활성화
        {
            if(sp3_timer<60)
                sp3_timer += Time.deltaTime;
            else
            {
                sp3_On=true;
                sp3_timer = 0;
            }
        
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Sp_ItemDatabase.Sp_itemDatabase.Sp_Item_total; i++)
        {
            Sp_Ef.Add(false);
        }
    }
    //특수아이템 여부 체크(자신이 작업하지 핞음)
    public void Sp_item_HaveCheck()
    {
        Sp_have.Clear();
        for (int i=0; i<Sp_Ef.Count;i++)
        {
            if(Sp_Ef[i]==true)
            {
                Sp_have.Add(i);
            }
        }
    }

    public int Sp_0(int dmg)//특수아이템 0번:일정시간 동안 공격 하지 안았을 때 다음 공격은 1.5배
    {
        if (Sp_Ef[0])       //흭득 했을 때 활성화
        {
            if (sp0_timer >= 10)//공격하지 않는 시간
            {
                sp0_dmg = Mathf.RoundToInt(Player_status.p_status.get_atk()*0.5f);
                SP_0_timer_initialize();
                sp0_dmg= dmg + sp0_dmg;
                
            }
            else
            {
                SP_0_timer_initialize();
                sp0_dmg = dmg;
            }
            return sp0_dmg;
        }
        else
        {
            return dmg;
        }
     
    }
   
    public void SP_0_timer_initialize()//특수아이템 0번 타이머 초기화(공격할 때 초기화)
    {
        if (Sp_Ef[0])       //획득시
        {
            sp0_timer = 0;
        }
    }
    public void Sp_1(Collider2D col)       //특수아이템 1번:공격이 적의 원거리 공격을 지운다
    {
        if (Sp_Ef[1])       //획득시
        {
            if (col.gameObject.CompareTag("Enemy_bullet"))//적의 공격과 충돌시 제거
            {
                Destroy(col.gameObject);
            }
        }
    }

    public void Sp_2(Collider2D col)//특수 아이템 2번:대쉬 했을 때 적과 충돌시 적을 통과하면서 데미지
    {
        if (Sp_Ef[2])       
        {
            
            if (col.gameObject.CompareTag("Enemy"))//적일 때
            {
                sp2_enemy = col.transform.parent.GetComponent<Unit>();
                if (!sp2_enemies.Contains(sp2_enemy))//공격 받지 않았다면
                {
                    Instantiate(sp2_effect, sp2_enemy.transform.position, Quaternion.identity);
                    sp2_enemies.Add(sp2_enemy);//피해를 받은 대상 리스트에 추가하여 중복 적용 방지
                    sp2_dmg = Mathf.RoundToInt(Player_status.p_status.get_atk() * 0.8f);//0.8배의 데미지
                    
                    sp2_enemy.hitted_SP(sp2_dmg);
                }
            }
        }
    }
    public void Sp_2_reset_list()//특수아이템 2번 리스트 초기화
    {
        int a = sp2_enemies.Count;
        for(int i = 0; i < a; i++)
        {
            sp2_enemies.RemoveAt(0);
        }
    }
    public bool Sp_3()//특수 아이템 3번:상태이상을 한번 막아주는 보호막 생성(방어 후 60초 후 회복)
    {
        if (Sp_Ef[3] && sp3_On)
        {
            sp3_On = false;
            return false;
        }
        else
        {
            return true;
        }
       

    }
    public void Sp_4(int a)      //특수 아이템 4번:적에게 준 데미지의 10% 만큼 체력 회복
    {
        if (Sp_Ef[4])
        {
            Player_status.p_status.set_hp(a / 10);
        }
    }
    public bool SP_5()//특수 아이템 5번:한 번 부활
    {
        if (Sp_Ef[5]&&!sp5_used)
        {
          
            used_revive_item_num = 5;
            return false;
        }
        else
        {
            return true;
        }
    }
    public void Sp_6(Item i)       //특수 아이템 6번: 아이템을 흭득할 때 한번에 2개 흭득한다
    {
        if (Sp_Ef[6])       //획득시
        {
            Gamemanager.GM.get_item_SP(i);
            
        }
        
    }

    public void Sp_8(Unit enemy)       //특수 아이템 8번:매우 낮은 확률로 적을 즉사
    {
        if (Sp_Ef[8]&&enemy.enemy_rank==0)
        {
            sp8_num = Random.Range(0, 10000);
            if (0 == sp8_num)
            {
                enemy.death();
            }
           
        }
       
    }
    public void Sp_9()       //특수 아이템 9번:흭득 후 죽인 적의 수에 비례하여 공격력 과 체력 보너스(20마리 간격)
    {
        if (Sp_Ef[9])
        {
            sp9_killenemy++;
            Player_status.p_status.set_spatk(sp9_killenemy / 20);
            Player_status.p_status.set_max_spHP(sp9_killenemy / 20);
        }
    }
   public void SP_11(Transform pos)//특수 아이템 11번:대쉬했을 때,공격 받았을 때 제자리에서 적의 시선을 집중시키는 미끼를 소환(전투당 한번)
    {
        if (!EF_11_room_chk&&Sp_Ef[11])
        {
            
            if (EF_11_illusion_instansi == null)
            {
                EF_11_illusion_instansi = Instantiate(EF_11_illusion.gameObject, pos.position, Quaternion.identity).GetComponent<spEF_11_illusion>();
            }
            else
            {
                EF_11_illusion_instansi.transform.position = pos.position;
                EF_11_illusion_instansi.gameObject.SetActive(true);
            }
            EF_11_room_chk = true;
        }
    }
    public void SP_11_room_chk()//미끼 사용을 갱신
    {
        if (Sp_Ef[11])
        {
            EF_11_room_chk = false;
        }
    }
}
