using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AI06_bulleet : MonoBehaviour//6�� ���� ���� ���� 
{
    //Ȱ��ȭ�ʸ� �� ��ġ�� ��� ǥ���ϰ�
    //���� �ð��� ���� �� ���� ������ Ȱ��ȭ ���Ѽ� ������ ó���Ѵ�
    public GameObject Bullet;
    public GameObject warning;
    // Start is called before the first frame update
    void Start()
    {
        if (Bullet == null)
        {
            Bullet = gameObject.transform.GetChild(0).gameObject;
        }
    }
    private void OnDisable()
    {
        var a = this.GetComponent<Animator>();
        a.SetTrigger("loop");
    }
    //��� ǥ��
    void set_warning()
    {
        warning.SetActive(true);
    }
    //��� ��Ȱ��ȭ
    void reset_warning()
    {
        warning.SetActive(false);
    }
    private void OnEnable()
    {
        
    }
    //���� ���� Ȱ��ȭ
    void set_bullet()
    {
        Bullet.SetActive(true);
    }
    void reset_bullet()
    {
        Bullet.SetActive(false);
    }
    void dstrooy_self()
    {
        this.gameObject.SetActive(false);
    }
}
