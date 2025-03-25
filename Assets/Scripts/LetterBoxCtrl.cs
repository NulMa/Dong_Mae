using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoxCtrl : MonoBehaviour{

    public RectTransform upper;
    public RectTransform lower;

    private void Awake() {

    }


    public IEnumerator letterboxOn() {
        upper.gameObject.SetActive(true);
        lower.gameObject.SetActive(true);
        for(int i = 0; i < 136; i++) {
            upper.anchoredPosition = new Vector2(0, 135-i);
            lower.anchoredPosition = new Vector2(0, -135+i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator letterboxOff() {
        for (int i = 0; i < 136; i++) {
            upper.anchoredPosition = new Vector2(0, i);
            lower.anchoredPosition = new Vector2(0, -i);
            yield return new WaitForSeconds(0.01f);
        }
        upper.gameObject.SetActive(false);
        lower.gameObject.SetActive(false);

    }
}
