using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpExtraCollectable : Collectable
{
    protected override void HandlerEvent(Player player)
    {
        GameManager.Ins.Player.CurrentHp += maxValueCollectable;
        if(GameManager.Ins.Player.CurrentHp >= GameManager.Ins.Player.MaxHp)
        {
            GameManager.Ins.Player.CurrentHp = GameManager.Ins.Player.MaxHp;
        }
    }
}
