using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameOBj : MonoBehaviour,GameObj
{
    protected virtual void Awake()
    {
        if ((rgd = this.gameObject.GetComponent<Rigidbody>()) == null)
        {
            rgd = this.gameObject.AddComponent<Rigidbody>();
            rgd.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    protected virtual void OnEnable()
    {
        Initialized();
        initialize_handler();
    }
    protected virtual void FixedUpdate()
    {
        //우선순위0번:캐릭터 활성화 여부
        if (!object_activasion)
            return;
        //우선순위1번:캐릭터 사망 여부
        if (isDie)
        {
            foreach (Action a in _Destroy_handler)
            {
                a.Invoke();
            }
            return;
        }
        //우선순위 2번:뷰포트 내부에 존재 여부
        if (check_viewport && !handle_object_by_viewport())
            return;
        //우선순위 3번: 캐릭터 조작불가 상태
        if (cant_handle_time > 0)//밀려남
        {
            foreach (Action<float> a in CantHandle_handler)
                a.Invoke(cant_handle_time);
            return;
        }
    }
    protected virtual void Initialized()//초기화
    {
        object_activasion = false;
        velocity_buffer = Vector3.zero;
        deadbody_duration = 0;
        cant_handle_time = 0;
        isDie = false;
        on_viewport = false;
        rgd.velocity = Vector3.zero;
        if(isShader)
            initialize_shader();
    }
    #region 활성화
    public bool object_activasion;
    Vector3 velocity_buffer;
    public virtual void active_obj()
    {
        object_activasion = true;
        if(velocity_buffer != Vector3.zero)
            rgd.velocity = velocity_buffer;
    }

    public virtual void deactive_chr()
    {
        object_activasion = false;
        velocity_buffer = rgd.velocity;
        rgd.velocity = Vector3.zero;
    }
    #endregion

    #region 제거
    public float deadbody_duration { get; set; }//잔해 시간
    protected bool is_pulling;
    protected bool isDie;
   protected virtual void _Destroy()
    {
        if ((deadbody_duration -= Time.deltaTime) <= 0){
            if (is_pulling)
                this.gameObject.SetActive(false);
            else
                Destroy(this.gameObject);
        }
    }
    #endregion

    #region 물리운동
    protected Rigidbody rgd;
    protected float speed=1;
    protected void Move(Vector3 vec)
    {
        rgd.velocity = vec.normalized * speed*10;
    }
    protected float cant_handle_time;
    public virtual void forced(Vector3 vec, float forced_time)
    {
        rgd.velocity = Vector3.zero;
        rgd.AddForce(-1 * vec, ForceMode.Impulse);
        cant_handle_time = forced_time;
    }
    #endregion

    #region 뷰포트처리
    bool on_viewport;//뷰포트에 존재하는가?
    bool check_viewport;//뷰포트의 영향을 받는가?
    void go_to_viewport()
    {
        if (rgd.velocity == Vector3.zero)
        {
            Vector3 v = Vector3.zero;
            if (viewport_position.x > 0.98f)
            {
                v.x = -2;
            }
            else if (viewport_position.x < 0.02f)
            {
                v.x = 2;
            }
            if (viewport_position.y > 0.98f)
            {
                v.z = -2;
            }
            else if (viewport_position.y < 0.02f)
            {
                v.z = 2;
            }
            rgd.velocity = v;
        }
        else
        {
            if (viewport_position.x <= 0.95f || viewport_position.x >= 0.05f || viewport_position.y <= 0.95f || viewport_position.y >= 0.05f)
            {
                rgd.velocity = Vector3.zero;
                on_viewport = true;
            }
        }
    }
    Vector3 viewport_position;
    void snap_to_viewport()
    {
        if (viewport_position.x > 0.95f)
        {
            viewport_position.x = 0.95f;
        }
        else if (viewport_position.x < 0.05f)
        {
            viewport_position.x = 0.05f;
        }
        if (viewport_position.y > 0.95f)
        {
            viewport_position.y = 0.95f;
        }
        else if (viewport_position.y < 0.05f)
        {
            viewport_position.y = 0.05f;
        }
        transform.position = Camera.main.ViewportToWorldPoint(viewport_position);
    }
    private bool handle_object_by_viewport()
    {
        viewport_position = Camera.main.WorldToViewportPoint(transform.position);
        if (on_viewport)
        {
            if(cant_handle_time > 0)
                snap_to_viewport();
            return true;
        }
        else
        {
            go_to_viewport();
            return false;
        }
       
    }

    #endregion

    #region shader
    [Header("쉐이더 세팅")]
    public bool isShader=true;
    public Color _Color;
    public Color _Emission;
    public Material Transparent_Material;
    
   ColorShaderManager shader = null;
    protected virtual void initialize_shader(ColorShaderManager m=null)
    {
        
        if (shader == null)
        {
            if (m != null)
                shader = m;
            else
                shader = new ColorShaderManager(this.gameObject);

            shader.Transparent_Material = Transparent_Material;
            shader.accept_shader_setting(_Color, _Emission);
        }
        else
            shader.set_original_shader();
    }
   void Destroy_shader()
    {
        if(shader!=null)
        shader.active_Transparent_Shader(deadbody_duration);
    }
    #endregion
    #region 핸들러
    protected List<Action> _Destroy_handler;
    protected List<Action<float>> CantHandle_handler;
    protected virtual void initialize_handler()
    {
        _Destroy_handler.Clear();
        register_Handler();
        if(isShader)
            register_shader_handler();
    }
    protected virtual void register_Handler()
    {
        _Destroy_handler.Add(_Destroy);
        CantHandle_handler.Add((t) => t-= Time.deltaTime);
    }
    protected virtual void register_shader_handler()
    {
        _Destroy_handler.Add(Destroy_shader);
    }
    protected void execute_handler(List<Action> handler)
    {
        foreach (Action a in handler)
        {
            a.Invoke();
        }
    }
    #endregion
}
