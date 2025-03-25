using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enumSkillType
{ 
    Active,
    Casting,
    Create,
    Charging,
    Stock,
}

[System.Serializable]
public class SkillScript : MonoBehaviour
{
    [SerializeField] protected PlayerScript.PlayerActionBind CurrentBind;
    [SerializeField] protected enumSkillType skillType;

    public PlayerScript.PlayerActionBind curBinding { get { return CurrentBind; }  }
    // Not Implemented.

    public virtual void Equip(PlayerScript.PlayerActionBind _bind)
    {
        Debug.LogError("SkillScript :: Check why inheritance Function Called.");
    }

    public virtual void UnEquip()
    {
        Debug.LogError("SkillScript :: Check why inheritance Function Called.");
    }
}
