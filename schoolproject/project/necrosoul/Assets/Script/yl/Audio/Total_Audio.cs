using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Total_Audio : MonoBehaviour
{
    public AudioMixer MasterMixer;
    public Slider AudioSlider;
    // Start is called before the first frame update

    public void AudioContorl_Total()    //������ ���� ����
    {
        float sound = AudioSlider.value;
        if (sound == -40f) MasterMixer.SetFloat("Master", -80);
        else MasterMixer.SetFloat("Master", sound);
    }

    public void AudioContorl_Bgm()  //bgm ���� ����
    {
        float sound = AudioSlider.value;
        if (sound == -40f) MasterMixer.SetFloat("BGM", -80);
        else MasterMixer.SetFloat("BGM", sound);
    }

    public void AudioContorl_SFX()      //ȿ���� ���� ����
    {
        float sound = AudioSlider.value;
        if (sound == -40f) MasterMixer.SetFloat("SFX", -80);
        else MasterMixer.SetFloat("SFX", sound);
    }

    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
}
