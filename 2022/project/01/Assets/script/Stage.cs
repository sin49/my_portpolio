using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Stage : MonoBehaviour, Character_observer
{
    //���� �����ؼ� �� ����
    //1�� ĳ���� ���� ����Ʈ�� Stage���� �и��� �� interface�� ����->������ interface�� �����ϴ°ɷ� ���� ���� ��ȯ ��ġ ����
    //2�� ������ �� ���� ����Ʈ -> �� ���� ����Ʈ�� ����
    //���� ����Ʈ�� ���� ���� ������ �� ����
    //�� ���� ����Ʈ���� number�� �̿��� ���� �����ͼ� ���ο� ����Ʈ�� ���� �� ���� ����Ʈ�� �߰�...
    //�� ���� ����Ʈ�� number�������� �� �����ϱ� (������ ���� �����ͺ��̽� �����ϱ�)
    //�۾��ϱ� ���� ������ �� ���߸���Ʈ�� ������ �� �� 1�� ��, 2�� �� ���� ��ġ(�ʼ��� �ƴ�),3�� �� Ǯ�� ����Ʈ(�ʼ��� �ƴѵ� �� �ʿ�� ����) 4�� �� ���� ����Ʈ number(�ʼ��� �ѵ� 1���� ��ġ�� ����)
    //���߸���Ʈ�� ù��°�� ���� ���� �ι�°�� ���� ����
    //ĳ���� ������ġ �۾��� ���� ����� ī�޶� �ۿ� �δ� ������ �ذ� ���ɤ�
    //�� ���� number�� ����Ʈ�� ù��²�� ���� ���� �ι�°�� ���� ������ ���� ����
    //�����ϱ� �ѵ� ���� ���� �غ��� �������� �� ��?
    //���� ��� :�� ���� �� ������ ĳ������ ������ �޾Ƽ� �� ������ ������ �����ؾ��� �ּ����� ĳ���͸� �̸� ����
    //���� �� ���� ������ �̸� ������ ĳ���͸� ���� 
    //���带 ������ �� �� ĳ���͸� Ȱ��ȭ
    //���� �ߺ��Ǵ� ��� Ǯ���� �� ��Ȱ���Ͽ� ���

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
        Debug.Log("�����..");
    }
    void win()
    {
        Debug.Log("�̰���!");
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
