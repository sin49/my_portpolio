using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AI06_bulleet : MonoBehaviour//6번 적의 구속 공격 
{
    //활성화됨면 그 위치에 경고를 표시하고
    //일정 시간이 지난 후 공격 판정을 활성화 시켜서 공격을 처리한다
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
    //경고 표시
    void set_warning()
    {
        warning.SetActive(true);
    }
    //경고 비활성화
    void reset_warning()
    {
        warning.SetActive(false);
    }
    private void OnEnable()
    {
        
    }
    //공격 판정 활성화
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
