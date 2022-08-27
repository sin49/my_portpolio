using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemCheck : MonoBehaviour//플레이어의 상호작용 처리 클레스
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
            if (Item_chk)//상점 아이템의 정보를 툴팁에 적용 시킨다
            {
                tooltip.SetActive(true);
                tooltip.transform.position = Camera.main.WorldToScreenPoint(tr.position + Vector3.up * tool_tip_pos_y);
                title_T.text = "『" + i.Name + "』";
                desc_T.text = i.Description;
                price_T.text = "＄" + price.ToString();
            }
            else if (refresh_chk)//새로고침의 정보를 툴팁에 적용시킨다
            {
                tooltip.SetActive(true);
                tooltip.transform.position = Camera.main.WorldToScreenPoint(tr.position + Vector3.up * tool_tip_pos_y);
                title_T.text = "새로고침";
                desc_T.text = "돈을 소비하여 품목을 새로고칩니다!";
                price_T.text = "＄" + price.ToString();
            }
            else
            {
                tooltip.SetActive(false);
            }
            if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.UP]))//상호작용을 시도했을 때
            {
                Collider2D[] Hits = Physics2D.OverlapBoxAll(Pos.position, boxSize, 0);//접촉한 오브젝트를 처리
                foreach (Collider2D Item in Hits)
                {
                  if (Item.gameObject.CompareTag("door"))//포탈
                    {
                        //다음 방으로 이동
                        if (Item.GetComponent<portallV2>().anim_check)
                        {
                            Item.GetComponent<portallV2>().move_player();
                            i_t.reset_ani();
                        }
                    }
                    else if (Item.gameObject.CompareTag("shop_item"))//상점 아이템
                    {
                        //아이템의 구매를 시도
                        var a = Item.GetComponent<shop_item>();
                        a.buy_item();
                    }
                    else if (Item.gameObject.CompareTag("shoprefresh"))//상점 새로고침
                    {
                        //상점의 새로고침을 시도
                        var a = Item.transform.parent.parent.GetComponent<new_shop>();
                        a.refresh();
                    }
                    else if (Item.gameObject.CompareTag("event"))//이벤트
                    {
                        //이벤트를 활성화
                        var a = Item.transform.GetComponent<Event_obj>();

                        a.Event_start();
                    }
                    else if (Item.gameObject.CompareTag("end_portal"))//다음 스테이지 포탈
                    {
                        //스테이지를 마무리
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
    //상점아이템<새로고침에 접촉했을 때 아이템 이름+효과+가격 등을 알리는 툴팁을 활성화 한다
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
    //툴팁을 비활성화 한다
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