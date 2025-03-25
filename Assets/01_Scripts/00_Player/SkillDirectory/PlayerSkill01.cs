using UnityEngine;

public class PlayerSkill01 : ISkillScript
{
    Playrer player;
    Rigidbody2D rigid;
    public string skillName = "Fist";
    SkillSet skillSet;
    SkillKeybinding keyBind;

    public enumSkillType Type = enumSkillType.Active;

    public PlayerSkill01Projectile FistPrefab;
    float curTime;
    float duration;
    bool ActiveSkill;

    float mp = 10f;

    public PlayerSkill01(Playrer _player, PlayerSkill01Projectile _prefab, SkillKeybinding _bind)
    {
        Init(_player);
        FistPrefab = _prefab;
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
        PlayerSkill01Projectile fp = Object.Instantiate(FistPrefab);
        fp.transform.position = Playrer.Instance.transform.position + Vector3.up;
        fp.GetComponent<Rigidbody2D>().AddForce(Playrer.Instance.sprite.flipX ? Vector2.left * 50f : Vector2.right * 30f, ForceMode2D.Impulse);
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