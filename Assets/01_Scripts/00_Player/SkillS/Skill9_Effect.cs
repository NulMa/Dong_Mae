using System.Collections;
using UnityEngine;


public class Skill9_Effect: MonoBehaviour
{
    Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Heal", true);
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            //PlayerManager.Instance.HP = 20f;

            //Destroy(gameObject);
        }
    }
}