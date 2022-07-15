using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerinputfield : MonoBehaviour//플레이어의 닉네임을 정한다
{
    static string playerNamePrefkey = "playername";
    public InputField _inputfield;
    public string name_;
    // Start is called before the first frame update
    void Start()
    {
        
        string defaultname = "default";
        InputField _inputfield = this.GetComponent<InputField>();
        if (_inputfield != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefkey))
            {
                Debug.Log("check");
                defaultname = PlayerPrefs.GetString(playerNamePrefkey);
                _inputfield.text = defaultname;//기억한 이름을 불려온다
            }
        }
        PhotonNetwork.player.NickName = defaultname;
        name_ = defaultname;
    }
    public void SetPlayerName()
    {
        PhotonNetwork.player.NickName = _inputfield.text;//이름을 바꾼다
        name_ = _inputfield.text;
        PlayerPrefs.SetString(playerNamePrefkey, _inputfield.text);//새로운 이름을 기억한다
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
