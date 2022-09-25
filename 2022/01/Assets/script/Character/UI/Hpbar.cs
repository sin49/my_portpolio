using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
   
    public Character_status c_status;
    Slider HP;

    private void Awake()
    {
        HP = this.GetComponent<Slider>();
        
    }
   

    // Update is called once per frame
    void Update()
    {
        if (c_status!=null)
        {
        
            this.transform.position = Camera.main.WorldToScreenPoint(c_status.transform.position + new Vector3(0,1,0.5f));

            HP.value = ( (float)c_status.current_HP / (float)c_status.HP);
           /* if (HP.value==0)
            {
                this.gameObject.SetActive(false);
            }*/
        }
      
    }
}
