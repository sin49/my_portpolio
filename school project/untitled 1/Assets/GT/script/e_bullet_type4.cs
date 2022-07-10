using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type4 : MonoBehaviour
{
    public float speed = 6;
    public Transform enemy_location;
    public int num;
    public GameObject[] bullet;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        num = Random.Range(0, 3);
        bullet[num].gameObject.SetActive(true);
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
        
        transform.position += dir * speed * Time.deltaTime;
    }
}
