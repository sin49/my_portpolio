using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathboard : MonoBehaviour//킬로그(누가 누구를 죽였는지 알림)
{
   public Text[] death_text = new Text[5];//저장용 배열
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        for(int i = 0; i < death_text.Length; i++)
        {
            //텍스트의 위치를 조정한다
            if(death_text[i]!=null)
            death_text[i].transform.position = this.transform.position + new Vector3(0, 50 - (i * 20), 0);//위치 조정
            
        }
        if (death_text[0] == null)//없을시 비활성화
        {
            this.gameObject.SetActive(false);
        }
        else//있을시
        {
            time += Time.deltaTime;
            if (time >= 5)
            {
                Destroy(death_text[0].gameObject);//시간 지나면 첫번째 배열 삭제
                time = 0;
                for(int i = 1; i<death_text.Length; i++)
                {
                    death_text[i - 1] = death_text[i];//배열들을 한칸씩 당김
                }
            }
        }
    }
}
