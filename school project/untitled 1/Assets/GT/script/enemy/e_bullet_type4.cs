using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type4 : MonoBehaviour //dir의 벡터로 움직이는 탄
{
    public float speed = 6;
    public Transform enemy_location;
    public int num;
    public GameObject[] bullet;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        //생성될때마다 색깔이 변한다
        num = Random.Range(0, 3);
        bullet[num].gameObject.SetActive(true);
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        //화면 밖으로 나갈시 파괴
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
        //지정된 dir값으로 이동
        transform.position += dir * speed * Time.deltaTime;
    }
}
