using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;

public enum enumPauseTree
{ 
    PauseMain,
    PauseEnd,
}

public enum enumPauseMain
{ 
    Continue,
    Return,
    Setting,
    Exit,
}

public enum enumPauseEnd
{ 
    YES,NO,
}


public class PauseController : MonoBehaviour
{
    public static PauseController Instance;
    public enumPauseTree CurrentState;

    public CanvasGroup MainCanvas;
    public CanvasGroup EndCanvas;

    [SerializeField] Button[] mainBtns; // 4
    [SerializeField] Button[] exitBtns; // 2

    [SerializeField] Button[] target;
    int CurIDX;
    int MaxIDX;

    [SerializeField] Color HighlightColor;
    [SerializeField] Color NormalColor;

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



        CurrentState = enumPauseTree.PauseMain;
        target = mainBtns;
        CurIDX = 0;
        MaxIDX = target.Length -1;

        mainBtns[0].onClick.AddListener(OnClickContinue);
        mainBtns[3].onClick.AddListener(OnClickExitInMain);

        exitBtns[0].onClick.AddListener(OnClickExitYes);
        exitBtns[1].onClick.AddListener(OnClickExitNo);

        gameObject.SetActive(false);
    }

    

    private void Start()
    {

    }

    public Color Colorization(bool isOn)
    {
        if (isOn)
        {
            return HighlightColor;
        }
        else
        {
            return NormalColor;
        }
    }

    public void OnClickContinue()
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        GetComponent<PlayerInput>().enabled = false;

        Playrer.Instance.GetComponent<PlayerInput>().enabled = true;
        Playrer.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void OnClickExitInMain()
    {
        EndCanvas.gameObject.SetActive(true);
        CurrentState = enumPauseTree.PauseEnd;
        target[CurIDX].GetComponent<Image>().color = Colorization(false);
        MenuTransit();
    }

    public void OnClickExitYes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnClickExitNo()
    {   
        EndCanvas.gameObject.SetActive(false);
        CurrentState = enumPauseTree.PauseMain;
        MenuTransit();
    }

    public void MenuTransit()
    {
        switch (CurrentState)
        {
            default:
            case enumPauseTree.PauseMain:
                target = mainBtns;
                break;
            case enumPauseTree.PauseEnd:
                target = exitBtns;
                break;
        }

        CurIDX = 0;
        MaxIDX = target.Length -1;
        target[CurIDX].GetComponent<Image>().color = Colorization(true);
    }

    public void OnUIMove(InputValue value)
    {
        // Cursor Move

        Vector2 inputVec = value.Get<Vector2>();
        if (inputVec.y < 0 || inputVec.x > 0)
        {
            target[CurIDX++].GetComponent<Image>().color = Colorization(false);
            if (CurIDX < 0f)
            {
                CurIDX = 0;
            }
            else if (CurIDX > MaxIDX)
            {
                CurIDX = MaxIDX;
            }
            target[CurIDX].GetComponent<Image>().color = Colorization(true);
        }
        else if (inputVec.y > 0 || inputVec.x < 0)
        {
            target[CurIDX--].GetComponent<Image>().color = Colorization(false);
            if (CurIDX < 0f)
            {
                CurIDX = 0;
            }
            else if (CurIDX > MaxIDX)
            {
                CurIDX = MaxIDX;
            }
            target[CurIDX].GetComponent<Image>().color = Colorization(true);
        }




    }

    public void OnUIKeyZ(InputValue value)
    {
        target[CurIDX].GetComponent<Image>().color = Colorization(false);
        target[CurIDX].onClick.Invoke();
    }

    public void OnUIKeyX(InputValue value)
    {

    }


}
