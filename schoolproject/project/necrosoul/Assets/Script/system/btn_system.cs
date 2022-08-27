using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class btn_system : MonoBehaviour//키보드로 작동시키는 ui 양식
{
    public List<Button> a = new List<Button>();//조작할 ui(버튼)을 list에 담기(순서대로)
    int select;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //select로 리스트에 들어간 버튼을 순차적으로 지정한다
    void Update()
    {
        BtnSystem(a);
    }
    void BtnSystem(List<Button> a)
    {
        for (int i = 0; i < a.Count; i++)//현재 선택중인 버튼을 그래픽 표시
        {
            if (i == select)//그래픽 요소가 없어서 interactable을 활용하여 그래픽 표시
            {
                if (a[i].IsInteractable() == true)//실 사용에 interactable은 안 건드는게 좋다!
                {
                    a[i].interactable = false;
                }

            }
            else
            {
                if (a[i].IsInteractable() == false)
                {
                    a[i].interactable = true;
                }
            }
        }
        float vr = Input.GetAxis("Horizontal");//키로 버튼 선택
        if (Input.GetButtonDown("Horizontal"))
        {
            if (vr > 0)//위로 이동
            {

                select--;
                if (select < 0)//리스트 맨 위에서 위로 이동시 맨밑으로
                    select = a.Count - 1;
            }
            else//아래로 이동
            {
                select++;
                if (select > a.Count - 1)//리스트 맨 밑에서 아래로 이동시 맨위로
                    select = 0;
            }
        }
       
        if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]))//공격키로 버튼의 onClick 활성화
        {

            ButtonClickedEvent btn = a[select].onClick;
            btn.Invoke();
            //toggle ver
           
           
        }
    }
}
