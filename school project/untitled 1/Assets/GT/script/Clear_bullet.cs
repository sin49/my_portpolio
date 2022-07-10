using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear_bullet : MonoBehaviour
{
    public GameObject[] bullet;
    public GameObject[] enemy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bullet = GameObject.FindGameObjectsWithTag("e_bullet");
        enemy = GameObject.FindGameObjectsWithTag("enemy");
        
        
    }
    public void run()
    {
        for (int i = 0; i < bullet.Length; i++)
        {
            Destroy(bullet[i]);

        }
        for (int i = 0; i < enemy.Length; i++)
        {
            Destroy(enemy[i]);

        }
    }
    public void run2()
    {
        for (int i = 0; i < bullet.Length; i++)
        {
            Destroy(bullet[i]);

        }
        
    }

}
