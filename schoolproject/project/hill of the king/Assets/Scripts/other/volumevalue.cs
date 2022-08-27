using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class volumevalue : MonoBehaviour//볼륨 조절
{
    public Slider volume;
    public float vol;
    // Start is called before the first frame update
    void Start()
    {
        vol = PlayerPrefs.GetFloat("prefsvol",1);
        volume.value = vol;
        AudioListener.volume = vol;
        
    }
    public void volume_value() {//볼륨을 조절함
        vol = volume.value;
        PlayerPrefs.SetFloat("prefsvol", vol);
        AudioListener.volume = vol;
    }
        
    // Update is called once per frame
    void Update()
    {
        
    }
}
