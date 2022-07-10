using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_spawner : MonoBehaviour
{
    public bool boss_spawn_check;
    public GameObject boss;
    public Transform spawn_location;
    public GameObject gamemanager;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.GetComponent<shooting_GameManager>().boss_phase)
        {
            if (GameObject.FindGameObjectWithTag("enemy") == null&& GameObject.FindGameObjectWithTag("Player"))
            {
                if (!boss_spawn_check)
                {
                    time += Time.deltaTime;
                    if (time >= 2)
                    {
                        GameObject boss1 = Instantiate(boss, spawn_location.position, spawn_location.rotation);
                        boss_spawn_check = true;
                    }
                }
            }
        }
    }
}
