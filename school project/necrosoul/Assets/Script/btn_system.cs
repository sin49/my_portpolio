using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class btn_system : MonoBehaviour//Ű����� �۵���Ű�� ui ���
{
    public List<Button> a = new List<Button>();//������ ui(��ư)�� list�� ���(�������)
    int select;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //select�� ����Ʈ�� �� ��ư�� ���������� �����Ѵ�
    void Update()
    {
        BtnSystem(a);
    }
    void BtnSystem(List<Button> a)
    {
        for (int i = 0; i < a.Count; i++)//���� �������� ��ư�� �׷��� ǥ��
        {
            if (i == select)//�׷��� ��Ұ� ��� interactable�� Ȱ���Ͽ� �׷��� ǥ��
            {
                if (a[i].IsInteractable() == true)//�� ��뿡 interactable�� �� �ǵ�°� ����!
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
        float vr = Input.GetAxis("Horizontal");//Ű�� ��ư ����
        if (Input.GetButtonDown("Horizontal"))
        {
            if (vr > 0)//���� �̵�
            {

                select--;
                if (select < 0)//����Ʈ �� ������ ���� �̵��� �ǹ�����
                    select = a.Count - 1;
            }
            else//�Ʒ��� �̵�
            {
                select++;
                if (select > a.Count - 1)//����Ʈ �� �ؿ��� �Ʒ��� �̵��� ������
                    select = 0;
            }
        }
       
        if (Input.GetKeyDown(Key_manager.Keys[Key_manager.KeyAction.ATTACK]))//����Ű�� ��ư�� onClick Ȱ��ȭ
        {

            ButtonClickedEvent btn = a[select].onClick;
            btn.Invoke();
            //toggle ver
           
           
        }
    }
}
