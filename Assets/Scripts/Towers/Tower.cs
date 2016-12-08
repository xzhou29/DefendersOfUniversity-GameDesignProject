using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public abstract class Tower : MonoBehaviour
{
    [SerializeField]
    private Projectile projectilePrefab;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float proc;

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float debuffDuration;

    public Sprite lvl1Sprite;

    public Sprite level2Sprite;

    public Sprite level3Sprite;

    public TowerUpgrade[] Upgrades { get; protected set; }

    private float attackTimer;

    private bool canAttack = true;

    public int Level { get; protected set; }

    private Animator myAnimator;

    private Queue<Monster> monsters = new Queue<Monster>();


    private SpriteRenderer mySpriteRenderer;

    //private SpriteRenderer spriteRender;

    //[SerializeField]
    public SpriteRenderer spriteRenderer;

    //private GameManager gameManager;

   // private HipsterController hipster;

    //private Monster thisMonster; // added Nov 14th

    private Monster target;

    public Element ElementType { get; protected set; }

    public int Price { get; set; }
    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public Monster Target
    {
        get
        {
            return target;
        }
    }

    public float Proc
    {
        get
        {
            return proc;
        }
    }

    public float ProjectileSpeed
    {
        get
        {
            return projectileSpeed;
        }
    }

    public TowerUpgrade NextUpgrade
    {
        get
        {
            if (Upgrades.Length > Level-1)
            {               
               
                return Upgrades[Level - 1];
            }
            return null;
        }
    }

    public float DebuffDuration
    {
        get
        {
            return debuffDuration;
        }

        set
        {
            this.debuffDuration = value;
        }
    }

    private void Awake()
    {
        //myAnimator = transform.parent.GetComponent<Animator>();

        mySpriteRenderer = transform.GetComponent<SpriteRenderer>();
        //gameManager = GetComponent<GameManager>();
        //hipster = GetComponent<HipsterController>();
        //thisMonster = GetComponent<Monster>();
        TowerSprite(lvl1Sprite);
        Level = 1;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
 
            Attack();     
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            monsters.Enqueue(other.GetComponent<Monster>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            target = null;
        }
    }

    private void Attack()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        if (Target != null && Target.IsActive && !Target.isInvisible)
        {
            if (canAttack)
            {
                Shoot();
               // myAnimator.SetTrigger("Attack");
                canAttack = false;
            }       
        }
        else if (monsters.Count > 0)
        {
            target = monsters.Dequeue();
        }
       
        if (target != null && !target.Alive)
        {
            target = null;
        }    
    }

    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        GameManager.Instance.UpdateTooltip();
    }


    public void Shoot()
    {
        if(ElementType != Element.NONE)
        {
            Projectile projectile = GameManager.Instance.Pool.GetObject(projectilePrefab.name).GetComponent<Projectile>();

            projectile.transform.position = transform.position;

            projectile.Initialize(this);
        }
        
    }


    /// Returns the towers current stats and upgraded stats
    public virtual string GetStats()
    {
        if (NextUpgrade != null)
        {
            //return null;
            return string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{4}</color>\nProc: {2}% <color=#00ff00ff>+{5}%</color>\nDebuff: {3}sec <color=#00ff00ff>+{6}</color>", Level, damage, proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
        }

       // return null;
        return string.Format("\nLevel: {0} \nDamage: {1}\nProc: {2}% \nDebuff: {3}sec", Level, damage, proc, DebuffDuration);

    }

    public virtual void Upgrade()
    {
        GameManager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.proc += NextUpgrade.ProcChance;
        this.DebuffDuration += NextUpgrade.DebuffDuration;

        Level++;
        GameManager.Instance.UpdateTooltip();

        if (this.ElementType == Element.FIRE && this.CompareTag("ArchitectureTower"))
        {
            ArchitectureTower archit = GetComponent<ArchitectureTower>();
            archit.rangeOfDetection();
            Debug.Log("Increase the range");
        }

        if (Level == 2)
        {
            TowerSprite(level2Sprite);
        }
        else if (Level == 3)
        {
            TowerSprite(level3Sprite);
        }

    }
    public void TowerSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public abstract Debuff GetDebuff();

}
