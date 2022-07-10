using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_special_bullet_2 : MonoBehaviour
{
    public float time;
    public float scale=1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 2)
        {
            transform.localScale = new Vector3(scale, scale, scale);
            scale -= 1f;
            if (scale <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
