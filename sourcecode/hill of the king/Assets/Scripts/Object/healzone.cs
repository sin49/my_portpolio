using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healzone : MonoBehaviour//리스폰지역에서 체력 회복
{
    public int team;
    public int heal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            
            var player = col.GetComponent<playercontroler>();
            if (player.team == team)//팀이 같으면
            {
                if (player.health == player.max_health)
                {
                    return;
                }
                else
                {
                    col.GetComponent<PhotonView>().RPC("player_healthup", PhotonTargets.All, -heal);//체력 회복
                }
            }
        }
    }
}
