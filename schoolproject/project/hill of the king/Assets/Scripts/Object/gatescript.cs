using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gatescript : MonoBehaviour//리스폰 장소 관련 스크립트
{
    public int team;
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
            if (col.GetComponent<playercontroler>().team != team)//팀이 다르면 접근을 막는다
            {
                float a= col.transform.position.x;
                if(team==0)
                    col.transform.position=new Vector3(a - 1, col.transform.position.y, col.transform.position.z);
                else if(team==1)
                    col.transform.position = new Vector3(a + 1, col.transform.position.y, col.transform.position.z);
            }
            else
            {
                return;
            }
        }
        if (col.CompareTag("bullet"))//팀이 다르면 총알을 막는다
        {
            if (team == 0) { 
                if (col.GetComponent<PhotonView>().owner.GetTeam() != PunTeams.Team.red)
                    PhotonNetwork.Destroy(col.gameObject);
        }
        else if (team == 1)
        {
                if (col.GetComponent<PhotonView>().owner.GetTeam() != PunTeams.Team.blue)
                    PhotonNetwork.Destroy(col.gameObject);
            
        }
        }
    }
}
