using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;

public enum enumInputPhase
{ 
    Player,
    Main,
    Seq,
}

[CreateAssetMenu(menuName = "InputSystem/InputManager")]
public class InputManager : ScriptableObject, IBlueprint.IActionPlayerActions, IBlueprint.IActionSeqActions, IBlueprint.IActionPauseActions
{
    IBlueprint _InputMap;

    private void OnEnable()
    {
        if (_InputMap == null)
        {
            _InputMap = new IBlueprint();

            _InputMap.ActionPlayer.SetCallbacks(this);
            _InputMap.ActionPause.SetCallbacks(this);
            _InputMap.ActionSeq.SetCallbacks(this);
        }
    }

    #region Action Switching
    public void SetAction(enumInputPhase _phase)
    {
        switch (_phase)
        { 
            case enumInputPhase.Player:
                SetPlayer();
                break;
            case enumInputPhase.Main:
                SetMain();
                break;
            case enumInputPhase.Seq:
                SetSeq();
                break;
        }
    }

    private void SetPlayer()
    {
        _InputMap.ActionPlayer.Enable();
        _InputMap.ActionPause.Disable();
        _InputMap.ActionSeq.Disable();
    }

    private void SetMain()
    {
        _InputMap.ActionPlayer.Disable();
        _InputMap.ActionPause.Enable();
        _InputMap.ActionSeq.Disable();
    }

    private void SetSeq()
    {
        _InputMap.ActionPlayer.Disable();
        _InputMap.ActionPause.Disable();
        _InputMap.ActionSeq.Enable();
    }
    #endregion

    // Main Menu
    #region Main Menu Action Define
    public event Action MainCancel;
    public event Action<Vector2> MainMove;
    public event Action MainPerform;
    public event Action MainMouse0;
    public event Action MainMouse1;
    public event Action<Vector2> MainMousePos;
    #endregion

    // TODO: Form Change Binding
    #region Main Menu Action Event
    public void OnMainCancel(InputAction.CallbackContext context)
    {

    }

    public void OnMainMove(InputAction.CallbackContext context)
    {

    }

    public void OnMainPerform(InputAction.CallbackContext context)
    {

    }

    public void OnMouse0(InputAction.CallbackContext context)
    {

    }

    public void OnMouse1(InputAction.CallbackContext context)
    {

    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {

    }
    #endregion

    //Player Variable Names :: {S}tarted / {P}erformed / {C}anceld
    #region Player Action Define
    public event Action BtnAs;
    public event Action<float> BtnAp;
    public event Action BtnAc;
    float Ahold = 0f;

    public event Action BtnSs;
    public event Action<float> BtnSp;
    public event Action BtnSc;
    float Shold = 0f;

    public event Action BtnZs;
    public event Action<float> BtnZp;
    public event Action BtnZc;
    float Zhold = 0f;

    public event Action BtnXs;
    public event Action<float> BtnXp;
    public event Action BtnXc;
    float Xhold = 0f;

    public event Action BtnCs;
    public event Action<float> BtnCp;
    public event Action BtnCc;
    float Chold = 0f;

    public event Action BtnNum1;
    public event Action BtnNum2;
    public event Action BtnNum3;
    public event Action BtnNum4;
    public event Action BtnNum5;

    // Add Form Change Actions

    public event Action<Vector2> PlayerMove;
    public event Action PlayerJump;
    public event Action PlayerDash;

    public event Action PlayerSkillStart;
    public event Action PlayerSkillCancel;

    #endregion

    // TODO: Form Change Binding
    #region Player Action Event


    public void OnBtnA(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Ahold = 0f;
            BtnAs?.Invoke();
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            Ahold += Time.deltaTime;
            BtnAp?.Invoke(Ahold);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Ahold = 0f;
            BtnAc?.Invoke();
        }
    }

    public void OnBtnS(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Shold = 0f;
            BtnSs?.Invoke();
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            Shold += Time.deltaTime;
            BtnSp?.Invoke(Shold);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Shold = 0f;
            BtnSc?.Invoke();
        }
    }



    public void OnBtnZ(InputAction.CallbackContext context)
    {

        bool hold = context.ReadValueAsButton();

        if (context.phase == InputActionPhase.Started)
        {
            // hold = true;
            Zhold = 0f;
            BtnZs?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // hold = false;
            Zhold = 0f;
            BtnZc?.Invoke();
        }

        if (hold)
        {
            Zhold += Time.deltaTime;
            BtnZp?.Invoke(Zhold);
        }
    }

    public void OnBtnX(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Xhold = 0f;
            BtnXs?.Invoke();
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            Xhold += Time.deltaTime;
            BtnXp?.Invoke(Xhold);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Xhold = 0f;
            BtnXc?.Invoke();
        }
    }

    public void OnBtnC(InputAction.CallbackContext context)
    {
        bool hold=false;
        if (context.phase == InputActionPhase.Started)
        {
            hold = true;
            Chold = 0f;
            BtnCs?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            hold = false;
            Chold = 0f;
            BtnCc?.Invoke();
        }
        if (hold)
        {
            Chold += Time.deltaTime;
            BtnCp?.Invoke(Chold);
        }

    }


    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerJump?.Invoke();
        }
    }

    public void OnPlayerDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerDash?.Invoke();
        }
    }

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerMove?.Invoke(context.ReadValue<Vector2>());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            PlayerMove?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnOpenSkill(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerSkillStart?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            PlayerSkillCancel?.Invoke();
        }
    }

    public void OnBtnNum1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            BtnNum1?.Invoke();
        }
    }

    public void OnBtnNum2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            BtnNum2?.Invoke();
        }
    }

    public void OnBtnNum3(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            BtnNum3?.Invoke();
        }
    }

    public void OnBtnNum4(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            BtnNum4?.Invoke();
        }
    }

    public void OnBtnNum5(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            BtnNum5?.Invoke();
        }
    }

    #endregion

    // Sequence
    #region Sequence Action Define

    public event Action SeqPlay;

    #endregion

    #region Sequence Action Event
    public void OnOnSeqPlay(InputAction.CallbackContext context)
    {
    }




    #endregion


}
