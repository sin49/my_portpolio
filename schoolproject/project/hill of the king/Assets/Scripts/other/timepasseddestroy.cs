using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timepasseddestroy : MonoBehaviour//시간이 지나면 파괴
{
    public float time = 0;
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
            time = 0;
           Destroy(this.gameObject);
        }
    }
}
