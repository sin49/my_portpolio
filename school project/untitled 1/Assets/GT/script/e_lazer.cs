using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_lazer : MonoBehaviour
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
    
