using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerScript;
using static SkillManager;

#region global enums



#endregion

public class FormManager : MonoBehaviour
{
    public static FormManager instance;
    public enumFormType CurrentForm;

    [SerializeField] List<FormScript> FormPrefab;
    Dictionary<enumFormType, FormScript> DictForm;
    bool[] hasForm;

    private void Init()
    {
        hasForm = new bool[FormPrefab.Count];
        // get hasSkill from DataManager.
        // FIXME: Not Implemented DataManager.
        hasForm[0] = true;
        hasForm[1] = true;
        hasForm[2] = true;
        hasForm[3] = true;
        hasForm[4] = true;

        string[] sNames = System.Enum.GetNames(typeof(enumFormType));
        for (int idx = 0; idx < sNames.Length; idx++)
        {
            if (sNames[idx] == enumFormType.None.ToString()) continue;
            FormScript so = Instantiate(FormPrefab[idx], transform);
            so.name = sNames[idx];
            so.gameObject.SetActive(false);
            DictForm[(enumFormType)idx] = so;
        }
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
    private void Start()
    {
        DictForm = new Dictionary<enumFormType, FormScript>();
        Init();
    }

    public enum PlayerFormBind
    {
        Key1, Key2, Key3, Key4, Key5, None,
    }

    public FormInfo FormMetamorp(PlayerFormBind _form)
    {

        enumFormType _name = enumFormType.None;
        switch (_form)
        {
            case PlayerFormBind.Key1:
                _name = enumFormType.Spirit1;
                break;
            case PlayerFormBind.Key2:
                _name = enumFormType.Spirit2;
                break;
            case PlayerFormBind.Key3:
                _name = enumFormType.Spirit3;
                break;
            case PlayerFormBind.Key4:
                _name = enumFormType.Spirit4;
                break;
            case PlayerFormBind.Key5:
                _name = enumFormType.Spirit5;
                break;
        }

        DictForm[_name].gameObject.SetActive(true);
        DictForm[_name].Metamorphosis();
        return DictForm[_name].getFormInfo();
    }

    public void FormRegress(enumFormType _form)
    {

        DictForm[_form].Regression();
        DictForm[_form].gameObject.SetActive(false);
    }


}
