using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProduce : MonoBehaviour
{   
    //�������� �����ϴ� �ڵ� (������ �����鿡 �� �ִ�)

    public Item item= new Item();  //������
    public ItemDatabase ItemDatabase;
    void Start()        //�������� �����ϸ� �������� �������� �����.
    {
        ItemDatabase=GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        if (this.gameObject.name == "Item(Clone)")
        {
            ItemCreate();
        }
    }

    public void ItemType()
    {
        int Ran = Random.Range(1, 100);      //1~10 ����ġ ���ϱ�

        if (1 <= Ran && 60 >= Ran)
        {
            item.ItemType = 1;
        }
        else if (61 <= Ran && 90 >= Ran)
        {
            item.ItemType = 2;
        }
        else if (91 <= Ran && 100 >= Ran)
        {
            item.ItemType = 3;
        }

        Debug.Log("������ ���: " + Ran);
    }

    public void ItemCreate()
    {
       /* if (item.ItemType == 1)
        {
            item=ItemDatabase.Nomal_Item[Random.Range(0,ItemDatabase.Nomal_Item.Count-1)].CreateItem();      //������ ����
            this.gameObject.GetComponent<SpriteRenderer>().sprite = item.Sprite;
        }
        else if (item.ItemType == 2)
        {
            item = ItemDatabase.Rare_Item[Random.Range(0, ItemDatabase.Rare_Item.Count-1)].CreateItem();      //������ ����
            this.gameObject.GetComponent<SpriteRenderer>().sprite = item.Sprite;
        }
        else if (item.ItemType == 3)
        {

            item = ItemDatabase.Epic_Item[Random.Range(0, ItemDatabase.Epic_Item.Count-1)].CreateItem();      //������ ����
            this.gameObject.GetComponent<SpriteRenderer>().sprite = item.Sprite;
        }*/

        item = ItemDatabase.item_list[Random.Range(0, ItemDatabase.item_list.Count - 1)].CreateItem();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = item.Sprite;
    }


    public void UpdataImage()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = item.Sprite;
    }
}
