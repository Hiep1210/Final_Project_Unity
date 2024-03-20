using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : Singleton<GamepadController>
{
    private Player m_player;

    private bool m_isPutLamp;
    private bool m_canMoveLeft;
    private bool m_canMoveRight;
    private bool m_canMoveUp;
    private bool m_canMoveDown;
    private bool m_isRoll;
    private bool m_isScour;
    private bool m_isStatic;
    private bool m_isFreeBullet;

    private bool m_isPistolGun;
    private bool m_isMachineGun;
    private bool m_isShotGun;

    public bool IsPutLamp { get => m_isPutLamp; }
    public bool CanMoveLeft { get => m_canMoveLeft; }
    public bool CanMoveRight { get => m_canMoveRight; }
    public bool CanMoveUp { get => m_canMoveUp; }
    public bool CanMoveDown { get => m_canMoveDown; }
    public bool IsStatic
    {
        get => m_isStatic;
    }
    public bool IsRoll { get => m_isRoll; }
    public bool IsScour { get => m_isScour; }

    public bool IsFreeBullet { get => m_isFreeBullet; }

    public bool IsPistolGun { get => m_isPistolGun; }
    public bool IsMachineGun { get => m_isMachineGun; }
    public bool IsShotGun { get => m_isShotGun; }

    public override void Awake()
    {
        MakeSingleton(false);
    }

    private void Start()
    {
        m_player = GameObject.FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        if (m_player && !m_player.IsDeadActor)
        {
            m_isPutLamp = Input.GetMouseButtonDown(2) ? true : false;

            float hozCheck = Input.GetAxisRaw("Horizontal");
            float vertCheck = Input.GetAxisRaw("Vertical");

            m_canMoveLeft = hozCheck < 0 ? true : false;
            m_canMoveRight = hozCheck > 0 ? true : false;
            m_canMoveDown = vertCheck < 0 ? true : false;
            m_canMoveUp = vertCheck > 0 ? true : false;

            m_isRoll = Input.GetKey(KeyCode.Space);

            m_isFreeBullet = Input.GetMouseButtonDown(0);

            if (!m_canMoveLeft && !m_canMoveRight && !m_canMoveDown && !m_canMoveUp && !m_isRoll && !m_isScour)
            {
                m_isStatic = true;
            }
            else
            {
                m_isStatic = false;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                m_isPistolGun = true;
                m_isMachineGun = false;
                m_isShotGun = false;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                m_isMachineGun = true;
                m_isPistolGun = false;
                m_isShotGun = false;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                m_isShotGun = true;
                m_isPistolGun = false;
                m_isMachineGun = false;
            }
        }
    }

}
