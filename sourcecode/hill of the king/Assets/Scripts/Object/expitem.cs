using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expitem : Photon.PunBehaviour//경험치 아이템
{
    int exp;
    public GameObject spawner;
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
            if (col.GetComponent<PhotonView>().owner == PhotonNetwork.player)
            {
                playerlv plv = FindObjectOfType<playerlv>();

                plv.exp += 100;
                spawner.GetComponent<expitemspawner>().itemspawnstate = false;
                PhotonNetwork.Destroy(this.gameObject);//플레이어에게 경험치를 100주고 이 오브젝트를 파괴
            }
        }
    }
}
