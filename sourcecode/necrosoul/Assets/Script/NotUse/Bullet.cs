using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool OutCheck;   //�����Ұ��� üũ

    public int Damge;     //������
    public float Speed;     //�Ѿ˽��ǵ�
    double_attack_effect D_attack;
    private void Start()
    {
       
    }

    void Update()
    {
        //this.transform.Translate(new Vector3(Speed,0,0) * 0.1f);
       
        CameraCheck();
    }

    public void CameraCheck()   //�Ѿ� �����Ұ��� üũ�Լ�
    {

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (OutCheck)
        {
            if (pos.x < 0f) { OutCheck = false; Invoke("DestroyBullet", 1); }
            else if (pos.x > 1f) { OutCheck = false; Invoke("DestroyBullet", 1); }
            else if (pos.y < 0f) { OutCheck = false; Invoke("DestroyBullet", 1); }
            else if (pos.y > 1f) { OutCheck = false; Invoke("DestroyBullet", 1); }
        }
    }
    

    public void DestroyBullet() //�Ѿ˻���
    {
        if (D_attack==null)
        {
            ObjectPool.ReturnObject(this);
            

        }
        else
        {
            var d_atk = this.transform.parent.parent.GetComponent<double_attack_effect>();
            d_atk.b_count++;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.layer==12)
        {
            DestroyBullet();
        }

    }
}
