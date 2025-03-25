using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuObject
{
    public enum MainMenuButton
    { 
        NEW,
        SAVED,
        SETTING,
        EXIT,
    }

    [SerializeField] private Toggle[] Menus;
    private Toggle CurrentButton;
    private int total_cursors;
    private int current_curosr = 0;

    public override void Cancel()
    {
        
    }

    public override void Remove()
    {
        Destroy(gameObject);
    }

    public override void CursorMove(Vector2 _vec)
    {
        if (_vec == Vector2.zero) return;

        if (_vec.x > 0 || _vec.y < 0)
        {
            current_curosr++;
        }
        else if (_vec.x < 0 || _vec.y > 0)
        {
            current_curosr--;
        }

        if (current_curosr > total_cursors - 1)
        {
            current_curosr = 0;
        }
        else if (current_curosr < 0)
        {
            current_curosr = total_cursors - 1;
        }
        CurrentButton.isOn = false;
        CurrentButton = Menus[current_curosr];
        CurrentButton.isOn = true;
    }

    public override void Init()
    {
        total_cursors = Menus.Length;

        // Save Exist Check.
        if (PlayerPrefs.HasKey("SAVE_EXIST") == false || PlayerPrefs.GetInt("SAVE_EXIST", 0) == 0)
        {
            PlayerPrefs.SetInt("SAVE_EXIST", 0);
            current_curosr = 0;
        }
        else
        {
            current_curosr = 1;
        }
        CurrentButton = Menus[current_curosr];
        CurrentButton.Select();
    }

    public override void Select()
    {
        switch ((MainMenuButton)current_curosr)
        {
            case MainMenuButton.NEW:
                MenuManager.Instance.swithcingMenu(MenuType.NewMenu);
                break;
            case MainMenuButton.SAVED:
                MenuManager.Instance.swithcingMenu(MenuType.LoadMenu);
                break;
            case MainMenuButton.SETTING:
                MenuManager.Instance.swithcingMenu(MenuType.SettingMenu);
                break;
            case MainMenuButton.EXIT:
                MenuManager.Instance.swithcingMenu(MenuType.ExitMenu);
                break;
        }
    }

    public override void SetCurosr(int _idx)
    {
        CurrentButton = Menus[_idx];
        current_curosr = _idx;
        CurrentButton.Select();
    }
}
