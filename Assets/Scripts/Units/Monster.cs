using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Monster : Unit
{
 
    [SerializeField]
    private Stat monsterHealth;

    [SerializeField]
    private Element elementType;

    [SerializeField]
    private bool isBoss;

    [SerializeField]
    private bool isHipster;

    [SerializeField]
    private bool isFrat;


    [SerializeField]
    private bool isLoner;

    public bool isInvisible = false;

    private LonerController loner;

    private HipsterController hipster;

    //private Renderer render;

    private SpriteRenderer render;

    private bool haveDied = false;

    private List<Debuff> debuffs = new List<Debuff>();

    public List<Debuff> newDebuffs = new List<Debuff>();

    public List<Debuff> DebuffsToRemove { get; private set; }

    private Vector3 currentPosition;

    private bool isRespawn = false;
    //private int invulnerability_1 = 1;
    //private int invulnerability_2 = 2;
    //private int invulnerability_3 = 3;
    //private int invulnerability_4 = 4;
    //private int invulnerability_5 = 5;

    private int waveHealth;

    public float MaxSpeed { get; private set; }

    public bool Alive
    {
        get
        {
            return monsterHealth.CurrentValue > 0;
        }
    }

    public Element ElementType
    {
        get
        {
            return elementType;
        }
    }


    protected override void Awake()
    {
        base.Awake();
        DebuffsToRemove = new List<Debuff>();
        MaxSpeed = Speed;
        monsterHealth.Initialize();
        hipster = GetComponent<HipsterController>();
        loner = GetComponent<LonerController>();
        render = GetComponent<SpriteRenderer>();
        //GetComponent<SpriteRenderer>().color = Color.white;
        haveDied = false;


    }
 
  
    private float time = 0.0f;
   // private float reSpawnWaitSeconds = 0.0f;

    protected override void Update()
    {
        if (isLoner)
        {
            isInvisible = loner.isInvisible;
        }
        HandleDebuffs();
        base.Update();

        time += Time.deltaTime;

        if (time >= 10.0f && Speed == 0)
        {
            Speed = 2;
            time = 0;
            //foreach (Debuff debuff in debuffs)
            //{
            //    debuff.Remove();
            //}
        }
         
     
    }
  

        
    public void AddDebuff(Debuff debuff)
    {

        if (!debuffs.Exists(x => x.GetType() == debuff.GetType()))
        {
            newDebuffs.Add(debuff);
        }
    }

    private void HandleDebuffs()
    {
        //If the monster has any new debuffs
        if (newDebuffs.Count > 0)
        {
            //Then we add them to the debuffs list
            debuffs.AddRange(newDebuffs);

            //Then clear new debuffs so that they only will be added once
            newDebuffs.Clear();
        }

        foreach (Debuff debuff in DebuffsToRemove)
        {
            debuffs.Remove(debuff);
        }
        DebuffsToRemove.Clear();

        foreach (Debuff debuff in debuffs) 
        {
            debuff.Update();
        }
    }


    public void TakeDamage(float damage, Element dmgSource) 
    {
        if (IsActive)//if the monster is in play

            //GameManager waveNum = GetComponent<GameManager>();

            //if ( waveNum.wave / 5 == 0 )

            //if (dmgSource == elementType)
            //{
            //    damage = 5 ;
            //     monsterHealth.CurrentValue -= damage;
            // }
            //else
            // {
            //  damage = damage / invulnerability;
            //invulnerability += 1;

            if (isHipster)
            {
                Debug.Log(hipster.cantBeHurt);

                if (hipster.cantBeHurt)
                {   
                    monsterHealth.CurrentValue -= damage;

                    if (dmgSource == Element.FROST)
                    {
                        GameManager.Instance.Currency += 1;
                    }
                } 
            }
            else
            {
                if (dmgSource == elementType)
                {
                    damage = damage - 1;

                    monsterHealth.CurrentValue -= damage;
                }

                else
                {
                    monsterHealth.CurrentValue -= damage;
                }

                if (dmgSource == Element.FROST)
                {
                    GameManager.Instance.Currency += 1;
                }
            }
           


        if (monsterHealth.CurrentValue <= 0)
            {
                currentPosition = transform.position;
            //if ((GameManager.Instance.waveCount == 4 || GameManager.Instance.waveCount == 9 || GameManager.Instance.waveCount == 14) && !haveDied)
            //if((GameManager.Instance.waveCount == 1 || GameManager.Instance.waveCount == 2 || GameManager.Instance.waveCount == 3) && !haveDied)
                if(isFrat && !haveDied)
                {
                Speed = 0;     
                isRespawn = true;
                haveDied = true;
                //SoundManager.Instance.PlaySFX("Splat");
                //GetComponent<SpriteRenderer>().color = Color.red;
                

            }

            else
            {
      
                //GetComponent<SpriteRenderer>().sortingOrder--;
                GameManager.Instance.Currency += 30;
                SoundManager.Instance.PlaySFX("Splat");  
                 
                myAnimator.SetTrigger("Die");


                Debug.Log("This enemy is dead");
                //render.color = Color.white;
                haveDied = false;
                IsActive = false;
                if(render.color == Color.black){
                    Debug.Log("Change the color");
                    //render.color = Color.black;
                }
                // GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        if (isRespawn)
        {         
            fratBoyRespawn(waveHealth);
        }
    }


    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {

        float progress = 0;
   
        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime * 1;
            yield return null;
        }
        
        transform.localScale = to;

           
        IsActive = true;

        if (remove)
        {
            Release();
        }

    }

    public void Spawn(int health)
    {
        this.monsterHealth.MaxVal = health;
        this.monsterHealth.CurrentValue = health;

        waveHealth = health;

        debuffs.Clear();

        transform.position = LevelManager.Instance.BluePortal.transform.position;

        StartCoroutine(Scale(new Vector3(0.1f, 0.1f,1.0f), new Vector3(1, 1, 1), false));

        SetPath(LevelManager.Instance.Path, false);
        render.color = Color.white;
        if (isLoner)
        {
            Debug.Log("IT IS LONER");
            loner.isInvisible = true;
        }

    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.name == "RedPortal")
        {
            IsActive = false; 
            other.GetComponent<Portal>().Animate();
            monsterHealth.CurrentValue = 0;

           StartCoroutine(Scale(new Vector3(1, 1, 1), new Vector3(0.1f, 0.1f, 1.0f), true));
            if(isBoss)
            {
                GameManager.Instance.Lives = 0;
            }
            else
            {
                GameManager.Instance.Lives--;
            }   
           // Destroy(gameObject);
        }
    }


    public void Release()
    {
        foreach (Debuff debuff in debuffs)
        {
            debuff.Remove();
        }

        GridPosition = new Point(0, 4);

        Speed = 2;

        GameManager.Instance.RemoveMonster(this);

        GameManager.Instance.Pool.ReleaseObject(gameObject);
    }

    public void fratBoyRespawn(int health)
    {
        this.monsterHealth.MaxVal = waveHealth;
        this.monsterHealth.CurrentValue = waveHealth;
        
        Speed = MaxSpeed;
        debuffs.Clear();

        transform.position = currentPosition;

        StartCoroutine(Scale(new Vector3(0.1f, 0.1f, 1.0f), new Vector3(1, 1, 1), false));
        //GetComponent<SpriteRenderer>().color = Color.red;
        isRespawn = false;
        render.color = Color.black;

    }
}
