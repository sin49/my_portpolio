using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ex_bullet : MonoBehaviour//강화탄:일반 탄보다 강력하며 적을 관통
{
    public float speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //오른쪽으로 이동
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        //화면 밖일시 자괴
        if (transform.position.x > 7.2)
        {
            Destroy(this.gameObject);
        }
    }
}
