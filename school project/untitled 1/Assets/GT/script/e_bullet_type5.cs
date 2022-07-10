using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type5 : MonoBehaviour//벽에 튕김
{
    public float Vspeed;
    public float Hspeed;
    public bool Vcheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(-1 * Hspeed * Time.deltaTime, 0));
        //가로이동
        if (Vcheck == true)
        {
            transform.Translate(new Vector2(0, -1 * Vspeed * Time.deltaTime));
        }
        else
        {
            transform.Translate(new Vector2(0, 1 * Vspeed * Time.deltaTime));
        }
        if(transform.position.y >= 3.8)
        {
            Vcheck = true;
        }
        else if (transform.position.y <= -3.8)
        {
            Vcheck = false;
        }
    }
}
