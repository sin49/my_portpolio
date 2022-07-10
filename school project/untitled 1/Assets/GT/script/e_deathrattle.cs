using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class e_deathrattle : MonoBehaviour
{
    public Transform[] position;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Enemy_basic>().e_hp <= 1)
        {
            for(int i = 0; i <= 3; i++)
            {
                GameObject enemy1 = Instantiate(enemy, position[i].position, position[i].rotation);
                enemy1.GetComponent<e_bulletManager>().e_bullet_mode = 2;
                enemy1.GetComponent<Enemy_basic>().e_hp = 7;
                enemy1.GetComponent<Enemy_basic>().e_type = 2;
                enemy1.GetComponent<Enemy_basic>().speed = 0.5f;
            }
            Destroy(this.gameObject);
        }
    }
}
