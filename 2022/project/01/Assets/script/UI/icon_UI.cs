using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class icon_UI : character_status_ui
{
    public Image chr_sprite_image;
    Button btn;

    public GameObject sprite_mask;
    private void Awake()
    {
        HP_slider.gameObject.SetActive(false);
        btn.gameObject.SetActive(false);
    }
    public override void update_information(int a, GameCharacter character)
    {
        if (a < 0)
        {
            sprite_mask.SetActive(true);
           
            return;
        }
        if (chr_sprite_image.sprite == null)
            chr_sprite_image.sprite = character.chr_sprite;
        base.update_information(a, character);
        if (LB_slider.check_vaule()==0)
            btn.interactable = true;


    }
    public void enable_icon_ui(GameCharacter chr)
    {
        set_sprite_button(chr);
        active_icon_UI();
    }
    void set_sprite_button(GameCharacter chr)
    {
        btn.onClick.AddListener(() => btn.interactable = false);
        btn.onClick.AddListener(chr.use_LB);
        chr_sprite_image.sprite = chr.chr_sprite;
       

        //LB함수 리스너로 추가
        //스프라이트 이미지 변경
    }
    void active_icon_UI()
    {
        HP_slider.gameObject.SetActive(true);
        btn.gameObject.SetActive(true);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
