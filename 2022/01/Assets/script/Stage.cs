using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public List<GameCharacter> Enemies;
    public Transform enemy_spawn_pos;
    public List<GameCharacter> Players;
    public Transform player_spawn_pos;
    public static Stage _stage = null;

    public List<GameCharacter> Enemy_create;

    public List<GameCharacter> Player_create;


    public Canvas HP_Canvas;
    public Canvas font_Canvas;
    public GameObject HPbar;
    public GameObject Damage_font;
    List<GameObject> Damage_font_pulling=new List<GameObject>();
   


    public void create_HPbar(Character_status gc)
    {
        GameObject a= Instantiate(HPbar,HP_Canvas.transform);
        a.GetComponent<Hpbar>().c_status = gc;
    }
    private void Awake()
    {
        _stage = this;
    }
    void Start()
    {
        
    }
    void create_character(List<GameCharacter> a, List<GameCharacter>b,Transform spawwn,Team t)
    {
        if (a.Count != b.Count)
        {
            for (int n = a.Count; n < b.Count; n++)
            {
                GameCharacter character = Instantiate(b[n],spawwn.position,spawwn.rotation).GetComponent<GameCharacter>();
                character.T= t;
                create_HPbar(character.status);
                a.Add(character);
            }
        }
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

        create_character(Enemy_create,Enemies,enemy_spawn_pos,Team.Enemy);
        create_character(Player_create, Players, player_spawn_pos,Team.Player);
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
}
