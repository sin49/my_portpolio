using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost_effect_rotation : MonoBehaviour//대쉬 잔상 특수효과
{
    ParticleSystem ps;
    public Material dash_ghost;
    public Material dash_ghost_mirror;
    // Start is called before the first frame update
    float dash_ghost_effect_timer;
    // Start is called before the first frame update
    void Start()
    {
        ps = this.GetComponent<ParticleSystem>();
        ParticleSystemRenderer r = this.GetComponent<ParticleSystemRenderer>();
        if (Gamemanager.GM.Player_obj.GetComponent<PlayerCharacter>().direction == 1)
            r.material = dash_ghost;
        else
            r.material = dash_ghost_mirror;
        dash_ghost_effect_timer = ps.main.duration + 0.02f;
    }

    // Update is called once per frame
    void FixedUpdate()//일정시간이 지나면 비활성화
    {
        if (dash_ghost_effect_timer > 0)
        {
            dash_ghost_effect_timer -= Time.deltaTime;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }
    public void OnEnable()//플레이어가 바라보는 방향에 맞도록 파티클 메터리얼을 변경한다(방향이 일치하도록)
    {
        ps = this.GetComponent<ParticleSystem>();
        ParticleSystemRenderer r = this.GetComponent<ParticleSystemRenderer>();
        if (Gamemanager.GM.Player_obj.GetComponent<PlayerCharacter>().direction == 1)
            r.material = dash_ghost;
        else
            r.material = dash_ghost_mirror;

        dash_ghost_effect_timer = ps.main.duration + 0.02f;
    }
}
