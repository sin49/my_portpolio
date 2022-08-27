using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_lazer : MonoBehaviour//적의 레이저 패턴 클레스
{
    
    public bool lazer_explosion;
    public float lazer_time;
    public float scale;
    public bool destroy_check;
    public float redcolor;
    public bool lazer_damage;
    public GameObject lazer_angle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

       //lazer_angle로 만들어진 위치에 플레이어가 닿을 시 사망하는 레이저를 생성
       //scale값으로 공격 범위를 키웠다가 일정scale이 되면 줄어든 후 파괴
            transform.localScale = new Vector3(scale, 40, scale);
            if (scale <=25f && destroy_check == false)
            {
                scale += 5f;
            }
            else
            {
                destroy_check = true;
            Camera.main.GetComponent<CameraShake>().Shake();
            if (destroy_check)
                {
                    scale -= 3f;
                    if (scale <= 0)
                    {
                        Destroy(lazer_angle.gameObject);
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
    
