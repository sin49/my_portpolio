using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_special_bullet_1 : MonoBehaviour
{
    public float speed = 2;
    public Transform enemy_location;
    public GameObject player_location;
    public Vector3 chasing_location;
    public int chasing_number;
    public bool chasing_check=false;
    public float time;
    public Vector3 dir;
    public GameObject explosion_bullet;
    public float spawn_bullet_time;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        spawn_bullet_time += Time.deltaTime;
        player_location = GameObject.FindWithTag("Player");
        if (!chasing_check)
        {
            time += Time.deltaTime;
            if (time >= 1)
            {
                speed = 1;
                chasing_location = player_location.transform.position;
                dir = chasing_location - enemy_location.position;
                chasing_check = true;
                chasing_number++;
                time = 0;
            }
            else
            {
                speed = 0;
            }
        }
        if (transform.position.x < -8)
        {
            chasing_check = false;
            Vector3 pos = transform.position;
            pos.x = -7f;
            transform.position = pos;
        }
        if (transform.position.x > 8)
        {
            chasing_check = false;
            Vector3 pos = transform.position;
            pos.x = 7f;
            transform.position = pos;
        }
        if (transform.position.y > 7)
        {
            chasing_check = false;
            Vector3 pos = transform.position;
            pos.y = 6f;
            transform.position = pos;
        }
        if (transform.position.y < -7)
        {
            chasing_check = false;
            Vector3 pos = transform.position;
            pos.y = -6f;
            transform.position = pos;
        }
        if (chasing_number>=4)
        {
            Destroy(this.gameObject);
        }
        transform.Translate(dir * speed * Time.deltaTime);
        if (spawn_bullet_time >= 0.3)
        {
            GameObject bullet = Instantiate(explosion_bullet, transform.position, transform.rotation);
            spawn_bullet_time = 0;
        }
    }
}
