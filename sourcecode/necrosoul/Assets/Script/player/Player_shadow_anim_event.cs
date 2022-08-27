using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_shadow_anim_event : MonoBehaviour//플레이어의 환영 에니메이션 이벤트 클레스
{
    public GameObject melee_1;
    public GameObject melee_1_instani;
    public GameObject melee_2;
    public GameObject melee_2_instani;
    public GameObject melee_3;
    public GameObject melee_3_instani;
    public GameObject air_melee_;
    public GameObject air_melee_instani;
    public float melee_1_reaction;
    public float melee_2_reaction;
    public float melee_3_reaction;
    public Player_shadow p_sh;
    //자괴
    public void Destroy_self()
    {
        Destroy(this.transform.parent.parent.gameObject);
    }
    
    public void Start()
    {
        p_sh = this.transform.parent.GetComponent<Player_shadow>();
    }
     
 
}
