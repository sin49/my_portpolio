using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class disconnectbutton : MonoBehaviour//서버 연결을 끊는 버튼
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void disconnect()//연결 끊기
    {
        
        PhotonNetwork.Disconnect();
        PhotonNetwork.player.SetScore(0);
        PhotonNetwork.player.SetTeam(PunTeams.Team.none);
        SceneManager.LoadScene(0);
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        Destroy(gameManager.gameObject);
    }
}
