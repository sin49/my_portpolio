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

    public bool choice;     //���õ� ���

    [SerializeField]
    Sp_Item sp_item=new Sp_Item();

    private void Start()
    {
       // Parent.SetActive(false);
    }
    private void OnEnable()
    {
        Debug.Log("���ھƾƾƾ� �ϰڽ��ϴپƾƾƾƾƤ��������������� ����� ������ ");
        sp_item = Sp_ItemDatabase.Sp_itemDatabase.Random_Sp_Item();
        Image.sprite = sp_item.Sprite;
        Name.text = sp_item.Name.ToString();
        Descrition.text = sp_item.Description.ToString();
    }

    private void OnDisable()
    {
        if (choice == false)   //���õ� �������� �ƴ϶�� �ٽ� ����Ʈ�� �ֱ�
        {
            Debug.Log("�� ���õ��� ���߾� ...");
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
            //Sp_ItemEffect.sp_itemeffect.Sp_Ef[sp_item.Foreignkey] = true;     ���� �׽�Ʈ x
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
