using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_starter : MonoBehaviour//세팅 메니저 생성
{
    public GameObject setting;
    List<GameObject> gm=new List<GameObject>();
    void Awake()
    {
        if (gm.Count == 0)
        {
            var a=Instantiate(setting);
            gm.Add(a);
        }
    }
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
