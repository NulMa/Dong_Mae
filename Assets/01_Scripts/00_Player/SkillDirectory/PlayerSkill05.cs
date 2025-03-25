using UnityEngine;

public class PlayerSkill05 : ISkillScript
{
    Playrer player;
    Rigidbody2D rigid;
    public string skillName = "Dash Attack";
    SkillSet skillSet;
    SkillKeybinding keyBind;

    float mp = 10f;

    public enumSkillType Type = enumSkillType.Active;

    public PlayerSkill05Effect EffectPrefab;
    public PlayerSkill05Effect nowEffect;
    float cooltime;

    bool skillActive;
    float duration;

    public PlayerSkill05(Playrer _player, PlayerSkill05Effect _prefab, SkillKeybinding keyBind)
    {
        Init(_player);
        EffectPrefab = _prefab;
        duration = 2f;
        cooltime = 5f;
        this.keyBind = keyBind;
    }

    public void Process()
    {
        if (!skillActive) cooltime -= Time.deltaTime;

        if (skillActive) 
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            duration -= Time.deltaTime;
            rigid.velocity += (Playrer.Instance.sprite.flipX ? Vector2.left : Vector2.right) * Time.deltaTime * 300f + Time.deltaTime * 10f * Vector2.up;
        } 

        if (duration < 0f && skillActive)
        {
            Object.Destroy(nowEffect.gameObject);
            duration = 2f;
            skillActive = false;
        }

        if (cooltime < 0f)
        {
            cooltime = 0f;
        }

        PlayerSkillManager.Instance.SkillCoolTimeUpdate(keyBind, cooltime, 3f);

    }

    public void Execute()
    {
        if (cooltime > 0f) return;
        if (PlayerManager.Instance.CurMP < mp) return;

        PlayerManager.Instance.CurMP -= mp;

        skillActive = true;
        nowEffect = Object.Instantiate<PlayerSkill05Effect>(EffectPrefab, Playrer.Instance.transform);
        nowEffect.transform.position = Playrer.Instance.transform.position + Vector3.up;
        Playrer.Instance.anim.SetTrigger("Dash");
        cooltime = 5f;
    }

    public void Init(Playrer _player)
    {
        rigid = _player.GetComponent<Rigidbody2D>();

    }

    public void Release()
    {
        if (skillActive == false) return;


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