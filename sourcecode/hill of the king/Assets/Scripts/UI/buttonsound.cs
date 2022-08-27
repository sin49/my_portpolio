using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonsound : MonoBehaviour//버튼 소리
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void clicksound()
    {
        AudioSource click_sound = GetComponent<AudioSource>();
        click_sound.Play();
    }
}
