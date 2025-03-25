using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainTitle : MonoBehaviour
{
    [SerializeField] Button[] TitleButtons;
    [SerializeField] Color HighlightColor;
    [SerializeField] Color NormalColor;

    int CurIDX;
    int MaxIDX;

    private void Start()
    {
        TitleButtons[0].onClick.AddListener(OnClickedStart);
        TitleButtons[1].onClick.AddListener(OnClickedStart);
        TitleButtons[3].onClick.AddListener(OnClickedEnd);

        CurIDX = 0;
        MaxIDX = TitleButtons.Length - 1;
    }

    public void OnClickedStart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OnClickedEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
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

    public void OnUIMove(InputValue value)
    {
        // Cursor Move

        Vector2 inputVec = value.Get<Vector2>();
        if (inputVec.y < 0 || inputVec.x > 0)
        {
            TitleButtons[CurIDX++].GetComponent<Image>().color = Colorization(false);
            if (CurIDX < 0f)
            {
                CurIDX = 0;
            }
            else if (CurIDX > MaxIDX)
            {
                CurIDX = MaxIDX;
            }
            TitleButtons[CurIDX].GetComponent<Image>().color = Colorization(true);
        }
        else if (inputVec.y > 0 || inputVec.x < 0)
        {
            TitleButtons[CurIDX--].GetComponent<Image>().color = Colorization(false);
            if (CurIDX < 0f)
            {
                CurIDX = 0;
            }
            else if (CurIDX > MaxIDX)
            {
                CurIDX = MaxIDX;
            }
            TitleButtons[CurIDX].GetComponent<Image>().color = Colorization(true);
        }

    }

    public void OnUIKeyZ(InputValue value)
    {
        TitleButtons[CurIDX].GetComponent<Image>().color = Colorization(false);
        TitleButtons[CurIDX].onClick.Invoke();
    }

    public void OnUIKeyX(InputValue value)
    {

    }
}


