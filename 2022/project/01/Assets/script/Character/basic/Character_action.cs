using System;
using System.Collections.Generic;
using UnityEngine;

public struct Character_action
{

    public Character_action(string name,iAct act, Team t, Func<Team,Vector3, List<int>, List<GameCharacter>> fu, List<int> i)
    {
        action_name=name;
        execute = new Action<int, List<GameCharacter>>(act.Active);
        init_time = act.init_delay();
        target_team = t;
        priority = fu;
        priority_number = i;
        execution = false;
    }
    public string action_name;
    Team target_team;
    public bool execution;
    public void Invoke(int DMG,GameCharacter c) { List<GameCharacter> targets = priority.Invoke(target_team,c.transform.position, priority_number); execute.Invoke(DMG, targets);execution = true; }//����
    Action<int, List<GameCharacter>> execute;//�ൿ
    Func<Team, Vector3, List<int>, List<GameCharacter>> priority;//�켱����
    List<int> priority_number;//�켱���� ���
    public float init_time;//���� �� ��� �ð�
}