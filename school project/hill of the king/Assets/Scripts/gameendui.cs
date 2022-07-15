using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gameendui : MonoBehaviour
{
    //게임끝낼때ui
    public Text red_win_text;
    public Text blue_win_text;
    public GameManager gameManager;
    public Text mvp_text;
    public Text player_score_text;
    public Text game_end_text;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        red_win_text.gameObject.SetActive(false);
        blue_win_text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.team_win == 1)
        {
            red_win_text.gameObject.SetActive(true);
        }else if (gameManager.team_win == 2)
        {
            blue_win_text.gameObject.SetActive(true);
        }
        /*if (gameManager.mvp_player != null)
            mvp_text.text = gameManager.mvp_player.NickName + "    total score: " + gameManager.mvp_player.GetScore();
        else
            Debug.Log("sometihng's wrong");*/
        player_score_text.text = "kill: " + gameManager.kill + " death: " + gameManager.death + " total score: " + PhotonNetwork.player.GetScore();

        game_end_text.text = "game will end in " + (int)gameManager.end_time + "second";
    }
}
