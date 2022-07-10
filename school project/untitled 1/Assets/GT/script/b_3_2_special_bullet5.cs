using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_3_2_special_bullet5 : MonoBehaviour
{
    public float speed=0.001f;
    public float speed_plus;
    public Transform enemy_location;
    public Vector3 player_location;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = player_location - enemy_location.position;
        speed_plus = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }
        speed += speed_plus;
        speed_plus += 0.001f;
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
