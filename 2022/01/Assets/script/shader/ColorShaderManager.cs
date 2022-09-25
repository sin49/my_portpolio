using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShaderManager : MonoBehaviour
{
    public Color color=new Vector4(1,1,1,0);
    public Color Emission;
    Renderer r;
    private void Awake()
    {
        r = this.GetComponent<Renderer>();
        r.material.SetColor("Color", color);
        r.material.SetColor("Emission", Emission);
    }
    public  void set_color(Vector4 a)
    {
        color = a;
       
    }
    public void set_emission(Vector4 a)
    {
        Emission = a;
     
    }
    // Update is called once per frame
    void Update()
    {
        r.material.SetColor("_Color", color);
        r.material.SetColor("_Emission", Emission);
    }
}
