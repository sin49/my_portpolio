using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Open : MonoBehaviour
{
    public GameObject Panel;
    public Animator ani;

    private void Awake()
    {
        Time.timeScale = 1;
        Debug.Log("��ŸƮ");
        //ani.SetBool("Check", false);
    }
    public void Openpanel()
    {
        Debug.Log("���̺� ��ư Ŭ��");
       // ani.SetBool("Check", true);
    }
}
