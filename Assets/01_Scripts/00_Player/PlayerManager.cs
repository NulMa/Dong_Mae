using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private Playrer player;

    public Transform parentObj;

    [SerializeField] Image HPbar;
    [SerializeField] Image[] SPbar;
    [SerializeField] Image MPbar;

    public float MaxHP;
    public float curHP;
    public float CurHP { get { return curHP; } set { curHP = Mathf.Clamp(value, 0f, MaxHP); HPGaugeUpdate();  if (curHP == 0f) DeadAction(); } }

    public float MaxMP { get; set; }
    public float curMP;
    public float CurMP { get { return curMP; } set { curMP = Mathf.Clamp(value, 0f, MaxMP); MPGaugeUpdate(); } }

    public float MaxSP { get; set; }
    public float curSP;
    public float CurSP { get { return curSP; } set { curSP = Mathf.Clamp(value, 0f, MaxSP); SPGaugeUpdate(); } }



    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    private void Start()
    {
        player = Playrer.Instance;

        //TODO : Temporary.
        MaxHP = 100f;
        MaxMP = 100f;
        MaxSP = 100f;

        CurHP = MaxHP;
        CurMP = MaxMP;
        CurSP = MaxSP;

    }

    private void Update()
    {

    }

    public void HPGaugeUpdate()
    {
        float fill = CurHP / MaxHP;
        HPbar.fillAmount = fill;
    }

    public void MPGaugeUpdate()
    {
        float fill = CurMP / MaxMP;
        MPbar.fillAmount = fill;
    }


    public void SPGaugeUpdate()
    {
        float term = MaxSP * 0.2f;
        int numb = (int)(CurSP / term);
        float remain = CurSP % term;

        //Debug.Log(numb);
        //Debug.Log(remain);

        for (int i = 0; i < numb; i++)
        {
            SPbar[i].fillAmount = 1f;
        }

        if (numb != 5)
        {
            if (numb != 0)
            {
                for (int i = numb - 1; i < 5; i++)
                {
                    SPbar[i].fillAmount = 0f;
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    SPbar[i].fillAmount = 0f;
                }
            }



            if (numb != 0)
            {
                SPbar[numb - 1].fillAmount = remain / term;
            }

        }

    }

    public void DeadAction()
    { 
        
    }
}