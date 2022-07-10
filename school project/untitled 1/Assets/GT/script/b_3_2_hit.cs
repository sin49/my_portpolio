using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_3_2_hit : MonoBehaviour
{
    public float color_time;
    public GameObject player_;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        player_ = GameObject.FindWithTag("Player");
        if(GetComponentInParent<boss_basic>().e_hp == 0)
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator makecolor()
    {
        for (float i = 1f; i >= 0; i -= color_time)
        {
            Color color = new Vector4(1, i, i, 1);
            transform.GetComponent<SpriteRenderer>().color = color;
            
            yield return 0;

        }
    }
    public void color_change2()
    {
            Color color = new Vector4(1, 1, 1, 1);
            transform.GetComponent<SpriteRenderer>().color = color;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bullet"))
        {
            if (!player_.GetComponent<shooting_player>().special_power)
            {
                player_.GetComponent<shooting_player>().power_gauge++;
            }
            col.GetComponent<Bullet>().hit_animation();
            col.GetComponent<Bullet>().speed = 0;
            GetComponentInParent<boss_basic>().e_hp--;
        }
    }
}
