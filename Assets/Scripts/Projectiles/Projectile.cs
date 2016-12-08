using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Monster target;

   
    private Tower tower;


    private Animator myAnimator;


    private Element element;

 
    void Awake()
    {

        this.myAnimator = GetComponent<Animator>();
    }


    /// <param name="tower"></param>
    public void Initialize(Tower tower)
    {
        //Sets the values
        this.target = tower.Target;
        this.tower = tower;
        this.element = tower.ElementType;
    }


    void Update()
    {
        if (target != null && target.IsActive) //If the target isn't null and the target isn't dead
        {
            //Move towards the position
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * tower.ProjectileSpeed);

            //Calculates the direction of the projectile
            Vector2 dir = target.transform.position - transform.position;

            //Calculates the angle of the projectile
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //Sets the rotation based on the angle
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (!target.IsActive) //If the target is inactive then we don't need the projectile anymore
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
        else
        {
            target.IsActive = false;
        }
    }

    /// <param name="other">The object the projectil hit</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        //If we hit a monster
        if (other.tag == "Monster")
        {
            Monster target = other.GetComponent<Monster>();

            target.TakeDamage(tower.Damage, tower.ElementType);

            ApplyDebuff();

            myAnimator.SetTrigger("Impact");

        }

    }


    private void ApplyDebuff()
    {
        //Checks if the target is immune to the debuff
        if (target.ElementType != element)
        {
            //Does a roll to check if we have to apply a debuff
            float roll = UnityEngine.Random.Range(0, 100);

            if (roll <= tower.Proc)
            {
                //applies the debuff
                target.AddDebuff(tower.GetDebuff());
            }
        }

    }
}
