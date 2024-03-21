using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    private PlayerStat m_playerStat;

    private StateMachine<StateAnimtorPlayer> m_playerFSM;

    private StateAnimtorPlayer m_previewState;

    private bool m_isRoll;
    private float m_currentTimeResetActionRoll;
    private float m_maxHp;

    public bool IsRoll { get => m_isRoll; }
    public float CurrentTimeResetActionRoll { get => m_currentTimeResetActionRoll; }
    public float MaxHp { get => m_maxHp; set => m_maxHp = value; }
    

    protected override void Awake()
    {
        base.Awake();

        if (actorStat != null)
        {
            m_playerStat = actorStat as PlayerStat;
        }

        InitPlayerFSM();

        MaxHp = m_playerStat.Hp;
        m_currentHp = MaxHp;
    }

    private void Update()
    {
        ReduceTimeAction(ref m_isRoll, ref m_currentTimeResetActionRoll, m_playerStat.RollRateTime);

        Debug.Log("Current Hp Player: " + m_currentHp);

    }

    public void Moving(Direction direction)
    {
        FlipFollowDirection(direction);

        if (direction == Direction.Left || direction == Direction.Right)
        {
            m_hozDir = direction == Direction.Left ? -1 : 1;
            m_rb.velocity = new Vector2(m_hozDir * m_currentSpeed, m_rb.velocity.y);
        }

        if (direction == Direction.Up || direction == Direction.Down)
        {
            m_vertDir = direction == Direction.Up ? 1 : -1;
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_currentSpeed * m_vertDir);
        }
    }

    public void MoveHozChecking()
    {
        if (GamepadController.Ins.CanMoveLeft)
        {
            Moving(Direction.Left);
        }

        if (GamepadController.Ins.CanMoveRight)
        {
            Moving(Direction.Right);
        }
    }

    public void MoveVerChecking()
    {
        if (GamepadController.Ins.CanMoveUp)
        {
            Moving(Direction.Up);
        }

        if (GamepadController.Ins.CanMoveDown)
        {
            Moving(Direction.Down);
        }
    }

    protected override void IsDead()
    {
        base.IsDead();

        ChangeState(StateAnimtorPlayer.Death);
    }


    #region FSM - Player

    private void ActiveState()
    {

    }

    private void InitPlayerFSM()
    {
        //FSM_MethodGen.GenAllMethodState<StateAnimtorPlayer>();

        m_playerFSM = StateMachine<StateAnimtorPlayer>.Initialize(this);

        m_playerFSM.ChangeState(StateAnimtorPlayer.Idle);
    }


    private void ChangeState(StateAnimtorPlayer newStateAnimatorPlayer)
    {
        m_previewState = m_playerFSM.State;
        m_playerFSM.ChangeState(newStateAnimatorPlayer);
    }

    void Idle_Enter()
    {
        m_currentSpeed = m_playerStat.MoveSpeed;
        m_rb.velocity = Vector3.zero;
    }
    void Idle_Update()
    {
        Helper.PlayAnim(animator, StateAnimtorPlayer.Idle.ToString());

        if (GamepadController.Ins.CanMoveLeft || GamepadController.Ins.CanMoveRight)
        {
            MoveHozChecking();
            ChangeState(StateAnimtorPlayer.Walk);
        }

        if (GamepadController.Ins.CanMoveUp || GamepadController.Ins.CanMoveDown)
        {
            MoveVerChecking();
            ChangeState(StateAnimtorPlayer.Walk);
        }

        if (GamepadController.Ins.IsRoll && !m_isRoll)
        {
            ChangeState(StateAnimtorPlayer.Roll);
        }
    }
    void Idle_Exit() { }

    void Walk_Enter()
    {
        m_currentSpeed = m_playerStat.MoveSpeed;
    }
    void Walk_Update()
    {
        Helper.PlayAnim(animator, StateAnimtorPlayer.Walk.ToString());

        MoveHozChecking();
        MoveVerChecking();

        if (GamepadController.Ins.IsStatic)
        {
            ChangeState(StateAnimtorPlayer.Idle);
        }

        if (GamepadController.Ins.IsRoll && !m_isRoll)
        {
            ChangeState(StateAnimtorPlayer.Roll);
        }
    }
    void Walk_Exit() { }

    void Roll_Enter()
    {
        m_currentSpeed = m_playerStat.RollSpeed;
    }
    void Roll_Update()
    {
        Helper.PlayAnim(animator, StateAnimtorPlayer.Roll.ToString());

        m_isRoll = true;

        MoveHozChecking();
        MoveVerChecking();

        if (GamepadController.Ins.IsStatic)
        {
            ChangeState(StateAnimtorPlayer.Idle);
        }
    }
    void Roll_Exit() { }

    void Scour_Enter()
    {
    }

    void Scour_Update()
    {
        Helper.PlayAnim(animator, StateAnimtorPlayer.Scour.ToString());
    }
    void Scour_Exit() { }

    void Death_Enter() { }
    void Death_Update()
    {
        Helper.PlayAnim(animator, StateAnimtorPlayer.Death.ToString());
        if (deadVfx != null)
        {
            GameObject deadVfxClone = GameObject.Instantiate(deadVfx, transform.position, Quaternion.identity);
            Destroy(deadVfxClone, 0.15f);
        }
        gameObject.SetActive(false);
    }
    void Death_Exit() { }

    #endregion
}
