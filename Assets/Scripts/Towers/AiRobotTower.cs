using System;
using UnityEngine;


class AiRobotTower : Tower
{


    [SerializeField]
    private float tickTime;


    [SerializeField]
    private float tickDamage;


    public float TickDamage
    {
        get
        {
            return tickDamage;
        }
    }


    public float TickTime
    {
        get
        {
            return tickTime;
        }
    }
    public override Debuff GetDebuff()
    {
        return new FireDebuff(TickDamage, TickTime, Target, DebuffDuration);
    }
    private void Start()
    {
        ElementType = Element.FIRE;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(1000, 4),
            new TowerUpgrade(1500, 4),
        };
    }
    public override string GetStats()
    {
        return String.Format("<color=#add8e6ff>{0}</color>{1}", "<size=20><b>AI Robot</b></size>", base.GetStats());
    }
}
