using System;


class StormTower : Tower
{
    private void Start()
    {
        ElementType = Element.STORM;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(300,8,0,5),
            new TowerUpgrade(600,16,1,5),
        };
    }

    public override Debuff GetDebuff()
    {
        return new StormDebuff(Target, DebuffDuration);
    }

    public override string GetStats()
    {
        return String.Format("<color=#add8e6ff>{0}</color>{1}", "<size=20><b>Operating System</b></size>", base.GetStats());
    }
}
