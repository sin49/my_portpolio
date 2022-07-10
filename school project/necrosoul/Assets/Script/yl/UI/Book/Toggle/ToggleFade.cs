using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFade : MonoBehaviour
{
    Toggle toggle;

    [Header("������ �̹��� �ֱ�")]
    public GameObject Image;
    // Start is called before the first frame update
    void Start()
    {
        toggle = this.gameObject.GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if(toggle.isOn) //������ ��
        {
            Image.SetActive(false);
        }
        else
        {
            Image.SetActive(true);
        }
    }
}
