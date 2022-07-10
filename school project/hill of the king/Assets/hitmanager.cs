using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitmanager : MonoBehaviour
{
    public int playernum;
    public int damage=10;
    // Start is called before the first frame update
    void Start()
    {
        playernum = gameObject.GetComponentInParent<playercontroller>().playernum;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
