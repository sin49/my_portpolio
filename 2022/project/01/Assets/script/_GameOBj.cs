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
        //�켱����0��:ĳ���� Ȱ��ȭ ����
        if (!object_activasion)
            return;
        //�켱����1��:ĳ���� ��� ����
        if (isDie)
        {
            foreach (Action a in _Destroy_handler)
            {
                a.Invoke();
            }
            return;
        }
        //�켱���� 2��:����Ʈ ���ο� ���� ����
        if (check_viewport && !handle_object_by_viewport())
            return;
        //�켱���� 3��: ĳ���� ���ۺҰ� ����
        if (cant_handle_time > 0)//�з���
        {
            foreach (Action<float> a in CantHandle_handler)
                a.Invoke(cant_handle_time);
            return;
        }
    }
    protected virtual void Initialized()//�ʱ�ȭ
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
    #region Ȱ��ȭ
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

    #region ����
    public float deadbody_duration { get; set; }//���� �ð�
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

    #region �����
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

    #region ����Ʈó��
    bool on_viewport;//����Ʈ�� �����ϴ°�?
    bool check_viewport;//����Ʈ�� ������ �޴°�?
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
    [Header("���̴� ����")]
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
    #region �ڵ鷯
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
