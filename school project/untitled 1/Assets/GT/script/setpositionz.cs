using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setpositionz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
