using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healitem : MonoBehaviour//회복 아이템
{
    public int healthper;
    public GameObject itemspawner;
    public bool check;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            var player = col.GetComponent<playercontroler>();
            if (player.health == player.max_health)//체력이 꽉차있으면 무시
            {
                return;
            }
            else
            {
                Debug.Log("check");
                if (!check)
                {
                    //  PhotonTargets.All =모든 클라이언트가 같은 정보를 가ㅣ게 함
                    col.GetComponent<PhotonView>().RPC("player_healthup", PhotonTargets.All, player.max_health*healthper/100);//체력을 회복
                    check = true;
                }
                itemspawner.GetComponent<healitemspawner>().itemspawnstate = false;
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
