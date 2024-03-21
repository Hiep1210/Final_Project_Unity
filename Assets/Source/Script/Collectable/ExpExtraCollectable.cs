using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpExtraCollectable : Collectable
{
    protected override void HandlerEvent(Player player)
    {
        GameManager.Ins.CurExpressInLevels += m_valueRandom;
    }
}
