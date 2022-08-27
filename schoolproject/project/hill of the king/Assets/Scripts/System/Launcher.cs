using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Photon.PunBehaviour//서버에서 설정을하고 시작하기위한 스크립트
{
    public PhotonLogLevel loglevel = PhotonLogLevel.Informational;
    public byte maxplayernumber = 8;
    string _gameVersion = "1";
    bool isconnecting = true;
    public GameObject controlpanel;
    public GameObject progresslabel;
    public GameObject optionui;
    GameManager ga;
    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.logLevel = loglevel;
        PhotonNetwork.autoJoinLobby=false;
        PhotonNetwork.automaticallySyncScene = true;
        
    }
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("prefsvol");
        progresslabel.SetActive(false);
        controlpanel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        ga = FindObjectOfType<GameManager>();
        if (ga != null)
        {
            Destroy(ga.gameObject);
        }
    }
    public void Connect()//서버에 연결
    {
        AudioSource button_sound = GetComponent<AudioSource>();
        button_sound.Play();
            progresslabel.SetActive(true);
            controlpanel.SetActive(false);
            if (PhotonNetwork.connected)
                PhotonNetwork.JoinRandomRoom();
            else
                PhotonNetwork.ConnectUsingSettings(_gameVersion);
 
    }
    public override void OnConnectedToMaster()//연결에 성공할 경우 룸에 들어감
    {
        if (isconnecting)
        {
            Debug.Log("aaaa");
            PhotonNetwork.JoinRandomRoom();
        }
    }
   
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)//연결에 실패할 경우 룸을 생성
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxplayernumber}, null);

    }
    public override void OnJoinedRoom()//룸에 들어 왔을 때
    {
        if (PhotonNetwork.room.PlayerCount == 1)
        {
            //다른 플레이어가 들어올 때 까지 대기
            // PhotonNetwork.LoadLevel(1);
            //현재는 바로 게임에 들어오게 설정되있다
            PhotonNetwork.LoadLevel(2);
        }
    }
    public override void OnDisconnectedFromPhoton()//연결이 끊겼을 때
    {
        progresslabel.SetActive(false);
        controlpanel.SetActive(true);
        base.OnDisconnectedFromPhoton();
    }
    public void exit_game()//게임 나가기
    {
        Application.Quit();
    }
    public void option_UI_on()//option ui 켜기
    {
        optionui.SetActive(true);
    }
    public void option_UI_off()//option ui 끄기
    {
        optionui.SetActive(false);
    }
    public void button_sound()//버튼 사운드
    {
        AudioSource button_sound = GetComponent<AudioSource>();
        button_sound.Play();
    }
}
