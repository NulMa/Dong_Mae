using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum enumFormType
{ 
    Spirit1, Spirit2, Spirit3, Spirit4, Spirit5, None
}

[System.Serializable]
public class FormInfo
{
    public enumFormType form;
    public string name;
    public string desc;
    public Sprite figure;

}

public class FormScript : MonoBehaviour
{
    [SerializeField] protected enumFormType FormType;
    [SerializeField] protected string FormName;
    [SerializeField] protected string FormDescription;
    [SerializeField] protected Sprite FormSprite;
    [SerializeField] protected float consumeFP;
    [SerializeField] protected bool isMetamorp;

    public enumFormType Form { get { return FormType; } }
    public string Name { get { return FormName; } }
    public string Desc { get { return FormDescription; } }
    public Sprite Figure { get { return FormSprite; } }

    public virtual void Metamorphosis() { }

    public virtual void Regression() { }

    public FormInfo getFormInfo()
    {
        return new FormInfo() { form = Form, name =Name, desc=Desc, figure=Figure }; 

    }
}
