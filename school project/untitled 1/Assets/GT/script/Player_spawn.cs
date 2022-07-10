using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player_spawn : MonoBehaviour
{
    public int Life;
    public Text Life_text;
    public GameObject Player;
    public Transform respawn_check;
    public float respawn_time = 1;
    public float respawn_time_check = 0;
    public Slider power_gauge1;
    public Slider power_gauge2;
    public Slider power_gauge3;
    public Text power_text;
    
    public GameObject clear;
    // Start is called before the first frame update
    void Start()
    {
        clear = GameObject.FindWithTag("clear");
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Life_text.text = ("life: " + Life);
        if (Life > 0)
        {
            if (GameObject.FindWithTag("Player") == null)
            {
                respawn_time_check += Time.deltaTime;
                if (respawn_time_check >= respawn_time)
                {
                    clear.GetComponent<Clear_bullet>().run2();
                    GameObject Player1 = Instantiate(Player, respawn_check.position, respawn_check.rotation);
                    Player1.GetComponent<shooting_playermove>().respawn_check = true;
                    Player1.GetComponent<shooting_player>().Player_ = gameObject;
                    Player1.GetComponent<shooting_player>().untouchable_state = true;
                    Player1.GetComponent<shooting_player>().power_gauge1 = power_gauge1;
                    Player1.GetComponent<shooting_player>().power_gauge2 = power_gauge2;
                    Player1.GetComponent<shooting_player>().power_gauge3 = power_gauge3;
                    Player1.GetComponent<shooting_player>().power_text= power_text;
                    Player1.GetComponent<shooting_player>().die_check = true;
                    respawn_time_check = 0;
                }
            }
        }
        else if(Life<=0)
        {
            SceneManager.LoadScene("gameover");
        }
    }
}
