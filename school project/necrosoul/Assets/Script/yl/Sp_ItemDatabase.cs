using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sp_ItemDatabase : MonoBehaviour
{
    public static Sp_ItemDatabase Sp_itemDatabase;


    [Header("Sp_Item")]
    public List<Sp_Item> Sp_Nomal_Item;
    public List<Sp_Item> Sp_Rare_Item;
    public List<Sp_Item> Sp_Epic_Item;
    public List<Sp_Item> Sp_SoldOut_Item;
    public List<Sp_Item> Sp_item_all;

    [Header("ItemCode")]
    [SerializeField] public int Sp_Nomal = 0;
    [SerializeField] public int Sp_Rare = 100;
    [SerializeField] public int Sp_Epic = 200;
    [SerializeField] public int Sp_End = 300;


    Sp_Item it = new Sp_Item();

    int RandomNum;      //랜덤수
    public int Sp_Item_total;   //총 개수

    [Header("각 등급의 확률")]
    [SerializeField]
    float nomal=50f;
    [SerializeField]
    float rare=30f;
    [SerializeField]
    float epic=20f;
    float percent;


    private void Awake()
    {
        Sp_itemDatabase = this;
        CreateSpItem();
    }


    public void CreateSpItem()
    {
        List<Dictionary<string, object>> Data = CSVReader.Read("Sp_item");

        for (int i = 0; i < Data.Count; i++)
        {
            it.Foreignkey = int.Parse(Data[i]["Foreignkey"].ToString());
            it.Sprite = Resources.Load(Data[i]["Image"].ToString(), typeof(Sprite)) as Sprite;  //이미지주기
            it.Name = Data[i]["Name"].ToString();
            it.Description = Data[i]["Description"].ToString();
            it.Rarity = Data[i]["Rarity"].ToString();
            Sp_Item_total++;
            
            Classify(it.CreateSp_Item());
        }

        for (int i = 0; i < Data.Count; i++)
        {
            
        }
    }


    public void Classify(Sp_Item i)    //아이템 분류작업
    {
        Sp_item_all.Add(i);
        if (i.Rarity=="nomal")
        {
            Sp_Nomal_Item.Add(i);
        }
        else if (i.Rarity == "rare")
        {
            Sp_Rare_Item.Add(i);
        }
        else if (i.Rarity == "epic")
        {
            Sp_Epic_Item.Add(i);
        }
        else                        //특성이 전부 떨어졌을 경우
        {
            Debug.Log("뽑을게 없다");
        }
        
    }

    public Sp_Item Random_Sp_Item(bool infinity=false)
    {
        if (!infinity) 
        {
            percent = Random.Range(0f, 100f);
        }
        else
        {
            percent = 0;
        }

        Sp_Item item = new Sp_Item();

        
        if (percent < nomal  && Sp_itemDatabase.Sp_Nomal_Item.Count!=0)              //노말뽑
        {
            RandomNum = Random.Range(0, Sp_itemDatabase.Sp_Nomal_Item.Count);
            item = Sp_itemDatabase.Sp_Nomal_Item[RandomNum];
            Sp_itemDatabase.Sp_Nomal_Item.RemoveAt(RandomNum);
            Debug.Log(percent + "확률로 " + item.Name + "을 뽑으셨습니다."+"노말");
            return item;
        }
        else if ((percent < nomal+rare && Sp_itemDatabase.Sp_Rare_Item.Count != 0) || (Sp_itemDatabase.Sp_Nomal_Item.Count == 0&& Sp_itemDatabase.Sp_Rare_Item.Count != 0))      //레어뽑
        {
            RandomNum = Random.Range(0, Sp_itemDatabase.Sp_Rare_Item.Count);
            item = Sp_itemDatabase.Sp_Rare_Item[RandomNum];
            Sp_itemDatabase.Sp_Rare_Item.RemoveAt(RandomNum);
            Debug.Log(percent + "확률로 " + item.Name + "을 뽑으셨습니다."+"레어");
            return item;
        }
        else if ((percent < 100 && Sp_itemDatabase.Sp_Epic_Item.Count != 0) || (Sp_itemDatabase.Sp_Rare_Item.Count == 0&& Sp_itemDatabase.Sp_Epic_Item.Count != 0))      //에픽뽑
        {
            RandomNum = Random.Range(0, Sp_itemDatabase.Sp_Epic_Item.Count);
            item = Sp_itemDatabase.Sp_Epic_Item[RandomNum];
            Sp_itemDatabase.Sp_Epic_Item.RemoveAt(RandomNum);
            Debug.Log(percent + "확률로 " + item.Name + "을 뽑으셨습니다."+"에픽");
            return item;
        }
        else if(!infinity)   //반복전
        {
            return Random_Sp_Item(true);
        }
        else
        {
            item = Sp_itemDatabase.Sp_SoldOut_Item[0];
            return item;
        }
    }
}


