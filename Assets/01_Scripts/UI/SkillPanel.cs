using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;



public class SkillPanel : MonoBehaviour
{
    public static SkillPanel instance;
    CanvasGroup mainPanel;
    CanvasGroup skillIcons;
    Dictionary<enumSkillName, SkillIcon> skills;
    SkillIcon targetIcon;
    enumSkillName targetName;
    SkillKeybinding targetBind;
    SkillIcon[] icons;

    [SerializeField] GameObject CursorAxis; // rotation axis.
    [SerializeField] GameObject CursorArrow; // Arrow for Check.
    [SerializeField] TMP_Text SkillNameText;
    [SerializeField] TMP_Text SkillDescriptionText;

    public Action<SkillKeybinding> EquipListener;
    

    private void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(gameObject); }

        EquipListener = new Action<SkillKeybinding>((data) => { });
    }

    private void Start()
    {
        mainPanel = GetComponent<CanvasGroup>();
        skills = new Dictionary<enumSkillName, SkillIcon>();
        

        icons = GetComponentsInChildren<SkillIcon>();
        for(int idx=0; idx < icons.Length; idx++)
        {
            int count = idx;
            enumSkillName t = (enumSkillName)count;
            skills[t] = icons[count];
            icons[count].image.sprite = PlayerSkillManager.Instance.SkillIconSet[count];
            // EquipListener += (() => icons[count].EquipCheck(targetBind));

            // TODO: has Check and SetActive.
        }

        CursorArrow.SetActive(false);
        gameObject.SetActive(false);
    }

    public void RollSelectCursor(Vector2 _inputVec)
    {
        if (_inputVec == Vector2.zero)
        {
            CursorArrow.SetActive(false);
            if (targetIcon != null) targetIcon = null;

            return;
        }

        CursorArrow.gameObject.SetActive(true);
        
        Vector3 dir = new Vector3(_inputVec.x, _inputVec.y, 0).normalized;
        float angle = Quaternion.FromToRotation(Vector3.up, dir).eulerAngles.z;
        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle));
        CursorAxis.transform.rotation = rot;


        float near = Mathf.Infinity;
        for (int idx = 0; idx < skills.Keys.Count; idx++)
        {
            Vector2 curPos = skills[(enumSkillName)idx].gameObject.transform.position;
            float comp = Vector2.Distance(curPos, CursorArrow.transform.position);
            if (near > comp)
            {
                near = comp;
                targetName = (enumSkillName)idx;
                targetIcon = skills[(enumSkillName)idx];
                
            }
            
        }

        Debug.Log($"Current Cursor : {targetIcon.SkillName}");
        SkillNameText.text = targetIcon.SkillName;
        SkillDescriptionText.text = targetIcon.SkillDescription;
    }

    public ISkillScript EquipSkill(SkillKeybinding _bind)
    {
        if (targetIcon == null) return null;

        ISkillScript command = PlayerSkillManager.Instance.GetSkillInstance(_bind, targetIcon.skillSet);

        EquipListener?.Invoke(_bind);
        targetIcon.Equip(_bind);

        return command;
    }

}
