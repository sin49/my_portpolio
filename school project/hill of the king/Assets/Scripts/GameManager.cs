using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour//게임을 끝내는 이벤트와 플레이어가 룸에 참가했을 때와 빠져나올 때의 이벤트 담당 
{
    public bool game_set;//게임끝내기 체크용
    public bool returncheck;
    public float end_time = 5;
    public int death;//플레이어가 죽은 횟수
    public int kill;//플레이어가 죽인 횟수
    int mvp;
    public PhotonPlayer mvp_player;
    public int team_win;
    public int countplayer;
    public GameObject disconnecttext;
    static public GameManager instance;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("prefsvol");
        Debug.Log("check");
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public void onleftroom()
    {
        SceneManager.LoadScene(0);
    }
    void loadArena()//게임을 시작하거나 플레이어를 찿는 씬으로 이동
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.Log("not masterclient");
        }
        PhotonNetwork.LoadLevel(PhotonNetwork.room.PlayerCount);


    }
    public void leaveroom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer otherPlayer)//다른 플레이어가 룸에 연결 되었을 때
    {
        Debug.Log("aaaa");
        if (PhotonNetwork.isMasterClient)
        {
            if(PhotonNetwork.countOfPlayers>=1)
                loadArena();
        }
        Debug.Log("aaa");

    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)//다른 플레이어가 룸에서 연결이 끊겼을 때
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject distext= Instantiate(disconnecttext,canvas.transform);
        distext.transform.position = canvas.transform.position+new Vector3(0,100,0);
        if (PhotonNetwork.isMasterClient)
        {
            if (PhotonNetwork.room.PlayerCount < 2)
            {
                if (!game_set)
                {
                    PhotonNetwork.LoadLevel(1);
                    Cursor.visible = true;//커서 숨기기
                    PhotonNetwork.player.SetTeam(PunTeams.Team.none);
                    Cursor.lockState = CursorLockMode.None;//커서 고정
                    returncheck = true;
                }
                else
                {
                    PhotonNetwork.player.SetScore(0);
                    Cursor.visible = true;//커서 숨기기
                    PhotonNetwork.player.SetTeam(PunTeams.Team.none);
                    Cursor.lockState = CursorLockMode.None;//커서 고정
                    PhotonNetwork.LoadLevel(0);
                    PhotonNetwork.Disconnect();

                    Destroy(this.gameObject);
                }
                
            }
        }

        // Update is called once per frame
        /*void Update()
        {
            countplayer = PhotonNetwork.countOfPlayers;
        }*/


    }
    public void red_win()//빨강 팀이 이겼을 때
    {
        team_win = 1;
        var a= PunTeams.PlayersPerTeam[PunTeams.Team.red];
        for(int i = 0; i < a.Count; i++)
        {
            int score = a[i].GetScore();
            if (a[mvp].GetScore() < a[i].GetScore())
            {
                mvp = i;
            }
        }
        PhotonPlayer p = a[mvp];
        Debug.Log("red team win");
        Debug.Log("mvp is" + p.NickName + " score: "+ p.GetScore());
        Debug.Log("your score: " + PhotonNetwork.player.GetScore() + " kill:" + kill+" death:"+death);
        game_set = true;
        Debug.Log("game will end in 5seconds");
    }
    public void blue_win()//파랑팀이 이겼을 때
    {
        team_win = 2;
        var a = PunTeams.PlayersPerTeam[PunTeams.Team.blue];
        for (int i = 0; i < a.Count; i++)
        {
            int score = a[i].GetScore();
            if (a[mvp].GetScore() < a[i].GetScore())
            {
                mvp = i;
            }
        }
        mvp_player = a[mvp];
        Debug.Log("blue team win");
        Debug.Log("mvp is" + mvp_player.NickName + " score: " + mvp_player.GetScore());
        Debug.Log("your score: " + PhotonNetwork.player.GetScore() + " kill:" + kill + " death:" + death);
        game_set = true;
        Debug.Log("game will end in 5seconds");
    }
    void end_game()//게임 끝내기
    {
        
        end_time -= Time.deltaTime;
        if (end_time <= 0)
        {
            PhotonNetwork.player.SetScore(0);
            Cursor.visible = true;//커서 숨기기
            Cursor.lockState = CursorLockMode.None;//커서 고정
            PhotonNetwork.LoadLevel(0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.none);
            PhotonNetwork.Disconnect();
            
        }
        
    }
    void Update()
    {
        if (returncheck && SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject distext = Instantiate(disconnecttext, canvas.transform);
            distext.transform.position = canvas.transform.position + new Vector3(0, 100, 0);
            Destroy(this.gameObject);
        }
        count = PhotonNetwork.countOfPlayers;
        if (game_set)
        {
            end_game();
        }
    }
}
