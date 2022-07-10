using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type6 : MonoBehaviour//폭팔탄
{
    public float time;
    public float scale=0.1f;
    public bool destroy_check;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 3)
        {
            
            transform.localScale = new Vector3(scale, scale, scale);
            if(scale <= 23f&&destroy_check==false)
            {
                scale += 4f;
            }
            else
            {
                destroy_check =true;
                if (destroy_check)
                {
                    scale -= 4f;
                    if (scale <= 0)
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}
