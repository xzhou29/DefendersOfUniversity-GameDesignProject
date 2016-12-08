using System;
using UnityEngine;


class ArchitectureTower : Tower
{
    public GameObject rangeSize;

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

        Upgrades = new TowerUpgrade[]
          {
                new TowerUpgrade(500, 5),
                new TowerUpgrade(750, 4),
          };

    }



    public override Debuff GetDebuff()
    {
        return new FireDebuff(TickDamage, TickTime, Target, DebuffDuration);
    }

    public void rangeOfDetection()
    {
        Debug.Log("here");

        rangeSize.gameObject.transform.localScale += new Vector3(1f, 1f, 0.0f);


    }

    public override string GetStats()
    {
        return String.Format("<color=#add8e6ff>{0}</color>{1}", "<size=20><b>Architecture Tower</b></size>", base.GetStats());
    }
}
