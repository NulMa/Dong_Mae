using System.Collections;
using UnityEngine;


public class PlayerSkill02Effect : ProjectileScript
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
            Debug.Log("Heal about 20");
            // PlayerManager.Instance.HP = 20f;
            PlayerManager.Instance.CurHP += 20f;

            Destroy(gameObject);
        }
    }
}