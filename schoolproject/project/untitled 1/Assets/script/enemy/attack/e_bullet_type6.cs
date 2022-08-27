using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type6 : MonoBehaviour//일정 시간 후 제자리에서 폭팔하는 탄
{
    public float time;
    public float scale=0.1f;
    public bool destroy_check;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        
        if (time >= 3)
        {
            //3초 뒤 정해진 scale 값 만큼 탄의 localScale 순간적으로 변경시켜서 탄의 범위를 넒힌다
            transform.localScale = new Vector3(scale, scale, scale);
            if(scale <= 23f&&destroy_check==false)
            {
                scale += 4f;
            }
            //일정 scale에 도달했다면 빠른 속도로 scale 값을 줄인 후 파괴한다
            else
            {
                destroy_check =true;
                if (destroy_check)
                {
                    scale -= 4f;
                    if (scale <= 0)
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}
