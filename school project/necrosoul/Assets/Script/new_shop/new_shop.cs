using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class new_shop : MonoBehaviour//����
{
    public Transform[] item_create_pos=new Transform[5];//������ ���� ��ġ
    
    public GameObject refresh_obj;//���ΰ�ħ
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

    void create_item()//���� �������� �����Ѵ�
    {
        for(int i = 0; i < item_create_pos.Length; i++)//N_num+C_num+s_num=item_create_pos.Length
        {

            if (i < C_num)//�Ҹ�ǰ�� �����Ѵ�
            {
                choose_obj = item_pulling(consumable_intansi);//�̸� ������ ���� ����Ʈ���� Ǯ���� ���� ����
               
            }else if (i < C_num + N_num)//�������� �����Ѵ�
            {
                choose_obj = item_pulling(item_intansi);
            }
            else if (i < C_num + N_num+S_num)//Ư�� �������� �����Ѵ�
            {
                choose_obj = item_pulling(sp_item_intansi);
            }
            else
            {

            }
            choose_obj.transform.position = item_create_pos[i].position;
            if (discount_num != discount_max_num)//���������� �� �Ϻθ� Ȱ���Ѵ�
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
    GameObject item_pulling(List<GameObject> l)//������ ������Ʈ Ǯ��
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
        //������ �Ǹŵ� �������� �̸� �����ϰ� Ǯ���Ѵ�(���ΰ�ħ�� �õ��� �� ������ ������ �� �ֵ��� Ǯ���� ���)
        for(int i = 0; i < 3; i++)//�Ҹ�ǰ
        {
            var a = Instantiate(shopitem, item_list);
            var b = a.GetComponent<shop_item>();
            b.item_element = shop_item.element.consumable;
            consumable_intansi.Add(a);
            total_item.Add(a);
            a.SetActive(false);
           
        }
        for (int i = 0; i < 3; i++)//������
        {
            var a = Instantiate(shopitem, item_list);
            var b = a.GetComponent<shop_item>();
            b.item_element = shop_item.element.normal;
            item_intansi.Add(a);
            total_item.Add(a);
            a.SetActive(false);
        }
        for (int i = 0; i < 2; i++)//Ư�� ������
        {
            var a = Instantiate(shopitem, item_list);
            var b = a.GetComponent<shop_item>();
            b.item_element = shop_item.element.special;
            sp_item_intansi.Add(a);
            total_item.Add(a);
            a.SetActive(false);
        }
        create_item();
        GameObject m_pos = Instantiate(Gamemanager.GM.shop_minimap_pos, this.transform);//���� ��ġ�� �̴ϸʿ� ǥ��
        m_pos.transform.position = this.transform.position;
    }
    public void refresh()//���ΰ�ħ
    {
        if (Player_status.p_status.Money >= curren_re_price)//�÷��̾��� ���� ������ ���ηΰ�ħ ��� �̻��� ��
        {
            discount_num = 0;//Ȱ�� �ʱ�ȭ
  
            for (int i = 0; i < total_item.Count; i++)//��� �����ۿ� refresh_item�� ����
            {
                if (total_item[i].activeSelf)
                {
                    total_item[i].GetComponent<shop_item>().refresh_item();
                    if (discount_num != discount_max_num)//���� Ȱ���� ����
                    {
                        if (total_item[i].GetComponent<shop_item>().discount_chk())
                        {
                            discount_num++;
                        }
                    }
                }
            }
            Gamemanager.GM.game_ev.when_lose_money(curren_re_price);//���� ��ħ ��� ��ŭ ���� �Ҹ�
            if (buy_particle == null)//���� ����Ʈ ��ƼŬ�� Ȱ��ȭ
            {
                GameObject a = Instantiate(Gamemanager.GM.money_used_particle.gameObject, this.transform);
                buy_particle = a;
                a.SetActive(false);
                ParticleSystem.MainModule d = a.GetComponent<ParticleSystem.MainModule>();
                //�Ҹ� ��뿡 ���� ��ƼŬ ���� �޶���
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
            //���ΰ�ħ ����� �����Ѵ�
            curren_re_price = re_price_add;
            re_price_add = Mathf.RoundToInt(curren_re_price);

        }
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
