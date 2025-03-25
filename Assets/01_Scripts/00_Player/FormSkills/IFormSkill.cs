using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFormSkill
{

    public void Started();
    public void Performed(float _hold);
    public void Canceled();
}
