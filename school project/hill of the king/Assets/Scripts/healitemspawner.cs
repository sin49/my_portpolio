using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healitemspawner : MonoBehaviour//회복아이템 생성기
{
    public GameObject healitem;
    public bool itemspawnstate;
    public float itemspawntime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.isMasterClient) {
            if (!itemspawnstate)//생성이 안되있으면
            {
                itemspawntime += Time.deltaTime;
                if (itemspawntime >= 30)
                {
                    GameObject item= PhotonNetwork.Instantiate(healitem.name, this.transform.position, this.transform.rotation, 0);
                    item.GetComponent<healitem>().itemspawner = this.gameObject;
                    itemspawntime = 0;
                    itemspawnstate = true;//회복아이템 생성
                }
            }
            else
            {
                return;
            }
                }
    }
}
