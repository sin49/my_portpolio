using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bulletConroller : Photon.PunBehaviour//총알 관련
{
    float time;
    public float bullet_speed;
    public int damage;
    //public Text kill_text;
   // public Camera cam;
    //float width;
   // float height;
   // public Vector3 screen_center;
   // public GameObject vision;
   // public Vector3 original_position;
    // Start is called before the first frame update
    void Start()
    {
        //vision = GameObject.FindGameObjectWithTag("vision");
        //original_position = new Vector2(transform.position.x,transform.position.y);
        //cam = GameObject.FindGameObjectWithTag("playercamera").GetComponent<Camera>();
        //width = cam.pixelWidth / 2;
        //height = cam.pixelHeight / 2;
        //screen_center = new Vector2(vision.transform.position.x,vision.transform.position.y);
    }
    // Update is called once per frame
    void Update()
    {
        //var dir_ = screen_center - new Vector3(original_position.x, original_position.y, 0) + transform.forward;
       // transform.Translate(dir_ * bullet_speed * Time.deltaTime);
       /* if (transform.position.x >= screen_center.x)
        {
            transform.position = new  Vector2(screen_center.x, this.transform.position.y);
        }
        if (transform.position.y >= screen_center.y)
        {
            transform.position = new Vector2(this.transform.position.x, screen_center.y);
        }*/
        time += Time.deltaTime;
        /*if (time > 3)
        {
            Destroy(this.gameObject);
        }*/
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
        if (col.CompareTag("Player"))
        {
            if (PhotonNetwork.player.GetTeam() != col.GetComponent<PhotonView>().owner.GetTeam())//다른팀이면
            {
                Debug.Log("col");
                col.GetComponent<PhotonView>().RPC("player_hitting",PhotonTargets.All,damage,GetComponent<PhotonView>().owner.NickName);//피격
                if (col.GetComponent<playercontroler>().health <= 0&&!col.GetComponent<playercontroler>().die_check)//적플레이어가 죽었다면
                {
                    
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
            else
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
