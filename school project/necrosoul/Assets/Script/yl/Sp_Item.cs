using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sp_Item
{
    //�⺻����
    public int Foreignkey;    //�������ڵ�
    public Sprite Sprite;   //�̹���
    public string Rarity;      //���
    public string Name;     //�̸�
    public string Description;   //����


    public void ChangeSp_Item(Sp_Item Sp_item)
    {
        this.Foreignkey = Sp_item.Foreignkey;
        this.Rarity = Sp_item.Rarity;
        this.Sprite = Sp_item.Sprite;
        this.Name = Sp_item.Name;
        this.Description = Sp_item.Description;
    }

    public Sp_Item CreateSp_Item()
    {
        Sp_Item a = new Sp_Item();
        a.Foreignkey = this.Foreignkey;
        a.Rarity = this.Rarity;
        a.Sprite = this.Sprite;
        a.Name = this.Name;
        a.Description = this.Description;
        return a;
    }
}
