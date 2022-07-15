using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expitemspawner : MonoBehaviour//경험치 아이템 생성기
{
    public GameObject expitem;
    public bool itemspawnstate;
    public float itemspawntime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.isMasterClient)//서버의 호스트가 존재할 때
        {
            if (!itemspawnstate)//생성이 안되있으면
            {
                itemspawntime += Time.deltaTime;
                //25처 간격으로 리스폰위치에 생성
                if (itemspawntime >= 25)
                {
                    GameObject item = PhotonNetwork.Instantiate(expitem.name, this.transform.position, this.transform.rotation, 0);
                    item.GetComponent<expitem>().spawner = this.gameObject;
                    itemspawntime = 0;
                    itemspawnstate = true;//생성
                }
            }
            else//생성이 되어있다면
            {
                return;
            }
        }
    }
}

