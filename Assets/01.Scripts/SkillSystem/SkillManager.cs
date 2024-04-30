using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSkill
{
    None = 0,
    Rolling = 1,
    CircleOrb = 2,
    //Satellite = 3,
    //Thunder = 4,
    //니네가 한개 만들게 된다.
}

public class SkillManager : MonoSingleton<SkillManager>
{
    private Dictionary<Type, Skill> _skills;

    private void Awake()
    {
        _skills = new Dictionary<Type, Skill>();

        foreach(PlayerSkill skillEnum in Enum.GetValues(typeof(PlayerSkill)))
        {
            if (skillEnum == PlayerSkill.None) continue;

            Skill skillCompo = GetComponent($"{skillEnum.ToString()}Skill") as Skill;
            Type type = skillCompo.GetType();
            _skills.Add(type, skillCompo);
        }
    }

    public T GetSkill<T>() where T : Skill
    {
        Type t = typeof(T);
        if (_skills.TryGetValue(t, out Skill target))
        {
            return target as T;
        }

        return null;
    }
    
    public Skill GetSkill(PlayerSkill skillEnum)
    {
        Type t = Type.GetType($"{skillEnum.ToString()}Skill"); // 이건 리플렉션
        if (t == null) return null;
        
        if (_skills.TryGetValue(t, out Skill target))
        {
            return target;
        }

        return null;
    }
}