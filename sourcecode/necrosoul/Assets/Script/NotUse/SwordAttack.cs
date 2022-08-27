using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Transform Pos;
    public Vector2 boxSize;
    float attack_timer;
    public float attack_time;
    public bool attack_animating;
    /// ///////////////////
    bool first_anim;
    public int attack_anim_frame;
    Player_animator p_anim;
    // Start is called before the first frame update
    void Start()
    {
        p_anim = this.transform.parent.GetComponent<Player_animator>();
    }
    public void swordAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(attack_timer<=0)
                attack_timer = attack_time;
        }
        if (attack_timer > 0)
        {
            attack_timer -= Time.deltaTime;
        }
        if (!attack_animating && attack_timer > 0)
        {
            attack_timer = 0;
           
                    p_anim.sword_attack_anim();

      
        }
     //�÷��̾� ���� Ŭ���� ������ �̺�Ʈ�� �ۿ� 1.Ŭ�� �����Ҷ� attack_animating�� true�� �ϰ� ������ ������ false(3�� ������ �ĵ��� ��� �ֱ����� ������ ���� ��)2.true���ϴ� �����ӿ� attack_anim_frame+1 2�϶��� 0 <-anim_frame������ ���ϸ����Ϳ� ���� �� ���� ���� 3.����Ʈ ���� ����Ʈ�� bullet�� ������ �����͸� ���Ӥ���

       
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Collider2D[] Hits = Physics2D.OverlapBoxAll(Pos.position, boxSize, 0);
            foreach (Collider2D collider in Hits)
            {
                if(collider.tag=="Enemy")
                {
                    Debug.Log("��������!");     //���� �̺�Ʈ �� ��� ������ ��
                }
            }
        }
    }
    private void FixedUpdate()
    {
        swordAttack();
    }

    private void OnDrawGizmos()     //�ӽ÷� �����ְ��ϴ� �Լ�
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Pos.position, boxSize);
    }
}
