using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerspawner : MonoBehaviour//플레이어 팀 배정,플레이어 오브젝트 생성
{
    playerlv playerlv;//능력치 체크
    public int player_hp;
    public float player_speed;
    public int bullet_damage;
    public float firedelay;
    public int bullet_speed;
    public float reloadtime;
    public int magazine;
    const int default_hp= 100;
    const float default_speed = 10;
    const int default_damage = 10;
    const float default_firedelay = 0.6f;
    const int default_bulletspeed = 40;
    const float default_reloadtime = 2.5f;
    const int default_magazine = 10;
    public GameObject Playerprefab;
    public GameManager gameManager;
    public bool playerspawned;
    public float respawn_time;
    public float max_respawn_time;
    public GameObject spawn_point_red;
    public GameObject spawn_point_blue;
    public GameObject diesconnectuiprefab;
    public bool esccheck;
    List<PhotonPlayer> teamPlayers_red;
    List<PhotonPlayer> teamPlayers_blue;
    public bool teamcheck;
    public GameObject respawnui;
    public Text death_ui;
    public Camera respawn_cam;
    public bool can_respawn;
    public GameObject death_board;
    public Text board_text;
    public int s_ability_number;
    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("prefsvol");
        playerlv = GetComponent<playerlv>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
      
        if (PhotonNetwork.player.IsMasterClient)//마스터 클라이언트는 무조건 빨강팀
        {
            Debug.Log("i'm red!");
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
            teamcheck = true;
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        player_hp = default_hp + (playerlv.heart * 20) - (playerlv.blade * 5)-(playerlv.wing*5);//플레이어의 능력치에 맞춰서 값 정하기
        player_speed = default_speed + (playerlv.wing * 2) - (playerlv.heart);
        bullet_damage = default_damage + (playerlv.blade * 2) - (playerlv.heart);
        firedelay = default_firedelay - (playerlv.storm * 0.1f) + (playerlv.blade * 0.05f);
        bullet_speed = default_bulletspeed + (playerlv.blade * 4) - (playerlv.storm*2);
        reloadtime = default_reloadtime - (playerlv.wing * 0.25f) + (playerlv.storm * 0.125f);
        magazine = default_magazine + (playerlv.storm * 2) - (playerlv.wing);
        //레벨이 5이하 일시 특수 능력 비활성화
        if (playerlv.lv < 5)
        {
            s_ability_number = 0;
        }
        else
        {
            //레벨이 5이상일시 특수 능력활성화+능력치 수치가 레벨의 절반보다 많을 때 능력치의 종류에 따라 특수능력의 능럭이 정햊진다
            if (playerlv.heart > (float)playerlv.lv / 2)
            {
                s_ability_number = 1;
            }
            else if(playerlv.blade > (float)playerlv.lv / 2)
            {
                s_ability_number = 2;
            }else if (playerlv.wing > (float)playerlv.lv / 2)
            {
                s_ability_number = 3;
            }else if (playerlv.storm > (float)playerlv.lv / 2)
            {
                s_ability_number = 4;
            }
        }
        if (gameManager != null)
        {
            if (gameManager.game_set)//게임이 안끝났으면 스폰 시키기
            {
                respawnui.SetActive(false);
                return;
            }
        }
        //팀정하기
        teamPlayers_red = PunTeams.PlayersPerTeam[PunTeams.Team.red];
        teamPlayers_blue = PunTeams.PlayersPerTeam[PunTeams.Team.blue];
        if(!teamcheck)
            player_team();
        //플레이어가 생성이 안되있다면 생성
        if (!playerspawned)
        {

            respawn_time += Time.deltaTime;
            respawnui.SetActive(true);
            
            respawn_cam.gameObject.SetActive(true);
            if (respawn_time >= max_respawn_time)//시간이 지나면
            {
                can_respawn = true;//리스폰 가능
                death_ui.gameObject.SetActive(false);

            }
            else//사망 후 재생성기간을 기다리는 중
            {
                death_ui.gameObject.SetActive(true);
                death_ui.text = ("can respawn in " + Mathf.Floor((max_respawn_time - respawn_time) * 10) / 10 + "seconds");
            }
        }
        else
        {
            respawnui.SetActive(false);
        }
        if (can_respawn && playerlv.lvpoint == 0)//스폰이 가능하고 포인트가 없다면 바로 생성
        {
            player_spawn();
        }

        if (Input.GetKeyDown(KeyCode.Escape))//esc로 매뉴 열기&닫기
        {
            if (esccheck == false)
            {
                //playeruiprefab.SetActive(false);
                diesconnectuiprefab.SetActive(true);
                esccheck = true;
                if (playerspawned)
                {
                    Cursor.visible = true;//커서 보이기
                    Cursor.lockState = CursorLockMode.None;//커서 고정 풀기
                }
            }
            else if (esccheck == true)
            {
                //playeruiprefab.SetActive(true);
                diesconnectuiprefab.SetActive(false);
                esccheck = false;
                if (playerspawned)
                {
                    Cursor.visible = false;//커서 숨기기
                    Cursor.lockState = CursorLockMode.Locked;//커서 고정
                }
            }
        }
    }
    
    void player_team()//플레이어 팀 정하기
    {
        
        int r=teamPlayers_red.Count;
        int b=teamPlayers_blue.Count;
        Debug.Log(r + " " + b);
        if (r > b)
        {
            //빨강팀이 파랑팀보다 많을경우 파랑팀
            Debug.Log("i'm blue!");
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);

        }
        else if(b>r)
        {
            //파랑팀이 빨강팀보다 많을경우 파랑팀
            Debug.Log("i'm red!");
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
        }
        else
        {
            //같을 경우 원하는 팀을 선택 가능
            Debug.Log("ummmm");
            return;
        }
        teamcheck = true;
    }
    public void player_spawn()//플레이어 오브젝트 생성하기
    {
        if (can_respawn == false)
        {
            Debug.Log("error");
            return;
        }
        respawn_cam.gameObject.SetActive(false);
        if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red)//빨간 팀일때 빨간팀 재생성지점에 생성
        {
            GameObject player = PhotonNetwork.Instantiate(Playerprefab.name, spawn_point_red.transform.position, spawn_point_red.transform.rotation, 0);
            int team = 0;
            //채력,대미지,스피드,총알스피드,딜레이,재장전,장탄수
            player.GetComponent<PhotonView>().RPC("player_team_set", PhotonTargets.All, team, player_hp, bullet_damage, player_speed, bullet_speed, firedelay, reloadtime, magazine,s_ability_number);
            playerspawned = true;
            respawn_time = 0;
            can_respawn = false;
        }
        else if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)//파란팀일때 파란팀 재생성지점에 생성
        {
            GameObject player = PhotonNetwork.Instantiate(Playerprefab.name, spawn_point_blue.transform.position, spawn_point_blue.transform.rotation, 0);
            int team = 1;
            player.GetComponent<PhotonView>().RPC("player_team_set", PhotonTargets.All, team, player_hp, bullet_damage, player_speed, bullet_speed, firedelay, reloadtime, magazine,s_ability_number);

            playerspawned = true;
            respawn_time = 0;
            can_respawn = false;
        }
    }
    
}
