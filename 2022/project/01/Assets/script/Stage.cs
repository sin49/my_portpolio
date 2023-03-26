using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Stage : MonoBehaviour, Character_observer
{
    //라운드 구분해서 적 생성
    //1번 캐릭터 생성 포인트를 Stage에서 분리한 뒤 interface로 선언->연결한 interface를 변경하는걸로 라운드 별로 소환 위치 지정
    //2번 기존의 적 생성 리스트 -> 적 종류 리스트로 변경
    //이중 리스트로 라운드 별로 생성할 적 생성
    //적 종류 리스트에서 number를 이용해 적을 가져와서 새로운 리스트를 생성 후 이중 리스트에 추가...
    //적 생성 리스트의 number가져오는 법 생각하기 (정보를 가진 데이터베이스 연결하기)
    //작업하기 전에 생각할 점 이중리스트로 만들어야 할 것 1번 적, 2번 적 생성 위치(필수는 아님),3번 적 풀링 리스트(필수는 아닌데 할 필요는 있음) 4번 적 생성 리스트 number(필수긴 한데 1번과 합치기 가능)
    //이중리스트의 첫번째는 라운드 숫자 두번째는 적의 숫자
    //캐릭터 생성위치 작업은 생성 방법을 카메라 밖에 두는 것으로 해결 가능ㅇ
    //적 생성 number의 리스트의 첫번짼는 라운드 숫자 두번째는 적의 종류로 생성 가능
    //복잡하긴 한데 각은 보임 해보면 어찌저찌 될 듯?
    //생성 방법 :각 라운드 별 생성할 캐릭터의 정보를 받아서 그 정보를 취합해 생성해야할 최소한의 캐릭터를 미리 생성
    //라운드 별 스폰 지역에 미리 생성한 캐릭터를 지정 
    //라운드를 시작할 때 그 캐릭터를 활성화
    //적이 중복되는 경우 풀링한 후 재활용하여 사용

    public List<GameCharacter> Enemies;
    public List<Transform> enemy_spawn_pos=new List<Transform>();
    public List<GameCharacter> Players;
    public List<Transform> player_spawn_pos= new List<Transform>();
    public static Stage _stage = null;

    public List<GameCharacter> Enemy_create;

    public List<GameCharacter> Player_create;

    //icon
    public GameObject icon_plate;
    public GameObject chr_icon_ui;
    icon_UI[] icon_ui_list = new icon_UI[5];

    public Canvas slider_Canvas;
    public Canvas font_Canvas;
    public GameObject CHRslider;
    List<character_slider> CHRslider_create = new List<character_slider>();
    public GameObject Damage_font;
    List<GameObject> Damage_font_pulling=new List<GameObject>();


    List<GameCharacter> characteer_pulling_list = new List<GameCharacter>();





    int round_num;
    List<List<GameCharacter>> Enemies_list = new List<List<GameCharacter>>();

  

    void create_slider(GameCharacter gc)
    {

        
        character_slider h  = Instantiate(CHRslider, slider_Canvas.transform).GetComponent<character_slider>();
        CHRslider_create.Add(h);
        gc.AddObserver(h);
        
    }
    void active_HPbar(GameCharacter gc)
    {
        foreach (character_slider a in CHRslider_create)
        {
            if (a.Character_transform==gc.transform)
            {
                a.update_information(gc.current_hp, gc);
                a.gameObject.SetActive(true);
            }
        }
    }
    private void Awake()
    {
        _stage = this;
        Enemies = Enemies.OrderBy(x => x.Distance_number).ToList();
        Players = Players.OrderBy(x => x.Distance_number).ToList();
        if (icon_plate != null && chr_icon_ui != null)
        {
            for (int i = 0; i < icon_ui_list.Length; i++)
            {
                GameObject a = Instantiate(chr_icon_ui);
                a.transform.SetParent(icon_plate.transform);
            }
        }
    }
 
    void Start()
    {
       
        create_player();
        create_enemy();
    }
    GameCharacter create_character__(List<GameCharacter> a, List<GameCharacter> b, List<Transform> spawwn,int n)
    {
                var character = character_pulling(b[n], spawwn[n]);
                character.index = n;
                a.Add(character);
        return character;
        
    }
    void create_player()
    {
        for (int n = Player_create.Count; n < Players.Count; n++)
        {
            GameCharacter chr= create_character__(Player_create, Players, player_spawn_pos, n);
            chr.T = Team.Player;
            icon_ui_list[4 - n].enable_icon_ui(chr);
        }
     }
    void create_enemy( )
    {
        for (int n = Enemy_create.Count; n < Enemies.Count; n++)
        {
            create_character__(Enemy_create, Enemies, enemy_spawn_pos, n).T = Team.Enemy;
        }
    }
    GameCharacter character_pulling(GameCharacter a,Transform spawwn)
    {
        
        foreach (GameCharacter chr in characteer_pulling_list)
        {
            if (!chr.gameObject.activeSelf&&chr.ID==a.ID)
            {
                chr.gameObject.SetActive(true);
                chr.gameObject.transform.position = spawwn.position;
                chr.gameObject.transform.rotation = spawwn.rotation;
                chr.transform.SetParent(spawwn);
                active_HPbar(chr);
                return chr;
            }
        }
        GameCharacter character = Instantiate(a, spawwn.position, spawwn.rotation).GetComponent<GameCharacter>();
        character.gameObject.transform.SetParent(spawwn.transform);
        characteer_pulling_list.Add(character);
        character.AddObserver(this);
        create_slider(character);
        return character;

    }
    public Damage_font pulling_damage_font()
    {
        GameObject obj=null;
       for(int i = 0; i < Damage_font_pulling.Count; i++)
        {
            if (!Damage_font_pulling[i].activeSelf)
            {
                obj = Damage_font_pulling[i];
                obj.SetActive(true);
                break;
            }
        }
        if (obj == null) {
            obj = Instantiate(Damage_font, font_Canvas.transform);
            Damage_font_pulling.Add(obj); 
        }
        return obj.GetComponent<Damage_font>();
    }
    
    void Update()
    {
        if (Enemies.Count == 0)
        {
            win();
        }   else if (Players.Count == 0)
        {
            lose();
        }
    }
    void lose()
    {
        Debug.Log("졌어요..");
    }
    void win()
    {
        Debug.Log("이겼어요!");
    }

    public void update_information(int a, GameCharacter character)
    {
        
        if (a > 0)
            return;
        GameCharacter c;
        switch (character.T)
        {
            case Team.Player:
                Player_create.Remove(character);
                c = Players[character.index];
                Players.Remove(c);
               
                break;
            case Team.Enemy:
                Enemy_create.Remove(character);
               c = Players[character.index];
                Enemies.Remove(c);

                break;
        }
        
    }
    
}
