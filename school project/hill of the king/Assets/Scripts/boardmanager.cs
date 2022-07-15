using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boardmanager : MonoBehaviour
{
    public GameObject death_board;
    public Text board_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    public void player_death_board(string a, string b)//누가 누구를 죽였는지 알리기
    {
        if (!death_board.activeInHierarchy)
            death_board.SetActive(true);
        Text t = Instantiate(board_text, death_board.transform);
        t.text = a + "=>" + b;
        deathboard d_board = death_board.GetComponent<deathboard>();
        int i = 0;
        while (d_board.death_text[i] != null)
        {
            i++;
            if (i == d_board.death_text.Length)
            {
                Destroy(d_board.death_text[0].gameObject);
                d_board.time = 0;
                for (int q = 1; q < d_board.death_text.Length; q++)
                {
                    d_board.death_text[q - 1] = d_board.death_text[q];
                }
                i--;
            }
        }
        d_board.death_text[i] = t;
    }
}
