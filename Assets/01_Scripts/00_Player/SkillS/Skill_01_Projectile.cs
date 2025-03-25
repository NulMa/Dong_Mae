using Cinemachine.Examples;
using UnityEngine;

public class Skill_01_Projectile : MonoBehaviour
{
    Animator animCtrl;
    Animation animClip;
    bool collActive;
    Collider2D coll;
    Rigidbody2D rigid;


    private void Start()
    {
        animCtrl = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {

        if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && collActive == false)
        {
            collActive = true;
            coll.enabled = true;
        }
        else if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}