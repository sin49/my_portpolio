using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_2_special_bullet : MonoBehaviour
{
    public float speed;
    public int Vcheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= 4)
        {
            Vcheck = 0;
        }
        else if (transform.position.y <= -4)
        {
            Vcheck = 1;
        }
        transform.Translate(new Vector2(-1 * speed * Time.deltaTime/2, 0));
        if (Vcheck == 0)
        {
            transform.Translate(new Vector2(0, -1 * speed * Time.deltaTime));
        }
        else
        {
            transform.Translate(new Vector2(0, 1 * speed * Time.deltaTime));
        }
    }
}
