using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class double_attack_effect : MonoBehaviour
{
    public GameObject bullet;
   public bool OutCheck;   //�����Ұ��� üũ
    public int b_count;
    public int Damge;     //������
    public float Speed;     //�Ѿ˽��ǵ�
    public Transform[] b_pos;
    private void Awake()
    {
        GameObject a= Instantiate(bullet, b_pos[0].position, this.transform.rotation);
        a.transform.SetParent(b_pos[0]);
        GameObject b = Instantiate(bullet, b_pos[1].position, this.transform.rotation);
        b.transform.SetParent(b_pos[1]);
    }
    private void Update()
    {
        if (b_count == 2)
        {
            Bullet b = this.GetComponent<Bullet>();
            b.DestroyBullet();
        }
    }
}
//////////////// ��å 1 Ǯ���� �� ������Ʈ ��å 2 �Ѿ� �ı��ɶ� �O�Ƽ�