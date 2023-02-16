using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Spawn : MonoBehaviour
{
    /**public spawn_point player_spawn;
    public spawn_point enemy_spawn;
    List<GameCharacter> Players;
    
    List<Dictionary<string, GameCharacter>> enemy = new List<Dictionary<string, GameCharacter>>();
    List<GameCharacter> Players_create;
    List<GameCharacter> Enemy_create;
    public void read_spawn_information(spawn_information s)
    {
        var a= s.chr_information_list[0].chr;
    }
    void create_chracter(GameCharacter obj)
    {
        Instantiate(obj.gameObject);
        obj.transform.SetParent(player_spawn.transform);

        obj.SetActive(false);
    }*/
    public spawn_point player_spawn;
    public spawn_point enemy_spawn;
    Dictionary<int,List< GameCharacter>> chr_pulling_dictionary = new Dictionary<int, List<GameCharacter>>();
    List<GameCharacter> player_chr;
    List<GameCharacter> Enemy_chr;
    int round_num;
    int current_round_num;
    GameCharacter character_pulling(GameCharacter a)
    {
        if (chr_pulling_dictionary.ContainsKey(a.ID))
        {
            for(int i = 0; i < chr_pulling_dictionary[a.ID].Count; i++)
            {
                if (!chr_pulling_dictionary[a.ID][i].gameObject.activeSelf)
                    return chr_pulling_dictionary[a.ID][i];
            }

        }
        else
        {
            chr_pulling_dictionary.Add(a.ID, new List<GameCharacter>());
        }
        GameCharacter character = Instantiate(a).GetComponent<GameCharacter>();
        chr_pulling_dictionary[a.ID].Add(character);
        return character;
      

    }
    GameCharacter create_character__(GameCharacter c)
    {
        var character = character_pulling(c);
        //a.Add(character);
        return character;

    }
    void set_spawner(spawn_point s,List<GameCharacter> c)
    {
        s.set_spawn_chr(c);
    }
   /* void create_player()
    {
        for (int n = Player_create.Count; n < Players.Count; n++)
        {
            GameCharacter chr = create_character__(Player_create, Players, player_spawn_pos, n);
            chr.T = Team.Player;
            icon_ui_list[4 - n].enable_icon_ui(chr);
        }
    }
    void create_enemy()
    {
        for (int n = Enemy_create.Count; n < Enemies.Count; n++)
        {
            create_character__(Enemy_create, Enemies, enemy_spawn_pos, n).T = Team.Enemy;
        }
    }*/
}
