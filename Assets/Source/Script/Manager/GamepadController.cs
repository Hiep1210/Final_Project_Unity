using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : Singleton<GamepadController>
{
    private bool m_isOnMobile;
    private bool m_isFireBullet;
    private bool m_isPutLamp;
    private bool m_isSwordAttack;
    private bool m_canMoveLeft;
    private bool m_canMoveRight;
    private bool m_canMoveUp;
    private bool m_canMoveDown;
    private bool m_isStatic;


    public float scrollHoldTime;
    private bool m_canScroll;
    private bool m_canCheckScroll;
    private float m_currentScrollHoldingTime;
    private bool m_isScrollHolding;


    public bool IsFireBulletAttack { get => m_isFireBullet; }
    public bool IsPutLamp {  get => m_isPutLamp; }
    public bool IsSwordAttack { get => m_isSwordAttack; set => m_isSwordAttack = value; }
    public bool CanMoveLeft { get => m_canMoveLeft; }
    public bool CanMoveRight { get => m_canMoveRight; }
    public bool CanMoveUp { get => m_canMoveUp; }
    public bool CanMoveDown { get => m_canMoveDown; }

    public bool CanScroll { get => m_canScroll; }
    public bool IsScrollHolding { get => m_isScrollHolding; set => m_isScrollHolding = value; }
    public bool IsStatic
    {
        get => m_isStatic = !m_canMoveLeft && !m_canMoveRight && !m_canMoveUp && !m_canMoveDown;
    }

    public override void Awake()
    {
        MakeSingleton(false);
    }

    private void Update()
    {
        if (!m_isOnMobile)
        {
            float hozCheck = Input.GetAxisRaw("Horizontal");
            float vertCheck = Input.GetAxisRaw("Vertical");

            m_canMoveLeft = hozCheck < 0 ? true : false;
            m_canMoveRight = hozCheck > 0 ? true : false;
            m_canMoveDown = vertCheck < 0 ? true : false;
            m_canMoveUp = vertCheck > 0 ? true : false;

            m_isFireBullet = Input.GetMouseButtonDown(0) ? true : false;
            m_isSwordAttack = Input.GetMouseButtonDown(1) ? true : false;
            m_canScroll = Input.GetKeyDown(KeyCode.Space);
            m_isPutLamp = Input.GetKeyDown(KeyCode.L) ? true : false;

            if (m_canScroll)
            {
                m_canCheckScroll = true;
                m_currentScrollHoldingTime = 0;
            }

            if (m_canCheckScroll)
            {
                m_currentScrollHoldingTime += Time.deltaTime;
                if (m_currentScrollHoldingTime > scrollHoldTime)
                {
                    m_isScrollHolding = Input.GetKey(KeyCode.Space);
                    if (!m_isScrollHolding) m_currentScrollHoldingTime = 0f;
                }
            }

        }
    }

}
