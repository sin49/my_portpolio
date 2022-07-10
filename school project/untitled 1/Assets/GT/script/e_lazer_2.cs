using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_lazer_2 : MonoBehaviour
{
    public bool lazer_explosion;
    public float lazer_time;
    public float scale;
    public bool destroy_check;
    public float redcolor;
    public bool lazer_damage;
    public GameObject lazer_angle;
    public GameObject enemy;
    public float time;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        transform.localScale = new Vector3(scale, 40, scale);
        if (scale <= 25 && destroy_check == false)
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
                if (time >= 0.2)
                {
                    for (int i = 0; i <= 5; i++)
                    {
                        GameObject enemy1 = Instantiate(enemy, lazer_angle.transform.position + new Vector3(-2 * i, 0, 0), lazer_angle.transform.rotation);
                        enemy1.GetComponent<Enemy_basic>().e_hp = 2;
                        enemy1.GetComponent<Enemy_basic>().speed = 1;
                        enemy1.GetComponent<Enemy_basic>().onchasing = true;
                        enemy1.GetComponent<Enemy_basic>().e_type = 2;
                        enemy1.GetComponent<e_bulletManager>().e_bullet_mode = 0;
                    }
                    time = 0;
                }
                if (scale <= 0)
                {
                    
                    Destroy(lazer_angle.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    }
