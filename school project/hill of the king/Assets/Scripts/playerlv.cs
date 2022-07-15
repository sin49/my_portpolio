using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerlv : MonoBehaviour//플레이어 레벨 경험치 능력치
{
    public int lv;//레벨
    public int exp;//경험치
    public int lvpoint;//포인트
    public int heart;//체력++++/이동속도-,공격력-
    public int blade;//공격력++탄속++/체력--,공격속도-
    public int wing;//이동속도++,재장전속도++/방어력+/체력--,탄창-
    public int storm;//공격속도++,탄창++/재장전속도-,탄속-
    // Start is called before the first frame update
    void Start()
    {
        lv = 1;
        
        lvpoint = 1;
    }

    // Update is called once per frame
    public void Update()
    {
        if (lv <= 5 && exp >= 100)
        {
            lv_up();
            exp -= 100;
        }else if (lv <= 10 && exp >= 150)
        {
            lv_up();
            exp -= 150;
        }else if (lv <= 15 && exp >= 200)
        {
            lv_up();
            exp -= 200;
        }else if (lv <= 19 && exp >= 250)
        {
            lv_up();
            exp -= 250;
        }else if (lv == 20)
        {
            exp = 0;
        }
    }
    public void heartup()//심장 레벨을 올린다
    {
        if (lvpoint <= 0||heart==10)
            return;
        lvpoint--;
        heart++;
    }
    public void bladup()//칼날 레벨을 올린다
    {
        if (lvpoint <= 0||blade==10)
            return;
        lvpoint--;
        blade++;
    }
    
  
    public void stormup()//폭풍 레벨을 올린다
    {
        if (lvpoint <= 0||storm==10)
            return;
        lvpoint--;
        storm++;
    }
    public void wingup()//날개 레벨을 올린다
    {
        if (lvpoint <= 0||wing==10)
            return;
        lvpoint--;
        wing++;
    }
    public void lv_up()//레벨이 올라간다
    {
        if (lv <= 20)
        {
            lv++;
            lvpoint++;
        }
    }
}
