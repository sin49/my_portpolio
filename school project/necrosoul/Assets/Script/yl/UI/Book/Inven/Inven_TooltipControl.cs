using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inven_TooltipControl : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public Tooltip tooltip;

    private void Awake()
    {
        tooltip = GameObject.Find("Inven_Canvas").gameObject.transform.Find("Tooltip").GetComponent<Tooltip>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Item item = GetComponent<Slot>().item;
        if(GetComponent<Slot>().FullCheck)
        {
            Debug.Log("����?"+item.Name+item.Rarity);
            tooltip.gameObject.SetActive(true);
            tooltip.SetupTooltip(item);
        }
        else
        {
            Debug.Log("����ִ� ���Դϴ�.");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false); 
    }

}
