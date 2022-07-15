using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_special_bullet_2 : MonoBehaviour//boss3_special_bullet_1.cs로 생성된 폭팔탄
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
        //빠른 속도로 자신의 scale을 줄인 후 파괴된다.
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
