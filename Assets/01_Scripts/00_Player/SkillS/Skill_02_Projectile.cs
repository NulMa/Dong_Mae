using UnityEngine;

public class Skill_02_Projectile : MonoBehaviour
{
    Animator animCtrl;
    Animation animClip;
    

    private void Start()
    {
        animCtrl = GetComponent<Animator>();
        float randPosY = Random.Range(0.0f, 2.0f);

        transform.position = new Vector3(PlayerScript.Instance.transform.position.x, PlayerScript.Instance.transform.position.y + randPosY, PlayerScript.Instance.transform.position.z);

    }

    private void Update()
    {
        if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}