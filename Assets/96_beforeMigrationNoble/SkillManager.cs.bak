using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region global enums
[System.Serializable]
public enum enumSkillName
{ 
    s1, s2, s3, s4, s5, s6, s7, s8, None,
}



#endregion



public class SkillManager : MonoBehaviour
{
    /*
     * Skill Manager
     * 1. get  Key Binding Register Message from Player and get Target Skill At Cursored Instance
     * 2. Skill Prefab / Instantiate
     * 3. Player Has Skill? or not, Such Skill will be locked from Skill Manager.
     */

    public static SkillManager instance;
    SkillPanel skillPanel;
    Dictionary<enumSkillName, SkillScript> DictSkill;
    Dictionary<enumKeyBinding, enumSkillName> DictBinding;

    [SerializeField] List<SkillScript> SkillPrefab;
    bool[] hasSkill;

    public enum enumKeyBinding
    {
        KeyZ,
        KeyX,
        KeyC,
    }

    private void Init()
    {
        hasSkill = new bool[SkillPrefab.Count];
        // get hasSkill from DataManager.
        // FIXME: Not Implemented DataManager.
        hasSkill[0] = true;
        hasSkill[1] = true;
        hasSkill[2] = true;
        hasSkill[3] = true;
        hasSkill[4] = true;
        hasSkill[5] = true;
        hasSkill[6] = true;
        hasSkill[7] = true;

        string[] sNames = System.Enum.GetNames(typeof(enumSkillName));
        for (int idx = 0; idx < sNames.Length; idx++)
        {
            if (sNames[idx] == enumSkillName.None.ToString()) continue;
            SkillScript so = Instantiate(SkillPrefab[idx], transform);
            so.name = sNames[idx];
            // so.gameObject.SetActive(true);
            DictSkill[(enumSkillName)idx] = so;
        }

        string[] sKeys = System.Enum.GetNames(typeof(enumKeyBinding));
        for (int idx = 0; idx < sKeys.Length; idx++)
        {
            DictBinding[(enumKeyBinding)idx] = enumSkillName.None;
        }
    }


    private void Start()
    {
        skillPanel = SkillPanel.instance;
        //DictSkill = new Dictionary<enumSkillName, SkillScript>();
        //DictBinding = new Dictionary<enumKeyBinding, enumSkillName>();
        //Init();



    }





    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    public bool RegisterSkill(enumKeyBinding _bind, enumSkillName _skill)
    {
        // Lock Check
        if (hasSkill[(int)_skill] == false) return false;

        if (DictBinding[_bind] != enumSkillName.None)
        {
            RemoveSkill(_bind, DictBinding[_bind]);
        }

        for (int idx = 0; idx < DictBinding.Count; idx++)
        {
            enumSkillName target = DictBinding[(SkillManager.enumKeyBinding)idx];
            if (target == _skill)
            {
                RemoveSkill((SkillManager.enumKeyBinding)idx);
            }
        }

        DictBinding[_bind] = _skill;
        DictSkill[_skill].Equip((PlayerScript.PlayerActionBind)System.Enum.Parse(typeof(PlayerScript.PlayerActionBind), _bind.ToString()));
        
        return true;
    }

    public void RemoveSkill(enumKeyBinding _bind)
    {
        enumSkillName target = DictBinding[_bind];
        if (target != enumSkillName.None)
        {
            DictSkill[target].UnEquip();
        }
        

    }

    public bool RemoveSkill(enumKeyBinding _bind, enumSkillName _skill)
    {
        DictBinding[_bind] = enumSkillName.None;
        DictSkill[_skill].UnEquip();
        

        return false;
    }

}
