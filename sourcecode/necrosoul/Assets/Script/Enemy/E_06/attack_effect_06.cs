using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_effect_06 : MonoBehaviour//6������ ���� Ŭ���� �÷��̾ ������ ���� �����̻� �ο�
{

    public bool hitted;
    public int Attack;
    public Vector2 hitted_force;
    bad_status b;
    // Start is called before the first frame update
    void Start()
    {
        //�����̻� b ����
        b = new bad_status(0, 2);

    }
    private void OnDisable()
    {
        hitted = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //�÷��̾ ���� �� �����̾�� �������� �ְ� �����̻� b�� �ο��Ѵ�
        if (!hitted)
                {
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        PlayerCharacter p = collision.GetComponent<PlayerCharacter>();
                p.get_bad_status(b);//�����̻� �ο�
                        p.hitted_event(this.transform.position,Vector2.zero);
                        p.player_hitted(Attack);
                
                    hitted = true;
                    }
                }
            
        
    }
}
