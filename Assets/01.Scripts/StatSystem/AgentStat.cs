using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SO/Stat")]
public class AgentStat : ScriptableObject
{
    public Stat strength;
    public Stat agility;
    public Stat damage;
    public Stat maxHealth;
    public Stat criticalChance;
    public Stat criticalDamage;
    public Stat armor;
    public Stat evasion;

    protected Agent _owner;
    protected Dictionary<StatType, Stat> _statDictionary;

    public virtual void SetOwner(Agent owner)
    {
        _owner = owner;
    }

    public virtual void IncreaseStatFor(int value, float duration, Stat targetStat)
    {
        _owner.StartCoroutine(StatModifyCoroutine(value, duration, targetStat));
    }

    protected IEnumerator StatModifyCoroutine(int value, float duration, Stat targetStat)
    {
        targetStat.AddModifier(value);
        yield return new WaitForSeconds(duration);
        targetStat.RemoveModifier(value);
    }

    protected virtual void OnEnable()
    {
        _statDictionary = new Dictionary<StatType, Stat>();

        Type agentStatType = typeof(AgentStat); //�� Ŭ������ Ÿ�������� �ҷ��ͼ�

        foreach(StatType typeEnum in Enum.GetValues(typeof(StatType)))
        {
            try
            {
                string fieldName = LowerFirstChar(typeEnum.ToString());
                FieldInfo statField = agentStatType.GetField(fieldName);
                _statDictionary.Add(typeEnum, statField.GetValue(this) as Stat);
            }catch(Exception ex)
            {
                Debug.LogError($"There are no stat - {typeEnum.ToString()} {ex.Message}");
            }
        }
    }

    private string LowerFirstChar(string input) 
        => $"{char.ToLower(input[0])}{input.Substring(1)}";

    
    public int GetDamage()
    {
        return damage.GetValue() + strength.GetValue() * 2;
    }

    public bool CanEvasion()
    {
        //��ø 1�� ȸ�� 1�ۼ�Ʈ ����.
        return IsHitPercent(evasion.GetValue() + agility.GetValue() * 10);
    }

    public int ArmoredDamage(int incomingDamage)
    {
        //���� 1�� ������ 0.5 ����
        return Mathf.Max(1, incomingDamage - Mathf.FloorToInt(armor.GetValue() * 0.5f));
    }

    public bool IsCritical(ref int incomingDamage)
    {
        if(IsHitPercent(criticalChance.GetValue()) )
        {
            incomingDamage =  Mathf.FloorToInt(
                    incomingDamage * criticalDamage.GetValue() * 0.0001f);
            return true;
        }
        return false;
    }

    protected bool IsHitPercent(int statValue) => Random.Range(1, 10000) < statValue;

    public void AddModifier(StatType type, int value)
    {
        _statDictionary[type].AddModifier(value);
    }

    public void RemoveModifier(StatType type, int value)
    {
        _statDictionary[type].RemoveModifier(value);
    }
}
