using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ex_bullet : MonoBehaviour
{
    public float speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        if (transform.position.x > 7.2)
        {
            Destroy(this.gameObject);
        }
    }
}
