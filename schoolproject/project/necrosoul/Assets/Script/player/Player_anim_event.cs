using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_anim_event : MonoBehaviour//�÷��̾��� ���ϸ��̼��� ���� �Ͼ�� �̺�Ʈ Ŭ����
{
    [Header("Effects")]
    public float melee_force;
    public float melee_1_anim_time;
    public float melee_2_anim_time;
    public float melee_3_anim_time;
    bool landing_ef_chk;
    public AnimationClip melee_1_clip;
    public AnimationClip melee_1_hold_clip;
    public AnimationClip melee_2_clip;
    public AnimationClip melee_2_hold_clip;
    public AnimationClip melee_3_clip;
    public AnimationClip melee_3_hold_clip;
    public AnimationClip air_melee_clip;
    public AnimationClip air_melee_hold_clip;
    public GameObject Player_ghost;
    public List<GameObject> Player_ghost_instansi=new List<GameObject>();
    public Transform created_list;
    public GameObject m_RunStopDust;
    public GameObject m_JumpDust;
    public GameObject m_LandingDust;
    public GameObject m_DodgeDust;
    public GameObject m_WallSlideDust;
    public GameObject m_WallJumpDust;
    public GameObject m_AirSlamDust;
    public GameObject m_ParryEffect;
    public GameObject melee_1;
    public GameObject melee_1_instani;
    public GameObject melee_2;
    public GameObject melee_2_instani;
    public GameObject melee_3;
    public GameObject melee_3_instani;
    public GameObject air_melee_;
    public GameObject air_melee_instani;
    public float melee_1_reaction;
    public float melee_2_reaction;
    public float melee_3_reaction;
    public int melee_dmg;
    private AudioManager_PrototypeHero m_audioManager;
    public PlayerCharacter m_player;
    melee_attack index;
    public Player_animator p_anim;

    void AE_set_melee_attack_dmg()//���� ���ݷ��� �÷��̾��� ���ݷ°� �����Ѵ�
    {

        melee_dmg= Gamemanager.GM.game_ev.when_P_A_Key_input(Player_status.p_status.get_atk());//���� �̺�Ʈ Ŭ������ ����Ű�� �������� ȣ���Ѵ�
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++)//�÷��̾��� ȯ��(�ܻ�) ȿ�� /���� �� Ȱ��ȭ.��Ȱ��ȭ �Ͽ� ���(Ǯ��)
        {
            //�ļ� �� Ǯ�� ����Ʈ�� �ְ� ��Ȱ��ȭ ��Ų��
            var a= Instantiate(Player_ghost);
            a.transform.GetChild(0).GetComponent<Player_shadow>().p_created = true;
            Player_ghost_instansi.Add(a);
            //������ ȯ���� Ǯ�� ������ ������Ʈ�� �ڽ����� ����
            a.transform.SetParent(created_list);
            a.SetActive(false);
        }
        m_player= GetComponentInParent<PlayerCharacter>();
        p_anim = GetComponent<Player_animator>();
        m_audioManager = AudioManager_PrototypeHero.instance;
    }
 
    GameObject player_ghost_pulling()//������Ų ȯ�� ����Ʈ�� ��Ȱ��ȭ �� ȯ���� ���ӿ�����Ʈ�� �ø��������� �˻��Ͽ� ��ȯ�Ѵ�(Ǯ��)
    {
        int index=0;
        for(int i = 0; i < Player_ghost_instansi.Count; i++)
        {
            if (!Player_ghost_instansi[i].activeSelf)//Ȱ��ȭ����� ����
            {
                index = i;
            }
        }
        return Player_ghost_instansi[index];//��Ȱ��ȭ�� ȯ�� ��ȯ(��� Ȱ��ȭ ���¸� 0���� ��ȯ)
    }
    //player_ghost_attack_effect(1,2,3,air)
    //�̴� ������ �߻����� �� ���� �ڽ��� �����ϰ� �ִ� ���� ���ϸ��̼ǰ� ���� ���ϸ��̼��� �����ϴ� ȯ���� �÷��̾��� ��ġ ��¦ ���ʿ� ��ȯ�Ѵ�(����)
    //ȯ���� Ǯ�����Ѽ� ���Ǹ� ���ӽð��� �����ϴ� ���ϸ��̼��� ���̸� ���󰣴�. ���ӽð��� ������ �ٽ� ��Ȱ��ȭ�Ѵ�.
    //�÷��̾��� ���� ���ϸ��̼ǰ� ȯ���� ���ϸ��̼��� ��ġ�鼭 ���޾� �����ϴ� ������ �����
    void player_ghost_attack_1_effect()
    {
        if (melee_1_instani.GetComponent<melee_attack>().Double_attack_on)//�̴� ���� ����
        {

                var a = player_ghost_pulling();//��Ȱ��ȭ�� ȯ���� �����´�
                
            //ȯ���� ������ �����Ѵ�(�����ų ���ϸ��̼�,ȯ���� ���ӽð�,ȯ���� Ư���� ����)
                var b = a.transform.GetChild(0).GetComponent<Player_shadow>();
                b.shadow_original_timer = melee_1_clip.length ;
                b.shadow_time = melee_1_clip.length;
                b.once_chk = true;
                b.anim_chk = false;
                b.shadow_type = true;
                b.animation_level = 3;
            //������ ȯ���� Ȱ��ȭ �Ѵ�
                a.SetActive(true);
  
          
        }
    }
    void player_ghost_attack_2_effect()
    {
        if (melee_2_instani.GetComponent<melee_attack>().Double_attack_on)
        {
            var a = player_ghost_pulling();

            var b = a.transform.GetChild(0).GetComponent<Player_shadow>();
            b.shadow_original_timer = melee_1_clip.length;
            b.shadow_time = melee_1_clip.length;
            b.once_chk = true;
            b.anim_chk = false;
            b.shadow_type = true;
            b.animation_level = 1;
            a.SetActive(true);
        }
    }
    
    void player_ghost_attack_3_effect()
    {
        if (melee_3_instani.GetComponent<melee_attack>().Double_attack_on)
        {
            var a = player_ghost_pulling();

            var b = a.transform.GetChild(0).GetComponent<Player_shadow>();
            b.shadow_original_timer = melee_1_clip.length;
            b.shadow_time = melee_1_clip.length;
            b.once_chk = true;
            b.anim_chk = false;
            b.shadow_type = true;
            b.animation_level = 2;
            a.SetActive(true);
        }
    }
    void player_air_ghost_attack_effect()
    {
        if (air_melee_instani.GetComponent<melee_attack>().Double_attack_on)
        {
            var a = player_ghost_pulling();

            var b = a.transform.GetChild(0).GetComponent<Player_shadow>();
            b.shadow_original_timer = melee_1_clip.length;
            b.shadow_time = melee_1_clip.length;
            b.once_chk = true;
            b.anim_chk = false;
            b.shadow_type = true;
            b.animation_level = 11;
            a.SetActive(true);
        }
    }
    //�÷��̾� ������ϸ��̼��� ������ ��� ó���ϴ� �Լ��� �����Ų��
    void player_died()
    {
        m_player.death();
    }
    //��Ȱ�� �����ϴٸ� ��Ȱó���ϴ� �Լ��� �����Ų��(���� ����)
    void player_resurrection()
    {

        m_player.ressurection();
    }
    //������ �����Ҷ� �ݵ����� �÷��̾ ��¦�� ������ ������ ���� ����
    void hitted_push(float x)
    {
        //addforce.impulse�� �̿��� �÷��̾ ���������� ª�� �Ÿ��� ���ݾ� ���´�(���ݹݵ�)
        var p = this.transform.parent.GetComponent<Rigidbody2D>();
       
        Vector3 v3 = new Vector3(x, 0, 0);

        p.AddForce(v3*m_player.direction, ForceMode2D.Impulse);
    }
    //velocity�� x���� 0���� ����� ���� �ݵ��� ������ ������ ������ ���ϰ� �Ѵ�
    void hitted_push_end()
    {
        var p = this.transform.parent.GetComponent<Rigidbody2D>();
        p.velocity = new Vector3(0, p.velocity.y);
    }
    //melee_on(1,2,3,air)
    //���� ���ϸ��̼� ���� �� �÷��̾��� ���� ����(������Ʈ)�� Ȱ��ȭ ���� �÷��̾��� ������ �����Ѵ�
    //�ѹ��� �÷��̾��� ������ ���������ʾҴٸ� ���� �� �� ������Ʈ�� �޴´�
    //�� ���� ���ݿ����� ������ ������Ʈ�� �ٽ� Ȱ��ȭ ��Ų��(Ǯ��)
    //melee_off(1,2,3,air)
    //���� ���ϸ��̼��� ������ ���ݹݵ��� ���ְ� ���� ������ ��Ȱ��ȭ ���Ѽ� �÷��̾��� ������ ������ ���´�
    void Melee_1_on()
    {
            hitted_push(3f);//���� �ݵ����� ��¦ ������ ���´�
        
        if (melee_1_instani == null)//������ ���� ���ٸ�
        {
           //���� �� �� ������ ������ �����Ѵ�
            var a = Instantiate(melee_1, melee_1.transform.position, Quaternion.identity);
            index = a.GetComponent<melee_attack>();
            index.melee_force = melee_force*3;
            index.set_effect_rotate = true;
            index.effect_rotation = 0;
            a.transform.rotation = this.transform.parent.rotation;
            index.Damage = melee_dmg;
            //������ ������ Ǯ�� ������ ������Ʈ�� �ڽ����� ����
            a.transform.SetParent(created_list);
            //�÷��̾ �����Ѵٴ� ���� �̺�Ʈ�� �߻������� �˸���
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);

            //������ Ȱ��ȭ �ϰ� �̴ܰ����� �߻��ߴٸ� �̴ܰ��� ������ �����Ѵ�
            a.SetActive(true);
            melee_1_instani = a;
            player_ghost_attack_1_effect();
        }
        else//������ ���� �ִٸ� ������ ������ ����Ѵ�( ���� ������ �������� �ʴ� ���� ���� ���� ����)
        {
            //���� ���� ����
            melee_1_instani.transform.position = melee_1.transform.position;
            melee_1_instani.transform.rotation = this.transform.parent.rotation;
            index = melee_1_instani.GetComponent<melee_attack>();
            index.melee_force = melee_force*3;
            index.on_crit = false;
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            //disable_hit�� ���� ���� ������ �ٽ� Ȱ��ȭ ��Ų��
            index.disable_hit = false;
            index.Damage = melee_dmg;
            //���� Ȱ��ȭ
            melee_1_instani.SetActive(true);
            player_ghost_attack_1_effect();
        }
     
    }
    public void Melee_1_off()
    {

        if (melee_1_instani!=null)
            melee_1_instani.SetActive(false);//������ ��Ȱ��ȭ �Ѵ�

        hitted_push_end();//���� �ݵ��� �����Ѵ�
    }


    //���߰��ݿ� �����ؼ� �ݵ��� �������� �ʴ´�
    void air_melee_on()
    {
       
        
        if (air_melee_instani == null)
        {
           
            var a = Instantiate(air_melee_, air_melee_.transform.position, Quaternion.identity);

            index = a.GetComponent<melee_attack>();
            index.melee_force = melee_force * 2;
            a.transform.rotation = this.transform.parent.rotation;
            index.Damage = melee_dmg;
            index.set_effect_rotate = true;
            index.effect_rotation = 0;
            a.transform.SetParent(created_list);
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            air_melee_instani = a;
   
            a.SetActive(true);

           
            
            player_air_ghost_attack_effect();
         
        }
        else
        {
           air_melee_instani.transform.position = air_melee_.transform.position;
            air_melee_instani.transform.rotation = this.transform.parent.rotation;
            index = air_melee_instani.GetComponent<melee_attack>();
            index.melee_force = melee_force*2;
            index.on_crit = false;
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            index.disable_hit = false;
            index.Damage = melee_dmg;
            air_melee_instani.SetActive(true);
            player_air_ghost_attack_effect();
        }
    }
    public void air_Melee_off()
    {
        if (air_melee_instani != null)
            air_melee_instani.SetActive(false);

      
    }
    void Melee_2_on()
    {

        if (m_player.on_rush)
        {
            hitted_push(2.5f);
            m_player.on_rush = false;
        }
        else
        {

            hitted_push(1f);
        }

       
        if (melee_2_instani == null)
        {
            
         
            var a = Instantiate(melee_2, melee_2.transform.position, Quaternion.identity);
            index = a.GetComponent<melee_attack>();
            index.melee_force = melee_force;
            index.Damage = melee_dmg;
            index.set_effect_rotate = true;
            a.transform.rotation = this.transform.parent.rotation;
            index.effect_rotation = -0.2f;
            a.transform.SetParent(created_list);
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            a.SetActive(true);
            melee_2_instani = a;
            // melee_2.SetActive(true);
            player_ghost_attack_2_effect();
        }
        else
        {
            melee_2_instani.transform.position = melee_2.transform.position;
            melee_2_instani.transform.rotation = this.transform.parent.rotation;
            index = melee_2_instani.GetComponent<melee_attack>();
            index.melee_force = melee_force;
            index.on_crit = false;
            index.Damage = melee_dmg;
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            index.disable_hit = false;
            
            melee_2_instani.SetActive(true);
            player_ghost_attack_2_effect();
        }
    }
    public void Melee_2_off()
    {
        if (melee_2_instani != null)
            melee_2_instani.SetActive(false);
        hitted_push_end();
    }
    void Melee_3_on()
    {
        hitted_push(1f);
        if (melee_3_instani == null)
        {
            
            var a = Instantiate(melee_3, melee_3.transform.position, Quaternion.identity);
            index = a.GetComponent<melee_attack>();
            index.melee_force = melee_force;
            index.Damage = melee_dmg;
            a.transform.rotation = this.transform.parent.rotation;
            index.set_effect_rotate = true;
            index.effect_rotation = 90;
            a.transform.SetParent(created_list);
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            a.SetActive(true);
            melee_3_instani = a;
            player_ghost_attack_3_effect();
        }
        else
        {
            melee_3_instani.transform.position = melee_3.transform.position;
            melee_3_instani.transform.rotation = this.transform.parent.rotation;
            index = melee_3_instani.GetComponent<melee_attack>();
            index.melee_force = melee_force;
            index.on_crit = false;
            index.Damage = melee_dmg;
            Gamemanager.GM.game_ev.when_P_Attack_effect(index);
            index.disable_hit = false;
            melee_3_instani.SetActive(true);
            player_ghost_attack_3_effect();
        }
        // melee_3.SetActive(true);
    }
    public void Melee_3_off()
    {
        if (melee_3_instani != null)
            melee_3_instani.SetActive(false);
        hitted_push_end();
    }
    //�÷��̾��� ���� ���� Ƚ���� �ʱ�ȭ�Ѵ�
    void jump_return()
    {
        //player_status =�÷��̾��� �������� �����ϴ� Ŭ����
        if (m_player.jump_count != Player_status.p_status.get_jump_count())
            m_player.jump_count = Player_status.p_status.get_jump_count();
    }





    //AE_XXXXX
    //���ϸ��̼ǿ� ���� ���带 ����ϰ� ����Ʈ�� ������Ų��
    //AE_XXXXX�� �ڽ�(�����)�� �ڵ带 ������ �ʰ� �ٸ� ����� �������.
    void AE_runStop()
    {
        m_audioManager.PlaySound("RunStop");
        float dustXOffset = 0.6f;
        float dustYOffset = 0.078125f;
        m_player.SpawnDustEffect(m_RunStopDust, dustXOffset, dustYOffset);
    }

    void AE_footstep()
    {
        jump_return();
        m_audioManager.PlaySound("Footstep");
    }

    void AE_Jump()
    {
        m_audioManager.PlaySound("Jump");

      
        
            float dustYOffset = 0.078125f;
            m_player.SpawnDustEffect(m_JumpDust, 0.0f, dustYOffset);
        
        
    }
   
    void AE_Landing()
    {
        if (m_player.landing_chk)
        {
            jump_return();
            p_anim.ground_anim_chk = false;
            if (p_anim.ground_anim_chk)
            {
                m_audioManager.PlaySound("Landing");
                float dustYOffset = 0.078125f;
                m_player.SpawnDustEffect(m_LandingDust, 0.0f, dustYOffset);
                p_anim.ground_anim_chk = false;
            }
        }
    }

 

    void AE_AttackAirLanding()
    {
        m_audioManager.PlaySound("AirSlamLanding");
        float dustYOffset = 0.078125f;
        m_player.SpawnDustEffect(m_AirSlamDust, 0.0f, dustYOffset);

    }

    void AE_Hurt()
    {
        m_audioManager.PlaySound("Hurt");
        p_anim.Hit_state = false;
    }

    void AE_Death()
    {
        m_audioManager.PlaySound("Death");
    }

    void AE_SwordAttack()
    {
        m_audioManager.PlaySound("SwordAttack");
    }

    void AE_SheathSword()
    {
        m_audioManager.PlaySound("SheathSword");
    }

  
}
