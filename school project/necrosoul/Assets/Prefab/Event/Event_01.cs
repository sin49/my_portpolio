using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Event_01 : MonoBehaviour
{
    int heal_rarity;
    public Text t;
    string s;
    float h;
        //50 30 19 1
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 100);
        if (rand < 50)
        {
            heal_rarity = 0;
            h = 0.1f;
            s = "����";
        }else if (rand < 80)
        {
            heal_rarity = 1;
            h = 0.3f;
            s = "����";
        }
        else if (rand < 99)
        {
            heal_rarity = 2;
            h = 0.5f;
            s = "�Ǹ���";
        }
        else
        {
            heal_rarity = 3;
            h = 1;
            s = "�Ϻ���";
        }
        t.text = "Ž�� �߿� " + s + " ������ �߰��ߴ�.";
    }

    public void heal_player()
    {
        Player_status.p_status.set_hp(Mathf.RoundToInt(Player_status.p_status.get_max_hp() * h));
    }
}
