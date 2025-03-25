using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;

    public enum PlayerActionBind
    {
        KeyZ, KeyX, KeyC, Key1, Key2, Key3, Key4, Key5, None

    }
    private InputManager InInstance;

    #region Action Def
    public Dictionary<string, Action> StartActions;
    public Dictionary<string, Action<float>> PerformActions;
    public Dictionary<string, Action> CancelActions;
    public Action SkillOpenAction;
    public Action SkillCloseAction;
    public Vector2 inputVec;
    #endregion

    #region Current Player State
    bool isMetamorp;
    bool isSkillOpen;
    public AtkType atkType;
    enumFormType CurrentForm;
    public bool PlayerInvincible;
    public bool PlayerAttackCheck;


    #endregion

    #region Physics
    public float maxSpeed;
    [SerializeField] private float MaxSpeedFactor = 2.5f;
    public float crawlSpeed;
    #endregion


    #region states
    public bool nearRope;
    public bool allowUnderJump;
    public bool onInteract;

    public bool isRope;
    public bool isWater;
    public bool onMovingPlat;
    #endregion

    public Vector3 rayPos;
    public float rayLen;

    // Jump Status
    public float jump;
    private bool isGround;

    #region Component Params
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator anim;
    [SerializeField] private FixedJoint2D fixJoint;
    [SerializeField] private Collider2D coll;

    public bool playerDirection { get { return sprite.flipX; } }
    #endregion

    // [SerializeField] SkillScript TestSkill;

    #region Transform Instances
    public Transform moveable;
    public Transform upperRope;
    public Transform lowerRope;

    #endregion

    #region enum defined
    // TODO: FormChange Action have to updated.


    // TODO: State not defined yet.
    public enum PlayerState
    {
        Idle,
        Jump,
    }
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        fixJoint = GetComponent<FixedJoint2D>();

        InInstance = GameManager.Instance.GetInputController();
        CurrentForm = enumFormType.None;
        ActionIntialize();





        // Test
        // Equip(PlayerActionBind.KeyZ, TestSkill);
    }

    private void FixedUpdate()
    {

        findRope();

        // LockFriction();

        //player movement
        Vector2 nextVec = Vector2.zero;
        if (inputVec != Vector2.zero)
        {
            nextVec = inputVec * maxSpeed;
            rigid.AddForce(Vector2.right * nextVec, ForceMode2D.Impulse);
        }

        //temp
        if (isRope)
        {
            if (nextVec.y > 0)
                transform.position = Vector3.MoveTowards(transform.position, upperRope.position, maxSpeed * Time.deltaTime);

            if (nextVec.y < 0)
            {// move lower
                if (lowerRope == null)
                {
                    fixJoint.connectedBody = null;
                    fixJoint.enabled = false;
                    isRope = false;
                }
                transform.position = Vector3.MoveTowards(transform.position, lowerRope.position, maxSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, upperRope.position) < 0.01f && upperRope.GetComponent<FixedJoint2D>() != null)
            {
                fixJoint.connectedBody = upperRope.GetComponent<Rigidbody2D>();
            }
            if (Vector3.Distance(transform.position, lowerRope.position) < 0.01f && lowerRope != null)
            {
                fixJoint.connectedBody = lowerRope.GetComponent<Rigidbody2D>();
            }

        }

        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed * crawlSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1) * crawlSpeed, rigid.velocity.y);
        }

        //player move anim
        bool move = (nextVec.x != 0) ? true : false;
        anim.SetBool("isMove", move);

        //jump to under check
        rayPos = transform.position - new Vector3(0, 2.5f, 0); //range to enable
        RaycastHit2D hit = Physics2D.Raycast(rayPos, transform.up * -1, rayLen);
        Debug.DrawRay(rayPos, transform.up * -1 * rayLen, Color.yellow); //visualize

        //moveable object hold
        if (moveable != null)
        {

            //object direction
            if (sprite.flipX)
                moveable.transform.position = transform.position + new Vector3(-1f, 0.3f, 0);
            else
                moveable.transform.position = transform.position + new Vector3(1f, 0.3f, 0);

            moveable.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }

        if (hit.collider == null)
        {
            allowUnderJump = true;

        }

        else if (hit.collider.gameObject.layer == 9 && allowUnderJump == true)
        {
            allowUnderJump = false;
            this.gameObject.layer = 6;
        }
    }

    #region Actions
    // TODO In Future,Saved File bind this Function.
    private void ActionIntialize()
    {
        StartActions = new Dictionary<string, Action>();
        PerformActions = new Dictionary<string, Action<float>>();
        CancelActions = new Dictionary<string, Action>();
        SkillOpenAction = new Action(() => { });
        SkillCloseAction = new Action(() => { });


        foreach (string act in System.Enum.GetNames(typeof(PlayerActionBind)))
        {
            if (act == PlayerActionBind.None.ToString()) continue;
            StartActions[act] = new Action(() => { });
            PerformActions[act] = new Action<float>((data) => { });
            CancelActions[act] = new Action(() => { });
        }

        InInstance.PlayerMove += OnPlayerMove;
        InInstance.PlayerJump += OnPlayerJump;
        InInstance.PlayerDash += OnPlayerDash;

        SkillOpenAction = () => { isSkillOpen = true; OnSkillOpen(); };
        SkillCloseAction = () => { isSkillOpen = false; OnSkillClose(); };
        InInstance.PlayerSkillStart += SkillOpenAction;
        InInstance.PlayerSkillCancel += SkillCloseAction;

        //InInstance.BtnNum1 += () => { OnMetamorp(PlayerFormBind.Key1); };
        //InInstance.BtnNum2 += () => { OnMetamorp(PlayerFormBind.Key2); };
        //InInstance.BtnNum3 += () => { OnMetamorp(PlayerFormBind.Key3); };
        //InInstance.BtnNum4 += () => { OnMetamorp(PlayerFormBind.Key4); };
        //InInstance.BtnNum5 += () => { OnMetamorp(PlayerFormBind.Key5); };

    }

    private void RegistryAction(PlayerActionBind _bind)
    {
        string act = _bind.ToString();
        //switch (_bind)
        //{
        //    case PlayerActionBind.KeyZ:
        //        InInstance.BtnZs += StartActions[act];
        //        InInstance.BtnZp += PerformActions[act];
        //        InInstance.BtnZc += CancelActions[act];
        //        break;
        //    case PlayerActionBind.KeyX:
        //        InInstance.BtnXs += StartActions[act];
        //        InInstance.BtnXp += PerformActions[act];
        //        InInstance.BtnXc += CancelActions[act];
        //        break;
        //    case PlayerActionBind.KeyC:
        //        InInstance.BtnCs += StartActions[act];
        //        InInstance.BtnCp += PerformActions[act];
        //        InInstance.BtnCc += CancelActions[act];
        //        break;
        //    case PlayerActionBind.KeyA:
        //        InInstance.BtnAs += StartActions[act];
        //        InInstance.BtnAp += PerformActions[act];
        //        InInstance.BtnAc += CancelActions[act];
        //        break;
        //    case PlayerActionBind.KeyS:
        //        InInstance.BtnSs += StartActions[act];
        //        InInstance.BtnSp += PerformActions[act];
        //        InInstance.BtnSc += CancelActions[act];
        //        break;
        //    default:
        //        break;
        //}
    }

    private void RemoveAction(PlayerActionBind _bind)
    {
        string act = _bind.ToString();
        //switch (_bind)
        //{
        //    case PlayerActionBind.KeyZ:
        //        InInstance.BtnZs -= StartActions[act];
        //        InInstance.BtnZp -= PerformActions[act];
        //        InInstance.BtnZc -= CancelActions[act];
        //        break;
        //    case PlayerActionBind.KeyX:
        //        InInstance.BtnXs -= StartActions[act];
        //        InInstance.BtnXp -= PerformActions[act];
        //        InInstance.BtnXc -= CancelActions[act];
        //        break;
        //    case PlayerActionBind.KeyC:
        //        InInstance.BtnCs -= StartActions[act];
        //        InInstance.BtnCp -= PerformActions[act];
        //        InInstance.BtnCc -= CancelActions[act];
        //        break;
        //    case PlayerActionBind.KeyA:
        //        InInstance.BtnAs -= StartActions[act];
        //        InInstance.BtnAp -= PerformActions[act];
        //        InInstance.BtnAc -= CancelActions[act];
        //        break;
        //    case PlayerActionBind.KeyS:
        //        InInstance.BtnSs -= StartActions[act];
        //        InInstance.BtnSp -= PerformActions[act];
        //        InInstance.BtnSc -= CancelActions[act];
        //        break;
        //    default:
        //        break;
        //}
    }

    public void OnSkillOpen()
    {
        //InInstance.PlayerMove -= OnPlayerMove;

        //RemoveAction(PlayerActionBind.KeyZ);
        //RemoveAction(PlayerActionBind.KeyX);
        //RemoveAction(PlayerActionBind.KeyC);

        //SkillPanel.instance.gameObject.SetActive(true);
        //InInstance.PlayerMove += SkillPanel.instance.RollSelectCursor;
        //InInstance.BtnZs += SkillPanel.instance.KeyAction[SkillKeybinding.KeyZ];
        //InInstance.BtnXs += SkillPanel.instance.KeyAction[SkillKeybinding.KeyX];
        //InInstance.BtnCs += SkillPanel.instance.KeyAction[SkillKeybinding.KeyC];


    }

    public void OnSkillClose()
    {
        //InInstance.PlayerMove -= SkillPanel.instance.RollSelectCursor;
        //InInstance.BtnZs -= SkillPanel.instance.KeyAction[SkillKeybinding.KeyZ];
        //InInstance.BtnXs -= SkillPanel.instance.KeyAction[SkillKeybinding.KeyX];
        //InInstance.BtnCs -= SkillPanel.instance.KeyAction[SkillKeybinding.KeyC];

        //SkillPanel.instance.gameObject.SetActive(false);
        //InInstance.PlayerMove += OnPlayerMove;

        //RegistryAction(PlayerActionBind.KeyZ);
        //RegistryAction(PlayerActionBind.KeyX);
        //RegistryAction(PlayerActionBind.KeyC);
    }

    public void OnMetamorp()
    {
        //if (PlayerManager.Instance.FP != PlayerManager.Instance.maxFP) return;

        //RemoveAction(PlayerActionBind.KeyA);
        //RemoveAction(PlayerActionBind.KeyS);


        //FormInfo _meta = FormManager.instance.FormMetamorp(_bind);
        //sprite.sprite = _meta.figure;
        //CurrentForm = _meta.form;

        //RegistryAction(PlayerActionBind.KeyA);
        //RegistryAction(PlayerActionBind.KeyS);

    }

    public void OffMetamorp()
    {
        //RemoveAction(PlayerActionBind.KeyA);
        //RemoveAction(PlayerActionBind.KeyS);
        //FormManager.instance.FormRegress(CurrentForm);
        //RegistryAction(PlayerActionBind.KeyA);
        //RegistryAction(PlayerActionBind.KeyS);
    }

    public void OnPlayerMove(Vector2 _vec)
    {
        //player movement, input system
        inputVec = _vec;

        //change player sprite flipX
        playerXflip();

        //change player collider size
        playerCrawl();
    }

    public void OnPlayerJump()
    {
        OnJump();
    }

    public void OnPlayerDash()
    {
        OnDash();
    }
    #endregion

    void LockFriction()
    {
        //player position freeze
        if (inputVec.x == 0)
        {  //no input = freeze
            if (!onMovingPlat && !isRope)
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void playerXflip()
    {
        //player sprite flipX
        if (inputVec.x < 0)
        {
            // sprite.flipX = true;
            sprite.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (inputVec.x > 0)
        {
            // sprite.flipX = false;
            sprite.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void OnInteract()
    {
        switch (this.transform.GetComponent<Scanner>().nearestTarget.tag)
        {
            case "MassObj":
                if (this.transform.GetComponent<Scanner>().nearestTarget.GetComponent<DistanceJoint2D>().connectedBody == this.rigid)
                    this.transform.GetComponent<Scanner>().nearestTarget.GetComponent<DistanceJoint2D>().connectedBody = this.transform.GetComponent<Scanner>().nearestTarget.GetComponent<Rigidbody2D>();
                else
                    this.transform.GetComponent<Scanner>().nearestTarget.GetComponent<DistanceJoint2D>().connectedBody = this.rigid;
                break;



            default:
                if (moveable != null)
                { //drop down object
                    moveable.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                    moveable = null;

                }
                else
                {
                    if (this.transform.GetComponent<Scanner>().nearestTarget != null)
                    {   //if moveable object is near
                        moveable = this.transform.GetComponent<Scanner>().nearestTarget;
                    }
                    else
                    {
                        StartCoroutine(interDelay());
                    }
                }
                break;
        }
    }

    public void playerCrawl()
    {
        //check player's crawl
        if (inputVec.y <= -0.05f)
        {
            coll.offset = new Vector2(0.02f, -0.45f);
            coll.GetComponent<CapsuleCollider2D>().size = new Vector2(0.8f, 1.25f);
            anim.SetBool("isCrawl", true);
            crawlSpeed = 0.5f;
            jump = 3;
        }
        else
        {
            coll.offset = new Vector2(0.02f, -0.1f);
            coll.GetComponent<CapsuleCollider2D>().size = new Vector2(0.8f, 2.5f);
            anim.SetBool("isCrawl", false);
            crawlSpeed = 1f;
            jump = 12;
        }
    }


    private void OnJump()
    {
        fixJoint.connectedBody = null;
        fixJoint.enabled = false;
        isRope = false;

        //jump anim
        if (!anim.GetBool("isJump") || isWater)
        {
            anim.SetBool("isJump", true);
            rigid.velocity = Vector2.up * jump;
        }

        if (allowUnderJump && inputVec.y <= -0.05f)
        {
            this.gameObject.layer = 8;
        }
    }

    private void OnDash()
    {
        //player dash
        if (anim.GetBool("isMove") && !anim.GetBool("isDash"))
        {

            //change layer : PlayerGhost
            this.gameObject.layer = 7;

            anim.SetBool("isDash", true);
            maxSpeed = maxSpeed * MaxSpeedFactor;
            rigid.AddForce(Vector2.right * 1 * inputVec.x, ForceMode2D.Impulse);
            //rigid.AddForce(Vector2.right * 1 * inputVec.x, ForceMode.Impulse);
        }
    }

    public void OffDash()
    {
        maxSpeed = maxSpeed / MaxSpeedFactor;
        anim.SetBool("isDash", false);

        //change layer : Player
        this.gameObject.layer = 6;
    }

    public void Equip(PlayerActionBind _bind, SkillScript _skill)
    {
        _skill.Equip(_bind);
        RegistryAction(_bind);
    }


    #region Collider
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Tilemaps")
        {// player landing
            anim.SetBool("isJump", false);            //jump over
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Rope") && !isRope && 0.05f < Mathf.Abs(inputVec.y))
        {
            Rigidbody2D rig = collision.gameObject.GetComponent<Rigidbody2D>();
            fixJoint.enabled = true;
            fixJoint.connectedBody = rig;
            isRope = true;

            //player's posX
            Vector2 playerPosition = transform.position;
            playerPosition.x = rig.transform.position.x;
            transform.position = playerPosition;
        }

        if (collision.transform.CompareTag("Water"))
        {
            Debug.Log("water");
            isWater = true;
            rigid.drag = 5;
            //rigid.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Water"))
        {
            Debug.Log("water out");
            isWater = false;
            rigid.drag = 0.2f;
            //rigid.gravityScale = 3;
        }
    }

    #endregion

    IEnumerator interDelay()
    {
        onInteract = true;
        yield return new WaitForSeconds(0.5f);
        onInteract = false;
    }

    private void findRope()
    {
        if (isRope)
        { //find upper & lower
            upperRope = fixJoint.connectedBody.GetComponent<FixedJoint2D>().connectedBody.transform;
            foreach (GameObject rope in fixJoint.connectedBody.GetComponentInParent<Ropes>().ropeIndex)
            {
                if (rope == null)
                {
                    lowerRope = null;
                    break;
                }
                if (rope.GetComponent<FixedJoint2D>().connectedBody == fixJoint.connectedBody)
                {
                    lowerRope = rope.transform;
                    break;
                }
            }

        }
        else
        {
            upperRope = null;
            lowerRope = null;
        }
    }

}
