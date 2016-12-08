using UnityEngine;


public class FireTower : Tower
{

    //[SerializeField]
    //private SpriteRenderer spriteRenderer;


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

    private void Start()
    {
    
        ElementType = Element.FIRE;

        //Sets up the upgrades
        Upgrades = new TowerUpgrade[]
            {
                new TowerUpgrade(500,10,5f,5,0.0f,10),
                new TowerUpgrade(1000,15,5f,5,0.0f,20),
            };


    }

    public override Debuff GetDebuff()
    {
        return new FireDebuff(TickDamage, TickTime, Target, DebuffDuration);
    }


    public override void Upgrade()
    {
        //Upgrades the tower
        this.tickTime -= NextUpgrade.TickTime;
        this.tickDamage += NextUpgrade.SpecialDamage;

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
        if (NextUpgrade != null) //If the next is avaliable
        {
            return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nTick damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Computer Scientist</b></size> ", base.GetStats(), TickTime, TickDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        //Returns the current upgrade
        return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2}\nTick damage: {3}", "<size=20><b>Computer Scientist</b></size> ", base.GetStats(), TickTime, TickDamage);
    }

}
