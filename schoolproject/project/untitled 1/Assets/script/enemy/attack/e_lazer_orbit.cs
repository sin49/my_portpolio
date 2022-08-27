using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_lazer_orbit : MonoBehaviour//레이저 궤도 클레스
{
    public float color_time;
    public float color_check;
    public GameObject lazer;
    public float scale;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("makecolor");
    }

    // Update is called once per frame
    void Update()
    {
        //scale 값을 점점 키워서 레이저 공격 범위를 예고
        transform.localScale = new Vector3(15, scale, scale);
        if (scale <= 4.8f)
        {
            scale += 0.15f;
        }
    }
    IEnumerator makecolor()
    {
        //레이저 궤도가 점점 불투명하게 나타나면셔 레이저 공격이 날라오는 시간을 에고
        for (float i = 0f; i <= 1; i += color_time)
        {
            Color color = new Vector4(1, 1, 1, i);
            transform.GetComponent<SpriteRenderer>().color = color;
            color_check = i;
            //궤도가 선명해졌으면 lazer를 활성화 시켜서 레이저 공격을 활성화한다.
            if (color_check >= 0.9)
            {
                lazer.gameObject.SetActive(true);
                Destroy(this.gameObject);
            }
            yield return 0;
            
        }
    }
}
