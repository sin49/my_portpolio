using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterShader : ColorShaderManager
{
    float hitted_emission_duration = 0.2f;
    float _hitted_emission_duration;
    Color hitted_emission_color;
  
   
    public float deadbody_duration { get; set; }
    float _deadbody_duration;
    Color Die_color;
    Color Original_Color;
    Color Original_Emission;
    Material Original_material;
    
    private void Awake()
    {
        Original_material = this.GetComponent<MeshRenderer>().material;
        r = this.GetComponent<Renderer>();
        set_color(color);
        set_emission(Emission);
        hitted_emission_color = new Vector4(1, 0, 0, 1);
        Original_Color = color;
        Original_Emission = Emission;
        Die_color =color;
    
      
   }
    public void initialize_shader()
    {
        this.GetComponent<MeshRenderer>().material = Original_material;
        set_color(Original_Color);
        set_emission(Original_Emission);
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
           
            set_emission(Original_Emission);
        }
        if (_deadbody_duration > 0)
        {
            _deadbody_duration -= Time.deltaTime;
            Die_color = new Vector4(Die_color.r, Die_color.g, Die_color.b, (_deadbody_duration / deadbody_duration));
            set_color(Die_color);
           
        }
    }
    public void change_hitted_emission()
    {
        set_emission(hitted_emission_color);
        _hitted_emission_duration = hitted_emission_duration;
    }
    public void active_death_Shader()
    {
        change_transparent_material();

        _deadbody_duration = deadbody_duration;
    }

   
}
