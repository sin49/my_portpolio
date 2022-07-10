using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_chasing : MonoBehaviour
{
    Enemy_basic e_basic;
    public GameObject enemy;
    public float C_time=1;
    // Start is called before the first frame update
    void Start()
    {
        e_basic = GetComponent<Enemy_basic>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Collide");
            enemy.GetComponent<Enemy_basic>().onchasing = true;
            StartCoroutine(ChaisngControl());
        }
    }
    public IEnumerator ChaisngControl()
    {
        while (true)
        {
            enemy.GetComponent<Enemy_basic>().onchasing = true;
            yield return new WaitForSeconds(C_time);
        }
    }
}
