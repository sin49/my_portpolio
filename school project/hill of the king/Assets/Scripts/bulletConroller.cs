using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bulletConroller : Photon.PunBehaviour//총알 관련 클레스
{
    float time;
    public float bullet_speed;
    public int damage;
   
    void Start()
    {
      
    }
    // Update is called once per frame
    void Update()
    {
      
        time += Time.deltaTime;
      
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ground"))
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        if (col.CompareTag("wall"))
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        if (col.CompareTag("Player"))//플레이어에게 맞았을때
        {
            if (PhotonNetwork.player.GetTeam() != col.GetComponent<PhotonView>().owner.GetTeam())//다른팀이면
            {
                Debug.Log("col");
                col.GetComponent<PhotonView>().RPC("player_hitting",PhotonTargets.All,damage,GetComponent<PhotonView>().owner.NickName);//피격
                if (col.GetComponent<playercontroler>().health <= 0&&!col.GetComponent<playercontroler>().die_check)//적플레이어가 죽었다면
                {
                    //점수흭득+경험치 흭득
                    Debug.Log("catch");
                    this.gameObject.GetComponent<PhotonView>().owner.SetScore(1+ PhotonNetwork.player.GetScore());
                    Debug.Log(PhotonNetwork.player.GetScore());
                    GameManager ga = GameObject.FindObjectOfType<GameManager>();
                    ga.kill++;
                    playerlv lv = GameObject.FindObjectOfType<playerlv>();
                    
                    Debug.Log("******");
                    //lv.gameObject.GetComponent<playerspawner>().player_death_board(this.photonView.owner.NickName, col.GetComponent<PhotonView>().owner.NickName);
                    lv.exp += 100;
                    col.GetComponent<playercontroler>().die_check = true;
                }
                PhotonNetwork.Destroy(this.gameObject);
            }
            else//같은팀일경우 그냥파괴
            {
                Debug.Log("col?");
                PhotonNetwork.Destroy(this.gameObject);
            }
            
        }
    }
    
    [PunRPC]
    public void set_damage(int a)//화력 동기화
    {
        damage = a;
    }
    [PunRPC]
    public void set_speed(int a)//속도 동기화
    {
        bullet_speed = a;
    }

   
}
