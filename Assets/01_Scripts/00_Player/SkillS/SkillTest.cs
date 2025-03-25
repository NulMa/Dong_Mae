using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : SkillScript, IBindSkill
{
    public void Started()
    {
        Debug.Log($"{gameObject.name} :: Started Bind: {CurrentBind}");
    }

    public void Performed(float _hold)
    {
    }
    public void Canceled()
    {
    }


    public override void Equip(PlayerScript.PlayerActionBind _bind)
    {
        CurrentBind = _bind;
        PlayerScript.Instance.StartActions[_bind.ToString()] = Started;
        PlayerScript.Instance.PerformActions[_bind.ToString()] = Performed;
        PlayerScript.Instance.CancelActions[_bind.ToString()] = Canceled;
    
        // Player / Action = Skill Action
    }

    public override void UnEquip()
    {
        PlayerScript.Instance.StartActions[CurrentBind.ToString()] -= Started;
        PlayerScript.Instance.PerformActions[CurrentBind.ToString()] -= Performed;
        PlayerScript.Instance.CancelActions[CurrentBind.ToString()] -= Canceled;
    }
}
