using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Stage : MonoBehaviour, Character_observer
{
    public List<GameCharacter> Enemies;
    public List<Transform> enemy_spawn_pos=new List<Transform>();
    public List<GameCharacter> Players;
    public List<Transform> player_spawn_pos= new List<Transform>();
    public static Stage _stage = null;

    public List<GameCharacter> Enemy_create;

    public List<GameCharacter> Player_create;

    public List<GameCharacter> for_test_sort_list=new List<GameCharacter>();

    

    public Canvas HP_Canvas;
    public Canvas font_Canvas;
    public GameObject HPbar;
    List<Hpbar> HPbar_create = new List<Hpbar>();
    public GameObject Damage_font;
    List<GameObject> Damage_font_pulling=new List<GameObject>();


    List<GameCharacter> characteer_pulling_list = new List<GameCharacter>();

    public bool infinite_create_chacter;

    void create_HPbar(GameCharacter gc)
    {

        
        Hpbar h  = Instantiate(HPbar, HP_Canvas.transform).GetComponent<Hpbar>();
        HPbar_create.Add(h);
        gc.AddObserver(h);
        
    }
    void active_HPbar(GameCharacter gc)
    {
        foreach (Hpbar a in HPbar_create)
        {
            if (a.Character==gc)
            {
                a.gameObject.SetActive(true);
                a.update_information(gc.current_hp, gc);
             
            }
        }
    }
    private void Awake()
    {
        _stage = this;
        Enemies = Enemies.OrderBy(x => x.status.Distance_number).ToList();
        Players = Players.OrderBy(x => x.status.Distance_number).ToList();
      
    }
 
    void Start()
    {
        create_character(Enemy_create, Enemies, enemy_spawn_pos, Team.Enemy);
        create_character(Player_create, Players, player_spawn_pos, Team.Player);
    }
    void create_character(List<GameCharacter> a, List<GameCharacter>b, List<Transform> spawwn,Team t)
    {
        if (a.Count != b.Count)
        {
            for (int n = a.Count; n < b.Count; n++)
            {
                var character= character_pulling(b[n],spawwn[n]);
                character.index = n;
                character.T= t;
               
               
                a.Add(character);
            }
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
                active_HPbar(chr);
                return chr;
            }
        }
        GameCharacter character = Instantiate(a, spawwn.position, spawwn.rotation).GetComponent<GameCharacter>();
        character.gameObject.transform.SetParent(spawwn.transform);
        characteer_pulling_list.Add(character);
        character.AddObserver(this);
        create_HPbar(character);
        return character;

    }
    public void creaate_damagee_font(int a,Transform t)
    {
        GameObject obj=null;
       for(int i = 0; i < Damage_font_pulling.Count; i++)
        {
            if (!Damage_font_pulling[i].activeSelf)
            {
                obj = Damage_font_pulling[i];
             
            }
        }
        if (obj == null) {
            obj = Instantiate(Damage_font, font_Canvas.transform);
            Damage_font_pulling.Add(obj);
        }
        obj.SetActive(true);
        obj.GetComponent<Damage_font>().set_font(a, t);
    }
    
    void Update()
    {
        if (infinite_create_chacter)
        {
            create_character(Enemy_create, Enemies, enemy_spawn_pos, Team.Enemy);
            create_character(Player_create, Players, player_spawn_pos, Team.Player);
        }
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
                if(infinite_create_chacter)
                    Players.Add(c);
                break;
            case Team.Enemy:
                Enemy_create.Remove(character);
               c = Players[character.index];
                Enemies.Remove(c);
                if (infinite_create_chacter)
                    Enemies.Add(c);
                break;
        }
        
    }
}
