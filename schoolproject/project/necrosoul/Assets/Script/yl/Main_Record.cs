using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Record : MonoBehaviour
{
    public static Main_Record M_Reord= new Main_Record();

    public KeepActionRecord Keep=new KeepActionRecord();

    // Start is called before the first frame update
    void Awake()
    {
        M_Reord = this;
        Keep.KeepLoad();
        Debug.Log("���� ��ġ" + Keep.T_EnemyKill);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Check_Ach(string name,string req)
    {
        Keep.KeepLoad();
        if (name=="��")
        {
            Debug.Log("�䱸����" + int.Parse(req) + "����" + Keep.T_EnemyKill);
            if(Keep.T_EnemyKill>=int.Parse(req))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        else
        {
            return false;
        }
    }

    
}
