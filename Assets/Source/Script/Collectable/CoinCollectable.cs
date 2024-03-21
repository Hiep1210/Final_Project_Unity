using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectable : Collectable
{
    protected override void HandlerEvent(Player player)
    {
        GameManager.Ins.Coin += m_valueRandom;
    }
}
