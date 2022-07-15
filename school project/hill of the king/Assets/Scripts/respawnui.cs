using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class respawnui : MonoBehaviour//스폰되기전의 ui
{
    public Text red_team_text;
    public Text blue_team_text;
    public Text leveltext;
    public GameObject[] active_button = new GameObject[4];
    public GameObject[] nonactive_image = new GameObject[4];
    public GameObject[] ability_text=new GameObject[4];
    public Text[] status_text = new Text[4];
    public GameObject respawn_button;
    public bool esccheck;
    public GameObject respawnmanager;
    playerlv playerlv;
    playerspawner spawner;
    public Text kill_text;
    public Text death_text;
    public Text score_text;
    GameManager game;

    string exp_;
    void Start()
    {
        playerlv = respawnmanager.GetComponent<playerlv>();
        spawner = respawnmanager.GetComponent<playerspawner>();
        game = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerlv.lv <= 5)
            exp_ = "/100";
        else if (playerlv.lv <= 10)
            exp_ = "/150";
        else if (playerlv.lv <= 15)
            exp_ = "/200";
        else if (playerlv.lv <= 19)
            exp_ = "/250";
        else if (playerlv.lv == 20)
            exp_ = "/MAX";
            if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red)
        {
            red_team_text.gameObject.SetActive(true);
            blue_team_text.gameObject.SetActive(false);
        }
        else if(PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
        {
            red_team_text.gameObject.SetActive(false);
            blue_team_text.gameObject.SetActive(true);
        }
        leveltext.text = "level: " + playerlv.lv +" exp: "+playerlv.exp+exp_+ " point: " + playerlv.lvpoint;
        if (playerlv.lvpoint <= 0)
        {
            for(int i = 0; i <= 3; i++)
            {
                nonactive_image[i].SetActive(true);
                active_button[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i <= 3; i++)
            {
                nonactive_image[i].SetActive(false);
                active_button[i].SetActive(true);
            }
        }
        if (spawner.s_ability_number != 0)
        {
            ability_text[spawner.s_ability_number - 1].SetActive(true);
        }
        status_text[0].text = "lv " + playerlv.heart;
        status_text[1].text = "lv " + playerlv.blade;
        status_text[2].text = "lv " + playerlv.wing;
        status_text[3].text = "lv " + playerlv.storm;
        if (spawner.can_respawn == true)
        {
            respawn_button.SetActive(true);
        }
        else
        {
            respawn_button.SetActive(false);
        }
        if (game != null)
        {
            kill_text.text = "kill: " + game.kill;
            death_text.text = "death:" + game.death;
        }
        score_text.text = "score:" + PhotonNetwork.player.GetScore();
    }
}
