using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour//아이템 데이터 베이스
{
    //이 스크립트는 다른 사람의 작업과 자신의 작업이 섞여있음
    //yl=다른사람 gt혹은 별다른 표시 없음=자신
    public static ItemDatabase itemDatabase; //싱글톤 yl

    [Header("Item")]//yl
    public List<Item> Nomal_Item;
    public List<Item> Rare_Item;
    public List<Item> Epic_Item;
    //gt
    public List<Item> item_list;//혼
    public List<Item> consumable_list;//소모품
    public List<Item> sp_list;//특수
    //gt
    public float normal_percent;
    public float rare_percent;
   
    //yl
    [Header("획득한 아이템 리스트")]
    public List<Item> GetItemList = new List<Item>();
    public bool Item_Have_Check;
    int GetItemCount;
    //gt
    List<Item> rarity_list=new List<Item>();
    Item it = new Item();
    int num;
    private void Update()
    {
        //아이템의 효과 적용 gt
        for (int i = 0; i < item_list.Count; i++)
        {
           
                if (item_list[i].num != 0)
                {
                Debug.Log(i + "아이템 적용중");
                    ItemEffect0.item0to10.effect(item_list[i]);
                }
            
        }

    }
    //yl
    public void Make_Get_item(Item i)
    {
        Debug.Log("아니 도대체 뭐가 문제야?" + i.Name);
        Item_Have_Check = false;
        num = 0;
        GetItemCount = 0;

        if (GetItemList != null)
        {
            for (int n = 0; n < GetItemList.Count; n++)     //인벤토리에 있는지 체크
            {
                if (GetItemList[n].Foreignkey == i.Foreignkey)
                {
                    num = n;
                    Item_Have_Check = true;
                }
            }
        }

        if(Item_Have_Check) //아이템창에 있다면
        {
            GetItemList[num].num++;
            ItemEffect0.item0to10.uneffect(GetItemList[num]);
            ItemEffect0.item0to10.effect(GetItemList[num]);
        }
        else //없다면
        {
            GetItemList.Add(i);
            GetItemCount = GetItemList.Count-1;
            GetItemList[GetItemCount].num++;
            ItemEffect0.item0to10.uneffect(GetItemList[GetItemCount]);
            ItemEffect0.item0to10.effect(GetItemList[GetItemCount]);
        }
    }

    //gt
    public void rarity_list_initialize()//아이템 생성에 사용된 특정 등급 리스트 초기화
    {
        for(int i = 0; i < rarity_list.Count; i++)
        {
            rarity_list.RemoveAt(0);
        }
    }
    public Item get_item_by_rarity(List<Item> i_list)//임의의 아이템 흭득(아이템 흭득 확률 각각 존재)
    {
        if (rarity_list.Count != 0)//리스트 초기화
        {
            rarity_list_initialize();
        }
        Item i=new Item();
        int rand = Random.Range(0, 100) + 1;//1~100까지
        if (rand<normal_percent)//커몬 등급
        {
            for(int n=0;n < i_list.Count; n++)
            {
                if (i_list[n].Rarity == "common")//db의 그 등급의 아이템을 리스트에 추가
                {
                    rarity_list.Add(i_list[n]);

                }
            }
        }else if (rand < normal_percent+rare_percent)//언커몬
        {
            for (int n = 0; n < i_list.Count; n++)
            {
                if (i_list[n].Rarity == "uncommon")
                {
                    rarity_list.Add(i_list[n]);

                }
            }
        }
        else//레어
        {
            for (int n = 0; n < i_list.Count; n++)
            {
                if (i_list[n].Rarity == "rare")
                {
                    rarity_list.Add(i_list[n]);

                }
            }
        }

        if (rarity_list.Count != 0)//리스트가 비지 않았다면 그 리스트 중에서 랜덤
        {
            rand = Random.Range(0, rarity_list.Count);
            
            return rarity_list[rand].CreateItem();
        }
        else//비었다면 완전 랜덤
        {
            rand = Random.Range(0, i_list.Count);
            return i_list[rand].CreateItem();
        }

        
    }
    public Item get_item_by_rarity_upper_rare(List<Item> i_list)//특정 등급의 아이템만 랜덤으로(언커몬 이상) 흭득 방식은 동일
    {
        if (rarity_list.Count != 0)
        {
            rarity_list_initialize();
        }
        Item i = new Item();
        int rand = Random.Range(0, 100) + 1;//1~100까지
        for (int n = 0; n < i_list.Count; n++)
        {
            if (i_list[n].Rarity == "uncommon" || i_list[n].Rarity == "rare")
            {
                rarity_list.Add(i_list[n]);

            }
        }
            if (rarity_list.Count != 0)
            {
                rand = Random.Range(0, rarity_list.Count);
                return rarity_list[rand].CreateItem();
            }
            else
            {
                rand = Random.Range(0, i_list.Count);
                return i_list[rand].CreateItem();
            }
        }
    
        public Item get_item_by_rarity(List<Item> i_list,string rarity)//특정 등급의 아이템만 흭득 흭득 방식은 동일
    {
        if (rarity_list.Count != 0)
        {
            rarity_list_initialize();
        }
        Item i = new Item();
        int rand = Random.Range(0, 100) + 1;//1~100까지
        if (rarity== "common")
        {
            for (int n = 0; n < i_list.Count; n++)
            {
                if (i_list[n].Rarity == "common")
                {
                    rarity_list.Add(i_list[n]);

                }
            }
        }
        else if (rarity == "uncommon")
        {
            for (int n = 0; n < i_list.Count; n++)
            {
                if (i_list[n].Rarity == "uncommon")
                {
                    rarity_list.Add(i_list[n]);

                }
            }
        }
        else
        {
            for (int n = 0; n < i_list.Count; n++)
            {
                if (i_list[n].Rarity == "rare")
                {
                    rarity_list.Add(i_list[n]);

                }
            }
        }
        if (rarity_list.Count != 0)
        {
            rand = Random.Range(0, rarity_list.Count);
            return rarity_list[rand].CreateItem();
        }
        else
        {
            rand = Random.Range(0, i_list.Count);
            return i_list[rand].CreateItem();
        }


    }
    private void Awake()
    {
        itemDatabase = this;
        CreateItem();
        
    }
    
    //yl
    public void CreateItem()
    {
        List<Dictionary<string, object>> Data = CSVReader.Read("ItemTree");
        
        for (int i = 0; i < Data.Count; i++)
        {
            it.Foreignkey = int.Parse(Data[i]["Foreignkey"].ToString());
            it.Sprite = Resources.Load(Data[i]["Image"].ToString(), typeof(Sprite)) as Sprite;  //이미지주기
            it.Name = Data[i]["Name"].ToString();
            it.Description = Data[i]["Description"].ToString();
            it.Rarity = Data[i]["Rarity"].ToString();
            it.ItemType =int.Parse(Data[i]["ItemType"].ToString());
            it.Money = int.Parse(Data[i]["Money"].ToString());
            Classify(it);
        }
    }
    //yl
    public void Classify(Item i)    //아이템 분류작업
    {
        if (i.ItemType == 1)
            item_list.Add(i.CreateItem());
        else if (i.ItemType == 2)
            sp_list.Add(i.CreateItem());
        else
            consumable_list.Add(i.CreateItem());


    }
   
}
