using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShaderManager
{
    protected Color color=new Vector4(1,1,1,0);
   protected Color Emission;
   public Material Transparent_Material;
   protected MeshRenderer m;
    protected Renderer r;
    float Transparent_duration = -1;
    protected Color Original_Color;
    protected Material Original_Material;
    protected Color Original_Emission;
    public ColorShaderManager(GameObject obj) 
    {
        r = obj.GetComponent<Renderer>();
        m = obj.GetComponent<MeshRenderer>();
    }
    public void accept_shader_setting(Color color,Color Emission)
    {
        set_color(color);
        Original_Color = color;
        set_emission(Emission);
        Original_Emission = Emission;
    }
    protected  void set_color(Vector4 a)
    {
        color = a;
        r.material.SetColor("_Color", color);
    }
    protected void set_emission(Vector4 a)
    {
        Emission = a;
        r.material.SetColor("_Emission", Emission);
    }
  public void set_Transparent()
    {
        if (Transparent_Material != null)
            m.material = Transparent_Material;
    }
    public void set_original_shader()
    {
        m.material = Original_Material;
        set_color(Original_Color);
        set_emission(Original_Emission);
        Transparent_duration = -1;
    }
    public void active_Transparent_Shader(float f)
    {
        if (Transparent_duration < 0)
        {
            set_Transparent();
            Transparent_duration = f;
        }
        else
        {
            set_color(new Vector4(color.r, color.g, color.b, (f / Transparent_duration)));
        }

    }
}
