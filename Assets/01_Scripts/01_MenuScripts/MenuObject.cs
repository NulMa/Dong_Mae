using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuObject : MonoBehaviour
{
    public abstract void Init();

    public abstract void Remove();
    public abstract void Select();
    public abstract void Cancel();
    public abstract void CursorMove(Vector2 _vec);

    public abstract void SetCurosr(int _idx);
}
