using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShader : ColorShaderManager
{
   
    float hitted_emission_duration = 0.2f;
    float _hitted_emission_duration;
    Color hitted_emission_color;






    public CharacterShader(GameObject obj):base(obj)
    {
        hitted_emission_color = new Vector4(1, 0, 0, 1);
    }

  public void set_Emission_by_team(Team t)
    {
        if (t == Team.Enemy)
            set_emission( use_enemy_emission());
        else
            set_emission(use_Player_emission());
    }
    Color use_enemy_emission()
    {
        Original_Emission = new Vector4(0.5f, 0.125f, 0.5f, 0);
        return Original_Emission;

    }
    Color use_Player_emission()
    {
        Original_Emission = new Vector4(0, 0, 0, 0);
        return Original_Emission;
       
    }
 
    public void change_hitted_emission(int n=0)
    {
        set_emission(hitted_emission_color);
        _hitted_emission_duration = hitted_emission_duration;
    }
    public bool return_hitted_emission()
    {
        if ( (_hitted_emission_duration -= Time.deltaTime) <= 0)
        {
            set_emission(Original_Emission);
            return true;
        }
        else 
            return false;
    }
  

   
}
