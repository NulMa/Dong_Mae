using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    [Serializable]
    public struct BackgroundArray {
        public Sprite sprite;
        public float speed;
        public int sortNum;
    }

    [SerializeField]
    BackgroundArray[] BGArray;

    private void Awake() {
        for(int i = 0 ; i < BGArray.Length ; i++) {

        }
    }

}
