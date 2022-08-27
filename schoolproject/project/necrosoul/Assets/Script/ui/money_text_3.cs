using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class money_text_3 : MonoBehaviour//돈 소모 텍스트
{
    Text t;
    Color c;
    float a = 3;
    float Timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        c = t.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer < a)
        {
            Timer += Time.deltaTime;
        }
        c.a = 1 - (1 * (Timer / a));
        t.color = c;
        t.text = "-" + Gamemanager.GM.lastest_lose_money.ToString() + "G";//마지막으로 소모한 돈의 수치
        if (c.a == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void Timer_zero()
    {
        Timer = 0;
    }
    private void OnEnable()
    {
        Timer = 0;

    }
}
