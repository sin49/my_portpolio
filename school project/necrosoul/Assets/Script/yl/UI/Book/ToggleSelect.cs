using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSelect : MonoBehaviour
{
    public Toggle My_toggle;
    public GameObject Select;
    // Start is called before the first frame update
    void Start()
    {
        
        Select.SetActive(false);
           
    }

    // Update is called once per frame
    void Update()
    {
        if (My_toggle.isOn)
        {
            Debug.Log("��� ���ष��");
            Select.SetActive(true);
        }
        else
        {
            Select.SetActive(false);
        }
    }
}
