using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_information : MonoBehaviour
{
   
    public int stage_num;
    public List< List<character_information>> chr_information_list = new List<List<character_information>>();
    public round_info[] round_info_array;
    void make_stage_information(GameCharacter chr,int number=1,int stage=1)
    {
       // character_information c = new character_information(chr, number, stage);
        //chr_information_list.Add(c);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public struct character_information
{
    public character_information(GameCharacter c, int N, int S)
    {
        chr = c;
        ID = c.ID;
        number = N;
        stage = S;
    }
    public GameCharacter chr;
    public int ID;

    public int number;
    public int stage;
}
[System.Serializable]
public struct round_info
{
    public int index;
    public List<character_information> round_chr;
}