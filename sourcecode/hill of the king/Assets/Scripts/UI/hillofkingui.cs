using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hillofkingui : MonoBehaviour//거점 관련 ui
{
    public Text red_conquer_text;
    public Text blue_conquer_text;
    public Image red_image;
    public Image blue_image;
    public Text red_text;
    public Text blue_text;
    public hillofking hok;
    public float image_x;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
       //gameManager = FindObjectOfType<GameManager>();
        hok = GameObject.FindObjectOfType<hillofking>();
        red_text.gameObject.SetActive(false);
        blue_text.gameObject.SetActive(false);
        image_x = 50;
    }

    // Update is called once per frame
    void Update()
    {

        red_conquer_text.text = ((int)hok.red_hill_time).ToString();
        blue_conquer_text.text = ((int)hok.blue_hill_time).ToString();
        //점령 상태를 텍스트로 표시
        if (hok.hillstate == 1)
        {
            red_text.gameObject.SetActive(true);
        }else if(hok.hillstate == 2)
        {
            blue_text.gameObject.SetActive(true);
        }else if (hok.hillstate == 0)
        {
            red_text.gameObject.SetActive(false);
            blue_text.gameObject.SetActive(false);
        }
        red_image.rectTransform.sizeDelta = new Vector2(image_x*hok.red_conquer_time/hok.conquer_time, red_image.rectTransform.sizeDelta.y);
        blue_image.rectTransform.sizeDelta = new Vector2(image_x * hok.blue_conquer_time / hok.conquer_time, blue_image.rectTransform.sizeDelta.y);
    }
}
