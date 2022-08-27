using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    static public List<Item> Item_InvenData = new List<Item>();    //�κ�
    static public List<Item> Use_InvenData = new List<Item>();     //���� ����

         //��

    public List<Slot> InvenSlot;           //�����ڸ�
    public List<Slot> UseInveSlot;         //�����ڸ�

    public int Item_InvenDataCount = 16;     //�κ��丮 ���� ����
    public int Use_InvenDataCount=8;      //���ǰ� �ִ� ���� ����

    public GameObject Item_get_UI;
    public List<Item> ui_item = new List<Item>();
    public List<Item_get_ui> ui_list = new List<Item_get_ui>();
    
    public void get_item(Item i)
    {
        InvenSlot[i.Foreignkey].item.num++;    //������ �߰�
        ui_item.Add(i);
       
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < InvenSlot.Count; i++)
        {
            if (InvenSlot[i] != null)
            {
                if (InvenSlot[i].item.num != 0)
                {
                    ItemEffect0.item0to10.effect(InvenSlot[i].item);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckList();
        }
        if (ui_list.Count == 0 && ui_item.Count != 0)
        {
            GameObject a = Instantiate(Item_get_UI);
            a.GetComponent<Item_get_ui>().item = ui_item[0];
            a.GetComponent<Item_get_ui>().inv = this;
            ui_item.RemoveAt(0);
            ui_list.Add(a.GetComponent<Item_get_ui>());
        }
    }


    public void Refresh()       //���� ������ ���Կ� ����ֱ�
    {
        
        for(int i=0;i< InvenSlot.Count;i++)     //���԰˻�
        {
            if(!InvenSlot[i].FullCheck)
            {
                InvenSlot[i].item = Item_InvenData[Item_InvenData.Count - 1];   //������ ã�� �ִ´� (���� �������� ���� ������)
                InvenSlot[i].ClassCheck();
                break;
            }
        }
    }

    public void Use_Refresh()       //���� ������ ���Կ� ����ֱ�
    {

        for (int i = 0; i < UseInveSlot.Count; i++)     //���԰˻�
        {
            if (!UseInveSlot[i].FullCheck)
            {
                UseInveSlot[i].item = Use_InvenData[Use_InvenData.Count - 1];   //������ ã�� �ִ´� (���� �������� ���� ������)
                UseInveSlot[i].ClassCheck();
                break;
            }
        }
    }

    public bool CheckItemOver(Item item)         //�ߺ�üũ
    {

        //for (int i = 0; i < Item_InvenData.Count; i++) �κ����x
        //{
        //    if (Item_InvenData[i].Foreignkey==item.Foreignkey) 
        //    {
        //        return true;
        //    }
          
        //}

        for (int i = 0; i < Use_InvenData.Count; i++)   
        {
            if (Use_InvenData[i].Foreignkey == item.Foreignkey)
            {
                return true;
            } 
        }

        return false;
    }

    

    public void Add_UseInvetory(Item item)
    {
        if (Use_InvenData.Count < Use_InvenDataCount)  //���� �ڸ��� ���� ��
        {
            int i = item.SlotNumber;
            Debug.Log("���Գѹ�:" + i);
            Use_InvenData.Add(item);    //������ �߰�
            /*Item_InvenData.Remove(item);
            InvenSlot[i].GetComponent<Image>().sprite = null;
            InvenSlot[i].item = null;*/
            ItemEffect0.item0to10.effect(item);
            Use_Refresh();      //�󽽷Կ� ������ �ֱ�
        }
        else
        {
            Debug.Log("�� á���ϴ�.");
        }
    }


    public bool CheckUseFull()
    {
        if(Use_InvenData.Count>=Use_InvenDataCount)
        {
            Debug.Log("��á��");
            return true;
        }
        return false;
    }



    //-----------�׽�Ʈ �� �Լ� 
    public void CheckList()     //����Ʈ Ȯ���ϴ� �Լ�
    {
        Debug.Log("�κ� ������");
        for (int i = 0; i < Item_InvenData.Count; i++)
        {

            Debug.Log("������ ������ ��ȣ: " + i + "����: " + Item_InvenData[i].SlotNumber + "�̸�" + Item_InvenData[i].Foreignkey + "�̹���" + Item_InvenData[i].Sprite);
        }
        Debug.Log("������� ������");
        for (int i = 0; i < Use_InvenData.Count; i++)
        {

            Debug.Log("������ ������ ��ȣ: " + i + "����: " + Use_InvenData[i].SlotNumber + "�̸�" + Use_InvenData[i].Foreignkey + "�̹���" + Use_InvenData[i].Sprite);
        }

    }

}
