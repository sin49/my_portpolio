using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemCheck : MonoBehaviour
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
            if (Item_chk)
            {
                tooltip.SetActive(true);
                tooltip.transform.position = Camera.main.WorldToScreenPoint(tr.position + Vector3.up * tool_tip_pos_y);
                title_T.text = "��" + i.Name + "��";
                desc_T.text = i.Description;
                price_T.text = "��" + price.ToString();
            }
            else if (refresh_chk)
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
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]))
            {
                Collider2D[] Hits = Physics2D.OverlapBoxAll(Pos.position, boxSize, 0);
                foreach (Collider2D Item in Hits)
                {
                    if (Item.gameObject.CompareTag("TreasureBox"))      //������ üũ
                    {
                        Debug.Log("���� �浹��");
                        Item.GetComponent<chest>().set_open();
                    }
                    else if (Item.gameObject.CompareTag("Item"))
                    {
                        if (Inventory.Item_InvenData.Count < Inven.GetComponent<Inventory>().Item_InvenDataCount)  //�κ� �ڸ��� ���� ��
                        {
                            Inventory.Item_InvenData.Add(Item.GetComponent<ItemProduce>().item);    //������ �߰�
                            Inven.GetComponent<Inventory>().Refresh();      //�󽽷Կ� ������ �ֱ�
                            Destroy(Item.gameObject);
                        }
                        else
                        {
                            Debug.Log("�� á���ϴ�.");
                        }
                    }
                    else if (Item.gameObject.CompareTag("door"))
                    {
                        if (Item.GetComponent<portallV2>().anim_check)
                        {
                            Item.GetComponent<portallV2>().move_player();
                            i_t.reset_ani();
                        }
                    }
                    else if (Item.gameObject.CompareTag("shop_item"))
                    {
                        var a = Item.GetComponent<shop_item>();
                        a.buy_item();
                    }
                    else if (Item.gameObject.CompareTag("shoprefresh"))
                    {
                        var a = Item.transform.parent.parent.GetComponent<new_shop>();
                        a.refresh();
                    }
                    else if (Item.gameObject.CompareTag("event"))
                    {
                        var a = Item.transform.GetComponent<Event_obj>();

                        a.Event_start();
                    }
                    else if (Item.gameObject.CompareTag("end_portal"))
                    {
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
        if(collision.gameObject.CompareTag("shoprefresh"))
        {
            var a = collision.transform.parent.parent.GetComponent<new_shop>();
          
                refresh_chk = true;

                
                tr = collision.transform;
            price = a.curren_re_price;
            
          
        }
    }
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
    private void OnDrawGizmos()     //�ӽ÷� �����ְ��ϴ� �Լ�
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Pos.position, boxSize);
    }
    /*private void OnTriggerStay2D(Collider2D Item)
    {

        if (Item.gameObject.CompareTag("TreasureBox"))      //������ üũ
        {
            Debug.Log("���� �浹��");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Item.GetComponent<chest>().set_open();
            }
        }

        if (Item.gameObject.CompareTag("Item"))      //������ üũ
        {
            Debug.Log("������ �浹��");
            if (Input.GetKeyDown(KeyCode.E))
            {
                //if (!Inventory.CheckItem(Item.GetComponent<ItemProduce>().item)) 
                //{
                    if (Inventory.Item_InvenData.Count < Inven.GetComponent<Inventory>().Item_InvenDataCount)  //�κ� �ڸ��� ���� ��
                    {
                        Inventory.Item_InvenData.Add(Item.GetComponent<ItemProduce>().item);    //������ �߰�
                        Inven.GetComponent<Inventory>().Refresh();      //�󽽷Կ� ������ �ֱ�
                        Destroy(Item.gameObject);
                    }
                    else
                    {
                        Debug.Log("�� á���ϴ�.");
                    }
            }
                //}
                //else
                //{
                //    Inventory.Money += 10;
                //    Destroy(Item.gameObject);
                //    Debug.Log("�ߺ�! �� ȹ��");
                //}
            
        


        
    }}*/
}

//if (Item.gameObject.CompareTag("TreasureBox"))      //������ üũ
//{
//    if (Input.GetKeyDown(KeyCode.E))
//    {
//        GameObject Item_D = Instantiate(prefab, new Vector3(Item.transform.position.x, Item.transform.position.y, Item.transform.position.z), Quaternion.identity);
//        int Ran = Random.Range(1, 6);
//        if (1 <= Ran && 3 >= Ran)
//        {
//            Item_D.GetComponent<Item>().ItemType = 1;
//        }
//        else if (4 <= Ran && 6 >= Ran)
//        {
//            Item_D.GetComponent<Item>().ItemType = 2;
//        }
//        Destroy(Item.gameObject);
//    }
//private void OnTriggerStay2D(Collider2D Item)
//{
//    if (Item.gameObject.CompareTag("Item"))      //������ üũ
//    {

//        if (Input.GetKeyDown(KeyCode.E))
//        {

//            Debug.Log("�κ� ����:" + Inventory.Item_InvenData.Count + " �� ���� : " + Inven.GetComponent<Inventory>().Item_InvenDataCount);
//            if (Inventory.Item_InvenData.Count < Inven.GetComponent<Inventory>().Item_InvenDataCount)  //�κ� �ڸ��� ���� ��
//            {
//                for (int i = 0; i < Inven.GetComponent<Inventory>().Item_InvenDataCount; i++)    //�󽽷� ã��
//                {
//                    if (Inven.GetComponent<Inventory>().InvenSlot[i].SlotCheck)      //���� �󽽷��̸�
//                    {
//                        Debug.Log("??");
//                        Item.GetComponent<ItemProduce>().item.SlotNumber = i;
//                        Inventory.Item_InvenData.Add(Item.GetComponent<ItemProduce>().item);    //������ �߰�
//                        break;
//                    }
//                }


//                Inven.GetComponent<Inventory>().Refresh();
//                Destroy(Item.gameObject);
//            }
//            else
//            {
//                Debug.Log("�� á���ϴ�.");
//            }
//        }
//    }
//    if (Item.gameObject.CompareTag("TreasureBox"))      //������ üũ
//    {

//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            Item.GetComponent<chest>().set_open();
//        }
//    }
//}