using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_lazer_orbit : MonoBehaviour
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
        transform.localScale = new Vector3(15, scale, scale);
        if (scale <= 4.8f)
        {
            scale += 0.15f;
        }
    }
    IEnumerator makecolor()
    {
        for (float i = 0f; i <= 1; i += color_time)
        {
            Color color = new Vector4(1, 1, 1, i);
            transform.GetComponent<SpriteRenderer>().color = color;
            color_check = i;
            if (color_check >= 0.9)
            {
                lazer.gameObject.SetActive(true);
                Destroy(this.gameObject);
            }
            yield return 0;
            
        }
    }
}
