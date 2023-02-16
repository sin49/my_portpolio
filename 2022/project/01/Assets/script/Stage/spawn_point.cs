using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_point : MonoBehaviour, spawn
{
    List<GameCharacter> chr_list;
    float spawn_distance_meter_x=30;
    float spawn_distance_meter_y=20;
    public void set_spawn_chr(List<GameCharacter> c)
    {
        chr_list = c;
    }

    public void spawn()
    {
        if(chr_list.Count>0)
        {
           foreach(GameCharacter c in chr_list)
            {
                if (c.gameObject.activeSelf)
                    return;
                switch (c.status.pos)
                {
                    case Position.front:
                        c.transform.position = this.transform.position + Vector3.forward * spawn_distance_meter_y;
                        break;
                    case Position.mid:
                        c.transform.position = this.transform.position;
                        break;
                    case Position.back:
                        c.transform.position = this.transform.position- Vector3.forward * spawn_distance_meter_y;
                        break;
                }
                int index = chr_list.IndexOf(c);
                if (index != 0)
                    c.transform.position = c.transform.position + Vector3.right * ((spawn_distance_meter_x * ((index + 1) / 2)) * (-1 * index % 2));
                c.transform.rotation = this.transform.rotation;
                c.gameObject.SetActive(true);
            }
        }
        else
            return;
    }
}
