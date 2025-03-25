using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FormSpirit : FormScript, IBindForm
{

    private void FixedUpdate()
    {
        if (isMetamorp == false) return;

        // PlayerManager.Instance.FP -= consumeFP;
        // Debug.Log(PlayerManager.Instance.FP);   
        //if (PlayerManager.Instance.FP <= 0f)
        //{
        //    //Regression Call.
        //    isMetamorp = false;
        //    Debug.Log("Regression Call");
        //    PlayerScript.Instance.OffMetamorp();
        //}
    }

    public override void Metamorphosis() 
    {
        isMetamorp = true;
        //PlayerScript.Instance.StartActions[PlayerScript.PlayerActionBind.KeyA.ToString()] += AStarted;
        //PlayerScript.Instance.PerformActions[PlayerScript.PlayerActionBind.KeyA.ToString()] += APerformed;
        //PlayerScript.Instance.CancelActions[PlayerScript.PlayerActionBind.KeyA.ToString()] += ACanceled;

        //PlayerScript.Instance.StartActions[PlayerScript.PlayerActionBind.KeyS.ToString()] += SStarted;
        //PlayerScript.Instance.PerformActions[PlayerScript.PlayerActionBind.KeyS.ToString()] += SPerformed;
        //PlayerScript.Instance.CancelActions[PlayerScript.PlayerActionBind.KeyS.ToString()] += SCanceled;
    }

    public override void Regression() 
    {
        isMetamorp = false;
        //PlayerScript.Instance.StartActions[PlayerScript.PlayerActionBind.KeyA.ToString()] -= AStarted;
        //PlayerScript.Instance.PerformActions[PlayerScript.PlayerActionBind.KeyA.ToString()] -= APerformed;
        //PlayerScript.Instance.CancelActions[PlayerScript.PlayerActionBind.KeyA.ToString()] -= ACanceled;

        //PlayerScript.Instance.StartActions[PlayerScript.PlayerActionBind.KeyS.ToString()] -= SStarted;
        //PlayerScript.Instance.PerformActions[PlayerScript.PlayerActionBind.KeyS.ToString()] -= SPerformed;
        //PlayerScript.Instance.CancelActions[PlayerScript.PlayerActionBind.KeyS.ToString()] -= SCanceled;
    }

    public void ACanceled()
    {

    }

    public void APerformed(float _hold)
    {

    }

    public void AStarted()
    {
        Debug.Log($"{gameObject.name} :: A Skill use.");
    }

    public void SCanceled()
    {

    }

    public void SPerformed(float _hold)
    {

    }

    public void SStarted()
    {
        Debug.Log($"{gameObject.name} :: B Skill use.");
    }


}
