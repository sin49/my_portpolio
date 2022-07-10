using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    public Animator anim;
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    public void StartAnimation()
    {
        Debug.Log("�ִϸ��̼� ����");
        anim.SetBool("Start", false);
        anim.SetBool("Start", true);
    }
    
    public void End()
    {
        Debug.Log("�ִϸ��̼� ����");
        anim.SetBool("Start", false);
        this.gameObject.SetActive(false);
    }
}