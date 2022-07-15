using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionbutton : MonoBehaviour//optionui를 열고 닫는 함수 관련 스크립트
{
    public GameObject option_ui;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void optiion_on()//option 열기
    {
        option_ui.SetActive(true);
    }
    public void optiion_off()//option 닫기
    {
        option_ui.SetActive(false);
    }
}
