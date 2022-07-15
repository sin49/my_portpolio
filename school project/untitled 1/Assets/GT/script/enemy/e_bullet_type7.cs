using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type7 : MonoBehaviour//보스3 특수패턴으로 왼족 직선으로만 움직이는 탄
{
    public float speed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -8)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.x > 8)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.y > 7)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }

        transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
    }
    
}
