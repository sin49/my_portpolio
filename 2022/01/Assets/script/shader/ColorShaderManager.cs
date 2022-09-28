using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShaderManager : MonoBehaviour
{
    public Color color=new Vector4(1,1,1,0);
    public Color Emission;
    public Material Transparent_Material;
    protected Renderer r;
 
    
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
    protected void change_transparent_material()
    {
     
        this.GetComponent<MeshRenderer>().material = Transparent_Material; 
            }
  
   
}
