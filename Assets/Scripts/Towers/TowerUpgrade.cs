﻿public class TowerUpgrade
{

    
    public int Price { get; private set; }

    public int Damage { get; private set; }

    public float DebuffDuration { get; private set; }


    public float ProcChance { get; private set; }


    public float SlowingFactor { get; private set; }


    public float TickTime { get; private set; }

    public int SpecialDamage { get; private set; }

    public TowerUpgrade(int price, int damage,  float debuffduration, float procChance)
    {
        this.Damage = damage;
        this.Price = price;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
    }

    public TowerUpgrade(int price,int damage, float debuffduration, float procChance, float SlowingFactor)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
        this.SlowingFactor = SlowingFactor;
        this.Price = price;
    }

    public TowerUpgrade(int price,int damage, float debuffduration, float procChance, float tickTime, int specialDamage)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
        this.TickTime = tickTime;
        this.SpecialDamage = specialDamage;
        this.Price = price;
    }
    public TowerUpgrade()
    {
        
    }

    public TowerUpgrade(int price, int damage)
    {
        this.Price = price;
        this.Damage = damage;
       
    }
    public TowerUpgrade(int price)
    {
        this.Price = price;
    }

}
