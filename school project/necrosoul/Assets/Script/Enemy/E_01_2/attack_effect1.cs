using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_effect1 : MonoBehaviour//적 공격 투사체 클레스
{
    Vector3 a;
    Rigidbody2D rgd2D;
    public Vector2 dir;
    public GameObject creator;
    public int Attack;
    public float bullet_spped;
    public Vector2 hitted_force;
    Vector2 p_pos;
    // Start is called before the first frame update
    void Start()
    {
        rgd2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //dir의 방향으로 bullet_spped속도로 날아감
        rgd2D.velocity=(dir.normalized * bullet_spped);
    }
    private void OnDisable()//비활성화 됐을 때 위치 조절
    {
        transform.position = new Vector2(-999, -999);
    }
    private void OnTriggerEnter2D(Collider2D collision)//벽,플레이어에게 닿았을 시 비활성화
    {
        if (collision.gameObject.layer == 12)
        {
            this.gameObject.SetActive(false);
      
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCharacter p = collision.GetComponent<PlayerCharacter>();

            p.hitted_event(this.transform.position);
            p.player_hitted(Attack);
            this.gameObject.SetActive(false);
        }
    }
}
