using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timepassednotactive : MonoBehaviour//시간이 지나면 비활성화
{
    public float time;
    public float time_max;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= time_max)
        {
            this.gameObject.SetActive(false);
        }
    }
}
