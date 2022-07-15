using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class s_ability_UI : MonoBehaviour//특수능력의 사용중과 재사용 대기시간을 알리는 ui
{
    public Image cool;
    public Image use;
    public playercontroler p_controler;
    public int index;
    public float y = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p_controler.s_ability[index].ability_use)//사용중
        {
            use.gameObject.SetActive(true);
            use.rectTransform.sizeDelta=new Vector2(use.rectTransform.sizeDelta.x,y * (p_controler.s_ability[index].effect_time - p_controler.abilitty_time) / p_controler.s_ability[index].effect_time);
        }
        else
        {
            use.gameObject.SetActive(false);
        }
        if (p_controler.ability_cool_down)//쿨다운 상태
        {
            cool.gameObject.SetActive(true);
            cool.rectTransform.sizeDelta = new Vector2(use.rectTransform.sizeDelta.x, y * (p_controler.s_ability[index].cool_time - p_controler.abilitty_time) / p_controler.s_ability[index].cool_time);
        }
        else
        {
            cool.gameObject.SetActive(false);
        }
    }
}
