using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_shadow_maker : MonoBehaviour//플레이어의 환영을 생성하는 클레스(튜토리얼 용도로 사용)
{
    public GameObject Player_shadow;
    GameObject Player_shdow_created;
    public float shadow_timer;
    public Transform spwan_pos;
    public GameObject UI;
    public bool shadow_trigger;
    public int shadow_animation_type;
    public bool mirror;
    // Start is called before the first frame update
    void Start()
    {
        if (UI != null)
        {
            UI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //환영이 한번도 생성되지않았다면 환영 생성
        if (Player_shadow!=null&&Player_shdow_created == null&&shadow_trigger==true)
        {
            make_shadow();
        }//그 다음부터는는 생성한 환영이 비활성화되면 정보와 위치를 초기화시키고 환영을 활성화 
        else if (!(Player_shdow_created == null) && Player_shdow_created.activeSelf==false)
        {
            reset_shadow();
        }
    }
    //환영의 위치 ,정보를 초기화 한 후 다시 활성화 시킨다
    void reset_shadow()
    {
      
        var b = Player_shdow_created.transform.GetChild(0).GetComponent<Player_shadow>();
        b.shadow_original_timer = shadow_timer;
        b.shadow_time = shadow_timer;
        b.shadow_type = true;
        b.once_chk = true;
        b.anim_chk = false;
        b.animation_level = shadow_animation_type;
        b.transform.position = spwan_pos.position;
        Player_shdow_created.SetActive(true);
    }
    //환영을 생성하고 지정된 액션을 수행한다
    void make_shadow()
    {
        
        GameObject a = Instantiate(Player_shadow, spwan_pos.position, Quaternion.identity);
        Player_shdow_created = a;
        var b= a.transform.GetChild(0).GetComponent<Player_shadow>();
        b.shadow_original_timer = shadow_timer;
        b.shadow_time = shadow_timer;
        b.shadow_type = true;
        b.once_chk = true;
        
        b.animation_level = shadow_animation_type;
    }
   //플레이어가 특정 위치에 있을 때 환영을 생성시킨다(튜토리얼)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shadow_trigger = true;
            if (UI != null)//튜토리얼 ui 활성화
            {
                UI.SetActive(true);
            }
        }
    }
    //플레이어가 특정 위치에 벗어나면 환영을 파괴한다
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shadow_trigger = false;
            if (UI != null)//튜토리얼 ui 비활성화
            {
                UI.SetActive(false);
            }
            if(Player_shdow_created != null)
            {
                Destroy(Player_shdow_created);
            }
        }
    }
}
