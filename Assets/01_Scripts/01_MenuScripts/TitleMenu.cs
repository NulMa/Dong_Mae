using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MenuObject
{
    [SerializeField] private Button btnAnyKey;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            OnClickedAction();
        }
    }

    public override void Cancel()
    {
        return;
    }

    public override void CursorMove(Vector2 _vec)
    {
        return;
    }

    public override void Init()
    {
        
    }

    public override void Remove()
    {
        Destroy(gameObject);
    }

    public override void Select()
    {
        OnClickedAction();
    }

    public void OnClickedAction()
    {
        // Destroy Action.
        MenuManager.Instance.swithcingMenu(MenuType.MainMenu);
        Destroy(gameObject);
    }

    public override void SetCurosr(int _idx)
    {
        throw new System.NotImplementedException();
    }
}
