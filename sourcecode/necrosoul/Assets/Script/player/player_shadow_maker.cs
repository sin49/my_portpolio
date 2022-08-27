using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_shadow_maker : MonoBehaviour//�÷��̾��� ȯ���� �����ϴ� Ŭ����(Ʃ�丮�� �뵵�� ���)
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
        //ȯ���� �ѹ��� ���������ʾҴٸ� ȯ�� ����
        if (Player_shadow!=null&&Player_shdow_created == null&&shadow_trigger==true)
        {
            make_shadow();
        }//�� �������ʹ´� ������ ȯ���� ��Ȱ��ȭ�Ǹ� ������ ��ġ�� �ʱ�ȭ��Ű�� ȯ���� Ȱ��ȭ 
        else if (!(Player_shdow_created == null) && Player_shdow_created.activeSelf==false)
        {
            reset_shadow();
        }
    }
    //ȯ���� ��ġ ,������ �ʱ�ȭ �� �� �ٽ� Ȱ��ȭ ��Ų��
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
    //ȯ���� �����ϰ� ������ �׼��� �����Ѵ�
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
   //�÷��̾ Ư�� ��ġ�� ���� �� ȯ���� ������Ų��(Ʃ�丮��)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shadow_trigger = true;
            if (UI != null)//Ʃ�丮�� ui Ȱ��ȭ
            {
                UI.SetActive(true);
            }
        }
    }
    //�÷��̾ Ư�� ��ġ�� ����� ȯ���� �ı��Ѵ�
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shadow_trigger = false;
            if (UI != null)//Ʃ�丮�� ui ��Ȱ��ȭ
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
