
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameCharacter : _GameOBj, Character {

    protected override void Awake()
    {
        base.Awake();
        s = Stage._stage;

    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        execute_frame_handler();
        set_target();//타겟 지정
        if (is_LB_can_use_anywhere && start_lb)
            use_LB();
        if (target_distance != -9999 && target_distance > Distance_number)
        {

            chase_target();
        }
        else
        {
            Character_Act();
            if (!is_LB_can_use_anywhere && start_lb)
                use_LB();
        }
    }
    #region 캐릭터
    [Header("캐릭터")]
    public int ID;
    public int index;
    public Sprite chr_sprite;
    #endregion
    #region 능력치
   
    public Team T { get; set; }
    int MAX_hp;
    public float Distance_number;
    int ATK;
    public Position this_pos;
    public int current_hp { get; set; }
    #endregion
    #region 초기화

    protected override void Initialized()
    {
        base.Initialized();
        //임시
        MAX_hp = 100;
        current_hp = MAX_hp;
    
        deadbody_duration = 3f;
        rgd.freezeRotation = true;
        initialize_target();
        initialize_action();
        initialize_LB();
        if(is_animation)
        initialize_animation();
    }

    #endregion
    #region 활성화
    public override void active_obj()
    {
        base.active_obj();
        if (is_animation)
        {
            C_ani.set_animation_speed(1);
        }
    }
    public override void deactive_chr()
    {
        base.deactive_chr();
        execute_handler(CancelAction_handler);
        if (is_animation)
        {
            C_ani.set_animation_speed(0);
        }
    }

    #endregion
    #region 피격&회복 처리

    void Die()
    {
        isDie = true;
        execute_handler(CancelAction_handler);
        //LB_gauge = 0;
        rgd.freezeRotation = false;
        rgd.velocity = Vector3.zero;
        forced(new Vector3(UnityEngine.Random.Range(8, 16), UnityEngine.Random.Range(8, 16), UnityEngine.Random.Range(8, 16)), 0);
    }
    void hitted(int a)
    {
        current_hp -= a;
        notifyObserver();
        if (current_hp <= 0)
        {
            current_hp = 0;
            Die();

            return;
        }
    }
    void create_damage_font(int a)
    {
        Damage_font font = s.pulling_damage_font();
        font.set_font(a, this.transform);
    }
    //public void heal(int a)
    //{
    //    if ((current_hp += a) > MAX_hp)
    //        current_hp = MAX_hp;
    //    //회복 폰트?
    //    notifyObserver();
    //}
    #endregion
    #region 밀려남
    public override void forced(Vector3 vec, float forced_time)
    {
        execute_handler(CancelAction_handler);
        base.forced(vec, forced_time);

    }
    #endregion
    #region 옵저버
    Stage s;
    List<Character_observer> MyObserver = new List<Character_observer>();
    public void AddObserver(Character_observer s)
    {
        MyObserver.Add(s);
        s.update_information(current_hp, this);
    }

    public void DeleteObserver(Character_observer s)
    {
        MyObserver.Remove(s);
    }
    void DeleteAllObserver()
    {

        for (int i = MyObserver.Count - 1; i > 0; i--)
        {
            MyObserver.RemoveAt(i);
        }
    }
    public void notifyObserver()
    {
        foreach (Character_observer s in MyObserver)
        {
            s.update_information(current_hp, this);
        }
    }
    #endregion
    #region 행동
    float action_delay = 1f;
    float delay_time;
    Queue<Character_action> action_queue = new Queue<Character_action>();
    Character_action _current_action = new Character_action() { execution = true };
    Character_Priority c_priority = new Character_Priority();
    Coroutine act_corutine;
    bool on_action;
    float init_timer;
    void initialize_action()
    {
        delay_time = 0;
        on_action = false;
        init_timer = 0;
    }
    Character_action action_pop()
    {
        Character_action _action = action_queue.Dequeue();
        action_queue.Enqueue(_action);
        return _action;
    }
    void Character_Act()
    {
        if (on_action)
        {
            init_timer += Time.deltaTime;
        } else if (delay_time > 0)
        {
            delay_time -= Time.deltaTime;
        }
        else
        {
            //취할 행동이 없다면 리턴
            if (action_queue.Count == 0)
                return;
            if (_current_action.execution)
                _current_action = action_pop();
            foreach(Action<Character_action> a in Action_handler)
            {
                a.Invoke(_current_action);
            }
        }
    }

 
  
    IEnumerator Act(Character_action a)
    {
        on_action = true;
        yield return new WaitUntil(() => init_timer >= a.init_time);
        init_timer = 0;
        a.Invoke(ATK, this);
        delay_time = action_delay;
        gain_LBgauge();
        on_action = false;
    }
    #endregion
    #region 필살기
    bool is_LB_can_use_anywhere;
    public float LB_gauge { get; set; }
    public readonly float LB_gauge_max = 100;
    float LB_gauge_gain;
    bool on_LB;
    bool start_lb;
    Character_action LimitBurst;
    void initialize_LB()
    {
        LB_gauge = 0;
    }
    void gain_LBgauge()
    {
        if (LB_gauge < LB_gauge_max)
            LB_gauge += LB_gauge_gain;
        else
            return;
        if (LB_gauge > LB_gauge_max)
            LB_gauge = LB_gauge_max;
    }

    public void use_LB()
    {
        if (LB_gauge >= LB_gauge_max)
        {
            execute_handler(CancelAction_handler);
            StartCoroutine(Act_LB());
            start_lb = false;
        }

    }
    IEnumerator Act_LB()
    {
        on_action = true;
        LimitBurst.execution = false;
        yield return new WaitForSeconds(LimitBurst.init_time);
        LimitBurst.Invoke(ATK, this);
        on_action = false;
        delay_time = action_delay;

    }
    #endregion
    #region 핸들러
    //피격 핸들러
    List<Action<int>> Hitted_Handler = new List<Action<int>>();
    //매 프레임 마다 발동하는 핸들러
    List<Func<bool>> frame_handelr = new List<Func<bool>>();
    //행동을 취소하는 핸들러
    List<Action> CancelAction_handler = new List<Action>();
    //행동을 수행하는 핸들러
    List<Action<Character_action>> Action_handler = new List<Action<Character_action>>();
    public void register_frame_handler(Func<bool> a)
    {
        frame_handelr.Add(a);
    }
    void execute_frame_handler()
    {
        for(int n = 0; n < frame_handelr.Count; n++)
        {
            if (frame_handelr[n].Invoke())
            {
                frame_handelr.RemoveAt(n);
                n--;
            }
        }
    }
   
    protected override void register_shader_handler()
    {
        base.register_shader_handler();
        Hitted_Handler.Add(chr_shader.change_hitted_emission);
        Hitted_Handler.Add(new Action<int>((num1) => {
            register_frame_handler(chr_shader.return_hitted_emission); 
        }));
      
    }
    protected override void initialize_handler()
    {
        base.initialize_handler();
        if (is_animation)
            register_animation_handler();
    }
    protected void register_animation_handler()
    {
        _Destroy_handler.Add(C_ani.active_death_animation);
        CantHandle_handler.Add(C_ani.active_stun_timer_animator);
        CancelAction_handler.Add(C_ani.stop_current_animation);
        Action_handler.Add(C_ani.active_action_animation);

    }
    protected override void register_Handler()
    {
        base.register_Handler();
        Hitted_Handler.Add(hitted);
        Hitted_Handler.Add(create_damage_font);
        Action_handler.Add((a) => act_corutine = StartCoroutine(Act(a)));
        CancelAction_handler.Add(() => StopCoroutine(act_corutine));
    }
    public void execute_Hit_handler(int n)
    {
        foreach (Action<int> a in Hitted_Handler)
        {
            a.Invoke(n);
        }
    }
    #endregion
    #region 타게팅&추격
    GameCharacter chasing_target;



    float target_distance;
    void initialize_target()
    {
        chasing_target = null;
    }
    void set_target()
    {
        if (chasing_target == null||!chasing_target.object_activasion || chasing_target.current_hp < 0)
            chasing_target = Character_Priority.instance.get_enemy_by_distance(T, this.transform.position);
        else
            target_distance = chasing_target == null ? -9999 : (chasing_target.transform.position - transform.position).magnitude;
    }
    void chase_target()
    {
        transform.LookAt(chasing_target.transform);
        rgd.velocity = transform.forward * 10 * speed;
    }
    #endregion
    #region 쉐이더
    protected CharacterShader chr_shader;
    protected override void initialize_shader(ColorShaderManager m = null)
    {
        if (chr_shader == null)
            chr_shader = new CharacterShader(this.gameObject);       
        base.initialize_shader(chr_shader); 
        chr_shader.set_Emission_by_team(T);
    }
    #endregion
    #region 에니메이션
    Character_Animation C_ani;
    [Header("에니메이션 세팅")]
    public bool is_animation=true;
    public RuntimeAnimatorController this_ani_controller;
    void initialize_animation()
    {
        if (C_ani == null)
            C_ani = new Character_Animation(this);
        else
            C_ani.initialize_animation();
    }
    #endregion
}

