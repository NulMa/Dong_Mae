using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBindForm
{
    public void AStarted();
    public void APerformed(float _hold);
    public void ACanceled();

    public void SStarted();
    public void SPerformed(float _hold);
    public void SCanceled();


    public void Metamorphosis();
    public void Regression();

}
