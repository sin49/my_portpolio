using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3_ : MonoBehaviour
{
    public bool endure_check;
    public float time;
    public GameObject boss3_2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endure_check)
        {
            time += Time.deltaTime;
            if (time > 8)
            {
                GameObject boss = Instantiate(boss3_2, new Vector3(5f, 0, 10), transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }
}
