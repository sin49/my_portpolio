using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type5 : MonoBehaviour//위 아래로 튕기면서 움직이는 탄환
{
    public float Vspeed;
    public float Hspeed;
    public bool Vcheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //  Hspeed만큼 왼쪽으로 이동
        transform.Translate(new Vector2(-1 * Hspeed * Time.deltaTime, 0));
        // Vcheck의 값에 따라 위 호근 아래로 움직임
        if (Vcheck == true)
        {
            transform.Translate(new Vector2(0, -1 * Vspeed * Time.deltaTime));
        }
        else
        {
            transform.Translate(new Vector2(0, 1 * Vspeed * Time.deltaTime));
        }
        //탄이 마지막으로 화면의 위쪽 혹은 아래 쪽에 도달했는지를 bool 형식의  Vcheck값으로 체크
        if (transform.position.y >= 3.8)
        {
            Vcheck = true;
        }
        else if (transform.position.y <= -3.8)
        {
            Vcheck = false;
        }
    }
}
