using System;
using UnityEngine;

public class BeakerTower : Tower
{

    [SerializeField]
    private float slowingFactor;

    public float SlowingFactor
    {
        get
        {
            return slowingFactor;
        }
    }

    private void Start()
    {

        ElementType = Element.FROST;

        Upgrades = new TowerUpgrade[]
            {
                new TowerUpgrade(200,3,1,10,50),
                new TowerUpgrade(500,3,1,10,70),
            };
    }

    
    /// <returns>A frost debuff</returns>
    public override Debuff GetDebuff()
    {
        return new FrostDebuff(SlowingFactor, DebuffDuration, Target);
    }

    public override void Upgrade()
    {
        //Upgrades the tower
        this.slowingFactor = NextUpgrade.SlowingFactor;
        base.Upgrade();
    }
    public override string GetStats()
    {
        if (NextUpgrade != null)  //If the next is avaliable
        {
            return String.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}% <color=#00ff00ff>+{3}</color>", "<size=20><b>Data Structure</b></size>", base.GetStats(), SlowingFactor, NextUpgrade.SlowingFactor);
        }

        //Returns the current upgrade
        return String.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}%", "<size=20><b>Data Structure</b></size>", base.GetStats(), SlowingFactor);
    }
}
