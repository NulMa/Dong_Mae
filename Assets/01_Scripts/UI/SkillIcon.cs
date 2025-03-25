using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] SkillKeybinding keybinding = SkillKeybinding.None;
    [SerializeField] public SkillSet skillSet;
    TMP_Text skillName;
    TMP_Text skillEquip;
    [SerializeField] string skillDescription;
    public Image image;
    
    
    public string SkillName { get { return skillName.text; } set { skillName.text = value; } }


    public string SkillDescription { get { return skillDescription; } set { skillDescription = value; } }

    public void EquipCheck(SkillKeybinding _bind)
    {

        if (keybinding == SkillKeybinding.None) return;

        if (keybinding != _bind) return;

        UnEquip();
        keybinding = SkillKeybinding.None;

    }

    public void Equip(SkillKeybinding _bind)
    {
        keybinding = _bind;
        if (skillEquip.gameObject.activeSelf) skillEquip.gameObject.SetActive(true);
        skillEquip.text = keybinding.ToString();
        skillEquip.gameObject.SetActive(true);
    }

    public void UnEquip()
    {
        keybinding = SkillKeybinding.None;
        skillEquip.text = "";
        skillEquip.gameObject.SetActive(false);
    }

    void Start()
    {
        skillName = transform.GetChild(0).GetComponent<TMP_Text>();
        skillEquip = transform.GetChild(1).GetComponent<TMP_Text>();  
        skillEquip.gameObject.SetActive(false);
        SkillPanel.instance.EquipListener += (data) => EquipCheck(data);
        image = GetComponent<Image>();
    }
}
