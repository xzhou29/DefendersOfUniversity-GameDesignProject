using System;


class TrashCan : Tower
{
    private void Start()
    {
        ElementType = Element.NONE;

        Upgrades = new TowerUpgrade[]
          {
                new TowerUpgrade(),
                new TowerUpgrade(),
          };

    }

    public override Debuff GetDebuff()
    {
        return null;
    }

    public override string GetStats()
    {
        return String.Format("<color=#add8e6ff>{0}</color>{1}", "<size=20><b>Trash Can</b></size>", base.GetStats());
    }
}
