using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[System.Serializable]
public enum MenuType
{ 
    Loading,
    TitleMenu,
    MainMenu,
    NewMenu,
    LoadMenu,
    SettingMenu,
    ExitMenu,
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    private InputManager InInstance;
    public CanvasGroup mainCanvas;

    public MenuObject LoadingMenuPrefab;
    public MenuObject TitleMenuPrefab;
    public MenuObject MainMenuPrefab;
    public MenuObject NewMenuPrefab;
    public MenuObject LoadMenuPrefab;
    public MenuObject SettingMenuPrefab;
    public MenuObject ExitMenuPrefab;
    public MenuObject CurrentMenu;
    public List<MenuObject> remainedMenus;

    public void cleanUp()
    {
        if (remainedMenus.Count != 0)
        {
            foreach (MenuObject mo in remainedMenus)
            {
                mo.Remove();
            }
        }
    }

    public void swithcingMenu(MenuType _type, bool kill = true)
    {
        Transform tfs = mainCanvas.transform;
        if (kill == true && CurrentMenu != null)
        {
            if (remainedMenus.Count != 0)
            {
                foreach (MenuObject mo in remainedMenus)
                {
                    mo.Remove();
                }
            }
            CurrentMenu.Remove();

        }
        else if (kill == false && CurrentMenu != null)
        {
            remainedMenus.Add(CurrentMenu);
        }

        switch (_type)
        {
            case MenuType.Loading:
                break;

            case MenuType.TitleMenu:
                CurrentMenu = Instantiate(TitleMenuPrefab, tfs);
                break;
            case MenuType.MainMenu:
                CurrentMenu = Instantiate(MainMenuPrefab, tfs);
                break;
            case MenuType.NewMenu:
                CurrentMenu = Instantiate(NewMenuPrefab, tfs);
                break;
            case MenuType.LoadMenu:
                CurrentMenu = Instantiate(LoadMenuPrefab, tfs);
                break;
            case MenuType.SettingMenu:
                CurrentMenu = Instantiate(SettingMenuPrefab, tfs);
                break;
            case MenuType.ExitMenu:
                CurrentMenu = Instantiate(ExitMenuPrefab, tfs);
                break;
        }

        CurrentMenu.Init();
    }

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
        // Resource Done.
        remainedMenus = new List<MenuObject>();
    }

    public void MenuInitialize()
    {
        InInstance = GameManager.Instance.GetInputController();
        InInstance.SetAction(enumInputPhase.Main);
        swithcingMenu(MenuType.TitleMenu);
    }

    public void ActionInit()
    {
        if (InInstance == null) return;

        InInstance.MainMove += OnMove;
        InInstance.MainPerform += OnSelect;
        InInstance.MainCancel += OnCancel;
        // InInstance.MainMouse0 += OnSelect;
        InInstance.MainMouse1 += OnCancel;
    }

    public void OnMove(Vector2 _vec)
    {
        if (CurrentMenu == null) return;
        CurrentMenu.CursorMove(_vec);
    }

    public void OnSelect()
    {
        if (CurrentMenu == null) return;
        CurrentMenu.Select();
    }

    public void OnCancel()
    {
        if (CurrentMenu == null) return;
        CurrentMenu.Cancel();
    }

}
