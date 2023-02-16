using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameOBj : MonoBehaviour,GameObj
{
   bool object_activasion;
    protected Rigidbody rgd;
    Vector3 velocity_buffer;

    public virtual void active_obj()
    {
        object_activasion = true;
        if(velocity_buffer != Vector3.zero)
            rgd.velocity = velocity_buffer;
    }

    public virtual void deactive_chr()
    {
        object_activasion = false;
        if (rgd! != null){
            velocity_buffer = rgd.velocity;
            rgd.velocity = Vector3.zero;
        }
    }


    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (!object_activasion)
            return;
    }
}
