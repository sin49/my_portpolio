using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillClass
{
    public string skill_name;       //�̸�
    public int foreignkey;          //���� Ű
    public Sprite skill_sprite;     //�̹���
    public float skill_cooltime;    //��Ÿ��
    public bool skill_check=false;        //��ų�� �ִ��� üũ

    public bool skill_available;    //��ų �۵��� �����Ѱ�?
    public GameObject skill_effect;//�ӽ�? � �������?

    List<Dictionary<string, object>> Data = CSVReader.Read("SkillTree");

    public SkillClass(string c, float t)
    {
        skill_name = c;
        skill_cooltime = t;
        skill_available = true;
    }

    public void skill_active()
    {
        if (skill_available == true)    //�۵�
        {
            Debug.Log("�۵��մϴ� ��ų�۵�");
            //skill_available =false;
            skill_available = false;
        }
    }

    public void GetSkillItem(Item item)
    {
        //foreignkey = item.Foreignkey;
        ChangeSkill();
    }
    public void ChangeSkill()
    {
        skill_name = Data[foreignkey]["Name"].ToString();
        skill_cooltime = float.Parse(Data[foreignkey]["CoolTime"].ToString());
        skill_sprite = Resources.Load(Data[foreignkey]["Image"].ToString(), typeof(Sprite)) as Sprite;
    }

    public IEnumerator CoolTIme()
    {
        var wait = new WaitForSeconds(1f);

        for (float i = skill_cooltime; i >= 0; i--)
        {
            yield return wait;
        }
        Debug.Log(skill_name + "��Ÿ�ӳ�");
        skill_available = true;
        yield return null; //

    }
}
