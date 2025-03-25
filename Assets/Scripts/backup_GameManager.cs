using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backup_GameManager : MonoBehaviour{

    public static backup_GameManager instance;
    public GameObject player;

    [Serializable]
    public struct Keys {
        public int code;
        public bool isHave;
    }
    [Header("KeyCodes")]
    public Keys[] keys;

    void Awake() {
        instance = this;

        DontDestroyOnLoad(gameObject); // dont destroy when scene is change

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
