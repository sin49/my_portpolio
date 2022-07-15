using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerUI : MonoBehaviour//플레이어 인게임 ui
{
    // Start is called before the first frame update
    public Text playerhealthText;//체력바
    public Text playermagazineText;
    public Image playergreenhealthbar;
    public Text leveltext;//레벨관련 텍스트
    public float x;
    
    public GameObject crosshair;//조준점

    public GameObject heart;//능력
    public GameObject blade;
    public GameObject wing;
    public GameObject storm;
    public GameObject none;
    string exp_;
    public playercontroler _target;
    void Start()
    {
        _target = transform.parent.GetComponent<playercontroler>();
        if (playergreenhealthbar != null)
            x = playergreenhealthbar.rectTransform.sizeDelta.x;
    }
    // Update is called once per frame
    void Update()
    {
        //레벨업에 필요한 요구 경험치 갱신
        if (_target.lv <= 5)
            exp_ = "/100";
        else if (_target.lv <= 10)
            exp_ = "/150";
        else if (_target.lv <= 15)
            exp_ = "/200";
        else if (_target.lv <= 19)
            exp_ = "/250";
        else if (_target.lv == 20)
            exp_ = "/MAX";
        float width = Screen.width / 2;
        float height = Screen.height / 2;
        //crosshair.transform.position = new Vector2(width, height);
        //플레이어 ui정보 갱신
        if (playerhealthText!=null)
            playerhealthText.text=_target.health.ToString();
        if(playergreenhealthbar!=null)
            playergreenhealthbar.rectTransform.sizeDelta=new Vector2((float)_target.health/(float)_target.max_health*x,playergreenhealthbar.rectTransform.sizeDelta.y);
        if (playermagazineText != null)
            playermagazineText.text = _target.magazine.ToString()+"/"+_target.return_max_magazine().ToString();
        leveltext.text = "lv: " + _target.lv + " exp: " + _target.exp+exp_;
        //플레이어가 죽었다면 ui 제거
        if (_target == null)
        {
            Destroy(this.gameObject);
            return;
        }
        //특수 능력의 종류 표시
        switch (_target.s_ability_number)
        {
            case 0:
                none.SetActive(true);
                break;
            case 1:
                heart.SetActive(true);
                break;
            case 2:
                blade.SetActive(true);
                break;
            case 3:
                wing.SetActive(true);
                break;
            case 4:
                storm.SetActive(true);
                break;
        }
    }
}
