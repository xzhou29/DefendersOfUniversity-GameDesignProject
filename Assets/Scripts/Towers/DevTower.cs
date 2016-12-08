using System;
using UnityEngine;

class DevTower : Tower
{ 
  
    public float tickTime;


    public float tickDamage;


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
        ElementType = Element.STORM;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(1500, 15),
            new TowerUpgrade(2000, 15),
        };
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (Level == 2)
        {
            TowerSprite(Color.green);
        }
        else if (Level == 3)
        {
            TowerSprite(Color.red);
        }

    }

    public void TowerSprite(Color color)
    {
        spriteRenderer.color = color;
    }

    public override string GetStats()
    {
        return String.Format("<color=#add8e6ff>{0}</color>{1}", "<size=20><b>Dev Tower</b></size>", base.GetStats());
    }


}
