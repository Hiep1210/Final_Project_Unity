using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Actor
{
    [Header("Smooth Scroll: ")]
    [Range(0,10f)]
    public float fallScrollMultipiler;
    [Range(0, 10f)]
    public float lowScrollMultipiler;
    public GameObject lamp;
    public int numberOfLamp = 5;
    private List<GameObject> list = new List<GameObject>();

    private GamepadController m_gPad;
    private PlayerStat m_playerStat;
    private StateMachine<PlayerStateAnimator> m_fsm;
    private PlayerStateAnimator m_previewState;

    private bool m_isScroll;
    private float m_isScrollTime;


    
    protected override void Awake()
    {
        base.Awake();
        if (actorStat != null)
        {
            m_playerStat = actorStat as PlayerStat;
        }

        m_currentmoveSpeed = m_playerStat.moveSpeed;
        m_isScroll = false;
        InitStateFSM();
    }

    // Start is called before the first frame update
    void Start()
    {
        while(numberOfLamp > 0)
        {
            GameObject lam = Instantiate(lamp);
            list.Add(lam);
            lam.SetActive(false);
            numberOfLamp--;
        }
        m_gPad = GamepadController.Ins;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead) return;
        ReduceActionTime(ref m_isScroll,ref m_isScrollTime, m_playerStat.scrollTimeRate);
    }

    private void FixedUpdate()
    {
        SmoothScroll();
    }

    private void LampChecking()
    {
        if (m_gPad.IsPutLamp)
        {
            PutLamp();
        }
    }

    private void PutLamp()
    {
        foreach (GameObject lamp in list)
        {
            if(lamp.activeSelf == false)
            {
                lamp.SetActive(true);
                lamp.transform.position = this.transform.position;
                return;
            }
        }
    }

    private void MoveChecking()
    {
        if (m_gPad.IsStatic)
        {
            m_rb.velocity = Vector2.zero;
            return;
        }

        if (m_gPad.CanMoveLeft)
        {
            Moving(Direction.Left);
        }

        if (m_gPad.CanMoveRight)
        {
            Moving(Direction.Right);
        }

        if (m_gPad.CanMoveUp)
        {
            Moving(Direction.Up);
        }

        if (m_gPad.CanMoveDown)
        {
            Moving(Direction.Down);
        }
    }

    private void Moving(Direction direction)
    {
        if (direction != Direction.Up && direction != Direction.Down)
        {
            FlipCharacters(direction);
        }

        if (direction == Direction.Left || direction == Direction.Right)
        {
            m_horizontal = direction == Direction.Left ? -1 : 1;
            m_rb.velocity = new Vector2(m_horizontal * m_currentmoveSpeed, m_rb.velocity.y);
        }

        if (direction == Direction.Up || direction == Direction.Down)
        {
            m_vertical = direction == Direction.Down ? -1 : 1;
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_vertical * m_currentmoveSpeed);
        }
    }

    private void Scroll()
    {
        float dir = spriteRenderer.transform.localScale.x > 0 ? 1 : -1;
        m_rb.velocity = new Vector2(dir * m_currentmoveSpeed, m_rb.velocity.y);
    }

    private void SmoothScroll()
    {
        if (m_fsm.State == PlayerStateAnimator.Scroll)
        {
            Vector2 dir = spriteRenderer.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            if (m_rb.velocity.y < 0)
            {
                m_rb.velocity += dir * Physics2D.gravity.x * (1 - fallScrollMultipiler) * Time.deltaTime;
            }

            if (m_rb.velocity.x > 0 && !m_gPad.IsScrollHolding)
            {
                m_rb.velocity += dir * Physics2D.gravity.x * (1 - lowScrollMultipiler) * Time.deltaTime;
            }
        }
    }


    #region FSM - Player
    protected virtual void InitStateFSM()
    {
        m_fsm = StateMachine<PlayerStateAnimator>.Initialize(this);
        m_fsm.ChangeState(PlayerStateAnimator.Idle);
    }

    private void ChangeState(PlayerStateAnimator newStateChange)
    {
        m_previewState = m_fsm.State;
        m_fsm.ChangeState(newStateChange);
    }

    private void InvokeChangeStateCo(PlayerStateAnimator newStateChange, float extraTime = 0f)
    {
        StartCoroutine(ChangeStateCo(newStateChange, extraTime));
    }

    private IEnumerator ChangeStateCo(PlayerStateAnimator newStateChange, float extraTime = 0f)
    {
        AnimationClip animationClip = Helper.GetClip(animator, newStateChange.ToString());
        if (animationClip != null)
        {
            yield return new WaitForSeconds(animationClip.length + extraTime);
        }
        ChangeState(newStateChange);
    }

    void Idle_Enter()
    {
        m_currentmoveSpeed = m_playerStat.moveSpeed;
        m_rb.velocity = Vector2.zero;
    }
    void Idle_Update()
    {
        Helper.PlayAnim(animator, PlayerStateAnimator.Idle.ToString());
        if (m_gPad.CanMoveLeft || m_gPad.CanMoveRight || m_gPad.CanMoveUp || m_gPad.CanMoveDown)
        {
            ChangeState(PlayerStateAnimator.Walk);
        }

        if (m_gPad.CanScroll && m_gPad && !m_isScroll)
        {
            ChangeState(PlayerStateAnimator.Scroll);
        }
    }
    void Idle_Exit() { }
    void Walk_Enter()
    {
        m_currentmoveSpeed = m_playerStat.moveSpeed;
    }
    void Walk_Update()
    {
        Helper.PlayAnim(animator, PlayerStateAnimator.Walk.ToString());

        MoveChecking();

        LampChecking();

        if (m_gPad.IsStatic)
        {
            ChangeState(PlayerStateAnimator.Idle);
        }


        if (m_gPad.CanScroll && m_gPad && !m_isScroll)
        {
            ChangeState(PlayerStateAnimator.Scroll);
        }
    }
    void Walk_Exit() { }
    void Run_Enter() { }
    void Run_Update() { }
    void Run_Exit() { }
    void Scroll_Enter() 
    {
        m_currentmoveSpeed = m_playerStat.speedScroll;
        m_isScroll = true;
        Scroll();
        if (m_gPad.IsStatic)
        {
            InvokeChangeStateCo(PlayerStateAnimator.Idle);
        }

        if (m_gPad.CanMoveLeft || m_gPad.CanMoveRight || m_gPad.CanMoveUp || m_gPad.CanMoveDown)
        {
            InvokeChangeStateCo(PlayerStateAnimator.Walk);
        }
    }
    void Scroll_Update() 
    {
        Helper.PlayAnim(animator, PlayerStateAnimator.Scroll.ToString());
    }
    void Scroll_Exit() { }
    void SwordAttack_Enter() { }
    void SwordAttack_Update() { }
    void SwordAttack_Exit() { }
    void FireBullet_Enter() { }
    void FireBullet_Update() { }
    void FireBullet_Exit() { }
    void Dead_Enter() { }
    void Dead_Update() { }
    void Dead_Exit() { }

    #endregion

}
