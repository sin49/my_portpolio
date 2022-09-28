using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_children_Shader : MonoBehaviour, Character_observer
{
    public ChracterShader shader;
    float deadbody_duration;
    Rigidbody rgd;
    public bool Object_flying;
    Vector3 localPosition;
    Quaternion localrotation;
    Transform parent;
    public void update_information(int a, GameCharacter character)
    {
        
        if (a <= 0)
        {
        
            if (Object_flying) {
                rgd.constraints = RigidbodyConstraints.None;
                rgd.transform.SetParent(this.transform.parent.parent);
                rgd.AddForce((-1*this.transform.parent.forward+Vector3.up)*0.000035f);
            }
           
                shader.active_death_Shader();
      
        }
        
    }
    private void Awake()
    {
        shader = this.GetComponent<ChracterShader>();
        GameCharacter chr = this.transform.parent.parent.GetComponent<GameCharacter>();
        chr.AddObserver(this);
        shader.deadbody_duration = chr.deadbody_duration;
        deadbody_duration= shader.deadbody_duration;
        if (Object_flying) {
            rgd = this.GetComponent<Rigidbody>();
            localrotation = this.transform.localRotation;
            localPosition = this.transform.localPosition;
            parent = this.transform.parent;
                }

        }
    private void Update()
    {
        if (Object_flying&& !parent.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        //initialize_children();
        
    }
    private void OnEnable()
    {
        initialize_children();
    }
    public void initialize_children()
    {
        shader.deadbody_duration = deadbody_duration;
        shader.initialize_shader();
        if (Object_flying)
        {
            this.transform.SetParent(parent);
            this.transform.localRotation = localrotation;
            this.transform.localPosition = localPosition;
            rgd.constraints = RigidbodyConstraints.FreezeAll;
     
        }
    }
}
