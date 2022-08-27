using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class air_melee : MonoBehaviour//플레이어의 공중공격 판정 클레스
{
    public Vector3 air_melee_pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = Gamemanager.GM.Player_obj.transform.position + air_melee_pos;

        //공중공격이 플레이어가 지상에 닿으면 판정이 사라짐
        if (Gamemanager.GM.Player_obj.GetComponent<PlayerCharacter>().onground)
        {
            this.gameObject.SetActive(false);
        }
    }
}
