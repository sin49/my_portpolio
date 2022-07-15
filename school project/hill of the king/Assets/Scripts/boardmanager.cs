using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boardmanager : MonoBehaviour//킬로그를 만드는 클레스
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
        //킬보드가 비활성화 되있다면 활성화 시킨다
        if (!death_board.activeInHierarchy)
            death_board.SetActive(true);
        //킬로그를 만든다
        Text t = Instantiate(board_text, death_board.transform);
        t.text = a + "=>" + b;
        deathboard d_board = death_board.GetComponent<deathboard>();
        //킬보드가 활성화되어있던 상태(킬로그가 남아있는 상태)라면 킬보드의 배열의 갑슬 한칸씩 뒤로 밀어넣어서 0번자리를 확보한다
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
        //킬보드의 0번 잦리에 킬로그를 넣는다
        d_board.death_text[i] = t;
    }
}
