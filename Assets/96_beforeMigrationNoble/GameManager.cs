using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private InputManager InInstance;
    PlayerScript player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        player = PlayerScript.Instance;

        // Player.
        // InInstance.SetAction(enumInputPhase.Player);]
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.MenuInitialize();
        }
        else
        {
            InInstance.SetAction(enumInputPhase.Player);
        }

    }

    public void NewGameInitialize()
    { 
        
    }

    public void ContinueInitialize()
    {

    }


    public InputManager GetInputController()
    {
        return InInstance;
    }


}
