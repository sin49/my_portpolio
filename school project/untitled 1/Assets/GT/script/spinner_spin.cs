using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinner_spin : MonoBehaviour
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
        spin_ += spin_add;
        transform.rotation = Quaternion.Euler(0, 0, spin_);
    }
}
