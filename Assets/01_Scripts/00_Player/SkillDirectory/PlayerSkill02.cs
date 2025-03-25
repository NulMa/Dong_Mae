using UnityEngine;
//using static UnityEngine.InputManagerEntry;

public class PlayerSkill02 : ISkillScript
{
    Playrer player;
    Rigidbody2D rigid;
    public string skillName = "Heal";
    SkillSet skillSet;
    SkillKeybinding keyBind;

    public enumSkillType Type = enumSkillType.Active;

    public PlayerSkill02Effect HealEffect;
    float curTime;
    float duration;
    bool ActiveSkill;
    float mp = 10f;

    public PlayerSkill02(Playrer _player, PlayerSkill02Effect _prefab, SkillKeybinding _bind)
    {
        Init(_player);
        HealEffect = _prefab;
        duration = 0.15f;
        keyBind = _bind;
    }

    public void Process()
    {
        curTime -= Time.fixedDeltaTime;
        duration -= Time.fixedDeltaTime;
        if (curTime < 0f)
        {
            ActiveSkill = false;
            curTime = 0f;
        }

        PlayerSkillManager.Instance.SkillCoolTimeUpdate(keyBind, curTime, 3f);

        if (duration < 0f)
        {
            duration = 0f;
        }

    }

    public void Execute()
    {
        if (curTime > 0f) return;
        if (PlayerManager.Instance.CurMP < mp) return;

        PlayerManager.Instance.CurMP -= mp;

        curTime = 3f;
        duration = 0.5f;
        ActiveSkill = true;
        PlayerSkill02Effect fp = Object.Instantiate(HealEffect, Playrer.Instance.transform);
    }

    public void Init(Playrer _player)
    {
        rigid = _player.GetComponent<Rigidbody2D>();

    }

    public void Release()
    {

    }

    public SkillSet GetBind()
    {
        return skillSet;
    }

    public void Init(Playrer playrer, SkillSet _set)
    {
        player = playrer;
        skillSet = _set;
    }
}