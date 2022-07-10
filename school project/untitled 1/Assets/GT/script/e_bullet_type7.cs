using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type7 : MonoBehaviour
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
