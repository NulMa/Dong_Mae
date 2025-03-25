using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Skill4 : SkillScript, IBindSkill
{
    public PlayerScript player;
    public string skillName = "ChargingBladeWind";
    public enumSkillType Type = enumSkillType.Charging;
    public Skill4Projectiles ProjectilePrefab;

    Vector3 Charge0Scale = new Vector3(1.5f, 2f, 1f);
    Vector3 Charge1Scale = new Vector3(3f, 4f, 1f);
    Vector3 Charge2Scale = new Vector3(6f, 8f, 1f);

    float coolTime = 0f;
    bool charging;
    float chargingTime;

    private void Start()
    {

    }

    private void Update()
    {
        coolTime -= Time.deltaTime;

        if (charging == false)
        {
            return;
        }

        chargingTime += Time.deltaTime;

    }



    public void Started()
    {
        if (coolTime > 0f) return;

        charging = true;

    }

    public void Performed(float _hold)
    {

    }

    public void Canceled()
    {
        if (charging == false) return;
        

        charging = false;
        coolTime = 3f;

        Vector3 scale;

        Debug.Log(chargingTime);
        if (chargingTime < 1.5f)
        {
            Skill4Projectiles pj = Instantiate<Skill4Projectiles>(ProjectilePrefab);
            pj.transform.SetParent(null);
            scale = Charge0Scale;
            scale.x = PlayerScript.Instance.playerDirection == true ? 1f : -1f;
            pj.transform.localScale = scale;
            pj.transform.position = PlayerScript.Instance.transform.position;
            pj.fire = true;
            
        }
        else if (chargingTime < 3f)
        {
            Skill4Projectiles pj = Instantiate<Skill4Projectiles>(ProjectilePrefab);
            pj.transform.SetParent(null);
            scale = Charge1Scale;
            scale.x = PlayerScript.Instance.playerDirection == true ? 1f : -1f;
            pj.transform.localScale = scale;
            pj.transform.position = PlayerScript.Instance.transform.position;
            pj.fire = true;
        }
        else if (chargingTime >= 3f)
        {
            Skill4Projectiles pj = Instantiate<Skill4Projectiles>(ProjectilePrefab);
            pj.transform.SetParent(null);
            scale = Charge2Scale;
            scale.x = PlayerScript.Instance.playerDirection == true ? 1f : -1f;
            pj.transform.localScale = scale;
            pj.transform.position = PlayerScript.Instance.transform.position;
            pj.fire = true;
        }

        chargingTime = 0f;
    }


    public override void Equip(PlayerScript.PlayerActionBind _bind)
    {
        CurrentBind = _bind;
        PlayerScript.Instance.StartActions[_bind.ToString()] = Started;
        PlayerScript.Instance.PerformActions[_bind.ToString()] = Performed;
        PlayerScript.Instance.CancelActions[_bind.ToString()] = Canceled;
    }

    public override void UnEquip()
    {
        PlayerScript.Instance.StartActions[CurrentBind.ToString()] -= Started;
        PlayerScript.Instance.PerformActions[CurrentBind.ToString()] -= Performed;
        PlayerScript.Instance.CancelActions[CurrentBind.ToString()] -= Canceled;
    }
}
