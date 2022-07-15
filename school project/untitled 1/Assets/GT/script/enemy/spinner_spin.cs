using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinner_spin : MonoBehaviour//오브젝트를 회전시키는 클레스
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
        //spin_값을 증가시키는 것으로 오브젝트를 회전
        spin_ += spin_add;
        transform.rotation = Quaternion.Euler(0, 0, spin_);
    }
}
