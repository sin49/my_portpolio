using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_star_spin : MonoBehaviour //플레이어를 조준하는 별모양 탄막을 회전 시키는 클레스
{
    public float spin_;
    public float spin_add;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //spin_ 각도만큼 z축으로 회전
        transform.rotation = Quaternion.Euler(0, 0, spin_);
        spin_ += spin_add;

    }
}
