using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum SkillKeybinding
{
    KeyZ,
    KeyX,
    KeyC,
    None
}

[System.Serializable]
public enum SkillSet
{ 
    PlayerSkill01, PlayerSkill02, PlayerSkill03, PlayerSkill04, PlayerSkill05, PlayerSkill06, PlayerSkill07, PlayerSkill08, None
}

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager Instance;

    [SerializeField] PlayerSkill01Projectile Skill01_Prefab; // Rock
    [SerializeField] PlayerSkill02Effect Skill02_Prefab; // Heal
    [SerializeField] PlayerSkill03Projectile Skill03_Prefab; // Ora Rush
    [SerializeField] GameObject Skill04_Effect; // Dark Blade Aura
    [SerializeField] PlayerSkill04Projectile Skill04_Prefab; // Dark Blade Aura
    [SerializeField] PlayerSkill05Effect Skill05_Prefab; // Dark Blade Aura
    [SerializeField] PlayerSkill04Projectile Skill06_Prefab; // Dark Blade Aura
    [SerializeField] PlayerSkill07Effect Skill07_Prefab; // Dark Blade Aura
    [SerializeField] PlayerSkill08Effect Skill08_Prefab; // Body.


    public Sprite DefaultSkillIcon;
    public Sprite[] SkillIconSet;

    public bool ChangeReady;
    public SkillSet KeyZ;
    public SkillSet KeyX;
    public SkillSet KeyC;

    private void FixedUpdate()
    {
        if (KeyZSprite.fillAmount == 1f && KeyXSprite.fillAmount == 1f && KeyCSprite.fillAmount == 1f)
        {
            ChangeReady = true;
        }
        else
        {
            ChangeReady = false;
        }
    }

    public void SkillCoolTimeUpdate(SkillKeybinding _key, float curTime, float maxTime)
    {
        switch (_key)
        {
            default:
            case SkillKeybinding.KeyZ:
                KeyZSprite.fillAmount = 1f - (curTime / maxTime);
                break;
            case SkillKeybinding.KeyX:
                KeyXSprite.fillAmount = 1f - (curTime / maxTime);
                break;
            case SkillKeybinding.KeyC:
                KeyCSprite.fillAmount = 1f - (curTime / maxTime);
                break;
        }

    }

    public ISkillScript KeyZAction;
    public ISkillScript KeyXAction;
    public ISkillScript KeyCAction;

    public Image KeyZSprite;
    public Image KeyXSprite;
    public Image KeyCSprite;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }


    private void Start()
    {
        KeyZ = SkillSet.None;
        KeyX = SkillSet.None;
        KeyC = SkillSet.None;
    }

    public void SkillAlreadyRegistered(SkillSet _set)
    {
        if (KeyZ == _set)
        {
            KeyZ = SkillSet.None;
            KeyZSprite.fillAmount = 1f;
            KeyZSprite.sprite = DefaultSkillIcon;
            if (KeyZAction != null)
            {
                KeyZAction = null;
            }
        }
        if (KeyX == _set)
        {
            KeyX = SkillSet.None;
            KeyXSprite.fillAmount = 1f;
            KeyXSprite.sprite = DefaultSkillIcon;
            if (KeyXAction != null)
            {
                KeyXAction = null;
            }
        }
        if (KeyC == _set)
        {
            KeyC = SkillSet.None;
            KeyCSprite.fillAmount = 1f;
            KeyCSprite.sprite = DefaultSkillIcon;
            if (KeyCAction != null)
            {
                KeyCAction = null;
            }
        }
    }

    public ISkillScript GetSkillInstance(SkillKeybinding _key, SkillSet _set)
    {
        SkillAlreadyRegistered(_set);
        switch (_key)
        {
            case SkillKeybinding.KeyZ:
                KeyZ = _set;
                KeyZAction = GetInstance(_key, _set);
                KeyZSprite.sprite = SkillIconSet[(int)_set];
                return KeyZAction;
                
            case SkillKeybinding.KeyX:
                KeyX = _set;
                KeyXAction = GetInstance(_key, _set);
                KeyXSprite.sprite = SkillIconSet[(int)_set];
                return KeyXAction;

            case SkillKeybinding.KeyC:
                KeyC = _set;
                KeyCAction = GetInstance(_key, _set);
                KeyCSprite.sprite = SkillIconSet[(int)_set];
                return KeyCAction;

            default:
                return null;
        }

    }

    public ISkillScript GetInstance(SkillKeybinding _key, SkillSet _set)
    {
        switch (_set)
        {
            case SkillSet.PlayerSkill01:
            default:
                return new PlayerSkill01(Playrer.Instance, Skill01_Prefab, _key);
            case SkillSet.PlayerSkill02:
                return new PlayerSkill02(Playrer.Instance, Skill02_Prefab, _key);
            case SkillSet.PlayerSkill03:
                return new PlayerSkill03(Playrer.Instance, Skill03_Prefab, _key);
            case SkillSet.PlayerSkill04:
                return new PlayerSkill04(Playrer.Instance, Skill04_Prefab, Skill04_Effect, _key);
            case SkillSet.PlayerSkill05:
                return new PlayerSkill05(Playrer.Instance, Skill05_Prefab, _key);
            case SkillSet.PlayerSkill06:
                return new PlayerSkill01(Playrer.Instance, Skill01_Prefab, _key);
            case SkillSet.PlayerSkill07:
                return new PlayerSkill07(Playrer.Instance, Skill07_Prefab, _key);
            case SkillSet.PlayerSkill08:
                return new PlayerSkill08(Playrer.Instance, Skill08_Prefab, _key);
        }
    }



}