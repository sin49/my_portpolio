using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade_out : MonoBehaviour
{
    public float fade;
    public bool fade_in;
    public int a;
    // Start is called before the first frame update
    void Start()
    {
        if (a == 1)
        {
            StartCoroutine("fadein1");
        }
        else if (a == 2)
        {
            StartCoroutine("fadein2");
        }else if (a == 3)
        {
            StartCoroutine("fadein3");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator fadeout1()
    {
        
        
            for (float i = 1f; i >= 0; i -= fade)
            {
                Color color = new Vector4(1, 0, 0, i);
                transform.GetComponent<Text>().color = color;
                if (i <= 0.2f)
                {
                Destroy(this.gameObject);
                
                }
                yield return 0;

            }
    }
    IEnumerator fadein1()
    {
        for (float i = 0f; i <= 1; i += fade)
        {
            Color color = new Vector4(1, 0, 0, i);
            transform.GetComponent<Text>().color = color;
            if (i >= 0.9f)
            {
                StartCoroutine("fadeout1");
                StopCoroutine("fadein1");
            }
            yield return 0;

        }
    }
    IEnumerator fadeout2()
    {


        for (float i = 1f; i >= 0; i -= fade)
        {
            Color color = new Vector4(1, 1, 0, i);
            transform.GetComponent<Text>().color = color;
            if (i <= 0.2f)
            {
                Destroy(this.gameObject);
            }
            yield return 0;

        }
    }
    IEnumerator fadein2()
    {
        for (float i = 0f; i <= 1; i += fade)
        {
            Color color = new Vector4(1, 1, 0, i);
            transform.GetComponent<Text>().color = color;
            if (i >= 0.9f)
            {
                StartCoroutine("fadeout2");
                StopCoroutine("fadein2");
            }
            yield return 0;

        }
    }
    IEnumerator fadeout3()
    {


        for (float i = 1f; i >= 0; i -= fade)
        {
            Color color = new Vector4(1, 1, 1, i);
            transform.GetComponent<Text>().color = color;
            if (i <= 0.1f)
            {
                Destroy(this.gameObject);

            }
            yield return 0;

        }
    }
    IEnumerator fadein3()
    {
        for (float i = 0f; i <= 1; i += fade)
        {
            Color color = new Vector4(1, 1, 1, i);
            transform.GetComponent<Text>().color = color;
            if (i >= 0.8f)
            {
                StartCoroutine("fadeout3");
                StopCoroutine("fadein3");
            }
            yield return 0;

        }
    }
}
