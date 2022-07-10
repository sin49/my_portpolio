using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_special_bullet_3 : MonoBehaviour
{
    public float speed = 1;
    public Transform enemy_location;
    public Vector3 player_location;
    public Vector3 dir;
    public float time;
    public float time_max;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        dir = player_location - enemy_location.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
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
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
        if (time >= time_max)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }
}
