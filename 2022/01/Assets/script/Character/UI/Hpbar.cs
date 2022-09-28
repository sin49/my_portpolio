using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour,Character_observer
{
   
    public GameCharacter Character;
    Slider HP;
    int max_HP;
    int current_hp;
 
    public void update_information(int a, GameCharacter character)
    {
     
        if (Character == null)
        {
            Character = character;
            max_HP = character.status.HP;
        }
        current_hp = a;
        if (a <= 0)
        {

            this.gameObject.SetActive(false);
            return;
        }
    }
  
    private void Awake()
    {
        HP = this.GetComponent<Slider>();
        
    }
   

    // Update is called once per frame
    void Update()
    {
       if(Character!=null)
            this.transform.position = Camera.main.WorldToScreenPoint(Character.transform.position + new Vector3(0,1,0.5f));

            HP.value = ( (float)current_hp / (float)max_HP);

       
    }
   
}
