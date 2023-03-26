using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class XMLreader {
    public static XMLreader instance;
    string this_Directory;
    XMLreader() {
        if (instance == null)
            instance = this;
        this_Directory = Directory.GetCurrentDirectory();
       
    }
    void Read(string xml_name)
    {
        GameCharacter chr = new GameCharacter();
        XmlDocument xml = new XmlDocument();
        xml.Load(this_Directory + "\\" + xml_name + ".xml");

        
        
    }
    void Read_character(XmlDocument xml,GameCharacter chr)
    {
        XmlNodeList node_list = xml.SelectNodes("/characterinfo/character");
        foreach(XmlNode node in node_list)
        {
            chr.name = node["name"].InnerText;
            chr.ID =XmlConvert.ToInt32(node["ID"].InnerText);
        }
    }
    void Read_status(XmlDocument xml)
    {
        
    }
    //public Character_action Read_skill_setting(string xml_name,iAct act)
    //{
    //    string s = act.ToString().Substring(act.ToString().Length - 6, 6);
    //    XmlDocument xml = new XmlDocument();
    //    xml.Load(this_Directory + "\\" + xml_name + ".xml");
    //    XmlNodeList node_list = xml.SelectNodes("/characterinfo/skill");
    //    XmlNodeList skill_cycle = node_list[1].ChildNodes;
    //    foreach (XmlNode skill in skill_cycle)
    //    {
    //        int number = XmlConvert.ToInt32(skill.InnerText);
    //        XmlNode _item = node_list[number + 2];
    //        obj.
    //    }
    //    return 
    //}
    public Character_action Read_skill_setting(string xml_name, iAct act,Team t)
    {
        string skill_string = act.ToString();
        int skill_string_index = skill_string.IndexOf("_")+1;
        string s = skill_string.Substring(skill_string_index,skill_string.Length-skill_string_index);
        XmlDocument xml = new XmlDocument();
        xml.Load(this_Directory + "\\" + xml_name + ".xml");
        XmlNodeList node_list = xml.SelectNodes("/characterinfo/skill/"+s);
        Team Target_team=Team.Enemy;
        Func<Team,Vector3, List<int>, List<GameCharacter>> priority_func = null;
        List<int> target_number = new List<int>();
        foreach (XmlNode setting in node_list)
        {
            switch (setting["targeting"].InnerText.ToLower())
            {
                case "enemy":
                    if (t == Team.Enemy)
                        Target_team = Team.Player;
                    else
                        Target_team = Team.Enemy;
                    break;
                case "player":
                    if (t == Team.Enemy)
                        Target_team = Team.Enemy;
                    else
                        Target_team = Team.Player;
                    break;
                default:
                    break;
            }
           
            switch (setting["priority"].InnerText.ToLower())
            {
                case "distance":
                    priority_func = new Func<Team,Vector3, List<int>, List<GameCharacter>>
                        (Character_Priority.instance.get_enemy_by_distance);
                    break;
                default:
                    break;
            }
           
            XmlNodeList target_list = setting["TargetNumber"].ChildNodes;
            foreach(XmlNode target in target_list)
            {
                target_number.Add(XmlConvert.ToInt32(target.InnerText));
            }
           

        }
        return new Character_action(s,act, Target_team, priority_func, target_number);
    }
    public void Read_skill(XmlDocument xml)
    {
        XmlNodeList node_list = xml.SelectNodes("/characterinfo/skill");
        XmlNodeList skill_cycle = node_list[1].ChildNodes;
        foreach(XmlNode skill in skill_cycle)
        {
            int number = XmlConvert.ToInt32(skill.InnerText);
            
        
        }
    }
    void Read_LimitBurst(XmlDocument xml)
    {
       
    }
}
