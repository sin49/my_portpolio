using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class new_shop : MonoBehaviour//상점
{
    public Transform[] item_create_pos=new Transform[5];//아이템 생성 위치
    
    public GameObject refresh_obj;//새로고침
    public int re_price;
   public int curren_re_price;
    public int discount_max_num;
    int discount_num;
    int re_price_add;
    public Transform item_list;
    public GameObject shopitem;
    public int N_num;
    public int C_num;
    public int S_num;
    public List<GameObject> consumable_intansi=new List<GameObject>();
    public List<GameObject> item_intansi = new List<GameObject>();
    public List<GameObject> sp_item_intansi = new List<GameObject>();
    public List<GameObject> total_item = new List<GameObject>();
    GameObject choose_obj;
    private GameObject buy_particle;

    void create_item()//상점 아이템을 생성한다
    {
        for(int i = 0; i < item_create_pos.Length; i++)//N_num+C_num+s_num=item_create_pos.Length
        {

            if (i < C_num)//소모품을 생성한다
            {
                choose_obj = item_pulling(consumable_intansi);//미리 생성된 랜덤 리스트에서 풀링을 통해 구현
               
            }else if (i < C_num + N_num)//아이템을 생성한다
            {
                choose_obj = item_pulling(item_intansi);
            }
            else if (i < C_num + N_num+S_num)//특수 아이템을 생성한다
            {
                choose_obj = item_pulling(sp_item_intansi);
            }
            else
            {

            }
            choose_obj.transform.position = item_create_pos[i].position;
            if (discount_num != discount_max_num)//상점아이템 중 일부를 활인한다
            {
                if (choose_obj.GetComponent<shop_item>().discount_chk())
                {
                    discount_num++;
                }
            }

            choose_obj.GetComponent<shop_item>().set_item();
            choose_obj.SetActive(true);


        }
    }
    GameObject item_pulling(List<GameObject> l)//아이템 오브젝트 풀링
    {
        for(int i = 0; i < l.Count; i++)
        {
            if (!l[i].activeSelf)
            {
                return l[i];
            }
        }
        return null;
    }
    void Start()
    {
        curren_re_price = re_price;
        re_price_add = Mathf.RoundToInt(re_price);
        //상점에 판매될 아이템을 미리 생성하고 풀링한다(새로고침을 시도할 때 원할히 구현할 수 있도록 풀링을 사용)
        for(int i = 0; i < 3; i++)//소모품
        {
            var a = Instantiate(shopitem, item_list);
            var b = a.GetComponent<shop_item>();
            b.item_element = shop_item.element.consumable;
            consumable_intansi.Add(a);
            total_item.Add(a);
            a.SetActive(false);
           
        }
        for (int i = 0; i < 3; i++)//아이템
        {
            var a = Instantiate(shopitem, item_list);
            var b = a.GetComponent<shop_item>();
            b.item_element = shop_item.element.normal;
            item_intansi.Add(a);
            total_item.Add(a);
            a.SetActive(false);
        }
        for (int i = 0; i < 2; i++)//특수 아이템
        {
            var a = Instantiate(shopitem, item_list);
            var b = a.GetComponent<shop_item>();
            b.item_element = shop_item.element.special;
            sp_item_intansi.Add(a);
            total_item.Add(a);
            a.SetActive(false);
        }
        create_item();
        GameObject m_pos = Instantiate(Gamemanager.GM.shop_minimap_pos, this.transform);//상점 위치를 미니맵에 표시
        m_pos.transform.position = this.transform.position;
    }
    public void refresh()//새로고침
    {
        if (Player_status.p_status.Money >= curren_re_price)//플레이어의 돈이 지정된 새로로고침 비용 이상일 때
        {
            discount_num = 0;//활인 초기화
  
            for (int i = 0; i < total_item.Count; i++)//모든 아이템에 refresh_item을 실행
            {
                if (total_item[i].activeSelf)
                {
                    total_item[i].GetComponent<shop_item>().refresh_item();
                    if (discount_num != discount_max_num)//새로 활인을 지정
                    {
                        if (total_item[i].GetComponent<shop_item>().discount_chk())
                        {
                            discount_num++;
                        }
                    }
                }
            }
            Gamemanager.GM.game_ev.when_lose_money(curren_re_price);//세로 고침 비용 만큼 돈을 소모
            if (buy_particle == null)//구매 이펙트 파티클을 활성화
            {
                GameObject a = Instantiate(Gamemanager.GM.money_used_particle.gameObject, this.transform);
                buy_particle = a;
                a.SetActive(false);
                ParticleSystem.MainModule d = a.GetComponent<ParticleSystem.MainModule>();
                //소모 비용에 따라 파티클 수가 달라짐
                if (curren_re_price > 30)
                {
                    d.maxParticles = 10;
                }
                else if (curren_re_price> 20)
                {
                    d.maxParticles = 6;
                }
                else
                {
                    d.maxParticles = 3;
                }
                a.transform.position = this.transform.position;
                a.SetActive(true);
            }
            else
            {
                if (buy_particle.activeSelf)
                {
                    buy_particle.SetActive(false);
                }
                ParticleSystem.MainModule d = buy_particle.GetComponent<ParticleSystem.MainModule>();
                if (curren_re_price > 30)
                {
                    d.maxParticles = 10;
                }
                else if (curren_re_price > 20)
                {
                    d.maxParticles = 6;
                }
                else
                {
                    d.maxParticles = 3;
                }
                buy_particle.SetActive(true);
            }
            //새로고침 비용이 증가한다
            curren_re_price = re_price_add;
            re_price_add = Mathf.RoundToInt(curren_re_price);

        }
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
