using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemCheck : MonoBehaviour//�÷��̾��� ��ȣ�ۿ� ó�� Ŭ����
{
    public GameObject Inven;
    public Transform Pos;
    public Vector2 boxSize;
    public GameObject tooltip;
    public item_text i_t;
    Text title_T;
    Text desc_T;
    Text price_T;
    Item i;
    Transform tr;
    public float tool_tip_pos_y;
    public bool Item_chk;
    public bool refresh_chk;
    int price;
    private void Start()
    {
        Inven = GameObject.Find("InventorySystem");
        title_T = tooltip.transform.GetChild(0).GetComponent<Text>();
        desc_T = tooltip.transform.GetChild(1).GetComponent<Text>();
        price_T = tooltip.transform.GetChild(2).GetComponent<Text>();
    }

    private void Update()
    {
        if (Gamemanager.GM.can_handle)
        {
            if (Item_chk)//���� �������� ������ ������ ���� ��Ų��
            {
                tooltip.SetActive(true);
                tooltip.transform.position = Camera.main.WorldToScreenPoint(tr.position + Vector3.up * tool_tip_pos_y);
                title_T.text = "��" + i.Name + "��";
                desc_T.text = i.Description;
                price_T.text = "��" + price.ToString();
            }
            else if (refresh_chk)//���ΰ�ħ�� ������ ������ �����Ų��
            {
                tooltip.SetActive(true);
                tooltip.transform.position = Camera.main.WorldToScreenPoint(tr.position + Vector3.up * tool_tip_pos_y);
                title_T.text = "���ΰ�ħ";
                desc_T.text = "���� �Һ��Ͽ� ǰ���� ���ΰ�Ĩ�ϴ�!";
                price_T.text = "��" + price.ToString();
            }
            else
            {
                tooltip.SetActive(false);
            }
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]))//��ȣ�ۿ��� �õ����� ��
            {
                Collider2D[] Hits = Physics2D.OverlapBoxAll(Pos.position, boxSize, 0);//������ ������Ʈ�� ó��
                foreach (Collider2D Item in Hits)
                {
                  if (Item.gameObject.CompareTag("door"))//��Ż
                    {
                        //���� ������ �̵�
                        if (Item.GetComponent<portallV2>().anim_check)
                        {
                            Item.GetComponent<portallV2>().move_player();
                            i_t.reset_ani();
                        }
                    }
                    else if (Item.gameObject.CompareTag("shop_item"))//���� ������
                    {
                        //�������� ���Ÿ� �õ�
                        var a = Item.GetComponent<shop_item>();
                        a.buy_item();
                    }
                    else if (Item.gameObject.CompareTag("shoprefresh"))//���� ���ΰ�ħ
                    {
                        //������ ���ΰ�ħ�� �õ�
                        var a = Item.transform.parent.parent.GetComponent<new_shop>();
                        a.refresh();
                    }
                    else if (Item.gameObject.CompareTag("event"))//�̺�Ʈ
                    {
                        //�̺�Ʈ�� Ȱ��ȭ
                        var a = Item.transform.GetComponent<Event_obj>();

                        a.Event_start();
                    }
                    else if (Item.gameObject.CompareTag("end_portal"))//���� �������� ��Ż
                    {
                        //���������� ������
                        var a = Item.transform.GetComponent<end_door>();
                        i_t.reset_ani();
                        a.End_portal();
                    }
                }
            }
        }
        else
        {
            tooltip.SetActive(false);
        }
    }
    //����������<���ΰ�ħ�� �������� �� ������ �̸�+ȿ��+���� ���� �˸��� ������ Ȱ��ȭ �Ѵ�
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("shop_item"))
        {
            var a = collision.GetComponent<shop_item>();
            if (!a.sold)
            {
                Item_chk = true;

                i = a.i;
                tr = a.transform;
                price = a.price;
            }
            else
            {
                Item_chk = false;
            }
        }
        if (collision.gameObject.CompareTag("shoprefresh"))
        {
            var a = collision.transform.parent.parent.GetComponent<new_shop>();

            refresh_chk = true;


            tr = collision.transform;
            price = a.curren_re_price;


        }
    }
    //������ ��Ȱ��ȭ �Ѵ�
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("shop_item"))
        {
            Item_chk = false;
            i = null;
            tr = null;
            price = 0;
        }
        if (other.gameObject.CompareTag("shoprefresh"))
        {


            refresh_chk = false;


            tr = null;
            price = 0;

        }
    }
    
}