using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightScale : MonoBehaviour
{
    public float scaleLoad;
    public float targetLoad;
    public bool isClear;
    public GameObject operatingPart;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update(){
        if(scaleLoad >= targetLoad && !isClear) {
            Operating();
            isClear = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "MassObj") {
            scaleLoad += collision.GetComponent<ObjectWithMass>().mass;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "MassObj") {
            scaleLoad -= collision.GetComponent<ObjectWithMass>().mass;
        }
    }

    public void Operating() {
        switch (operatingPart.tag) {
            case "Gate":
                operatingPart.GetComponent<Gate>().otherOpen();
                break;
        }   
    }
}
