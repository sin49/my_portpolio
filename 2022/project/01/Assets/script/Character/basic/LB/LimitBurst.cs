using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBurst : attack_basic
{
    public override void Active(int DMG, List<GameCharacter> l = null)
    {
        on_LB_situaltion();
        base.Active(DMG, l);
        off_LB_situaltion();
    }
   
    //�ٽ�! LB�ߵ� �� LB�� ���� ���� ���ܿ��� ���� ��Ȱ��ȭ
    //LB�ߵ��� ĳ���� �̿��� ĳ������ ���ϸ��̼� corutine���¸� �����ϰ� idle&�ǰݸ� �ߵ���Ű�� �����
    public virtual void on_LB_situaltion()
    {

      
    }

    public virtual void off_LB_situaltion()
    {
        
    }
}
