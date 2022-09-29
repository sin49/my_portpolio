using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class range_attack_normal : attack_basic
{

    GameObject bullet;
    GameObject bullet_create;
    Transform bullet_create_transform;
    range_attack_normal(GameObject bullet,Transform bullet_create_transform){
        this.bullet = bullet;
        this.bullet_create_transform = bullet_create_transform;
        }

    protected override void attack_by_type( int damage,GameCharacter obj )
    {
        if(obj!=null)
        create_bullet(bullet);

      
    }

  

    void create_bullet(GameObject bullet)
    {
        if (bullet_create == null)
        {
            bullet_create = Instantiate(bullet, this.transform);
            create_bullet(bullet_create);
        }
        else
        {
            create_bullet(bullet_create);
        }
        bullet.transform.position = bullet_create_transform.position;
        bullet.SetActive(true);
    }
}
