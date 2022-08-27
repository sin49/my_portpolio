using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Outit_Splash : MonoBehaviour       //������ ��Ÿ����
{
    GameObject SplashObj;               //�ǳڿ�����Ʈ
    Image image;                            //�ǳ� �̹���

    [SerializeField]
    string NextScence;

    private bool checkbool = false;     //���� ���� ���� ����

    public Animator Charic;
    Color color;

    void Awake()
    {
                                   
        SplashObj = this.gameObject;                         //��ũ��Ʈ ������ ������Ʈ
        image = SplashObj.GetComponent<Image>();             //�ǳڿ�����Ʈ�� �̹��� ����
        color = image.color;                                 //color �� �ǳ� �̹��� ����
       

    }



    void Update()
    {
        StartCoroutine("MainSplash");                        //�ڷ�ƾ    //�ǳ� ���� ����

        if (checkbool)                                            //���� checkbool �� ���̸�
        {
            color.a = 1;                      //�ǳ� �ı�, ����
            checkbool = false;
           
        }

    }



    IEnumerator MainSplash()
    {
  
        for (int i = 0; i <= 100; i++)                            //for�� 100�� �ݺ� 0���� ���� �� ����
        {

            color.a += Time.deltaTime * 0.01f;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.01

            image.color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

            if (image.color.a >= 1)                        //���� �ǳ� �̹��� ���� ���� 0���� ũ��
            {
                checkbool = true;                              //checkbool �� 
                Gamemanager.GM.fade_out_complete = true;
            }

        }
        yield return null;                                        //�ڷ�ƾ ����

    }

}
