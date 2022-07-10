using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init_Splash : MonoBehaviour
{
    GameObject SplashObj;               //�ǳڿ�����Ʈ
    Image image;                            //�ǳ� �̹���
    Color color;

    private bool checkbool = false;     //���� ���� ���� ����


    void Awake()
    {
        SplashObj = this.gameObject;                         //��ũ��Ʈ ������ ������Ʈ
        image = SplashObj.GetComponent<Image>();    //�ǳڿ�����Ʈ�� �̹��� ����
        color = image.color;
    }


    void Update()
    {
        StartCoroutine("MainSplash");                        //�ڷ�ƾ    //�ǳ� ���� ����

        if (checkbool)                                            //���� checkbool �� ���̸�
        {
            color.a = 1;                      //�ǳ� �ı�, ����
            checkbool = false;
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator MainSplash()
    {

        for (int i = 100; i >= 0; i--)                            //for�� 100�� �ݺ� 0���� ���� �� ����
        {

            color.a -= Time.deltaTime * 0.005f;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.01

            image.color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

            if (image.color.a <= 0)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
            {
                checkbool = true;                              //checkbool �� 
                Gamemanager.GM.fade_in_complete=true;
            }
        }
        yield return null;                                        //�ڷ�ƾ ����
    }

}
