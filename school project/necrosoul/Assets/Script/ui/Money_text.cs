using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Money_text : MonoBehaviour//돈 표시 텍스트(현재,흭득,소모)
{
    Text t;//현재 돈
    Text get_T;//흭득 돈
    Text lose_T;//돈 소모
    void Start()
    {
        t = GetComponent<Text>();
        get_T=transform.GetChild(0).GetComponent<Text>();
        lose_T = transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player_status.p_status.Money != 0)
        {
            t.text = GetThousandCommaText(Player_status.p_status.Money) + "G";
        }
        else
        {
            t.text =   "0G";
        }
        if (Gamemanager.GM.get_money_chk)
        {
            lose_T.gameObject.SetActive(false);
            get_T.gameObject.SetActive(true);
            get_T.GetComponent<money_text_2>().Timer_zero();
            Gamemanager.GM.get_money_chk = false;
        }
        if (Gamemanager.GM.lose_money_chk)
        {
            lose_T.gameObject.SetActive(true);
            get_T.gameObject.SetActive(false);
            lose_T.GetComponent<money_text_3>().Timer_zero();
            Gamemanager.GM.lose_money_chk = false;
        }
    }
    
    public string GetThousandCommaText(int data) { return string.Format("{0:#,###}", data); }

}
