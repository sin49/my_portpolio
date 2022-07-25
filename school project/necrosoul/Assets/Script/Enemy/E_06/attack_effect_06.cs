using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_effect_06 : MonoBehaviour//6번적의 공격 클레스 플레이어가 닿을시 구속 상태이상 부여
{

    public bool hitted;
    public int Attack;
    public Vector2 hitted_force;
    bad_status b;
    // Start is called before the first frame update
    void Start()
    {
        //상태이상 b 설정
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
        //플레이어가 닿을 시 프레이어에게 데미지를 주고 상태이상 b를 부여한다
        if (!hitted)
                {
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        PlayerCharacter p = collision.GetComponent<PlayerCharacter>();
                p.get_bad_status(b);//상태이상 부여
                        p.hitted_event(this.transform.position,Vector2.zero);
                        p.player_hitted(Attack);
                
                    hitted = true;
                    }
                }
            
        
    }
}
