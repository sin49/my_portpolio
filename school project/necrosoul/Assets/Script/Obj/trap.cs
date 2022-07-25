using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour//닿을시 플레이어가 피해를 받는 함정
{
    int damage;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerCharacter p = collision.gameObject.GetComponent<PlayerCharacter>();
           
            p.hitted_event(this.transform.position);
            p.player_hitted(damage);
        }
    }
}
