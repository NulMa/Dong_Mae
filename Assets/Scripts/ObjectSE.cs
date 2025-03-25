using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSE : MonoBehaviour{

    Collider2D coll;
    AudioSource audio;
    public AudioClip[] audios;
    


    // Start is called before the first frame update
    void Start(){
        audio = GetComponent<AudioSource>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.transform.tag == "Player") {
            playSE();
        }
    }


    public void playSE() {
        audio.clip = audios[0];
        audio.Play();
    }
}
