using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet : MonoBehaviour
{
    public float speed;
    public Transform enemy_location;
    public Vector3 player_location;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = player_location - enemy_location.position;
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
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
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
