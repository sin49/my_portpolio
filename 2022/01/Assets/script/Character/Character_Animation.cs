using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Animation : MonoBehaviour
{
    float hitted_emission_duration=0.6f;
    float _hitted_emission_duration;
    Color hitted_emission_color;

    float die_alpha_duration = 1f;
    float _die_alpha_duration;
    Color Die_color;

    Color Original_color;
    Color Original_Emission;
    ColorShaderManager C_manager;
    Animator ani;
    public delegate void active_animation();
    public delegate void _action_animation(int n);
    public active_animation hitted_animation;
    public _action_animation action_animation;
    private void Awake()
    {
        hitted_emission_color = new Vector4(1, 0, 0, 1);
        C_manager = this.GetComponent<ColorShaderManager>();
        ani = this.GetComponent<Animator>();
        Original_color = C_manager.color;
        Die_color = Original_color;
        Original_Emission = C_manager.Emission;
        hitted_animation += new active_animation(change_hitted_emission);
        hitted_animation += new active_animation(active_hitted_animation);
        action_animation += new _action_animation(active_action_animation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_hitted_emission_duration > 0)
        {
            _hitted_emission_duration -= Time.deltaTime;

        }
        else
        {
            C_manager.set_emission(Original_Emission);
        }
        if (_die_alpha_duration > 0)
        {
            _die_alpha_duration -= Time.deltaTime;
            Die_color = new Vector4(Die_color.r, Die_color.g, Die_color.b, (_die_alpha_duration / die_alpha_duration));
            C_manager.set_color(Die_color);
        }
    }

    void change_hitted_emission()
    {
        C_manager.set_emission(hitted_emission_color);
        _hitted_emission_duration = hitted_emission_duration;
    }
    public void change_hitted_event()
    {
        hitted_animation-= new active_animation(change_hitted_emission);
        hitted_animation -= new active_animation(active_hitted_animation);
        hitted_animation += new active_animation(active_death_animation);
    }
    void active_hitted_animation()
    {
        ani.SetTrigger("hitted");
       
    }
   void active_action_animation(int n)
    {
        ani.SetTrigger("action" + n);
    }
    void active_death_animation()
    {
        ani.SetTrigger("Die");
        _die_alpha_duration = die_alpha_duration;
    }
}
