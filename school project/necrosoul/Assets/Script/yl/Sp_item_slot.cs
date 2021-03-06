using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sp_item_slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject Parent;
    public Image Image;
    public Text Name;
    public Text Descrition;

    public bool choice;     //識澱吉 企雌

    [SerializeField]
    Sp_Item sp_item=new Sp_Item();

    private void Start()
    {
       // Parent.SetActive(false);
    }
    private void OnEnable()
    {
        Debug.Log("獣切焼焼焼焦 馬畏柔艦陥焼焼焼焼焼たたたたたたたた 什凪屡 焼戚奴 ");
        sp_item = Sp_ItemDatabase.Sp_itemDatabase.Random_Sp_Item();
        Image.sprite = sp_item.Sprite;
        Name.text = sp_item.Name.ToString();
        Descrition.text = sp_item.Description.ToString();
    }

    private void OnDisable()
    {
        if (choice == false)   //識澱吉 焼戚奴戚 焼艦虞檎 陥獣 軒什闘拭 隔奄
        {
            Debug.Log("貝 識澱鞠走 公梅嬢 ...");
            Sp_ItemDatabase.Sp_itemDatabase.Classify(sp_item);
        }
        choice = false;
        sp_item = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            choice = true;
            Gamemanager.GM.sp_item_check=true;
            Gamemanager.GM.can_handle = true;
            //Sp_ItemEffect.sp_itemeffect.Sp_Ef[sp_item.Foreignkey] = true;     焼送 砺什闘 x
            Parent.SetActive(false);
        }
    }

    public void SelectItem()
    {
        choice = true;
        Gamemanager.GM.sp_item_check = true;
        Gamemanager.GM.can_handle = true;
        Sp_ItemEffect.sp_itemeffect.Sp_Ef[sp_item.Foreignkey-1] = true;
        Sp_ItemEffect.sp_itemeffect.Sp_item_HaveCheck();
        Parent.SetActive(false);

    }
}
