using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop_item : MonoBehaviour
{
    public int N_price;//normal
    public int R_price;//rare
    public int E_price;//epic
    public int SP_price;//special
    public bool sold;
    public bool discount;
    public Sprite sold_image;
    public Sprite item_image;
    public int price;
    SpriteRenderer s_r;
    GameObject buy_particle;
    AudioManage_Main m_audio;
    public enum element { normal, special, consumable };

    public element item_element;


    public Item i;



    public void set_item()//지정된 타입에 따라 특정 종류의 랜덤한 아이템을 생성한다
    {
        ItemDatabase.itemDatabase.rarity_list_initialize();
        switch (item_element)
        {

            case element.normal:
                i = ItemDatabase.itemDatabase.get_item_by_rarity(ItemDatabase.itemDatabase.item_list);
                break;

            case element.consumable:
                i = ItemDatabase.itemDatabase.get_item_by_rarity(ItemDatabase.itemDatabase.consumable_list, "uncommon");
                break;
            case element.special:
                i = ItemDatabase.itemDatabase.get_item_by_rarity(ItemDatabase.itemDatabase.sp_list);
                break;
        }
       

    }
    public void set_price(Item i)//생성된 아이템의 희귀도를 확인하고 그에 따라 가격을 정한다
    {
        if (item_element == element.consumable|| item_element == element.special)
        {
            price = i.Money;
        }
        else
        {
            switch (i.Rarity)
            {
                case "common":
                    price = N_price;
                    break;
                case "uncommon":
                    price = R_price;
                    break;
                case "rare":
                    price = E_price;
                    break;
            }
        }
        if (discount)
        {
            price = Mathf.RoundToInt(price * 0.5f);
        }
    }

    public bool discount_chk()//이 품목을 활인할지를 난수로 정한다
    {
        int rand = Random.Range(0, 100) ;
        if (rand < 20)
        {
            discount = true;
            return true;
        }
        else
        {
            discount = false;
            return false;
        }
    }
    public void refresh_item()//아이템을 새로고침 한다(아이템을 새로 생성,판매 ,활인 상태 초기화)
    {
        set_item();
       
        sold = false;
        discount = false;
    }
    public void buy_item()//아이템 구매
    {
        if (Player_status.p_status.Money >= price && !sold)//판매되지않았고 플레이어가 돈을 충분히 가지고 있을 때
        {
            m_audio.purchase();
            switch (i.ItemType)//구매한 품목의 종류에 따라 효과 적용
            {
                case 1://아이템
                    Gamemanager.GM.get_item(i);
                    break;
                case 2://특수 아이템
                    i.special_effect();
                    break;
                case 3://소모품
                    i.consumable_effect();
                    break;
            }
            Gamemanager.GM.game_ev.when_lose_money(price);//돈 소비
            sold = true;//이 오브젝트를 판매된 상태로 만든다
            if (buy_particle == null)//구매 이펙트 파티클 생성
            {
                GameObject a = Instantiate(Gamemanager.GM.money_used_particle.gameObject, this.transform);
                buy_particle = a;
                a.SetActive(false);
                ParticleSystem.MainModule d=a.GetComponent<ParticleSystem>().main;
                if (price > 30)
                {
                    d.maxParticles = 10;
                }
                else if (price > 20)
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
                if (price > 30)
                {
                    d.maxParticles = 10;
                }
                else if (price > 20)
                {
                    d.maxParticles = 6;
                }
                else
                {
                    d.maxParticles = 3;
                }
                buy_particle.SetActive(true);
            }
            i = null;
        }
    }
    void Start()
    {
        m_audio = AudioManage_Main.instance;
        s_r = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sold)
        {
            s_r.sprite = sold_image;
        }
        else
        {
            set_price(i);

            if (i != null)//판매 됐다면 이 오브젝트의 스프라이트를 변경한다
                item_image = i.Sprite;
            if(item_image!=null)
                s_r.sprite = item_image;
        }
            }
}
