using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{

    public Item item=null;
    public bool FullCheck=false;      //�ڸ��� �ִ��� üũ
    public bool UseSlot=false;      //������������ üũ
    public GameObject Inven;
    public GameObject ToolTip;
    public bool NullPlace = false;

    public Inventory InvenScirpt;

    [Header("������")]
    public int ItemCode;


    Color Cl;
    //���������� 100���� ����
    [SerializeField] int SlotNumber;


    private void Awake()
    {
        ToolTip = GameObject.Find("Inven_Canvas").gameObject.transform.Find("Tooltip").gameObject;
        Inven = GameObject.Find("InventorySystem");
        InvenScirpt = GameObject.Find("InventorySystem").GetComponent<Inventory>();
        if (!NullPlace)
        {
            if (UseSlot)
            {
                Debug.Log("��뽽�Կ� �ֽ��ϴ�" + SlotNumber);
                InvenScirpt.UseInveSlot[SlotNumber - 100]=this;
            }
            else
            {
                
                Debug.Log("�κ����Կ� �ֽ��ϴ�" + SlotNumber);
                InvenScirpt.InvenSlot[SlotNumber] = this;
            }
        }
        item = null;
    }
    private void Start()
    {
        if (!UseSlot)
        {
            ItemMake(ItemCode);
            ClassCheck();
            item.parent = this.gameObject;
        }

    }

    private void Update()
    {
        if (!NullPlace)
        {
            if (FullCheck)
            {
                Cl.r = 255 / 255f;
                Cl.g = 255 / 255f;
                Cl.b = 255 / 255f;
                Cl.a = 1;
                this.gameObject.GetComponent<Image>().color = Cl;
            }
            else
            {
                Cl.r = 101 / 255f;
                Cl.g = 74 / 255f;
                Cl.b = 89 / 255f;
                Cl.a = 1;
                this.gameObject.GetComponent<Image>().color = Cl;
            }
        }
        
    }
  
    public void ItemMake(int i)     //���Կ� ������ �����
    {
        //if (i<ItemDatabase.itemDatabase.Rare)
        //{
        //    item = ItemDatabase.itemDatabase.Nomal_Item[i];
        //}
        //else if (i<ItemDatabase.itemDatabase.Epic)
        //{
        //    item = ItemDatabase.itemDatabase.Rare_Item[i- ItemDatabase.itemDatabase.Rare];
        //}
        //else if(i<ItemDatabase.itemDatabase.End)
        //{
        //    item = ItemDatabase.itemDatabase.Epic_Item[i- ItemDatabase.itemDatabase.Epic];
        //}
        //else
        //{
        //    item = ItemDatabase.itemDatabase.item_list[i - ItemDatabase.itemDatabase.Nomal];
        //}
        item=ItemDatabase.itemDatabase.item_list[i];
    }

    public void ChoseItem(Item i)
    {
        item=i;
    }
    
    public void FullCheckManger()       //�巡�� ��� �� ���� �۾�
    {
        //���� �������� �ڱ⽽�Թ�ȣ�� �ִ´�.
        
        Debug.Log("�����ڸ�:"+this.gameObject.name);
        if (item == null)  //���� ������̸�
        {
            Debug.Log("??����2");
            FullCheck = false;   //��밡�� üũ
        }
        else
        {
            item.SlotNumber = SlotNumber;
            Debug.Log("??����333333");
            FullCheck = true;
        }
    }

    // Ŭ���� üũ�� ����üũ, �̹���, ���Թ�ȣ �ֱ�   �� �����Ű�� �ܰ�
    public void ClassCheck()
    {
        Item item = this.gameObject.GetComponent<Slot>().item;

        if (item==null)         //�������� ������
        {
            FullCheck = false;
        }
        else                    //������
        {
            FullCheck = true;
            this.gameObject.GetComponent<Image>().sprite = item.Sprite;
            item.SlotNumber = SlotNumber;
        }
    }

    public void UseAndItemChange()      //���� �ٲٴ� ��
    {

        if (item.SlotNumber >= 100) //���� �ڽ��� ���� ���Կ� �ִ�.
        {

            Debug.Log("����" + item.Sprite.name);
            Inventory.Use_InvenData.Add(item);      //����
            Inventory.Item_InvenData.Remove(item);  //�κ���������

        }
        else        //�ƴϴ�
        {
            Debug.Log("�κ�" + item.Sprite.name);
            Inventory.Item_InvenData.Add(item);     //�κ����Գֱ�
            Inventory.Use_InvenData.Remove(item);   //��������
        }
    }

    //------------------------------Ŭ���̺�Ʈ

    public PointerEventData.InputButton btn1 = PointerEventData.InputButton.Left;
    public PointerEventData.InputButton btn2 = PointerEventData.InputButton.Right;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == btn2)       //��Ŭ������ ��
        {
            if (item != null)
            {
                if (SlotNumber < 100)   //�κ������̶��
                {
                    if (!Inven.GetComponent<Inventory>().CheckUseFull()&&!item.Item_Useing && item.num>0)
                    {
                        Debug.Log("��Ŭ��");
                        item.Item_Useing = true;
                        Inven.GetComponent<Inventory>().Add_UseInvetory(item);
                        ItemEffect0.item0to10.uneffect(item);     //ȿ������
                        Debug.Log("ȿ���� ����ǰ� �ֽ��ϴ�.");
                        //ClassCheck();
                        ToolTip.gameObject.SetActive(false);
                        
                    }
                }
                else if (SlotNumber>=100)   //������ �������̶��
                {
                    Debug.Log("��Ŭ������ ���Ͽ� �������� ���� �Ǿ����ϴ�");
                    
                    //���� �����ۻ���
                    Inventory.Use_InvenData.Remove(item);
                    this.gameObject.GetComponent<Image>().sprite=null;
                    item.parent.GetComponent<Slot>().item.Item_Useing = false;
                    ItemEffect0.item0to10.uneffect(item);     //ȿ������
                    item = null;
                    ClassCheck();
                }
            }
            else
            {
                Debug.Log("��� ��Ŭ�� �ϴ���");
            }
        }
    }

    public int getslotnumber()
    {
        return SlotNumber;
    }
    public void Item_eqiq()
    {
        if (item != null)
        {
            if (SlotNumber < 100)   //�κ������̶��
            {
                if (!Inven.GetComponent<Inventory>().CheckUseFull() && !item.Item_Useing && item.num > 0)
                {
                    Debug.Log("��Ŭ��");
                    item.Item_Useing = true;
                    Inven.GetComponent<Inventory>().Add_UseInvetory(item);
                    ItemEffect0.item0to10.uneffect(item);     //ȿ������
                    Debug.Log("ȿ���� ����ǰ� �ֽ��ϴ�.");
                    //ClassCheck();
                    ToolTip.gameObject.SetActive(false);

                }
            }
            else if (SlotNumber >= 100)   //������ �������̶��
            {
                Debug.Log("��Ŭ������ ���Ͽ� �������� ���� �Ǿ����ϴ�");

                //���� �����ۻ���
                Inventory.Use_InvenData.Remove(item);
                this.gameObject.GetComponent<Image>().sprite = null;
                item.parent.GetComponent<Slot>().item.Item_Useing = false;
                ItemEffect0.item0to10.uneffect(item);     //ȿ������
                item = null;
                ClassCheck();
            }
        }
    }

}
