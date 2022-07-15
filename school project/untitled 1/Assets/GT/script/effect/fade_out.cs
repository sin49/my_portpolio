using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade_out : MonoBehaviour//글자를 페이드 인,아웃하는 클레스
{
    public float fade;
    public bool fade_in;
    public int a;
    // Start is called before the first frame update
    void Start()
    {
        //a의 값에 따라 지젇된 페이드 인 코루틴 실행
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
    //1=빨간 글자 2=노란 글자 3=하얀 글자
    //fadein 코루틴을 실행시켜 글자를 나타나게 함
    //fadein 코루틴으로 글자가 선명해지면 자동으로 fadein 코루틴을 멈추고 fadeout 코루틴을 실행
    // fadeout 글자가 투명해지면 text를 파괴
    IEnumerator fadeout1()
    {
        
        
            for (float i = 1f; i >= 0; i -= fade)
            {
            //글자를 점점 투명하게 한다
                Color color = new Vector4(1, 0, 0, i);
                transform.GetComponent<Text>().color = color;
            //글자가 투명해지면 이 오브젝트(text)를 파괴
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
            //글자를 점점 선명하게 함
            Color color = new Vector4(1, 0, 0, i);
            transform.GetComponent<Text>().color = color;
            //글자가 완전히 선명해지면 fadein을 멈추고 fadeout를 실행
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
