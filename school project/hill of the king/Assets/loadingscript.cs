using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadingscript : MonoBehaviour
{
    public GameObject[] playernumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playernumber=GameObject.FindGameObjectsWithTag("Player");
        if (playernumber.Length == 2)
        {
            Destroy(this.gameObject);
        }
    }
}
