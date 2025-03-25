using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FormSkillTable
{ 
    public int Skill_index; // Unique ID
    public string Skill_name; // Name
    public int Skill_usesp; // SP
    public float Skill_cooldown; // Cooltime
    public float Skill_casttime; // casting Time
    public int Skill_count; // Stock
    public int Skill_obcount; // Number of Object for Creation
    public int Skill_ob_index; // ?
    public float Skill_change; // Charging Time
    public Animation Skill_ani; // Animation
    public Texture2D IconName; // Texture
    public Material Skill_effect_index; // maybe Shader Mat?
}


public abstract class FormSkill : MonoBehaviour
{
    [SerializeField] protected enumSkillType SkillType;
    [SerializeField] protected FormSkillTable SkillContainer; 



}